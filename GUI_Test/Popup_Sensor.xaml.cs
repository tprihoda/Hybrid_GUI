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
    public partial class Popup_Sensor : Window
    {
        public Popup_Sensor(DataObject passed, string description)
        {
            InitializeComponent();
            this.DataContext = passed;
            Name.Text = passed.Name;
            Description.Text = description;
            Description.TextWrapping = TextWrapping.Wrap;
            Caution_Limit.Text = passed.Caution_level.ToString();
            Warning_Limit.Text = passed.Warning_level.ToString();
            Value.Text = passed.Value.ToString();
            passed.OnDataChanged += Passed_OnDataChanged;
        }
           

        private void Passed_OnDataChanged(object sender, DataEventArgs e)
        {
            Value.Text = e.passed.ToString();
        }
    }
}
