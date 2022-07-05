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
    public class HotelDetailsViewModel : BindableBase
    {
        public HotelDetailsViewModel(Hotel selectedHotel, HandleType handleType)
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
                    BindProperties(selectedHotel);
                    break;
                case HandleType.View:
                    ChangeBtnVisibility = Visibility.Visible;
                    LabelVisibility = Visibility.Visible;
                    BindProperties(selectedHotel);
                    break;
            }
        }

        private void BindProperties(Hotel selectedHotel)
        {
            Hotel = selectedHotel;
            Name = selectedHotel.Name;
            Address = selectedHotel.Address;
            Country = selectedHotel.Country;
            City = selectedHotel.City;
            Phone = selectedHotel.Phone;
            ContactPerson = selectedHotel.TheContactPerson;
        }

        private readonly HandleType _handleType;

        private Hotel _hotel = null;
        public Hotel Hotel
        {
            get => _hotel;
            set => SetProperty(ref _hotel, value);
        }

        private DelegateCommand _applyCommand;
        public DelegateCommand ApplyCommand => _applyCommand ??= new DelegateCommand(() => this.Apply());

        private DelegateCommand _changeCommand;
        public DelegateCommand ChangeCommand => _changeCommand ??= new DelegateCommand(() => this.ChangeView());

        private void ChangeView()
        {
            LabelVisibility = Visibility.Collapsed;
            ChangeBtnVisibility = Visibility.Collapsed;
        }

        private void Apply()
        {
            if (string.IsNullOrWhiteSpace(Name) ||
                string.IsNullOrWhiteSpace(Address) ||
                string.IsNullOrWhiteSpace(City) ||
                string.IsNullOrWhiteSpace(Country) ||
                string.IsNullOrWhiteSpace(Phone) ||
                string.IsNullOrWhiteSpace(ContactPerson))
            {
                MessageBox.Show("Все поля должны быть заполнены!");
                return;
            }
            if (_handleType == HandleType.Add)
                Hotel = new Hotel();
            Hotel.Name = Name;
            Hotel.Address = Address;
            Hotel.City = City;
            Hotel.Country = Country;
            Hotel.Phone = Phone;
            Hotel.TheContactPerson = ContactPerson;

            Answer = true;

            CloseWindow(this);
        }

        public bool Answer = false;

        private Visibility _labelVisibility;
        public Visibility LabelVisibility { get => _labelVisibility; set => SetProperty(ref _labelVisibility, value); }

        private Visibility _changeBtnVisibility;
        public Visibility ChangeBtnVisibility { get => _changeBtnVisibility; set => SetProperty(ref _changeBtnVisibility, value); }

        private string _name;
        public string Name { get => _name; set => SetProperty(ref _name, value); }

        private string _title;
        public string Title { get => _title; set => SetProperty(ref _title, value); }

        private string _address;
        public string Address { get => _address; set => SetProperty(ref _address, value); }

        private string _city;
        public string City { get => _city; set => SetProperty(ref _city, value); }

        private string _country;
        public string Country { get => _country; set => SetProperty(ref _country, value); }

        private string _phone;
        public string Phone { get => _phone; set => SetProperty(ref _phone, value); }

        private string _contactPerson;
        public string ContactPerson { get => _contactPerson; set => SetProperty(ref _contactPerson, value); }
    }

    public enum HandleType
    {
        Add, Change, View
    }
}
