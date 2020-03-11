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
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using System.Diagnostics;
using System.Speech.Synthesis;
using System.Windows.Forms;
using System.IO;

namespace GUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string symbol_path = System.IO.Path.Combine(Environment.CurrentDirectory, @"P_and_ID_Symbols\");
        
        SpeechSynthesizer synth = new SpeechSynthesizer();
        static public TCPSocket client = new TCPSocket("10.0.0.10", 8080);
        
        System.IO.Ports.SerialPort sp = new System.IO.Ports.SerialPort("COM11", 115200, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
        enum Keystates { Ready, NotReady, Disconnected };
        Keystates Keystatus = Keystates.Disconnected;


        MainWindowVM Graph_Object = new MainWindowVM();

        public static DataObject Altitude = new DataObject("Altitude", 800000, 800000);
        public static DataObject P1 = new DataObject("P1", 100, 200);
        public static DataObject P2 = new DataObject("P2", 100, 200);
        public static DataObject P3 = new DataObject("P3", 100, 200);
        public static DataObject P4 = new DataObject("P4", 100, 200);

        public static DataObject T1 = new DataObject("T1", 200, 300);
        public static DataObject T2 = new DataObject("T2", 200, 300);
        public static DataObject T3 = new DataObject("T3", 200, 300);
        public static DataObject T4 = new DataObject("T4", 200, 300);
        public static DataObject T5 = new DataObject("T5", 200, 300);
        public static DataObject T6 = new DataObject("T6", 200, 300);

        public MainWindow()
        {
            EnsureBrowserEmulationEnabled();
            string fileName = "log.txt";
            string path = System.IO.Path.Combine(Environment.CurrentDirectory, @"Logs\", fileName);
            System.IO.StreamWriter file = new System.IO.StreamWriter(path);
            DataContext = this;
            RotateTransform rotate = new RotateTransform(90);
            InitializeComponent();

            Rocket_Image.Source = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Rocket_flipped_propane.png"));
            Glow_Plug.Source = new BitmapImage(new Uri(MainWindow.symbol_path + @"\Glow_Plug\Glow_Plug_Off.png"));
            try
            {
                sp.Open();
            }
            catch (System.IO.IOException)
            {
                System.Windows.MessageBox.Show("Launch Keys Disconnected", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
            }
            //ensures that secondWindow is owned by MainWindow
            Window1 secondWindow = new Window1(this);
            SourceInitialized += (s, a) =>
            {
                secondWindow.Owner = this;
            };
            secondWindow.Show();
            var secondaryScreen = Screen.AllScreens.Where(s => !s.Primary).FirstOrDefault();

            if (secondaryScreen != null)
            {
                var workingArea = secondaryScreen.WorkingArea;
                secondWindow.Left = workingArea.Left;
                secondWindow.Top = workingArea.Top;
                secondWindow.Width = workingArea.Width;
                secondWindow.Height = workingArea.Height;

                if (IsLoaded)
                {
                    secondWindow.WindowState = WindowState.Maximized;
                }
            }



            Graph.DataContext = Graph_Object;
            Altitude.OnDataChanged += new DataObject.DataChanged(Graph_Object.OnDataChanged_Handler);
            

            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(.05);
            timer.Tick += timer_Tick;
            timer.Start();

            synth.SetOutputToDefaultAudioDevice();
            client.connected = false;

            initObjects();
            initDataObjects();

        }

        void initDataObjects()
        {

        }

        void timer_Tick(object sender, EventArgs e)
        {
            //Pressure.Value += 1;
            //Temperature.Value -= 1;
            Altitude.Value += 1;
            if (Warning_count > 0 && !Warning_Acknowledged)
            {
                if (synth.State == SynthesizerState.Ready)
                {
                    synth.SpeakAsync("Warning");
                }
            }
            else if (Caution_count > 0 && !Caution_Acknowledged)
            {
                if (synth.State == SynthesizerState.Ready)
                {
                    synth.SpeakAsync("Caution");
                }
            }
        }

        void Fill_Command_Button_Click(object sender, RoutedEventArgs e, State passed)
        {
            MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you wish to Begin this Seqeuence?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Sequence_Stack.Children.Clear();
                for (int i = 0; i < passed.text_count; i++)
                {
                    Sequence_Stack.Children.Add(passed.stackPanels[i].stackpanel);
                    //Sequence_Stack.Children.Add(passed.grids[i].grid);
                }
            }
        }


        void Valve_Click(object sender, RoutedEventArgs e, Valve passed)
        {
            Popup_Valve newWindow = new Popup_Valve(passed, passed.Description);
            newWindow.Owner = this;
            newWindow.Top = PointToScreen(Mouse.GetPosition(this)).Y - 150;
            newWindow.Left = PointToScreen(Mouse.GetPosition(this)).X - 100;
           
            newWindow.Show();
        }
        void Glow_Click(object sender, RoutedEventArgs e, Valve passed)
        {
            Popup_Ignitor newWindow = new Popup_Ignitor(passed, passed.Description);
            newWindow.Owner = this;
            newWindow.Top = PointToScreen(Mouse.GetPosition(this)).Y - 150;
            newWindow.Left = PointToScreen(Mouse.GetPosition(this)).X - 100;

            newWindow.Show();
        }
        void Sensor_Click(object sender, RoutedEventArgs e, DataObject passed)
        {
            Popup_Sensor newWindow = new Popup_Sensor(passed, "PlaceHolder");
            newWindow.Owner = this;
            newWindow.Top = PointToScreen(Mouse.GetPosition(this)).Y - 150;
            newWindow.Left = PointToScreen(Mouse.GetPosition(this)).X - 100;

            newWindow.Show();
        }

        void Connect_Button_Click(object sender, RoutedEventArgs e)
        {
            if (client.connected == false)
            {
                MessageBoxResult result = System.Windows.MessageBox.Show("Are you sure you wish to Connect?", "Confirm", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    client.init();
                    if (client.connected == true)
                    {
                        client.sendMessage(0);
                        System.Windows.MessageBox.Show("Connection Established", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    }
                }
            }
            else
            {
                System.Windows.MessageBox.Show("Connection Already Established", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }
        private bool Caution_Acknowledged = false;
        void Caution_Button_Click(object sender, RoutedEventArgs e)
        {
            Caution_Acknowledged = true;
        }
        private bool Warning_Acknowledged = false;
        void Warning_Button_Click(object sender, RoutedEventArgs e)
        {
            Warning_Acknowledged = true;
        }
        

        void initObjects()
        {
            Valve Fill = new Valve(Fill_Valve, "Fill Valve", false, "Valve used to control the fill process");
            Valve Tank = new Valve(Tank_Vent, "Tank Vent Valve", true, "Valve used to vent the Tank");
            Valve Propane = new Valve(Propane_Valve, "Propane Valve", true, "Valve used to control propane flow");
            Valve Fill_Vent = new Valve(Fill_Vent_Valve, "Fill Vent Valve", true, "Valve used to vent the fill line");
            Valve Servo_valve = new Valve(Servo_element, "Servo Valve", true, "Valve used to control nitrous into the motor");
            Fill_Arm_Contact fill_Arm_Contact = new Fill_Arm_Contact(Contact, "Fill Arm Contactor", "Contactor used to contect fill arm to the rocket");
            Tank_Level tank_Level = new Tank_Level(Tank_Level_Image);

            Fill.image.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Valve_Click(sender, e, Fill));
            Tank.image.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Valve_Click(sender, e, Tank));
            Propane.image.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Valve_Click(sender, e, Propane));
            Fill_Vent.image.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Valve_Click(sender, e, Fill_Vent));
            Servo_valve.image.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Valve_Click(sender, e, Servo_valve));
            //Glow_Plug.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Ignitor_Click(sender, e, Servo_valve));

            P1_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, P1));
            P1.textBlock = P1_Display;
            P1.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            P1.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            P2_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, P2));
            P2.textBlock = P2_Display;
            P2.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            P2.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            P3_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, P3));
            P3.textBlock = P3_Display;
            P3.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            P3.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            P4_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, P4));
            P4.textBlock = P4_Display;
            P4.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            P4.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T1_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T1));
            T1.textBlock = T1_Display;
            T1.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T1.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T2_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T2));
            T2.textBlock = T2_Display;
            T2.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T2.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T3_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T3));
            T3.textBlock = T3_Display;
            T3.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T3.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T4_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T4));
            T4.textBlock = T4_Display;
            T4.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T4.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T5_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T5));
            T5.textBlock = T5_Display;
            T5.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T5.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);
            T6_Display.MouseLeftButtonDown += new MouseButtonEventHandler((sender, e) => Sensor_Click(sender, e, T6));
            T6.textBlock = T6_Display;
            T6.OnDataCaution += new DataObject.DataCaution(OnDataCaution_Handler);
            T6.OnDataWarning += new DataObject.DataWarning(OnDataWarning_Handler);

            string[] Idle_state_text = {};
            State Idle_state = new State(State.Name.Idle, Idle_state_text, 30, Sequence_Stack.Width);
            string[] Fill_state_text = { "This text should wrap", "Lets see how this works", "State 3", "State 4" };
            State Fill_state = new State(State.Name.Fill_Tank, Fill_state_text, 30, Sequence_Stack.Width);
            string[] Fill_Disconnect_state_text = { "State 1", "State 2", "State 3", "State 4" };
            State Fill_Disconnect_State = new State(State.Name.Disconnect_Fill_Arm, Fill_Disconnect_state_text, 30, Sequence_Stack.Width);
            string[] Door_Close_state_text = { "State 1", "State 2", "State 3", "State 4" };
            State Door_Close_state = new State(State.Name.Close_Fill_Door, Door_Close_state_text, 30, Sequence_Stack.Width);
            string[] Launch_state_text = { "Burn Wire Check", "Ignition", "Open Valve", "Take Off" };
            State Launch_state = new State(State.Name.Launch, Launch_state_text, 30, Sequence_Stack.Width);

            Caution_Button.Click += delegate (object sender, RoutedEventArgs e) { Caution_Button_Click(sender, e); };
            Warning_Button.Click += delegate (object sender, RoutedEventArgs e) { Warning_Button_Click(sender, e); };
            Fill_Button.Click += delegate (object sender, RoutedEventArgs e) { Fill_Command_Button_Click(sender, e, Fill_state); };
            Fill_Disconnect_Button.Click += delegate (object sender, RoutedEventArgs e) { Fill_Command_Button_Click(sender, e, Fill_Disconnect_State); };
            Door_Button.Click += delegate (object sender, RoutedEventArgs e) { Fill_Command_Button_Click(sender, e, Door_Close_state); };
            Launch_Button.Click += delegate (object sender, RoutedEventArgs e) { Fill_Command_Button_Click(sender, e, Launch_state); };
            
            Pad_Connect_Button.Click += delegate (object sender, RoutedEventArgs e) { Connect_Button_Click(sender, e); };

        }

        private double Caution_count = 0;
        public void OnDataCaution_Handler(object sender, DataEventArgs e)
        {
            Caution_count += e.passed;
            if (Caution_count > 0)
            {
                Caution_Button.Background = Brushes.Yellow;
                synth.SpeakAsync("Caution");
                Caution_Acknowledged = false;
            }

            else
            {
                Caution_Button.Background = Brushes.LightGray;
            }


        }
        private double Warning_count = 0;
        public void OnDataWarning_Handler(object sender, DataEventArgs e)
        {
            Warning_count += e.passed;
            if (Warning_count > 0)
            {
                Warning_Button.Background = Brushes.Red;
                synth.SpeakAsync("Warning");
                Warning_Acknowledged = false;
            }
            else
            {
                Warning_Button.Background = Brushes.LightGray;
            }
            
        }

        public static void EnsureBrowserEmulationEnabled(string exename = "GUI.exe", bool uninstall = false)
        {

            try
            {
                using (
                    var rk = Registry.CurrentUser.OpenSubKey(
                            @"SOFTWARE\Microsoft\Internet Explorer\Main\FeatureControl\FEATURE_BROWSER_EMULATION", true)
                )
                {
                    if (!uninstall)
                    {
                        dynamic value = rk.GetValue(exename);
                        if (value == null)
                            rk.SetValue(exename, (uint)11001, RegistryValueKind.DWord);
                    }
                    else
                        rk.DeleteValue(exename);
                }
            }
            catch
            {
            }
        }
    }

}
