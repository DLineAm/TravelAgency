using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Threading;

using TravelAgency.Models;
using TravelAgency.Services;
using TravelAgency.Views;

using static TravelAgency.Services.WindowMappingService;

namespace TravelAgency.ViewModels
{
    public class ClientsControlViewModel : BindableBase
    {
        public ClientsControlViewModel()
        {
            _searchTimer.Interval = TimeSpan.FromSeconds(1);
            _searchTimer.Tick += (s, e) => { Search(); };
            Clients = GetClients().Result;
        }

        private bool _isAsc = true;

        private readonly DispatcherTimer _searchTimer = new DispatcherTimer();

        private List<Client> _clients;
        public List<Client> Clients { get => _clients; set => SetProperty(ref _clients, value); }

        private List<Client> _allClients;
        public List<Client> AllClients { get => _allClients; set => SetProperty(ref _allClients, value); }

        private Client _selectedClient;
        public Client SelectedClient { get => _selectedClient; set => SetProperty(ref _selectedClient, value); }

        private DelegateCommand _sortCommand;
        public DelegateCommand SortCommand => _sortCommand ??= new DelegateCommand(() => this.Sort(true));

        private DelegateCommand _getClientCommand;
        public DelegateCommand GetClientCommand => _getClientCommand ??= new DelegateCommand(this.GetClientDetails);

        private DelegateCommand _deleteClientCommand;
        public DelegateCommand DeleteClientCommand => _deleteClientCommand ??= new DelegateCommand(this.DeleteClient);

        private DelegateCommand _addClientCommand;
        public DelegateCommand AddClientCommand => _addClientCommand ??= new DelegateCommand(this.AddClient);

        private async void AddClient()
        {
            var result = await OpenClientsDetailsView(HandleType.Add);

            var date = result.DateOfBirth?.ToString("yyyy-MM-dd");

            App.db.Clients.Add(result);
            await App.db.SaveChangesAsync();

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Insert, result)
            //    .SetConnection(connection)
            //    .Build().To<SqlCommand>();

            //await connection.OpenAsync();
            //command.CommandType = CommandType.Text;
            //var Id = await command.ExecuteScalarAsync();

            Clients = await GetClients();
            App.ShowNotification("Добавление клиента", $"Клиент {result.Surname} {result.Name} {result.Patronymic} добавлен.");
        }

        private async void GetClientDetails()
        {
            if (SelectedClient == null) return;

            var result = await OpenClientsDetailsView(HandleType.View);

            SelectedClient = result;
            await App.db.SaveChangesAsync();

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText(CommandBuilder.CommandType.Update, result)
            //    .SetConnection(connection)
            //    .Build() as SqlCommand;

            //await connection.OpenAsync();
            //await command.ExecuteNonQueryAsync();

            Clients = await GetClients();
        }

        private async void DeleteClient()
        {
            if (SelectedClient == null) return;

            try
            {
                var client = SelectedClient;

                App.db.Clients.Remove(SelectedClient);
                await App.db.SaveChangesAsync();

                //await using var connection = new SqlConnection(App.GetConString());

                //var command = BuilderFactory.NewBuilder<CommandBuilder>()
                //    .SetCommandText(CommandBuilder.CommandType.Delete, SelectedClient)
                //    .SetConnection(connection)
                //    .Build() as SqlCommand;

                //await connection.OpenAsync();
                //await command.ExecuteNonQueryAsync();

                Clients = await GetClients();
                App.ShowNotification("Удаление клиента", $"Клиент №{client.Id} ({client.Surname} {client.Name} {client.Patronymic}) удален.");
            }
            catch (Exception e)
            {
                App.ShowNotification("Ошибка удаления клиента", e.Message);
            }
        }

        private string _nameSearch;
        public string NameSearch
        {
            get => _nameSearch;
            set
            {
                SetProperty(ref _nameSearch, value);
                Search();
            }
        }

        private int _selectedGender = 0;
        public int SelectedGender
        {
            get => _selectedGender;
            set
            {
                SetProperty(ref _selectedGender, value);
                Sort(false);
            }
        }

        private void Sort(bool fromButton)
        {
            if (fromButton)
                _isAsc = !_isAsc;
            Clients = AllClients;
            if (SelectedGender == 0)
                Clients = Clients.Where(p => p.Gender == "М" || p.Gender == "Ж").ToList();
            else
                Clients = Clients.Where(p => SelectedGender == 1 ? p.Gender == "М" : p.Gender == "Ж").ToList();
            Clients = _isAsc
                ? Clients.AsEnumerable().OrderBy(p => p.Name).ToList()
                : Clients.AsEnumerable().OrderByDescending(p => p.Name).ToList();
        }

        private void Search()
        {
            if (string.IsNullOrWhiteSpace(NameSearch)
                || Clients.Count == 0)
            {
                Clients = AllClients;
            }

            Clients = Clients.Where(p => Services.Math.LevenshteinDistance(p.Name, NameSearch) < 6 ||
                                         Services.Math.LevenshteinDistance(p.Surname, NameSearch) < 6 ||
                                         Services.Math.LevenshteinDistance(p.Patronymic, NameSearch) < 6).ToList();
            _searchTimer.Stop();
        }

        private async Task<List<Client>> GetClients()
        {

            //await using var connection = new SqlConnection(App.GetConString());
            //var command = BuilderFactory.NewBuilder<CommandBuilder>()
            //    .SetCommandText("SELECT * FROM Clients")
            //    .SetConnection(connection)
            //    .Build()
            //    .To<SqlCommand>();
            //var adapter = new SqlDataAdapter(command);
            //connection.Open();
            //var dataSet = new DataSet();
            //adapter.Fill(dataSet);

            //AllClients = dataSet.Tables[0].AsEnumerable().Select(datarow => new Clients()
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
            //}).ToList();

            return AllClients = App.db.Clients.ToList();
        }

        private async Task<Client> OpenClientsDetailsView(HandleType handleType)
        {
            var vm = new ClientDetailsViewModel(SelectedClient, handleType)
            {
                Title = handleType != HandleType.Add
                ? SelectedClient.Surname + " " + SelectedClient.Name + " " + SelectedClient.Patronymic
                : "Добавление клиента"
            };

            ShowModalWindow<ClientDetails>(vm);

            while (true)
            {

                await Task.Delay(TimeSpan.FromSeconds(2));
                if (vm.Answer)
                    break;
            }

            return vm.Client;
        }
    }
}
