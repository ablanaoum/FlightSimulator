using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace FlightSimulator.Views
{
    public partial class Joystick :UserControl
    {
        public Joystick()
        {
            InitializeComponent();
            center = new Point(Base.Width / 2 - KnobBase.Width / 2, Base.Height / 2 - KnobBase.Height / 2);
            radius = Base.Width / 2;
        }
        private Point mouseDownLoc = new Point();
        private Point center;
        private double radius;


        private void centerKnob_Completed(object sender, EventArgs e) { }


        private void Knob_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Knob.ReleaseMouseCapture();
            knobPosition.X = 0;
            knobPosition.Y = 0;
   
        }

        private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                double x = e.GetPosition(this).X - mouseDownLoc.X;
                double y = e.GetPosition(this).Y - mouseDownLoc.Y;
                double dist = Math.Sqrt(x * x + y * y);
                double m = y / x;
                double maxDist = Base.Width / 2 - KnobBase.Width / 2;
                if (dist <= maxDist)
                {
                    knobPosition.X = x;
                    knobPosition.Y = y;
                }
                else
                {
                    knobPosition.X = maxDist / Math.Sqrt(m * m + 1);
                    if (x < 0)
                    {
                        knobPosition.X = -1 * knobPosition.X;
                    }
                    knobPosition.Y = m * knobPosition.X;
                }
             
            }
        }

        private void Knob_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                mouseDownLoc = e.GetPosition(this);
                Knob.CaptureMouse();
            }
        }
    }
}
