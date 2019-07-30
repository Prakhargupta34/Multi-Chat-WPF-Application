using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Client
{
    public class ClientSocketApplication
    {
        static Socket clientSocket;
        const int MESSAGESIZE = 1024 * 1024;
        public static void RunClient(string strIPAddr, string strPortNumber, String clientName)
        {
            clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            IPAddress ipAddr = null;

            try
            {
                if (!IPAddress.TryParse(strIPAddr, out ipAddr))
                {
                    MainWindow.disp.Text = "IP Address is not valid...Please restart the application";
                    return;
                }
                int portNumber = 0;

                if (!int.TryParse(strPortNumber.Trim(), out portNumber))
                {
                    MainWindow.disp.Text = "Port Number is not valid...Please restart the application";
                    return;
                }
                if (portNumber <= 0 && portNumber > 65535)
                {
                    MainWindow.disp.Text = "Port Number is not valid, should be between 0 & 65535...Please restart the application";
                    return;
                }

                clientSocket.Connect(ipAddr, portNumber);

                MainWindow.disp.Text = "Connected to the server...";

                Byte[] buffName = Encoding.ASCII.GetBytes(clientName);

                clientSocket.Send(buffName);

                MainWindow.disp.Text += "\nType message to send\n";

                Thread receiveThread = new Thread(new ThreadStart(Receive));
                receiveThread.IsBackground = true;
                receiveThread.Start();
            }
            catch (Exception ex)
            {
                MainWindow.disp.Text = ex.ToString()+ "\nPlease restart the application...";
                clientSocket.Close();
            }
        }
        public static void Send(string message)
        {
            try
            {
                    Byte[] buffSend = Encoding.ASCII.GetBytes(message);
                    clientSocket.Send(buffSend);
            }
            catch (Exception ex)
            {
                MainWindow.disp.Text = ex.ToString() + "\nPlease restart the application...";
                clientSocket.Close();
            }
        }
        static void Receive()
        {
            try
            {
                while (true)
                {
                    Byte[] buffReceive = new Byte[MESSAGESIZE];
                    int nRecv = clientSocket.Receive(buffReceive);
                    string data = Encoding.ASCII.GetString(buffReceive, 0, nRecv);
                    string clientName = data.Split('^')[1];
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow.disp.Text += $"\n{clientName} : {data.Split('^')[0]}";
                }, DispatcherPriority.ContextIdle);
                }
            }
            catch (Exception ex)
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    MainWindow.disp.Text = ex.ToString() + "\nPlease restart the application...";
                }, DispatcherPriority.ContextIdle);  
                clientSocket.Close();
            }
        }

    }
}
