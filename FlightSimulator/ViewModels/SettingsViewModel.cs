using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

namespace FlightSimulatorApp
{
    public class SettingsViewModel : VMNotifyPropertyChanged
    {
        private IAirplaneModel model;

        // Constructor
        public SettingsViewModel(IAirplaneModel airplaneModel)
        {
            this.model = airplaneModel;
            model.PropertyChanged +=
            delegate (Object sender, PropertyChangedEventArgs e) {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }

        public void connect()
        {
            model.connect(this.ip, Int32.Parse(this.port));
            model.start();
        }

        public void disconnect()
        {
            model.disconnect();
        }

        // Properties
        private string ip;
        public string VM_Ip
        {
            get { return this.ip; }
            set
            {
                if (this.ip != value)
                {
                    this.ip = value;
                }
            }
        }

        private string port;
        public string VM_Port
        {
            get { return this.port; }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                }
            }
        }
    }
}
