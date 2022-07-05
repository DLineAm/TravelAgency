using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

using TravelAgency.Models;
using static TravelAgency.Services.WindowMappingService;
//using static TravelAgency.Services.CommandBuilder;
using System.Data;
using System.Linq;
using PdfSharp.Pdf;
using TravelAgency.Services;

namespace TravelAgency.ViewModels
{
    class VouchersDetailsViewModel : BindableBase
    {
        public VouchersDetailsViewModel(Voucher voucher, HandleType handleType)
        {
            _handleType = handleType;
            switch (handleType)
            {
                case HandleType.Add:
                    ChangeBtnVisibility = Visibility.Collapsed;
                    LabelVisibility = Visibility.Collapsed;
                    break;
                case HandleType.Change:
                    ChangeBtnVisibility = Visibility.Collapsed;
                    LabelVisibility = Visibility.Collapsed;
                    break;
                case HandleType.View:
                    ChangeBtnVisibility = Visibility.Visible;
                    LabelVisibility = Visibility.Visible;
                    break;
            }
            BindProperties(voucher);
        }

        private async void BindProperties(Voucher voucher)
        {
            if (voucher != null)
                Voucher = voucher;

            Clients = await GetClients();
            Hotels = await GetHotels();
            RestTypes = await GetRestTypes();
            AdditionalServices = await GetAdditionalServices();
            Stuffs = await GetStuffs();

            PaymentStatuses = new List<string> { "Оплачено", "Не оплачено" };
            BookingStatuses = new List<string> { "Забронирован", "Не забронирован" };

            if (_handleType == HandleType.Add) return;
            Client = Clients.FirstOrDefault(p => Voucher.ClientId == p.Id);
            Hotel = Hotels.FirstOrDefault(p => Voucher.HotelId == p.Id);
            RestType = RestTypes.FirstOrDefault(p => Voucher.RestTypeId == p.Id);
            Stuff = Stuffs.FirstOrDefault(p => Voucher.StuffId == p.Id);
            PaymentStatus = Voucher.PaymentStatus;
            BookingStatus = Voucher.BookingStatus;
            StartDate = Voucher.StartDate.Value;
            EndDate = Voucher.EndDate.Value;
            Duration = Voucher.Duration.Value;

            AdditionalService1 = AdditionalServices.FirstOrDefault(p => Voucher.AdditService1Id == p.Id);
            AdditionalService2 = AdditionalServices.FirstOrDefault(p => Voucher.AdditService2Id == p.Id);
            AdditionalService3 = AdditionalServices.FirstOrDefault(p => Voucher.AdditService3Id == p.Id);
        }

        private async Task<List<Stuff>> GetStuffs()
        {
            //var result = await GetPrimitiveList<Stuff>();
            //return result.AsEnumerable().Select(dataRow => new Stuff
            //{
            //    Id = dataRow.Field<int>("Id"),
            //    Name = dataRow.Field<string>("Name"),
            //    Surname = dataRow.Field<string>("Surname"),
            //    Patronymic = dataRow.Field<string>("Patronymic"),
            //    Age = dataRow.Field<int>("Age"),
            //    Address = dataRow.Field<string>("Address"),
            //    Phone = dataRow.Field<string>("Phone"),
            //    PassportCode = dataRow.Field<int>("PassportCode")
            //}).ToList();

            return App.db.Stuffs.ToList();
        }

        private async Task<List<AdditionalService>> GetAdditionalServices()
        {
            //var k = BuilderFactory.NewBuilder<PdfBuilder>()
            //    .SetTitle("fdgdgsdfg")
            //    .Build() as PdfDocument;

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("SELECT * FROM AdditionalServices")
            //    .SetConnection(connection)
            //    .Build() as SqlCommand;

            //var adapter = new SqlDataAdapter(command);
            //await connection.OpenAsync();
            //var dataset = new DataSet();
            //adapter.Fill(dataset);

            //var result = dataset.Tables[0].AsEnumerable().Select(datarow => new AdditionalServices
            //{
            //    Id = datarow.Field<int>("Id"),
            //    Name = datarow.Field<string>("Name"),
            //    Description = datarow.Field<string>("Description"),
            //    Price = datarow.Field<int>("Price")
            //}).ToList();

            return App.db.AdditionalServices.ToList();
        }

        private async Task<List<RestType>> GetRestTypes()
        {
            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("SELECT * FROM RestTypes")
            //    .SetConnection(connection)
            //    .Build() as SqlCommand;

            //var adapter = new SqlDataAdapter(command);
            //await connection.OpenAsync();
            //var dataset = new DataSet();
            //adapter.Fill(dataset);

            //var result = dataset.Tables[0].AsEnumerable().Select(datarow => new RestTypes()
            //{
            //    Id = datarow.Field<int>("Id"),
            //    Name = datarow.Field<string>("Name"),
            //    Description = datarow.Field<string>("Description"),
            //    Restrictions = datarow.Field<string>("Restrictions")
            //}).ToList();

            return App.db.RestTypes.ToList();
        }

        private async Task<List<Hotel>> GetHotels()
        {
            //await using var connection = new SqlConnection(App.GetConString());

            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("SELECT * FROM Hotels")
            //    .SetConnection(connection)
            //    .Build().To<SqlCommand>();

            ////const string querry = "SELECT * FROM Hotels";
            //var request = new SqlCommand(command.CommandText, connection);
            //var adapter = new SqlDataAdapter(request);
            //connection.Open();
            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);

            //var result = dataSet.Tables[0].AsEnumerable()
            //    .Select(datarow => new Hotels
            //    {
            //        Id = datarow.Field<int>("Id"),
            //        Name = datarow.Field<string>("Name"),
            //        Country = datarow.Field<string>("Country"),
            //        City = datarow.Field<string>("City"),
            //        Address = datarow.Field<string>("Address"),
            //        Phone = datarow.Field<string>("Phone"),
            //        TheContactPerson = datarow.Field<string>("TheContactPerson")
            //    }).ToList();

            return App.db.Hotels.ToList();
        }

        private async Task<List<Client>> GetClients()
        {
            //await using var connection = new SqlConnection(App.GetConString());

            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("SELECT * FROM Clients")
            //    .SetConnection(connection)
            //    .Build().To<SqlCommand>();

            //var adapter = new SqlDataAdapter(command);
            //await connection.OpenAsync();
            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);

            //var result = dataSet.Tables[0].AsEnumerable().Select(datarow => new Clients()
            //{
            //    Id = datarow.Field<int>("Id"),
            //    Name = datarow.Field<string>("Name"),
            //    Surname = datarow.Field<string>("Surname"),
            //    Patronymic = datarow.Field<string>("Patronymic"),
            //    DateOfBirth = datarow.Field<DateTime>("DateOfBirth"),
            //    Gender = datarow.Field<string>("Gender"),
            //    Address = datarow.Field<string>("Address"),
            //    Phone = datarow.Field<string>("Phone"),
            //    PassportCode = datarow.Field<string>("PassportCode")
            //}).ToList(); ;

            return App.db.Clients.ToList();
        }

        private async Task<DataTable> GetPrimitiveList<T>()
        {
            //var listType = typeof(T);
            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText($"SELECT * FROM {listType.Name}")
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //var adapter = new SqlDataAdapter(command);
            //await connection.OpenAsync();
            //var dataset = new DataSet();
            //adapter.Fill(dataset);

            return new DataTable();
        }

        private Voucher _voucher;

        public Voucher Voucher
        {
            get => _voucher;
            set => SetProperty(ref _voucher, value);
        }

        private List<Client> _clients;

        public List<Client> Clients
        {
            get => _clients;
            set => SetProperty(ref _clients, value);
        }

        private Client _client;

        public Client Client
        {
            get => _client;
            set => SetProperty(ref _client, value);
        }

        private DateTime _startDate = DateTime.Now;
        public DateTime StartDate
        {
            get => _startDate;
            set { SetProperty(ref _startDate, value); ResolveDuration(); }
        }

        private DateTime _endDate = DateTime.Now;
        public DateTime EndDate
        {
            get => _endDate;
            set { SetProperty(ref _endDate, value); ResolveDuration(); }
        }

        private int _duration;
        public int Duration
        {
            get => _duration;
            set => SetProperty(ref _duration, value);
        }

        private List<Hotel> _hotels;
        public List<Hotel> Hotels
        {
            get => _hotels;
            set => SetProperty(ref _hotels, value);
        }

        private Hotel _hotel;
        public Hotel Hotel
        {
            get => _hotel;
            set => SetProperty(ref _hotel, value);
        }

        private List<RestType> _restTypes;
        public List<RestType> RestTypes
        {
            get => _restTypes;
            set => SetProperty(ref _restTypes, value);
        }

        private RestType _restType;
        public RestType RestType
        {
            get => _restType;
            set => SetProperty(ref _restType, value);
        }

        private List<AdditionalService> _additServices;
        public List<AdditionalService> AdditionalServices
        {
            get => _additServices;
            set => SetProperty(ref _additServices, value);
        }

        private AdditionalService _additService1;
        public AdditionalService AdditionalService1
        {
            get => _additService1;
            set => SetProperty(ref _additService1, value);
        }

        private AdditionalService _additService2;
        public AdditionalService AdditionalService2
        {
            get => _additService2;
            set => SetProperty(ref _additService2, value);
        }

        private AdditionalService _additService3;
        public AdditionalService AdditionalService3
        {
            get => _additService3;
            set => SetProperty(ref _additService3, value);
        }

        private List<Stuff> _stuffs;
        public List<Stuff> Stuffs
        {
            get => _stuffs;
            set => SetProperty(ref _stuffs, value);
        }

        private Stuff _stuff;
        public Stuff Stuff
        {
            get => _stuff;
            set => SetProperty(ref _stuff, value);
        }

        private List<string> _bookingStatuses = new List<string>() { "Забронирован", "Не забронирован" };
        public List<string> BookingStatuses
        {
            get => _bookingStatuses;
            set => SetProperty(ref _bookingStatuses, value);
        }

        private string _bookingStatus;
        public string BookingStatus
        {
            get => _bookingStatus;
            set => SetProperty(ref _bookingStatus, value);
        }

        private List<string> _paymentStatuses = new List<string>() { "Оплачено", "Не оплачено" };
        public List<string> PaymentStatuses
        {
            get => _paymentStatuses;
            set => SetProperty(ref _paymentStatuses, value);
        }

        private string _paymentStatus;
        public string PaymentStatus
        {
            get => _paymentStatus;
            set => SetProperty(ref _paymentStatus, value);
        }

        public string Title
        {
            get;
            set;
        }

        private Visibility _labelVisibility;
        public Visibility LabelVisibility { get => _labelVisibility; set => SetProperty(ref _labelVisibility, value); }

        private Visibility _changeBtnVisibility;
        public Visibility ChangeBtnVisibility { get => _changeBtnVisibility; set => SetProperty(ref _changeBtnVisibility, value); }

        private DelegateCommand _applyCommand;
        public DelegateCommand ApplyCommand => _applyCommand ??= new DelegateCommand(this.Apply);

        private DelegateCommand _changeCommand;
        public DelegateCommand ChangeCommand => _changeCommand ??= new DelegateCommand(this.ChangeView);

        private void ChangeView()
        {
            LabelVisibility = Visibility.Collapsed;
            ChangeBtnVisibility = Visibility.Collapsed;
        }

        private HandleType _handleType;
        public bool Answer { get; private set; }

        private void Apply()
        {
            if (Client == null ||
                Stuff == null ||
                RestType == null ||
                Hotel == null ||
                AdditionalService1 == null ||
                AdditionalService2 == null ||
                AdditionalService3 == null ||
                string.IsNullOrWhiteSpace(PaymentStatus) ||
                string.IsNullOrWhiteSpace(BookingStatus)
                )
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            if (StartDate >= EndDate)
            {
                MessageBox.Show("Дата вылета должна быть меньше даты конца");
                return;
            }
            if (_handleType == HandleType.Add)
                Voucher = new Voucher();

            Voucher.StartDate = StartDate;
            Voucher.EndDate = EndDate;
            Voucher.ClientId = Client.Id;
            Voucher.Client = Client;
            Voucher.StuffId = Stuff.Id;
            Voucher.Stuff = Stuff;
            Voucher.Duration = (EndDate - StartDate).Days;
            Voucher.HotelId = Hotel.Id;
            Voucher.Hotel = Hotel;
            Voucher.RestTypeId = RestType.Id;
            Voucher.RestType = RestType;
            Voucher.AdditService1Id = AdditionalService1.Id;
            Voucher.AdditService1 = AdditionalService1;
            Voucher.AdditService2Id = AdditionalService2.Id;
            Voucher.AdditService2 = AdditionalService2;
            Voucher.AdditService3Id = AdditionalService3.Id;
            Voucher.AdditService3 = AdditionalService3;
            Voucher.BookingStatus = BookingStatus;
            Voucher.PaymentStatus = PaymentStatus;

            Answer = true;

            CloseWindow(this);
        }

        private void ResolveDuration()
        {
            if(EndDate > StartDate)
                Duration = (int)(EndDate - StartDate).TotalDays;
        }
    }
}
