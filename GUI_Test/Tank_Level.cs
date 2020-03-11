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
    public class Tank_Level
    {

        public delegate void StateChanged(object sender, ContactEventArgs e);
        public event StateChanged OnStateChanged;
        public class ContactEventArgs : EventArgs
        {
            public Fill_Arm_Contact passed;
        }
        public Image image;

        private int Level_Value = 50;
        public int Level
        {
            get { return Level_Value; }
            set {
                Level_Value = value;
                Change_Tank_Level(Level_Value);
            }
        }
   
        protected virtual void RaiseStateChange(ContactEventArgs e)
        {
            if (OnStateChanged != null) { OnStateChanged(this, e); }
        }

        public Tank_Level(Image passed_Source)
        {
            image = passed_Source;
            image.Source = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Tank_Fill_Level\Tank_Level_" + 50 + ".png"));

            //image.Width = 35;
            //image.Height = 35;

        }
        void Change_Tank_Level(int level)
        {
            level += 10;
            if (level > 100)
            {
                level = 0;

            }
            image.Source = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Tank_Fill_Level\Tank_Level_" + level + ".png"));
            image.Margin = new Thickness(-170, -134 - 2.7 * level, 0, 0);
        }
    }
}
