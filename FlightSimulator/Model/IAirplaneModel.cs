using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public interface IAirplaneModel : INotifyPropertyChanged
    {
        // Connection to the airplane
        void connect();
        void disconnect();
        void start();
        void addSetCommand(string varName, double value);

        // Dashboard properties
        double Heading { set; get; }
        double VerticalSpeed { set; get; }
        double GroundSpeed { set; get; }
        double Airspeed { set; get; }
        double Altitude { set; get; }
        double Roll { set; get; }
        double Pitch { set; get; }
        double Altimeter { set; get; }

        // Map properties
        double Longitude { set; get; }
        double Latitude { set; get; }
        Location Location { set; get; }

        // Settings properties
        string Ip { set; get; }
        int Port { set; get; }

        /*
         
         // Controls properties
        double Rudder { set; get; }
        double Elevator { set; get; }
        double Throttle { set; get; }
        double Aileron { set; get; }

         */
    }
}
