using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.ComponentModel;
using Microsoft.Maps.MapControl.WPF;
using System.Diagnostics;
using System.Windows.Threading;
using System.Configuration;
using FlightSimulator;

namespace FlightSimulatorApp
{
    public class MyAirplaneModel : IAirplaneModel
    {
        private ITelnetClient client;
        private volatile Boolean stop;
        // Array of tuples of <simulator's variable name, value>
        private Tuple<string, double>[] simVars;
        // Queue of 'set' commands to send to the simulator
        private Queue<string> commands;
        private readonly object syncLock;
        // Stopwatch to measure server response time
        private Stopwatch stopWatch;
        // Timer to perform an action repeatedly within a given interval
        private DispatcherTimer timer;

        // Constructor
        public MyAirplaneModel()
        {
            this.client = new MyTelnetClient();
            this.stop = false;
            this.simVars = this.createSimVarsArr();
            this.commands = new Queue<string>();
            this.syncLock = new object();
            this.stopWatch = new Stopwatch();
            this.timer = new DispatcherTimer();
            // Set the timer to perform the function repeatedly within 2 seconds
            this.timer.Interval = TimeSpan.FromSeconds(2);
            this.timer.Tick += timerTick;
            // Default IP and port
            Ip = ConfigurationManager.AppSettings["ip"];
            Port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
        }

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
        public void connect()
        {
            ConnectionErrorMessage = string.Empty;
            try
            {
                client.connect(this.ip, this.port);
                ErrorScreen = "Welcome to Flight Simulator!";
            }
            catch (Exception)
            {
                ConnectionErrorMessage = "Unable to connect, please try again";
                throw new Exception("Unable to connect, please try again");
            }
        }

        public void reconnect()
        {
            disconnect();
            this.simVars = this.createSimVarsArr();
            this.commands.Clear();
            Ip = ConfigurationManager.AppSettings["ip"];
            Port = Int32.Parse(ConfigurationManager.AppSettings["port"]);
            ErrorScreen = "Oops! Connection went wrong. Try to reconnect or close the simulator.";
        }

        public void disconnect()
        {
            stop = true;
            timer.Stop();
            client.disconnect();
        }

        public void addSetCommand(string varName, double value)
        {
            string command = "set " + varName + " " + value + "\n";
            this.commands.Enqueue(command);
        }

        public void start()
        {
            this.stop = false;
            timer.Start();
            // Thread for getting values from the simulator
            new Thread(delegate ()
            {
                string varName, command, receivedMessageFromGet;
                try
                {
                    while (!stop)
                    {
                        // Get values from the simulator
                        for (int i = 0; i < 10; i++)
                        {
                            varName = simVars[i].Item1;
                            command = "get " + varName + "\n";
                            // Send 'get' command to the server and receive the returned value
                            receivedMessageFromGet = writeAndRead(command);
                            try
                            {
                                simVars[i] = new Tuple<string, double>(varName, Double.Parse(receivedMessageFromGet));
                            }
                            catch (Exception)
                            {
                                Console.WriteLine("Exception: Invalid value received for \"{0}\"", varName);
                                ErrorScreen = "Exception: Invalid value received for \"" + varName + "\"";
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
                        Location = new Location(Latitude, Longitude);

                        // Read the data in 4Hz
                        Thread.Sleep(250);
                    }
                }
                catch (Exception)
                {
                    reconnect();
                }
            }).Start();

            // Tread for sending values to the simulator
            new Thread(delegate ()
            {
                string receivedMessageFromSet;
                try
                {
                    while (!stop)
                    {
                        // While the commands queue is not empty
                        if (commands.Count != 0)
                        {
                            receivedMessageFromSet = writeAndRead(commands.Dequeue());
                        }

                        // Read the data in 4Hz
                        Thread.Sleep(1);
                    }
                }
                catch (Exception)
                {
                    reconnect();
                }
            }).Start();
        }

        public string writeAndRead(string command)
        {
            lock (syncLock)
            {
                string message;
                client.write(command);
                // Measure the server response time
                stopWatch.Restart();
                // Thread.Sleep(10000);  // Test waiting for response from the server for 10 seconds
                message = client.read();
                stopWatch.Reset();
                return message;
            }
        }

        // The function notifies the user when the server is busy and responds slowly
        public void timerTick(object sender, EventArgs e)
        {
            // If the server did not respond for at least 8-10 seconds
            if (stopWatch.ElapsedMilliseconds > 8000)
            {
                ErrorScreen = "Notice: Server is busy...";
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

        // Dashboard properties
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

        private string errorScreen;
        public string ErrorScreen
        {
            get { return this.errorScreen; }
            set
            {
                if (this.errorScreen != value)
                {
                    this.errorScreen = value;
                    this.NotifyPropertyChanged("ErrorScreen");
                    Thread.Sleep(300);
                }
            }
        }

        // Map properties
        private double longitude;
        public double Longitude
        {
            get { return this.longitude; }
            set
            {
                if (this.longitude != value)
                {
                    if ((value >= -180) && (value <= 180))
                    {
                        this.longitude = value;
                        this.NotifyPropertyChanged("Longitude");
                    }
                    else
                    {
                        ErrorScreen = "Exception: Invalid value received for /position/longitude-deg";
                    }
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
                    if ((value >= -90) && (value <= 90))
                    {
                        this.latitude = value;
                        this.NotifyPropertyChanged("Latitude");
                    }
                    else
                    {
                        ErrorScreen = "Exception: Invalid value received for /position/latitude-deg";
                    }
                }
            }
        }

        private Location location;
        public Location Location
        {
            get { return this.location; }
            set
            {
                if (this.location != value)
                {
                    this.location = value;
                    this.NotifyPropertyChanged("Location");
                }
            }
        }

        // Settings properties
        private string ip;
        public string Ip
        {
            get { return this.ip; }
            set
            {
                if (this.ip != value)
                {
                    this.ip = value;
                    this.NotifyPropertyChanged("Ip");
                }
            }
        }

        private int port;
        public int Port
        {
            get { return this.port; }
            set
            {
                if (this.port != value)
                {
                    this.port = value;
                    this.NotifyPropertyChanged("Port");
                }
            }
        }

        private string connectionErrorMessage;
        public string ConnectionErrorMessage
        {
            get { return this.connectionErrorMessage; }
            set
            {
                if (this.connectionErrorMessage != value)
                {
                    this.connectionErrorMessage = value;
                    this.NotifyPropertyChanged("ConnectionErrorMessage");
                }
            }
        }
    }
}
