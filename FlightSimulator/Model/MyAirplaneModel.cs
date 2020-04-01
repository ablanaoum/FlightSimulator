using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulatorApp
{
    public class MyAirplaneModel : IAirplaneModel
    {
        //private static MyAirplaneModel instance;
        private ITelnetClient client;
        private volatile Boolean stop;
        private Tuple<string, double>[] simVars;
        // 'set' commands to send to the simulator
        private Queue<string> commands;
        private readonly object syncLock;


        public MyAirplaneModel()
        {
            this.client = new MyTelnetClient();
            this.stop = false;
            this.simVars = this.createSimVarsArr();
            this.commands = new Queue<string>();
            this.syncLock = new object();
        }

        
         /*
        public static MyAirplaneModel GetInstance()
        {
            if (instance == null)
            {
                instance = new MyAirplaneModel();
            }
            return instance;
        }*/

        

        // Create array of tuples of <simulator's variable name, value>
        private Tuple<string, double>[] createSimVarsArr()
        {
            Tuple<string, double>[] simVarsArr =
            {
                new Tuple<string, double> ("/instrumentation/heading-indicator/indicated-heading-deg", 0),
                new Tuple<string, double> ("/instrumentation/gps/indicated-vertical-speed", 0),
                new Tuple<string, double> ("/instrumentation/gps/indicated-ground-speed-kt", 0),
                new Tuple<string, double> ("/instrumentation/airspeed-indicator/indicated-speed-kt", 0),
                new Tuple<string, double> ("/instrumentation/gps/indicated-altitude-ft", 0),
                new Tuple<string, double> ("/instrumentation/attitude-indicator/internal-roll-deg", 0),
                new Tuple<string, double> ("/instrumentation/attitude-indicator/internal-pitch-deg", 0),
                new Tuple<string, double> ("/instrumentation/altimeter/indicated-altitude-ft", 0),
                new Tuple<string, double> ("/position/longitude-deg", 0),
                new Tuple<string, double> ("/position/latitude-deg", 0)
            };

            return simVarsArr;
        }

        // Connection to the airplane
        public void connect(string ip, int port)
        {
            client.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            client.disconnect();
        }

        public void addSetCommand(string varName, double value)
        {
            string command = "set " + varName + " " + value + "\n";
            this.commands.Enqueue(command);
        }

        public void start()
        {
            // Thread for getting values from the simulator
            new Thread(delegate ()
            {
                string varName, command, receivedMessageFromGet;
                while (!stop)
                {
                    /*
                    // Get dashboard values from the simulator
                    tcpClient.write("get indicated-heading-deg\n");
                    Heading = Double.Parse(tcpClient.read());
                    tcpClient.write("get gps_indicated-vertical-speed\n");
                    GpsVerticalSpeed = Double.Parse(tcpClient.read());
                    tcpClient.write("get gps_indicated-ground-speed-kt\n");
                    GpsGroundSpeed = Double.Parse(tcpClient.read());
                    tcpClient.write("get airspeed-indicator_indicated-speed-kt\n");
                    AirspeedIndicatorSpeed = Double.Parse(tcpClient.read());
                    tcpClient.write("get gps_indicated-altitude-ft\n");
                    GpsAltitude = Double.Parse(tcpClient.read());
                    tcpClient.write("get attitude-indicator_internal-roll-deg\n");
                    AttitudeIndicatorInternalRoll = Double.Parse(tcpClient.read());
                    tcpClient.write("get attitude-indicator_internal-pitch-deg\n");
                    AttitudeIndicatorInternalPitch = Double.Parse(tcpClient.read());
                    tcpClient.write("get altimeter_indicated-altitude-ft\n");
                    AltimeterAltitude = Double.Parse(tcpClient.read());
                    */

                    // Get values from the simulator
                    for (int i = 0; i < 10; i++)
                    {
                        varName = simVars[i].Item1;
                        command = "get " + varName + "\n";
                        // Send 'get' command to the server
                        //client.write(command);
                        receivedMessageFromGet = writeAndRead(command);
                        try
                        {
                            //simVars[i] = new Tuple<string, double>(varName, Double.Parse(client.read()));
                            simVars[i] = new Tuple<string, double>(varName, Double.Parse(receivedMessageFromGet));
                        }
                        catch (Exception)
                        {
                            Console.WriteLine("Exception: Invalid value received for \"{0}\"", varName);
                        }
                    }
                    // Set the properties
                    Heading = simVars[0].Item2;
                    VerticalSpeed = simVars[1].Item2;
                    GroundSpeed = simVars[2].Item2;
                    Airspeed = simVars[3].Item2;
                    Altitude = simVars[4].Item2;
                    Roll = simVars[5].Item2;
                    Pitch = simVars[6].Item2;
                    Altimeter = simVars[7].Item2;
                    Longitude = simVars[8].Item2;
                    Latitude = simVars[9].Item2;
                    Location = new Location(Latitude,Longitude);

                    // Read the data in 4Hz
                    Thread.Sleep(250);
                }
            }).Start();

            // Tread for sending values to the simulator
            new Thread(delegate ()
            {
                string receivedMessageFromSet;
                while (!stop)
                {
                    // While the commands queue is not empty
                    if (commands.Count != 0)
                    {
                        receivedMessageFromSet = writeAndRead(commands.Dequeue());
                        //client.write(commands.Dequeue());
                        // Do nothing with the returned value
                        //client.read();
                    }

                    // Read the data in 4Hz
                    Thread.Sleep(50);
                }
            }).Start();
        }

        public string writeAndRead(string command)
        {
            lock (syncLock)
            {
                client.write(command);
                return client.read();
            }
        }

        // INotifyPropertyChanged implementation
        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }

        // Dashboard properties implementation
        private double heading;
        public double Heading
        {
            get { return this.heading; }
            set
            {
                if (this.heading != value)
                {
                    this.heading = value;
                    this.NotifyPropertyChanged("Heading");
                }
            }
        }

        private double verticalSpeed;
        public double VerticalSpeed
        {
            get { return this.verticalSpeed; }
            set
            {
                if (this.verticalSpeed != value)
                {
                    this.verticalSpeed = value;
                    this.NotifyPropertyChanged("VerticalSpeed");
                }
            }
        }

        private double groundSpeed;
        public double GroundSpeed
        {
            get { return this.groundSpeed; }
            set
            {
                if (this.groundSpeed != value)
                {
                    this.groundSpeed = value;
                    this.NotifyPropertyChanged("GroundSpeed");
                }
            }
        }

        private double airspeed;
        public double Airspeed
        {
            get { return this.airspeed; }
            set
            {
                if (this.airspeed != value)
                {
                    this.airspeed = value;
                    this.NotifyPropertyChanged("Airspeed");
                }
            }
        }

        private double altitude;
        public double Altitude
        {
            get { return this.altitude; }
            set
            {
                if (this.altitude != value)
                {
                    this.altitude = value;
                    this.NotifyPropertyChanged("Altitude");
                }
            }
        }

        private double roll;
        public double Roll
        {
            get { return this.roll; }
            set
            {
                if (this.roll != value)
                {
                    this.roll = value;
                    this.NotifyPropertyChanged("Roll");
                }
            }
        }

        private double pitch;
        public double Pitch
        {
            get { return this.pitch; }
            set
            {
                if (this.pitch != value)
                {
                    this.pitch = value;
                    this.NotifyPropertyChanged("Pitch");
                }
            }
        }

        private double altimeter;
        public double Altimeter
        {
            get { return this.altimeter; }
            set
            {
                if (this.altimeter != value)
                {
                    this.altimeter = value;
                    this.NotifyPropertyChanged("Altimeter");
                }
            }
        }

        // Map properties implementation
        private double longitude;
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    this.longitude = value;
                    this.NotifyPropertyChanged("Longitude");
                }
            }
        }

        private double latitude;
        public double Latitude
        {
            get { return this.latitude; }
            set
            {
                if (this.latitude != value)
                {
                    this.latitude = value;
                    this.NotifyPropertyChanged("Latitude");
                }
            }
        }

        private Location location;
        public Location Location
        {
            get
            {
                return location;
            }
            set
            {
                location = value;
                NotifyPropertyChanged("Location");
            }
        }

        /*

        // Controls properties implementation
        private double rudder;
        public double Rudder
        {
            get { return this.rudder; }
            set
            {
                if (this.rudder != value)
                {
                    this.rudder = value;
                    this.NotifyPropertyChanged("Rudder");
                }
            }
        }

        private double elevator;
        public double Elevator
        {
            get { return this.elevator; }
            set
            {
                if (this.elevator != value)
                {
                    this.elevator = value;
                    this.NotifyPropertyChanged("Elevator");
                }
            }
        }

        private double throttle;
        public double Throttle
        {
            get { return this.throttle; }
            set
            {
                if (this.throttle != value)
                {
                    this.throttle = value;
                    this.NotifyPropertyChanged("Throttle");
                }
            }
        }

        private double aileron;
        public double Aileron
        {
            get { return this.aileron; }
            set
            {
                if (this.aileron != value)
                {
                    this.aileron = value;
                    this.NotifyPropertyChanged("Aileron");
                }
            }
        }

         */
    }
}
