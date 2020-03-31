using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class MapViewModel : VMNotifyPropertyChanged
    {
        private IAirplaneModel model;

        // Constructor
        public MapViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        // Map properties
        public double VM_Longitude
        {
            get { return model.Longitude; }
        }

        public double VM_Latitude
        {
            get { return model.Latitude; }
        }
    }
}
