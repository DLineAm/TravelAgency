using Microsoft.EntityFrameworkCore;

using PdfSharp.Drawing;
using PdfSharp.Pdf;

using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Threading;

using TravelAgency.Models;
using TravelAgency.Services;
using TravelAgency.Views;

using static TravelAgency.Services.WindowMappingService;

namespace TravelAgency.ViewModels
{
    class VouchersControl : BindableBase
    {
        private readonly DispatcherTimer _searchTimer = new DispatcherTimer();
        public VouchersControl()
        {
            Vouchers = GetVouchers().Result;

            _searchTimer.Interval = TimeSpan.FromSeconds(1);
            _searchTimer.Tick += (s, e) => Search();

            App.db.SaveChangesFailed += Db_SaveChangesFailed;
            App.db.SavedChanges += Db_SavedChanges;
        }

        private void Db_SavedChanges(object sender, Microsoft.EntityFrameworkCore.SavedChangesEventArgs e)
        {
            //throw new NotImplementedException();
            App.ShowNotification("SavedChanges Event", "AcceptAllChangesOnSuccess = " + e.AcceptAllChangesOnSuccess + "\n" +
               "EntitiesSavedCount = " + e.EntitiesSavedCount);
        }

        private void Db_SaveChangesFailed(object sender, Microsoft.EntityFrameworkCore.SaveChangesFailedEventArgs e)
        {
            //MessageBox.Show(e.Exception.Message);
            App.ShowNotification("Ошибка сохранения изменений", e.Exception.Message);
        }

        private async Task<List<Voucher>> GetVouchers()
        {
            //await using var connection = new SqlConnection(App.GetConString());

            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("select v.Id, v.StartDate, v.EndDate, v.Duration, v.ClientId, v.HotelId, v.StuffId," +
            //" v.AdditService1Id, v.AdditService2Id, v.AdditService3Id, v.RestTypeId, v.PaymentStatus," +
            //    " v.BookingStatus, c.Name, c.Surname, c.Patronymic, h.Name, s.Name, s.Surname," +
            //    " s.Patronymic, a1.Name, a2.Name, a3.Name, r.Name" + " from Vouchers as v" +
            //    " inner join Clients as c on" + " v.ClientId = c.Id" + " inner join Hotels as h on" +
            //    " v.HotelId = h.Id" + " inner join Stuff as s on" + " v.StuffId = s.Id" +
            //    " inner join AdditionalServices as a1 on" + " v.AdditService1Id = a1.Id" +
            //    " inner join AdditionalServices as a2 on" + " v.AdditService2Id = a2.Id" +
            //    " inner join AdditionalServices as a3 on" + " v.AdditService3Id = a3.Id" +
            //    " inner join RestTypes as r on" + " v.RestTypeId = r.Id")
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //await connection.OpenAsync();

            //var adapter = new SqlDataAdapter(command);
            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);

            //AllVouchers = dataSet.Tables[0].AsEnumerable().Select(datarow => new Vouchers
            //{
            //    Id = datarow.Field<int>(0),
            //    StartDate = datarow.Field<DateTime>(1),
            //    EndDate = datarow.Field<DateTime>(2),
            //    Duration = datarow.Field<int>(3),
            //    ClientId = datarow.Field<int>(4),
            //    HotelId = datarow.Field<int>(5),
            //    StuffId = datarow.Field<int>(6),
            //    AdditService1Id = datarow.Field<int>(7),
            //    AdditService2Id = datarow.Field<int>(8),
            //    AdditService3Id = datarow.Field<int>(9),
            //    RestTypeId = datarow.Field<int>(10),
            //    PaymentStatus = datarow.Field<string>(11),
            //    BookingStatus = datarow.Field<string>(12),
            //    ClientName = datarow.Field<string>(13),
            //    ClientSurname = datarow.Field<string>(14),
            //    ClientPatronymic = datarow.Field<string>(15),
            //    HotelName = datarow.Field<string>(16),
            //    StuffName = datarow.Field<string>(17),
            //    StuffSurname = datarow.Field<string>(18),
            //    StuffPatronymic = datarow.Field<string>(19),
            //    AdditServiceName1 = datarow.Field<string>(20),
            //    AdditServiceName2 = datarow.Field<string>(21),
            //    AdditServiceName3 = datarow.Field<string>(22),
            //    RestTypeName = datarow.Field<string>(23)
            //}).ToList();

            return AllVouchers = App.db.Vouchers.AsNoTracking().Select(p => new Voucher
            {
                Id = p.Id,
                StartDate = p.StartDate,
                EndDate = p.EndDate,
                Duration = p.Duration,
                HotelId = p.HotelId,
                Hotel = p.Hotel,
                RestTypeId = p.RestTypeId,
                RestType = p.RestType,
                AdditService1Id = p.AdditService1Id,
                AdditService1 = p.AdditService1,
                AdditService2Id = p.AdditService2Id,
                AdditService2 = p.AdditService2,
                AdditService3Id = p.AdditService3Id,
                AdditService3 = p.AdditService3,
                ClientId = p.ClientId,
                Stuff = p.Stuff,
                Client = p.Client,
                StuffId = p.StuffId,
                BookingStatus = p.BookingStatus,
                PaymentStatus = p.PaymentStatus
            }).ToList();
        }

        private string _nameSearch;
        public string NameSearch
        {
            get => _nameSearch;
            set
            {
                SetProperty(ref _nameSearch, value);
                _searchTimer.Stop();
                _searchTimer.Start();
            }
        }

        private void Search()
        {
            Vouchers = AllVouchers.Where(p => Services.Math.LevenshteinDistance(p.Client.Name, NameSearch) < 6 ||
            Services.Math.LevenshteinDistance(p.Client.Surname, NameSearch) < 6 ||
            Services.Math.LevenshteinDistance(p.Client.Patronymic, NameSearch) < 6 ||
            Services.Math.LevenshteinDistance(p.Stuff.Name, NameSearch) < 6 ||
            Services.Math.LevenshteinDistance(p.Stuff.Surname, NameSearch) < 6 ||
            Services.Math.LevenshteinDistance(p.Stuff.Patronymic, NameSearch) < 6).ToList();

            if (Vouchers.Count == 0)
                Vouchers = AllVouchers;
            _searchTimer.Stop();
        }

        private async void AddVoucher()
        {
            var result = await OpenVouchersDetailsView(HandleType.Add);

            await App.db.Vouchers.AddAsync(result);
            await App.db.SaveChangesAsync();
            App.db.Entry(result).State = EntityState.Added;

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Insert, result)
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //await connection.OpenAsync();
            //var Id = await command.ExecuteScalarAsync();

            Vouchers = await GetVouchers();
            App.db.Entry(result).State = EntityState.Detached;
            App.db.Entry(result.AdditService1).State = EntityState.Detached;
            App.db.Entry(result.AdditService2).State = EntityState.Detached;
            App.db.Entry(result.AdditService3).State = EntityState.Detached;
            App.ShowNotification("Добавление путевки", $"Путевка №{result.Id} добавлена успешно.");
        }

        private void Sort(bool fromButton)
        {
            if (_isAsc)
                _isAsc = !_isAsc;
            Vouchers = _isAsc
                ? Vouchers.AsEnumerable().OrderBy(p => p.Client.Surname).ToList()
                : Vouchers.AsEnumerable().OrderByDescending(p => p.Client.Surname).ToList();

        }

        private async void GetVoucherDetails()
        {
            if (CurrentVoucher == null)
                return;
            //App.db.ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;

            //App.db.Entry(CurrentVoucher).State = EntityState.Detached;
            //var v = App.db.Vouchers.FirstOrDefault(p => p.Id == CurrentVoucher.Id);
            //App.db.Entry(CurrentVoucher.AdditService1).State = EntityState.Detached;
            //App.db.Entry(CurrentVoucher.AdditService2).State = EntityState.Detached;
            //App.db.Entry(CurrentVoucher.AdditService3).State = EntityState.Detached;
            var v = await OpenVouchersDetailsView(HandleType.View);
            //CurrentVoucher = App.db.Vouchers.AsNoTracking().FirstOrDefault(p => p.Id == v.Id);
            CurrentVoucher = v;
            //App.db.Entry(CurrentVoucher).State = EntityState.Modified;
            //Vouchers = await GetVouchers();
            //await App.db.SaveChangesAsync();
            App.db.Update(CurrentVoucher);
            //DbConnection.GetContext().Vouchers.Update(result);
            //CurrentVoucher = result;
            await App.db.SaveChangesAsync();

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Update, result)
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //await connection.OpenAsync();
            //await command.ExecuteNonQueryAsync();

            Vouchers = await GetVouchers();

        }

        private async Task<Voucher> OpenVouchersDetailsView(HandleType handleType)
        {
            var vm = new VouchersDetailsViewModel(CurrentVoucher, handleType)
            {
                Title = handleType != HandleType.Add
                ? $"Путевка №{CurrentVoucher.Id}"
                : "Добавление путевки"
            };

            ShowModalWindow<VoucherDetails>(vm);

            while (true)
            {

                await Task.Delay(TimeSpan.FromSeconds(2));
                if (vm.Answer)
                    break;
            }

            return vm.Voucher;
        }

        private async void DeleteVoucher()
        {
            if (CurrentVoucher == null)
                return;

            App.db.Vouchers.Remove(CurrentVoucher);
            await App.db.SaveChangesAsync();

            var id = CurrentVoucher.Id;

            //await using var connection = new SqlConnection(App.GetConString());

            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Delete, CurrentVoucher)
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //await connection.OpenAsync();
            //await command.ExecuteNonQueryAsync();

            Vouchers = await GetVouchers();
            App.ShowNotification("Удаление путевки", $"Путевка №{id} успешно удалена.");
        }


        private List<Voucher> _vouchers;

        public List<Voucher> Vouchers { get => _vouchers; set { SetProperty(ref _vouchers, value); if(_currentVoucher != null) App.db.Entry(_currentVoucher).State = EntityState.Detached; } }

        private Voucher _currentVoucher;
        public Voucher CurrentVoucher { get => _currentVoucher; set 
            {
                if (value != null)
                {
                    var v = App.db.Vouchers.FirstOrDefault(p => p.Id == value.Id);
                    SetProperty(ref _currentVoucher, v);
                }
                else 
                {
                    SetProperty(ref _currentVoucher, value);
                }
            } }

        private List<Voucher> _allVouchers;

        public List<Voucher> AllVouchers { get => _allVouchers; set => SetProperty(ref _allVouchers, value); }

        private DelegateCommand _sortCommand;
        public DelegateCommand SortCommand => _sortCommand ??= new DelegateCommand(() => this.Sort(true));

        private DelegateCommand _getVoucherCommand;
        public DelegateCommand GetVoucherCommand => _getVoucherCommand ??= new DelegateCommand(this.GetVoucherDetails);

        private DelegateCommand _deleteVoucherCommand;
        public DelegateCommand DeleteVoucherCommand => _deleteVoucherCommand ??= new DelegateCommand(this.DeleteVoucher);

        private DelegateCommand _addVoucherCommand;
        public DelegateCommand AddVoucherCommand => _addVoucherCommand ??= new DelegateCommand(this.AddVoucher);

        private DelegateCommand _printVoucherCommand;
        public DelegateCommand PrintVoucherCommand => _printVoucherCommand ??= new DelegateCommand(this.PrintVoucher);

        private void PrintVoucher()
        {
            if (CurrentVoucher == null)
                return;

            var mainFont = new XFont("Times New Roman", 22, XFontStyle.Bold);
            var font = new XFont("Times New Roman", 18, XFontStyle.Regular);

            var filename = $"Voucher{CurrentVoucher.Id}.pdf";

            const int x = 150;

            try
            {
                BuilderFactory.NewBuilder<PdfBuilder>()
                       .SetTitle($"Путевка №{CurrentVoucher.Id}")

                       .DrawString($"Путевка №{CurrentVoucher.Id}", mainFont, 0, 15)
                       .DrawLine(XColor.FromKnownColor(XKnownColor.Black), 2, new XPoint(x, 45), new XPoint(450, 45))


                       .DrawString($"Клиент: {CurrentVoucher.Client.Surname} {CurrentVoucher.Client.Name} {CurrentVoucher.Client.Patronymic}",
                           font, x, 45, XStringFormats.CenterLeft)
                       .DrawString($"Сотрудник: {CurrentVoucher.Stuff.Surname} {CurrentVoucher.Stuff.Name} {CurrentVoucher.Stuff.Patronymic}",
                           font, x, 75, XStringFormats.CenterLeft)
                       .DrawString($"Отель: {CurrentVoucher.Hotel.Name}", font, x, 105, XStringFormats.CenterLeft)
                       .DrawString($"Вид отдыха: {CurrentVoucher.RestType.Name}", font, x, 135, XStringFormats.CenterLeft)

                       .DrawString($"Доп услуга 1: {CurrentVoucher.AdditService1.Name}", font, x, 165, XStringFormats.CenterLeft)
                       .DrawString($"Доп услуга 2: {CurrentVoucher.AdditService2.Name}", font, x, 195, XStringFormats.CenterLeft)
                       .DrawString($"Доп услуга 3: {CurrentVoucher.AdditService3.Name}", font, x, 225, XStringFormats.CenterLeft)

                       .DrawString($"Статус оплаты: {CurrentVoucher.PaymentStatus}", font, x, 255, XStringFormats.CenterLeft)
                       .DrawString($"Статус бронирования: {CurrentVoucher.BookingStatus}", font, x, 285, XStringFormats.CenterLeft)

                       .DrawLine(XColor.FromKnownColor(XKnownColor.Gray), 2, new XPoint(x, 460), new XPoint(450, 460))

                       .DrawString($"Дата вылета: {CurrentVoucher.StartDate?.ToShortDateString()}", font, x, 325, XStringFormats.CenterLeft)
                       .DrawString($"Дата прилета: {CurrentVoucher.EndDate?.ToShortDateString()}", font, x, 355, XStringFormats.CenterLeft)
                       .Build()
                       .To<PdfDocument>()
                       .Save(filename);

                var process = new Process();
                process.StartInfo = new ProcessStartInfo(filename)
                {
                    UseShellExecute = true
                };
                process.Start();
            }
            catch (Exception e)
            {
                App.ShowNotification("Ошибка pdf формат", e.Message);
            }
        }

        private bool _isAsc = true;
    }
}
