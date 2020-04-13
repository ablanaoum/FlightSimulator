using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    // ViewModel Class.
    public class ViewModel : VMNotifyPropertyChanged
    {
        private IAirplaneModel model;
        public ControlsViewModel ControlsViewModel { get; private set; }
        public DashboardViewModel DashboardViewModel { get; private set; }
        public MapViewModel MapViewModel { get; private set; }
        public SettingsViewModel SettingsViewModel { get; private set; }


        // Constructor.
        public ViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            this.ControlsViewModel = new ControlsViewModel(this.model);
            this.DashboardViewModel = new DashboardViewModel(this.model);
            this.MapViewModel = new MapViewModel(this.model);
            this.SettingsViewModel = new SettingsViewModel(model);
        }
    }
}
