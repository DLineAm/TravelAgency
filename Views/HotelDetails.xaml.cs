using System.Windows;
using TravelAgency.Services;

namespace TravelAgency.Views
{
    /// <summary>
    /// Interaction logic for HotelDetails.xaml
    /// </summary>
    public partial class HotelDetails : Window, IView
    {
        public HotelDetails()
        {
            InitializeComponent();
        }

        public void Minimize()
        {
            if (this.WindowState == WindowState.Minimized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }

            this.WindowState = WindowState.Minimized;
        }

        public void Maximize()
        {
            if (this.WindowState == WindowState.Maximized)
            {
                this.WindowState = WindowState.Normal;
                return;
            }

            this.WindowState = WindowState.Maximized;
        }

        public void UpdateResource(string resourceName, object value)
        {
            throw new System.NotImplementedException();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
