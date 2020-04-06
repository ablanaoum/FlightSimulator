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
            model.connect();
            model.start();
        }

        public void reconnect()
        {
            model.reconnect();
        }

        public void disconnect()
        {
            model.disconnect();
        }

        // Properties
        public string VM_Ip
        {
            get { return model.Ip; }
            set { model.Ip = value; }
        }

        public int VM_Port
        {
            get { return model.Port; }
            set { model.Port = value; }
        }

        public string VM_ConnectionErrorMessage
        {
            get { return model.ConnectionErrorMessage; }
        }
    }
}
