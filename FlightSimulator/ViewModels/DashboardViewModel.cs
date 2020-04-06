using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class DashboardViewModel : VMNotifyPropertyChanged
    {
        private IAirplaneModel model;

        // Constructor
        public DashboardViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        // Dashboard properties
        public double VM_Heading
        {
            get { return model.Heading; }
        }

        public double VM_VerticalSpeed
        {
            get { return model.VerticalSpeed; }
        }

        public double VM_GroundSpeed
        {
            get { return model.GroundSpeed; }
        }

        public double VM_Airspeed
        {
            get { return model.Airspeed; }
        }

        public double VM_Altitude
        {
            get { return model.Altitude; }
        }

        public double VM_Roll
        {
            get { return model.Roll; }
        }

        public double VM_Pitch
        {
            get { return model.Pitch; }
        }

        public double VM_Altimeter
        {
            get { return model.Altimeter; }
        }

        public string VM_ErrorScreen
        {
            get { return model.ErrorScreen; }
        }
    }
}