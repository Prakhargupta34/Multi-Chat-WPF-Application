using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static TextBox disp;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            disp = display;
            if (ipaddr.Text.Equals(String.Empty) || portNumber.Text.Equals(String.Empty) || clientName.Text.Equals(String.Empty))
                disp.Text = "Please fill all the fields...";
            else
            {
                ClientSocketApplication.RunClient(ipaddr.Text, portNumber.Text, clientName.Text);
                Connect.IsEnabled = false;
            }
            //ipaddr.Visibility = Visibility.Hidden;
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            ClientSocketApplication.Send(Message.Text);
            disp.Text += "\n" + Message.Text;
            Message.Text = String.Empty;
        }

        private void TextBox_TextChanged_1(object sender, TextChangedEventArgs e)
        {

        }
    }
}
