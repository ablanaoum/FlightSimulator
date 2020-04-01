using FlightSimulator.Views;
using FlightSimulatorApp;
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
using System.Windows.Shapes;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for Conection.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        IAirplaneModel airplaneModel;
        private DashboardViewModel dashboardVM;
        private MapViewModel mapVM;
        private ControlsViewModel controlsVM;
        private SettingsViewModel settingsVM;

        public MainWindow()
        {
            InitializeComponent();
            airplaneModel = new MyAirplaneModel();
            //ViewModel viewModel = new ViewModel(airplaneModel);
            dashboardVM = new DashboardViewModel(airplaneModel);
            mapVM = new MapViewModel(airplaneModel);
            controlsVM = new ControlsViewModel(airplaneModel);
            settingsVM = new SettingsViewModel(airplaneModel);
            DataContext = settingsVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            settingsVM.connect();
            this.Hide();
            Simulator si = new Simulator();
            si.Show();
            this.Close();
        }
    }
}
