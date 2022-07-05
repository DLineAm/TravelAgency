using Prism.Commands;
using Prism.Mvvm;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

using TravelAgency.Models;

using static TravelAgency.Services.WindowMappingService;

namespace TravelAgency.ViewModels
{
    public class ClientDetailsViewModel : BindableBase
    {
        public ClientDetailsViewModel(Client client, HandleType handleType)
        {
            _handleType = handleType;
            ChangeBtnVisibility = handleType == HandleType.View ? Visibility.Visible : Visibility.Collapsed;
            LabelVisibility = handleType == HandleType.View ? Visibility.Visible : Visibility.Collapsed;

            if(handleType == HandleType.View || handleType == HandleType.Change)
            {
                BindProperties(client);
            }
            
        }

        private void BindProperties(Client client)
        {
            Client = client;
            Name = client.Name;
            Surname = client.Surname;
            Patronymic = client.Patronymic;
            Address = client.Address;
            Gender = client.Gender;
            DateOfBirth = client.DateOfBirth.Value;
            Phone = client.Phone;
            PassportCode = client.PassportCode;
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

        private void Apply()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Surname) ||
                string.IsNullOrWhiteSpace(Gender) ||
                string.IsNullOrWhiteSpace(Patronymic) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(PassportCode))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            if (_handleType == HandleType.Add)
                Client = new Client();
            Client.Name = Name;
            Client.Surname = Surname;
            Client.Patronymic = Patronymic;
            Client.Address = Address;
            Client.Gender = Gender;
            Client.DateOfBirth = DateOfBirth;
            Client.Phone = Phone;
            Client.PassportCode = PassportCode;

            Answer = true;

            CloseWindow(this);
        }

        private Client _client;
        public Client Client { get => _client; set => SetProperty(ref _client, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _surname;
        public string Surname { get => _surname; set => SetProperty(ref _surname, value); }

        private string _patronymic;
        public string Patronymic { get => _patronymic; set => SetProperty(ref _patronymic, value); }

        private string _gender;
        public string Gender { get => _gender; set => SetProperty(ref _gender, value); }

        private List<string> _genders = new List<string> { "М", "Ж" };
        public List<string> Genders { get => _genders; set => SetProperty(ref _genders, value); }

        private DateTime _dateOfBirth = DateTime.Now;
        public DateTime DateOfBirth { get => _dateOfBirth; set => SetProperty(ref _dateOfBirth, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _passportCode;
        public string PassportCode { get => _passportCode; set => SetProperty(ref _passportCode, value); }

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        public bool Answer = false;

        private HandleType _handleType;
    }
}
