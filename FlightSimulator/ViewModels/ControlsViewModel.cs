using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class ControlsViewModel : VMNotifyPropertyChanged
    {
        private IAirplaneModel model;

        // Constructor.
        public ControlsViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        // Controls properties.
        public double VM_Rudder
        {
            get { return model.Rudder; }
            set
            {
                if (model.Rudder != value)
                {
                    model.Rudder = value;
                    model.AddSetCommand("/controls/flight/rudder", model.Rudder);
                }
            }
        }

        public double VM_Elevator
        {
            get { return model.Elevator; }
            set
            {
                if (model.Elevator != value)
                {
                    model.Elevator = value;
                    model.AddSetCommand("/controls/flight/elevator", model.Elevator);
                }
            }
        }

        public double VM_Throttle
        {
            get { return model.Throttle; }
            set
            {
                if (model.Throttle != value)
                {
                    model.Throttle = value;
                    model.AddSetCommand("/controls/engines/current-engine/throttle", model.Throttle);
                }
            }
        }

        public double VM_Aileron
        {
            get { return model.Aileron; }
            set
            {
                if (model.Aileron != value)
                {
                    model.Aileron = value;
                    model.AddSetCommand("/controls/flight/aileron", model.Aileron);
                }
            }
        }
    }
}
