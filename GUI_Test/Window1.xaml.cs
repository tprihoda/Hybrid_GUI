using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
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

namespace GUI
{
    /// <summary>
    /// Interaction logic for Window1.xaml
    /// </summary>
    
    public partial class Window1 : Window
    {
        MainWindowVM Pressure_Graph_Object;
        MainWindowVM Temperature_Graph_Object;


        public Window1(MainWindow passed)
        { 
            InitializeComponent();
            
            webBrowser.Navigate("https://forecast.weather.gov/MapClick.php?lat=44.5645659&lon=-123.2620435#.XlaImKhKiUk");
            webBrowser.Navigated += new NavigatedEventHandler(wbMain_Navigated);
            Pressure_Graph_Object = new MainWindowVM();
            Temperature_Graph_Object = new MainWindowVM();
            
            Pressure_Graphs.DataContext = Pressure_Graph_Object;
            Temperature_Graphs.DataContext = Temperature_Graph_Object;
            MainWindow.P1.OnDataChanged += new DataObject.DataChanged(Pressure_Graph_Object.OnDataChanged_Handler);
            MainWindow.T1.OnDataChanged += new DataObject.DataChanged(Temperature_Graph_Object.OnDataChanged_Handler);


            Console.KeyDown += new KeyEventHandler((sender, e) => ConsoleKeyPressed(sender, e));
            Pressure_Graphs.KeyDown += new KeyEventHandler((sender, e) => KeyPressed(sender, e));
            Temperature_Graphs.KeyDown += new KeyEventHandler((sender, e) => KeyPressed(sender, e));
        }

        private void ConsoleKeyPressed(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Return)
            {
                int command;
                string text = Console.Text;
                Console.Clear();
                if (text == "help")
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "1: Launch\n2: Ignitor On\n3: Ignitor Off\n4: Valve Open\n5: Valve Close\n6: Deluge On\n 7: Deluge OFF";
                    textBlock.FontSize = 20;
                    MessagePanel.Children.Add(textBlock);
                    return;
                }

                try { command = int.Parse(text); }
                catch
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "ERROR: Invalid Command";
                    textBlock.FontSize = 20;
                    MessagePanel.Children.Add(textBlock);
                    return;

                }
                if(MainWindow.client.connected == true)
                {
                    MainWindow.client.sendMessage(command);
                }
                else
                {
                    TextBlock textBlock = new TextBlock();
                    textBlock.Text = "ERROR: No Connection to Server";
                    textBlock.FontSize = 20;
                    MessagePanel.Children.Add(textBlock);
                    return;
                }
               
            }
        }
        private void KeyPressed(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Return)
            {
                TraversalRequest request = new TraversalRequest( FocusNavigationDirection.Left);
                UIElement keyboardFocus = Keyboard.FocusedElement as UIElement;
                if (keyboardFocus != null)
                {
                    keyboardFocus.MoveFocus(request);
                }
            }
        }

        void wbMain_Navigated(object sender, NavigationEventArgs e)
        {
            SetSilent(webBrowser, true); // make it silent
        }
        public static void SetSilent(WebBrowser browser, bool silent)
        {
            if (browser == null)
                throw new ArgumentNullException("browser");

            // get an IWebBrowser2 from the document
            IOleServiceProvider sp = browser.Document as IOleServiceProvider;
            if (sp != null)
            {
                Guid IID_IWebBrowserApp = new Guid("0002DF05-0000-0000-C000-000000000046");
                Guid IID_IWebBrowser2 = new Guid("D30C1661-CDAF-11d0-8A3E-00C04FC9E26E");

                object webBrowser;
                sp.QueryService(ref IID_IWebBrowserApp, ref IID_IWebBrowser2, out webBrowser);
                if (webBrowser != null)
                {
                    webBrowser.GetType().InvokeMember("Silent", BindingFlags.Instance | BindingFlags.Public | BindingFlags.PutDispProperty, null, webBrowser, new object[] { silent });
                }
            }
        }
        [ComImport, Guid("6D5140C1-7436-11CE-8034-00AA006009FA"), InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
        private interface IOleServiceProvider
        {
            [PreserveSig]
            int QueryService([In] ref Guid guidService, [In] ref Guid riid, [MarshalAs(UnmanagedType.IDispatch)] out object ppvObject);
        }

        

    }
    
}
