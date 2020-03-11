using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;


namespace GUI
{
    public class Valve
    {

        public delegate void StateChanged(object sender, ValveEventArgs e);
        public event StateChanged OnStateChanged;
        public class ValveEventArgs : EventArgs
        {
            public Valve passed;
        }
        public enum ValveColor { Black, Red, Yellow, Blue, Green };
        public enum State { Opened, Closed, Unknown };
        public Image image;
        private bool isOpen_Value;
        public bool isOpen
        {
            get { return isOpen_Value; }
            set {
                  isOpen_Value = value;
                ValveEventArgs e = new ValveEventArgs();
                e.passed = this;
                RaiseStateChange(e);
                }
        }

        public string Name;
        public string Description;
        public bool IsOverriden = false;
        private State Feedback_Value = State.Unknown;
        public State Feedback
        {
            get { return Feedback_Value; }
            set { Feedback_Value = value; }
        }
        private State Command_Value = State.Unknown;
        public State Command
        {
            get { return Command_Value; }
            set { Command_Value = value; }
        }
        public ValveColor color;
        double rotation = 0;
        public BitmapImage Black = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Valve\Black.png"));
        public BitmapImage Red = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Valve\Red.png"));
        public BitmapImage Yellow = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Valve\Yellow.png"));
        public BitmapImage Green = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Valve\Green.png"));
        public BitmapImage Blue = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Valve\Blue.png"));
   
        protected virtual void RaiseStateChange(ValveEventArgs e)
        {
            if (OnStateChanged != null) { OnStateChanged(this, e); }
        }

        public Valve(Image passed_Source, string name, bool rotate, string description)
        {
            Name = name;
            Description = description;
            image = passed_Source;
            image.Source = Black;
            image.RenderTransformOrigin = new Point(0.5, 0.5);
            if (rotate)
            {
                image.RenderTransform = new RotateTransform(90);
                rotation = 90;
            }
            image.Width = 47;
            image.Height = 47;
            
            isOpen_Value = true;

        }
        
        public void Open()
        {
            image.Source = Black;
            color = Valve.ValveColor.Black;
            image.RenderTransform = new RotateTransform(rotation);
            MainWindow.client.sendMessage(4);
            isOpen = true;
        }
        public void Close()
        {
            image.Source = Black;
            color = Valve.ValveColor.Black;
            image.RenderTransform = new RotateTransform(rotation + 90);
            //send close command
            MainWindow.client.sendMessage(5);
            isOpen = false;
        
        }
        public void Toggle()
        {
            image.Source = Black;
            color = Valve.ValveColor.Black;
            if (isOpen)
            {
                image.RenderTransform = new RotateTransform(rotation + 90);
                isOpen = false;
            }
            else
            {               
                image.RenderTransform = new RotateTransform(rotation);
                isOpen = true;
            }
        }

        public void ChangeColor(ValveColor Color)
        {
            if (isOpen)
            {
                switch (Color)
                {
                    case ValveColor.Red:
                        color = ValveColor.Red;
                        image.Source = Red;
                        break;
                    case ValveColor.Yellow:
                        image.Source = Yellow;
                        color = ValveColor.Yellow;
                        break;
                    case ValveColor.Green:
                        image.Source = Green;
                        color = ValveColor.Green;
                        break;
                    case ValveColor.Black:
                        image.Source = Black;
                        color = ValveColor.Black;
                        break;
                }
            }
            else
            {
                switch (Color)
                {
                    case ValveColor.Red:
                        image.Source = Red;
                        color = ValveColor.Red;
                        break;
                    case ValveColor.Yellow:
                        image.Source = Yellow;
                        color = ValveColor.Yellow;
                        break;
                    case ValveColor.Blue:
                        image.Source = Blue;
                        color = ValveColor.Blue;
                        break;
                    case ValveColor.Black:
                        image.Source = Black;
                        color = ValveColor.Black;
                        break;
                }
            }
        }
    }
}
