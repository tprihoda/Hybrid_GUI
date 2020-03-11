using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;

namespace GUI
{
    public class DataEventArgs : EventArgs
    {
        public double passed;
    }
    public class DataObject
    {
        public delegate void DataChanged(object sender, DataEventArgs e);
        public event DataChanged OnDataChanged;

        public delegate void DataCaution(object sender, DataEventArgs e);
        public event DataCaution OnDataCaution;

        public delegate void DataWarning(object sender, DataEventArgs e);
        public event DataWarning OnDataWarning;

        public string Name;

        private bool Caution_Raised = false;
        private bool Warning_Raised = false;
        public int Caution_level;
        public int Warning_level;
        public TextBlock textBlock = null;
        private double local_Value;
        public double Value
        {
            get { return local_Value; }
            set
            {
                local_Value = value;
                DataEventArgs e = new DataEventArgs();
                e.passed = local_Value;
                if (textBlock != null)
                {
                    textBlock.Text = Name + ": " + value.ToString();
                }
                RaiseDataChange(e);
                if (value >= Warning_level & !Warning_Raised)
                {
                    e.passed = 1;
                    RaiseDataWarning(e);
                    Warning_Raised = true;
                    if (textBlock != null)
                    {
                        textBlock.Background = Brushes.Red;
                    }
                }
                else
                {
                    if (value < Warning_level & Warning_Raised)
                    {
                        e.passed = -1;
                        RaiseDataWarning(e);
                        Warning_Raised = false;
                        {
                            textBlock.Background = Brushes.Transparent;
                        }
                    }
                    if (value >= Caution_level & !Caution_Raised)
                    {
                        e.passed = 1;
                        RaiseDataCaution(e);
                        Caution_Raised = true;
                        {
                            textBlock.Background = Brushes.Yellow;
                        }
                    }
                    else if (value < Caution_level & Caution_Raised)
                    {
                        e.passed = -1;
                        RaiseDataCaution(e);
                        Caution_Raised = false;
                        {
                            textBlock.Background = Brushes.Transparent;
                        }
                    }
                }
            }
        }

        public DataObject(string name, int caution_level, int warning_level)
        {
            Name = name;
            Caution_level = caution_level;
            Warning_level = warning_level;
        }

        public void OnDataCaution_Handler(object sender, DataEventArgs e)
        {
            
        }

        public void OnDataWarning_Handler(object sender, DataEventArgs e)
        {
            
        }

        protected virtual void RaiseDataChange(DataEventArgs e)
        {
            if (OnDataChanged != null) { OnDataChanged(this, e); }
        }

        protected virtual void RaiseDataCaution(DataEventArgs e)
        {
            if (OnDataCaution != null) { OnDataCaution(this, e); }
        }

        protected virtual void RaiseDataWarning(DataEventArgs e)
        {
            if (OnDataWarning != null) { OnDataWarning(this, e); }
        }
    }
}
