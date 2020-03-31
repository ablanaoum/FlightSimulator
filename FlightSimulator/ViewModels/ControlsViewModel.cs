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

        // Constructor
        public ControlsViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        // Controls properties
        private double rudder;
        public double VM_Rudder
        {
            get { return this.rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = value;
                    model.addSetCommand("/controls/flight/rudder", this.rudder);
                }
            }
        }

        private double elevator;
        public double VM_Elevator
        {
            get { return this.elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = value;
                    model.addSetCommand("/controls/flight/elevator", this.elevator);
                }
            }
        }

        private double throttle;
        public double VM_Throttle
        {
            get { return this.throttle; }
            set
            {
                if (this.throttle != value)
                {
                    this.throttle = value;
                    model.addSetCommand("/controls/engines/current-engine/throttle", this.throttle);
                }
            }
        }

        private double aileron;
        public double VM_Aileron
        {
            get { return this.aileron; }
            set
            {
                if (this.aileron != value)
                {
                    this.aileron = value;
                    model.addSetCommand("/controls/flight/aileron", this.aileron);
                }
            }
        }


        /*

        // Navigators properties
        public double VM_Rudder
        {
            get { return model.Rudder; }
        }

        public double VM_Elevator
        {
            get { return model.Elevator; }
        }

        public double VM_Throttle
        {
            get { return model.Throttle; }
        }

        public double VM_Aileron
        {
            get { return model.Aileron; }
        }

        */
    }
}
