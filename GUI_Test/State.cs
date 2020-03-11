using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace GUI
{
    class State
    {
        //what to call this? class resources?
        public enum Name { Fill_Tank, Disconnect_Fill_Arm, Close_Fill_Door, Launch, Approach_Config, Break, Idle };

        //public variables
        public Name Current
        {
            get { return current_vavlue; }
            set {
                  current_vavlue = value;
                  //raise state change event
                }
        }
        internal class textTemplate
        {
            public TextBlock Block = new TextBlock();
            public textTemplate(string text, double font_size )
            {
                Block.FontSize = font_size;
                Block.Text = text;
                Block.TextWrapping = TextWrapping.Wrap;
                Block.Background = System.Windows.Media.Brushes.White;
                Block.HorizontalAlignment = HorizontalAlignment.Center;
                Block.Width = 220;
                Block.TextAlignment = TextAlignment.Center;
                Block.VerticalAlignment = VerticalAlignment.Center;
                Block.Margin = new Thickness(10, 10, 10, 10);
                Grid.SetColumn(Block, 0);
            }

        }
        internal class checkboxTemplate
        {
            public CheckBox checkbox = new CheckBox();
            public checkboxTemplate()
            {
                checkbox.IsEnabled = false;
                checkbox.HorizontalAlignment = HorizontalAlignment.Right;
                checkbox.VerticalAlignment = VerticalAlignment.Center;
                checkbox.Margin = new Thickness(10, 10, 10, 10);
                Grid.SetColumn(checkbox, 1);
            }

        }
        internal class stackPanelTemplate
        {
            public DockPanel stackpanel = new DockPanel();
            public stackPanelTemplate(textTemplate text, checkboxTemplate checkbox, double width)
            {
                stackpanel.Children.Add(text.Block);
                stackpanel.Children.Add(checkbox.checkbox);
                stackpanel.Width = width-20;

            }

        }
        internal class gridTemplate
        {
            public Grid grid = new Grid();
            public gridTemplate(textTemplate text, checkboxTemplate checkbox, double width)
            {
                ColumnDefinition textCol = new ColumnDefinition();
                textCol.Width = new GridLength(240);
                ColumnDefinition checkCol = new ColumnDefinition();
                checkCol.Width = new GridLength(30);
                grid.HorizontalAlignment = HorizontalAlignment.Center;
                grid.ColumnDefinitions.Add(textCol);
                grid.ColumnDefinitions.Add(checkCol);
                grid.Children.Add(text.Block);
                grid.Children.Add(checkbox.checkbox);
                grid.Width = width;

            }

        }

        public textTemplate[] text;
        public checkboxTemplate[] checkbox;
        public stackPanelTemplate[] stackPanels;
        public gridTemplate[] grids;

        public int text_count;

        public State(Name name, string[] passed, double font_size, double width)
        {
            text_count = passed.Length;
            text = new textTemplate[text_count];
            checkbox = new checkboxTemplate[text_count];
            stackPanels = new stackPanelTemplate[text_count];
            grids = new gridTemplate[text_count];

            for (int i = 0; i < text_count; i++)
            {
                text[i] = new textTemplate(passed[i], font_size);
                checkbox[i] = new checkboxTemplate();
                stackPanels[i] = new stackPanelTemplate(text[i], checkbox[i], width);
                //grids[i] = new gridTemplate(text[i], checkbox[i], width);
            }

        }


        //private variables
        private Name current_vavlue = Name.Idle;

    }
}
