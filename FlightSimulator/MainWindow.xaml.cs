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
    /// <summary>
    /// Interaction logic for Conection.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        
        public MainWindow()
        {
            InitializeComponent();
            DataContext = (Application.Current as App).settingsVM;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                (Application.Current as App).settingsVM.connect();
                this.Hide();
                Simulator si = new Simulator();
                si.Show();
                this.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0}", ex.Message);
            }
        }
    }
}
