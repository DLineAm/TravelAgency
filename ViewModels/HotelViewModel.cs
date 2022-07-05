using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

using Prism.Commands;
using Prism.Mvvm;

using TravelAgency.Models;
using TravelAgency.Services;
using TravelAgency.Views;

//using static TravelAgency.Services.CommandBuilder;
using static TravelAgency.Services.WindowMappingService;

namespace TravelAgency.ViewModels
{
    public class HotelViewModel : BindableBase
    {
        private bool _isAsc = true;

        private readonly DispatcherTimer _searchTimer = new DispatcherTimer();

        private List<Hotel> _hotels;

        public List<Hotel> Hotels
        {
            get => _hotels;
            set => SetProperty(ref _hotels, value);
        }

        private Hotel _currentHotel;

        public Hotel CurrentHotel
        {
            get => _currentHotel;
            set => SetProperty(ref _currentHotel, value);
        }

        private string _currentSort = "Название";
        public string CurrentSort
        {
            get => _currentSort;
            set
            {
                value = value.Remove(0, value.IndexOf(":") + 2);
                SetProperty(ref _currentSort, value);
                Sort(false);
            }
        }

        private Visibility _buttonVisibility = Visibility.Hidden;
        public Visibility ButtonVisibility { get => _buttonVisibility; set => SetProperty(ref _buttonVisibility, value); }

        private string _currentCountry;
        public string CurrentCountry
        {
            get => _currentCountry;
            set
            {
                SetProperty(ref _currentCountry, value);
                Sort(false);
            }
        }

        private List<string> _countries;
        public List<string> Countries { get => _countries; set => SetProperty(ref _countries, value); }

        private DelegateCommand _sortCommand;
        public DelegateCommand SortCommand => _sortCommand ??= new DelegateCommand(() => this.Sort(true));

        private DelegateCommand _getHotelCommand;
        public DelegateCommand GetHotelCommand => _getHotelCommand ??= new DelegateCommand(this.GetHotelDetails);

        private DelegateCommand _deleteHotelCommand;
        public DelegateCommand DeleteHotelCommand => _deleteHotelCommand ??= new DelegateCommand(this.DeleteHotel);

        private DelegateCommand _addHotelCommand;
        public DelegateCommand AddHotelCommand => _addHotelCommand ??= new DelegateCommand(this.AddHotel);

        private async void AddHotel()
        {
            var result = await OpenHotelDetailsView(HandleType.Add);

            await App.db.Hotels.AddAsync(result);
            await App.db.SaveChangesAsync();

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Insert, result)
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //await connection.OpenAsync();
            //var id = await command.ExecuteScalarAsync();

            Hotels = GetHotels();
            App.ShowNotification("Добавление отеля", $"Отель {result.Name} добавлен.");
        }

        private async void DeleteHotel()
        {
            if (CurrentHotel == null)
                return;

            try
            {
                var hotel = CurrentHotel;

                App.db.Remove(CurrentHotel);
                await App.db.SaveChangesAsync();
                

                //await using var connection = new SqlConnection(App.GetConString());

                //var command = BuilderFactory.NewBuilder<CommandBuilder>()
                //    .SetCommandText(CommandBuilder.CommandType.Delete, CurrentHotel)
                //    .SetConnection(connection)
                //    .Build()
                //    .To<SqlCommand>();

                //await connection.OpenAsync();
                //await command.ExecuteNonQueryAsync();

                Hotels = GetHotels();
                App.ShowNotification("Удаление отеля", $"Отель №{hotel.Id} ({hotel.Name}) удален.");
            }
            catch (Exception e)
            {
                App.ShowNotification("Ошибка удаления отеля", e.Message);
            }
        }

        private async void GetHotelDetails()
        {
            if (CurrentHotel == null)
                return;

            var result = await OpenHotelDetailsView(HandleType.View);

            CurrentHotel = result;
            await App.db.SaveChangesAsync();

            //await using var connection = new SqlConnection(App.GetConString());

            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Update, result)
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();

            //connection.Open();

            //command.ExecuteNonQuery();

            Hotels = GetHotels();
        }



        private void Sort(bool fromButton)
        {
            if (fromButton)
                _isAsc = !_isAsc;
            Hotels = App.db.Hotels.ToList();
            Hotels = _isAsc
                ? Hotels.AsQueryable().OrderBy(hotel => CurrentSort == "Название" ? hotel.Name : hotel.Address).ToList()
                : Hotels.AsQueryable().OrderByDescending(hotel => CurrentSort == "Название" ? hotel.Name : hotel.Address).ToList();
            if (CurrentCountry != null)
                Hotels = Hotels.Where(p => p.Country == CurrentCountry).ToList();
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
            if (string.IsNullOrWhiteSpace(NameSearch) ||
                Hotels.Count == 0)
            {
                Hotels = App.db.Hotels.ToList();
                return;
            }
            Hotels = Hotels.Where(p => Services.Math.LevenshteinDistance(p.Name, NameSearch) < 6).ToList();
            _searchTimer.Stop();
        }

        public HotelViewModel()
        {
            _searchTimer.Interval = TimeSpan.FromSeconds(1);
            _searchTimer.Tick += (s, e) => { Search(); };

            //using var connection = new SqlConnection(App.GetConString());
            Hotels = GetHotels();

            CurrentCountry = Countries[0];
        }

        private List<Hotel> GetHotels()
        {
            //await using var connection = new SqlConnection(App.GetConString());

            //var s = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText<Hotels>(CommandBuilder.CommandType.Update)
            //    .Build()
            //    .To<SqlCommand>();

            //const string querry = "SELECT * FROM Hotels";
            //var request = new SqlCommand(querry, connection);
            //var adapter = new SqlDataAdapter(request);
            //connection.Open();

            // await s.ExecuteNonQueryAsync();

            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);
            //AllHotels = dataSet.Tables[0].AsEnumerable()
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
            //Hotels = AllHotels;

            Countries = GetCountries();
            Hotels = App.db.Hotels.ToList();
            Sort(false);
            //return Hotels.ToList();
            //var k = App.db.Hotels.ToList();
            //MessageBox.Show(k.Count.ToString());
            return Hotels;
        }

        private List<string> GetCountries()
        {
            //using var connection = new SqlConnection(App.GetConString());
            //const string querry = "SELECT Distinct Country From Hotels";
            //var request = new SqlCommand(querry, connection);
            //var adapter = new SqlDataAdapter(request);
            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);
            //return dataSet.Tables[0].AsEnumerable()
            //    .Select(r => r.Field<string>(0))
            //    .ToList();
            return App.db.Hotels.AsEnumerable().Select(p => p.Country).Distinct().ToList();
        }

        private async Task<Hotel> OpenHotelDetailsView(HandleType handleType)
        {
            var vm = new HotelDetailsViewModel(CurrentHotel, handleType) { Title = handleType == HandleType.Add ? "Добавление отеля" : CurrentHotel.Name };
            ShowModalWindow<HotelDetails>(vm);

            while (true)
            {

                await Task.Delay(TimeSpan.FromSeconds(2));
                if (vm.Answer)
                    break;
            }

            return vm.Hotel;
        }
    }
}