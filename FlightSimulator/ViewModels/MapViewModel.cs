using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

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
        private double longitude;
        public double VM_Longitude
        {
            get { return model.Longitude; }
            set { longitude = value; }
        }

        private double latitude;
        public double VM_Latitude
        {
            get { return model.Latitude; }
            set { latitude = value; }
        }

        private Location location;
        public Location VM_Location
        {
            get
            {
                location = model.Location;
                return location;
            }
        }
    }
}
