using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;

namespace FlightSimulator.Views
{
    // Constructor.
    public partial class Joystick : UserControl
    {
        // Dependency propreties of rudder and elevator.
        public static readonly DependencyProperty RudderProperty = DependencyProperty.Register("Rudder", typeof(double), typeof(Joystick));
        public static readonly DependencyProperty ElevatorProperty = DependencyProperty.Register("Elevator", typeof(double), typeof(Joystick));
        private Point mouseDownLoc = new Point();
        private double maxDist;

        // Constructor.
        public Joystick()
        {
            InitializeComponent();
            // Max distance the center of knob can get without go out from joystick's base.
            maxDist = Base.Width / 2 - KnobBase.Width / 2;
        }

        private void Knob_MouseUp(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            Knob.ReleaseMouseCapture();
            // Return x,y to the center (0,0).
            knobPosition.X = 0;
            knobPosition.Y = 0;
            // Update the rudder and elevator according to x,y.
            Rudder = knobPosition.X;
            Elevator = knobPosition.Y;
        }

        private void Knob_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {
            // If the mouse button is pressed.
            if (e.LeftButton == System.Windows.Input.MouseButtonState.Pressed)
            {
                // Temp x,y for the current mouse position.
                double x = e.GetPosition(this).X - mouseDownLoc.X;
                double y = e.GetPosition(this).Y - mouseDownLoc.Y;
                // Distance of temp x,y from center.
                double dist = Math.Sqrt(x * x + y * y);
                double m;
                // If the mouse position is inside the joystick.
                if (dist <= maxDist)
                {
                    // Update the knob position according to the mouse position.
                    knobPosition.X = x;
                    knobPosition.Y = y;
                    // Normalize and update rudder and elevator.
                    SetNormalRudder();
                    SetNormalElevator();
                }
                // If the mouse position is outside the joystick.
                else
                {
                    // Edge case - Joystick is vertical.
                    if (x == 0)
                    {
                        // Update x position to 0.
                        knobPosition.X = 0;
                        // If y is positive - update y position to max distance.
                        if (y > 0)
                        {
                            knobPosition.Y = maxDist;
                        }
                        // If y is nagative - update y position to minus max distance.
                        else
                        {
                            knobPosition.Y = -1 * maxDist;
                        }
                        // Update rudder to 0 and normalize and update elevator.
                        Rudder = 0;
                        SetNormalElevator();
                    }
                    // Any other case (dintance bigger then max dist and x isn't 0).
                    else
                    {
                        // Calculate slope between (x,y) to center (0,0).
                        m = y / x;
                        // Calculate and update x position to max dist in the same slop.
                        knobPosition.X = maxDist / Math.Sqrt(m * m + 1);
                        // if x is nagative - multiply the knob position by -1.
                        if (x < 0)
                        {
                            knobPosition.X = -1 * knobPosition.X;
                        }
                        // Calculate and update y position by the x knob position and slop.
                        knobPosition.Y = m * knobPosition.X;
                        // Normalize and update rudder and elevator.
                        SetNormalRudder();
                        SetNormalElevator();
                    }
                    
                }
             
            }
        }

        private void Knob_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            // If mouse button is down.
            if (e.ChangedButton == System.Windows.Input.MouseButton.Left)
            {
                // Save the mouse position.
                mouseDownLoc = e.GetPosition(this);
                // Only the capturing knob receives the mouse events until released.
                Knob.CaptureMouse();
            }
        }

        private void centerKnob_Completed(object sender, EventArgs e) { }

        private void SetNormalRudder()
        {
            // Normalize and set rudder by the x knob position.
            Rudder = 2 * ((knobPosition.X + maxDist) / (maxDist * 2)) - 1;
        }

        private void SetNormalElevator()
        {
            // Normalize and set elevator by the y knob position.
            Elevator = -1 * (2 * ((knobPosition.Y + maxDist) / (maxDist * 2)) - 1);
        }

        // Dependency Properties.
        public double Rudder
        {
            get
            {
                return (double)GetValue(RudderProperty);
            }
            set
            {
                SetValue(RudderProperty, value);
            }
        }

        public double Elevator
        {
            get
            {
                return (double)GetValue(ElevatorProperty);
            }
            set
            {
                SetValue(ElevatorProperty, value);
            }
        }
    }
}
