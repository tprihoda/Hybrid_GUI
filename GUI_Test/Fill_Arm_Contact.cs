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
    public class Fill_Arm_Contact
    {

        public delegate void StateChanged(object sender, ContactEventArgs e);
        public event StateChanged OnStateChanged;
        public class ContactEventArgs : EventArgs
        {
            public Fill_Arm_Contact passed;
        }
        public enum ContactColor { Black, Red, Yellow, Blue, Green };
        public enum State { Opened, Closed, Unknown };
        public Image image;
        private bool isOpen_Value;
        public bool isOpen
        {
            get { return isOpen_Value; }
            set {
                  isOpen_Value = value;
                ContactEventArgs e = new ContactEventArgs();
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
        public ContactColor color;
        double rotation = 0;
        public BitmapImage Open_Black = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Open\Contact_Open_Black.png"));
        public BitmapImage Closed_Black = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Closed\Contact_Closed_Black.png"));
        public BitmapImage Open_Red = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Open\Contact_Open_Red.png"));
        public BitmapImage Closed_Red = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Closed\Contact_Closed_Red.png"));
        public BitmapImage Open_Yellow = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Open\Contact_Open_Yellow.png"));
        public BitmapImage Closed_Yellow = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Closed\Contact_Closed_Yellow.png"));
        public BitmapImage Open_Green = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Open\Contact_Open_Green.png"));
        public BitmapImage Closed_Blue = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Contact_Closed\Contact_Closed_Blue.png"));
   
        protected virtual void RaiseStateChange(ContactEventArgs e)
        {
            if (OnStateChanged != null) { OnStateChanged(this, e); }
        }

        public Fill_Arm_Contact(Image passed_Source, string name, string description)
        {
            Name = name;
            Description = description;
            image = passed_Source;
            image.Source = Open_Black;

            image.Width = 35;
            image.Height = 35;
            
            isOpen_Value = true;

        }
        
        public void Open()
        {
            image.Source = Open_Black;
            color = Fill_Arm_Contact.ContactColor.Black;
            image.RenderTransform = new RotateTransform(rotation);
            isOpen = true;
        }
        public void Close()
        {
            image.Source = Closed_Black;
            color = Fill_Arm_Contact.ContactColor.Black;
            image.RenderTransform = new RotateTransform(rotation + 90);
            isOpen = false;
        
        }
        public void Toggle()
        {
            
            if (isOpen)
            {
                image.Source = Closed_Black;
                color = Fill_Arm_Contact.ContactColor.Black;
            }
            else
            {
                image.Source = Open_Black;
                color = Fill_Arm_Contact.ContactColor.Black;
            }
        }

        public void ChangeColor(ContactColor Color)
        {
            if (isOpen)
            {
                switch (Color)
                {
                    case ContactColor.Red:
                        color = ContactColor.Red;
                        image.Source = Open_Red;
                        break;
                    case ContactColor.Yellow:
                        image.Source = Open_Yellow;
                        color = ContactColor.Yellow;
                        break;
                    case ContactColor.Green:
                        image.Source = Open_Green;
                        color = ContactColor.Green;
                        break;
                    case ContactColor.Black:
                        image.Source = Open_Black;
                        color = ContactColor.Black;
                        break;
                }
            }
            else
            {
                switch (Color)
                {
                    case ContactColor.Red:
                        image.Source = Closed_Red;
                        color = ContactColor.Red;
                        break;
                    case ContactColor.Yellow:
                        image.Source = Closed_Yellow;
                        color = ContactColor.Yellow;
                        break;
                    case ContactColor.Blue:
                        image.Source = Closed_Blue;
                        color = ContactColor.Blue;
                        break;
                    case ContactColor.Black:
                        image.Source = Closed_Black;
                        color = ContactColor.Black;
                        break;
                }
            }
        }
    }
}
