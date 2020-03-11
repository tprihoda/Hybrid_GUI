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
using System.Windows.Shapes;

namespace GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    public partial class Popup_Ignitor : Window
    {
        public Popup_Ignitor(Valve passed, string description)
        {
            InitializeComponent();
            this.DataContext = passed;
            Name.Text = passed.Name;
            Description.Text = description;
            Description.TextWrapping = TextWrapping.Wrap;
            if (passed.IsOverriden)
            {
                Override.Content = "ON";
                Override.Background = Brushes.Green;
                Override_Open.Opacity = 1.00;
                Override_Close.Opacity = 1.00;
                if (passed.isOpen)
                {
                    Override_Open.Background = Brushes.Green;
                    Override_Close.Background = Brushes.Red;
                }
                else
                {
                    Override_Close.Background = Brushes.Green;
                    Override_Open.Background = Brushes.Red;
                }

            }
            else
            {
                Override.Content = "OFF";
                Override.Background = Brushes.LightGray;
                Override_Open.Background = Brushes.LightGray;
                Override_Close.Background = Brushes.LightGray;
                Override_Open.Opacity = 0.50;
                Override_Close.Opacity = 0.50;
            }
            

            Override.Click += delegate (object sender, RoutedEventArgs e) { Override_Click(sender, e, passed); };
            Override_Open.Click += delegate (object sender, RoutedEventArgs e) { Override_State_Click(sender, e, passed); };
            Override_Close.Click += delegate (object sender, RoutedEventArgs e) { Override_State_Click(sender, e, passed); };
            passed.OnStateChanged += new Valve.StateChanged(Passed_OnStateChanged);
            }

        private void Passed_OnStateChanged(object sender, Valve.ValveEventArgs e)
        {
            if (e.passed.isOpen)
            {
                Override_Open.Background = Brushes.Green;
                Override_Close.Background = Brushes.Red;

            }
            else
            {
                Override_Close.Background = Brushes.Green;
                Override_Open.Background = Brushes.Red;
            }
        }

        private void Override_Click(object sender, RoutedEventArgs e, Valve passed)
        {
            if (passed.IsOverriden)
            {
                passed.IsOverriden = false;
                Override.Content = "OFF";
                Override.Background = Brushes.LightGray;
                Override_Open.Background = Brushes.LightGray;
                Override_Close.Background = Brushes.LightGray;
                Override_Open.Opacity = 0.50;
                Override_Close.Opacity = 0.50;
                
            }
            else
            {
                passed.IsOverriden = true;
                Override.Content = "ON";
                Override.Background = Brushes.Green;
                Override_Open.Opacity = 1.00;
                Override_Close.Opacity = 1.00;
                if (passed.isOpen)
                {
                    Override_Open.Background = Brushes.Green;
                    Override_Close.Background = Brushes.Red;
                }
                else
                {
                    Override_Close.Background = Brushes.Green;
                    Override_Open.Background = Brushes.Red;
                }
            }
        }
        private void Override_State_Click(object sender, RoutedEventArgs e, Valve passed)
        {
            if (passed.IsOverriden)
            {
                if (sender == Override_Open)
                {
                    passed.Open();
                    Override_Open.Background = Brushes.Green;
                    Override_Close.Background = Brushes.Red;

                }
                else if (sender == Override_Close)
                {
                    passed.Close();
                    Override_Close.Background = Brushes.Green;
                    Override_Open.Background = Brushes.Red;
                }

            }
        }
    }
}
