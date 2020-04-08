using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using FlightSimulatorApp;
using Microsoft.Maps.MapControl.WPF;


namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml.
    /// </summary>
    public partial class Simulator : Window
    {  
        public Simulator()
        {
            InitializeComponent();
            myMap.DataContext = (Application.Current as App).mapVM;
            dash.DataContext = (Application.Current as App).dashboardVM;
            myControl.DataContext = (Application.Current as App).controlsVM;
        }

        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).settingsVM.Disconnect();
            this.Close();
        }

        private void Reconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            (Application.Current as App).settingsVM.Reconnect();
            // Return to main window for reconnection.
            MainWindow logIn = new MainWindow();
            logIn.Show();
            this.Close();
        }
    }
}
