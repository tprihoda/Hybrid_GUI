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
    class Servo
    {
        public enum ServoColor { Black, Red, Yellow, Blue, Green};
        public Image image;
        public bool isOpen;
        public ServoColor color;
        public BitmapImage Open = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Open\Servo_Open_Black.png"));
        public BitmapImage Closed = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Closed\Servo_Closed_Black.png"));
        public BitmapImage Red_Open = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Open\Servo_Open_Red.png"));
        public BitmapImage Red_Closed = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Closed\Servo_Closed_Red.png"));
        public BitmapImage Yellow_Open = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Open\Servo_Open_Yellow.png"));
        public BitmapImage Yellow_Closed = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Closed\Servo_Closed_Yellow.png"));
        public BitmapImage Green_Open = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Open\Servo_Open_Green.png"));
        public BitmapImage Blue_Closed = new BitmapImage(new Uri(@"C:\Users\Thomas Prihoda\Documents\!Personal\MissionControl\P_and_ID_Symbols\Servo_Closed\Servo_Closed_Blue.png"));

        public Servo(Image passed_Source)
        {
            image = passed_Source;
            image.Source = Open;
            isOpen = true;
        }

        public void ChangeColor(ServoColor Color)
        {
            if (isOpen)
            {
                switch (Color)
                {
                    case ServoColor.Red:
                        image.Source = Red_Open;
                        color = ServoColor.Red;
                        break;
                    case ServoColor.Yellow:
                        image.Source = Yellow_Open;
                        color = ServoColor.Yellow;
                        break;
                    case ServoColor.Green:
                        image.Source = Green_Open;
                        color = ServoColor.Green;
                        break;
                    case ServoColor.Black:
                        image.Source = Open;
                        color = ServoColor.Black;
                        break;
                }
            }
            else
            {
                switch (Color)
                {
                    case ServoColor.Red:
                        image.Source = Red_Closed;
                        color = ServoColor.Red;
                        break;
                    case ServoColor.Yellow:
                        image.Source = Yellow_Closed;
                        color = ServoColor.Yellow;
                        break;
                    case ServoColor.Blue:
                        image.Source = Blue_Closed;
                        color = ServoColor.Blue;
                        break;
                    case ServoColor.Black:
                        image.Source = Closed;
                        color = ServoColor.Black;
                        break;
                }
            }
        }
    }
}
