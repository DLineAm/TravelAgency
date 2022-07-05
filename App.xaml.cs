using System;
using Prism.Ioc;
using System.Windows;
using TravelAgency.ViewModels;
using TravelAgency.Views;
using static  TravelAgency.Services.WindowMappingService;

namespace TravelAgency
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        private const string ConnectionString = "Data Source=169.254.131.3; Initial Catalog=Rieltors_Evsyutin; Persist Security Info=True; User Id=stud; Password=Qwerty123456$";
        private const string LocalConnectionString = @"Data Source=(localdb)\MSSQLLocalDB; Initial Catalog=Rieltors_Evsyutin";

        public ResourceDictionary ThemeDictionary => Resources.MergedDictionaries[1];

        private static readonly MainWindowViewModel vm = new MainWindowViewModel();

        public static Rieltors_EvsyutinContext db = new Rieltors_EvsyutinContext();

        protected override Window CreateShell()
        {
            db.ChangeTracker.QueryTrackingBehavior = Microsoft.EntityFrameworkCore.QueryTrackingBehavior.TrackAll;
            var window = Container.Resolve<MainWindow>();
            window.DataContext = vm;
            RegisterWindow(vm, window);
            return window;
        }

        public static string GetConString()
        {
            return LocalConnectionString;
        }

        public void ChangeTheme(Uri uri)
        {
            ThemeDictionary.MergedDictionaries.Clear();
            ThemeDictionary.MergedDictionaries.Add(new ResourceDictionary { Source = uri });
        }

        public static void ShowNotification(string title, string content)
        {
            vm.Notification.Show(title, content);
        }

        public static void ChangeBorders(int value)
        {
            vm.BorderThickness = value;
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {

        }

        private void PrismApplication_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            e.Handled = true;
          
        }
    }
}
