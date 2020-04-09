using FlightSimulator.Views;
using FlightSimulatorApp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;


namespace FlightSimulator
{
    // Interaction logic for MainWindow.xaml.
    public partial class MainWindow : Window
    {  
        public MainWindow()
        {
            InitializeComponent();
            // Define the data contex as settingVM.
            DataContext = (Application.Current as App).settingsVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Try to connect model by click on button.
                (Application.Current as App).settingsVM.Connect();
                this.Hide();
                // Create new window of the simulator.
                Simulator si = new Simulator();
                // Show the simulator window.
                si.Show();
                // Close this connection window.
                this.Close();
            }
            catch (Exception ex)
            {
                // Print an Error message that there is a problem with connection.
                Console.WriteLine("{0}", ex.Message);
            }
        }
    }
}
