﻿using System.Windows;
using TravelAgency.Services;

namespace TravelAgency.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, IView
    {
        public MainWindow()
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
                App.ChangeBorders(1);
                return;
            }

            this.WindowState = WindowState.Maximized;
            App.ChangeBorders(7);
        }

        public void UpdateResource(string resourceName, object value)
        {
            this.Resources[resourceName] = value;
        }
    }
}
