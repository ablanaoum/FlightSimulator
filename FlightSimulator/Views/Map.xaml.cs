using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulator.Views
{
    // Map Class - Interaction logic for Map.xaml.
    public partial class Map : UserControl
    {
        // Constructor.
        public Map()
        {
            InitializeComponent();
            myMap.Mode = new AerialMode(true);
        }
    }
}
