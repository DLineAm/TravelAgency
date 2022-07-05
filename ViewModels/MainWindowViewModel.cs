using System;
using System.Net.Mime;
using System.Windows;
using System.Windows.Media;
using Prism.Commands;
using Prism.Mvvm;
using TravelAgency.Services;

namespace TravelAgency.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {

        private object _currentPage;

        public object CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }


        private bool _isDarkTheme = true;
        public bool IsDarkTheme
        {
            get => _isDarkTheme;
            set => SetProperty(ref _isDarkTheme, value);
        }

        private DelegateCommand _changeThemeCommand;
        public DelegateCommand ChangeThemeCommand => _changeThemeCommand ??= new DelegateCommand(this.ChangeTheme);

        private void ChangeTheme()
        {
            App app = (App) Application.Current;
            if (_isDarkTheme)
            {
                //view.UpdateResource("BackgroundColor", new SolidColorBrush(Color.FromRgb(255, 255, 255)));
                //view.UpdateResource("TabBackgroundColor", new SolidColorBrush(Color.FromRgb(169, 169, 169)));
                //view.UpdateResource("ForegroundColor", new SolidColorBrush(Color.FromRgb(0, 0, 0)));
                app.ChangeTheme(new Uri("/Resources/LightTheme.xaml", UriKind.RelativeOrAbsolute));
                IsDarkTheme = !IsDarkTheme;
                return;
            }
            //view.UpdateResource("BackgroundColor", new SolidColorBrush(Color.FromArgb(255, 56, 56, 97)));
            //view.UpdateResource("TabBackgroundColor", new SolidColorBrush(Color.FromArgb(255, 28, 28, 44)));
            //view.UpdateResource("ForegroundColor", new SolidColorBrush(Color.FromRgb(255, 255, 255)));
            app.ChangeTheme(new Uri("/Resources/DarkTheme.xaml", UriKind.RelativeOrAbsolute));
            IsDarkTheme = !IsDarkTheme;
        }

        private string _title = "Travel Agency";
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

        private int _borderThickness = 1;
        public int BorderThickness { get => _borderThickness; set => SetProperty(ref _borderThickness, value); }

        private int _selectedTab = 0;
        public int SelectedTab { get => _selectedTab; set => SetProperty(ref _selectedTab, value); }


        private DelegateCommand<int?> _changeTabCommand;
        public DelegateCommand<int?> ChangeTabCommand => _changeTabCommand ??= new DelegateCommand<int?>(this.ChangeTab);

        private void ChangeTab(int? tab)
        {
            if (tab == null) return;
            SelectedTab = (int)tab;
        }

        private Notification _notification = new Notification();
        public Notification Notification { get => _notification; set => SetProperty(ref _notification, value); }

        private DelegateCommand _closeNotificationCommand;
        public DelegateCommand CloseNotificationCommand => _closeNotificationCommand ??= new DelegateCommand(this.Notification.Close);
    }
}
