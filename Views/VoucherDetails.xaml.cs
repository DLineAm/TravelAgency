using System;
using System.Windows;

using TravelAgency.Services;

namespace TravelAgency.Views
{
    /// <summary>
    /// Логика взаимодействия для VoucherDetails.xaml
    /// </summary>
    public partial class VoucherDetails : Window, IView
    {
        public VoucherDetails()
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
            throw new NotImplementedException();
        }
    }
}
