using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Maps.MapControl.WPF;

namespace FlightSimulator.Views
{
    public partial class Map : UserControl
    {
        // Interaction logic for Map.xaml.
        public Map()
        {
            InitializeComponent();
            myMap.Mode = new AerialMode(true);
        }
    }
}
