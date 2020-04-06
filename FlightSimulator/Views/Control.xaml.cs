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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace FlightSimulator.Views
{
    /// <summary>
    /// Interaction logic for Control.xaml
    /// </summary>
    public partial class Control : UserControl
    {
        public Control()
        {
            InitializeComponent();
        }

        private void aileron_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Application.Current as App).controlsVM.VM_Aileron = aileron.Value;
        }

        private void throttle_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            (Application.Current as App).controlsVM.VM_Throttle = throttle.Value;
        }

        private void rudder_TextChanged(object sender, TextChangedEventArgs e)
        {
            double valueRud = Convert.ToDouble(rudder.Text);
            (Application.Current as App).controlsVM.VM_Rudder = valueRud;
        }

        private void elevator_TextChanged(object sender, TextChangedEventArgs e)
        {
            double valueEle = Convert.ToDouble(elevator.Text);
            (Application.Current as App).controlsVM.VM_Elevator = valueEle;
        }
    }
}
