using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Windows;
using System.Windows.Threading;
using Newtonsoft.Json;


namespace GUI
{
    public class TCPSocket
    {

        //public variables
        public bool connected;
        public bool delay;
        public struct obj
        {
            public string Name;
            public dynamic Value;
            public obj(string arg1, dynamic arg2)
            {
                Name = arg1;
                Value = arg2;
            }
        };
        //private variables
        private Stopwatch stopWatch = new Stopwatch();
        private DispatcherTimer timer = new DispatcherTimer();
        private string Address;
        private IPAddress ip;
        private Int32 Port;
        private TcpClient client;
        private NetworkStream stream;

        //public functions

        public TCPSocket(string address, Int32 port)
        {
            Address = address;
            ip = IPAddress.Parse(Address);
            Port = port;
        }
        public void init()
        {
            if (connected == false)
            {
                try { client = new TcpClient(Address, Port); }
                catch (SocketException)
                {
                    MessageBox.Show("Connection Failed", "ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                stream = client.GetStream();
                connected = true;
                delay = true;
                timer.Interval = TimeSpan.FromSeconds(.10);
                timer.Tick += timer_Tick;
                timer.Start();
            }

        }
        
        public void sendMessage(int command)
        {
            if (connected == true)
            {
                obj JsonObj = new obj("CMD", command);
                string JasonText = JsonConvert.SerializeObject(JsonObj);
                Byte[] data = System.Text.Encoding.UTF8.GetBytes(JasonText);
                //Byte[] data = System.Text.Encoding.UTF8.GetBytes(command.ToString());
                try { stream.Write(data, 0, data.Length); }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Connection Failed", "Send ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    connected = false;
                }
                if (command == 128) { connected = false; }
            }
        }
        public void recvMessage()
        {
            if (connected == true)
            {
                Byte[] buffer = new Byte[1024];
                try { stream.Read(buffer, 0, buffer.Length); }
                catch (System.IO.IOException)
                {
                    MessageBox.Show("Connection Failed", "Recieve ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
                    connected = false;
                    return;
                }
                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, buffer.Length);
                string[] messages = message.Split(';');
                for (int index = 0; index < messages.Length; index++)
                {
                    IList<obj> ObjList = new List<obj>();
                    ObjList = JsonConvert.DeserializeObject<List<obj>>(messages[index]);
                    Paser(ObjList);
                }
            }

        }
        private void Paser(IList<obj> ObjList)
        {
            //placeholder till I do this :)
            if (ObjList != null)
            {
                for (int i = 0; i < ObjList.Count; i++)
                {
                    switch (ObjList[i].Name)
                    {
                        case "P1":
                            MainWindow.P1.Value = ObjList[i].Value;
                            break;
                        case "P2":
                            MainWindow.P2.Value = ObjList[i].Value;
                            break;
                        case "P3":
                            MainWindow.P3.Value = ObjList[i].Value;
                            break;
                        case "P4":
                            MainWindow.P4.Value = ObjList[i].Value;
                            break;
                        case "T1":
                            MainWindow.T1.Value = ObjList[i].Value;
                            break;
                        case "T2":
                            MainWindow.T2.Value = ObjList[i].Value;
                            break;
                        case "T3":
                            MainWindow.T3.Value = ObjList[i].Value;
                            break;
                        case "T4":
                            MainWindow.T4.Value = ObjList[i].Value;
                            break;
                        case "T5":
                            MainWindow.T5.Value = ObjList[i].Value;
                            break;
                        case "T6":
                            MainWindow.T6.Value = ObjList[i].Value;
                            break;
                    }
                }
            }
        }
        private void timer_Tick(object sender, EventArgs e)
        {
            if (connected == true)
            {
                sendMessage(0);
                recvMessage();
            }
            
            //MessageBox.Show("Connection Failed", "Recieve ERROR", MessageBoxButton.OK, MessageBoxImage.Error);
        }

        //private variables
    }
}
