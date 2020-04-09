using FlightSimulatorApp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace FlightSimulator
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        IAirplaneModel airplaneModel;
        public SettingsViewModel settingsVM;
        public DashboardViewModel dashboardVM;
        public MapViewModel mapVM;
        public ControlsViewModel controlsVM;

        void App_Startup(object sender, StartupEventArgs e)
        {
            // Initialize all view models and model in high level class-App.
            airplaneModel = new MyAirplaneModel();
            settingsVM = new SettingsViewModel(airplaneModel);
            dashboardVM = new DashboardViewModel(airplaneModel);
            mapVM = new MapViewModel(airplaneModel);
            controlsVM = new ControlsViewModel(airplaneModel);
        }
    }
}
