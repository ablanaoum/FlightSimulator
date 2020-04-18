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
    // Simulator Class - Interaction logic for Simulator.xaml.
    public partial class Simulator : Window
    {  
        // Constructor.
        public Simulator()
        {
            InitializeComponent();
            // Define the data context of the map to mapVM.
            myMap.DataContext = (Application.Current as App).mapVM;
            // Define the data context of the dashboard to dashboardVM.
            dash.DataContext = (Application.Current as App).dashboardVM;
            // Define the data context of the control to controlVM.
            myControl.DataContext = (Application.Current as App).controlsVM;
        }

        // Logic behind the Close button.
        private void Close_Button_Click(object sender, RoutedEventArgs e)
        {
            // Close button calls Disconnect function in settingVM.
            (Application.Current as App).settingsVM.Disconnect();
            // Close this window.
            this.Close();
        }

        // Logic behind the Reconnect button.
        private void Reconnect_Button_Click(object sender, RoutedEventArgs e)
        {
            // Return to main window for reconnection.
            MainWindow logIn = new MainWindow();
            // Show the main window.
            logIn.Show();
            // Close this window.
            this.Close();
            // Reconnect button calls Reconnect function in settingVM.
            (Application.Current as App).settingsVM.Reconnect();
        }
    }
}
