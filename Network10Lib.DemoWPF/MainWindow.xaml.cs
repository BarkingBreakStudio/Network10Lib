using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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

namespace Network10Lib.DemoWPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        TcpConnectionN10? connection;

        public MainWindow()
        {
            InitializeComponent();

            cmd_SendMessage.IsEnabled = false;
            cmd_CloseServer.IsEnabled = false;
            cmd_CloseClient.IsEnabled = false;
            refreshStatus();
        }

        /**** UI Events ********************************************************************************************************/

        private async void cmd_OpenServer_Click(object sender, RoutedEventArgs e)
        {
            if (connection is null && IPAddress.TryParse(txt_ServerIpAdr.Text, out IPAddress? Ipadr) && Ipadr is not null && int.TryParse(txt_ServerPort.Text, out int port))
            {
                cmd_OpenServer.IsEnabled = false;
                cmd_OpenClient.IsEnabled = false;
                connection = new TcpConnectionN10 { ServerConnectionId = txt_ServerIdentifier.Text, ClientConnectionId = txt_ClientIdentifier.Text };
                connection.PlayerConnected += Connection_PlayerConnected;
                connection.PlayerDisonnected += Connection_PlayerDisonnected;
                connection.MessageReceived += Connection_MessageReceived;
                connection.Connected += refreshStatus;
                connection.Disonnected += refreshStatus;
                try
                {
                    await connection.OpenServer(port, Ipadr);
                    cmd_CloseServer.IsEnabled = true;
                    cmd_SendMessage.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    cmd_CloseClient_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Invalid IpAdress or port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void cmd_CloseServer_Click(object sender, RoutedEventArgs e)
        {
            if (connection is not null)
            {
                cmd_CloseServer.IsEnabled = false;
                await connection.Close();
                connection.PlayerConnected -= Connection_PlayerConnected;
                connection.PlayerDisonnected -= Connection_PlayerDisonnected;
                connection.MessageReceived -= Connection_MessageReceived;
                connection.Connected -= refreshStatus;
                connection.Disonnected -= refreshStatus;
                connection = null;
                cmd_OpenServer.IsEnabled = true;
                cmd_OpenClient.IsEnabled = true;
                cmd_SendMessage.IsEnabled = false;
            }
        }

        private async void cmd_OpenClient_Click(object sender, RoutedEventArgs e)
        {
            if (connection is null && IPAddress.TryParse(txt_ClientIpAdr.Text, out IPAddress? Ipadr) && Ipadr is not null && int.TryParse(txt_ClientPort.Text, out int port))
            {
                cmd_OpenServer.IsEnabled = false;
                cmd_OpenClient.IsEnabled = false;
                connection = new TcpConnectionN10 { ServerConnectionId = txt_ServerIdentifier.Text, ClientConnectionId = txt_ClientIdentifier.Text };
                connection.MessageReceived += Connection_MessageReceived;
                connection.Connected += refreshStatus;
                connection.Disonnected += refreshStatus;
                try
                {
                    await connection.OpenClient(port, Ipadr);
                    cmd_CloseClient.IsEnabled = true;
                    cmd_SendMessage.IsEnabled = true;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    cmd_CloseServer_Click(sender, e);
                }
            }
            else
            {
                MessageBox.Show("Invalid IpAdress or port.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async void cmd_CloseClient_Click(object sender, RoutedEventArgs e)
        {
            if (connection is not null)
            {
                cmd_CloseClient.IsEnabled = false;
                await connection.Close();
                connection.MessageReceived -= Connection_MessageReceived;
                connection.Connected -= refreshStatus;
                connection.Disonnected -= refreshStatus;
                connection = null;
                cmd_OpenServer.IsEnabled = true;
                cmd_OpenClient.IsEnabled = true;
                cmd_SendMessage.IsEnabled = false;
            }
        }

        private void cmd_sendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (int.TryParse(txt_sendMessageDestination.Text, out int destination))
            {
                connection?.SendObject(txt_sendMessageData.Text, destination);
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (connection is not null)
            {
                if (connection.IsServer)
                {
                    cmd_CloseServer_Click(sender, new RoutedEventArgs());
                }
                else
                {
                    cmd_CloseClient_Click(sender, new RoutedEventArgs());
                }
            }
        }

        private void cmd_CloseWindow_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void cmd_OpenNewWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow newWindow = new MainWindow();
            newWindow.Title = "Additional Window";
            newWindow.Show();
        }

        /**** Callbacks ********************************************************************************************************/

        private void refreshStatus()
        {
            if (!Dispatcher.CheckAccess()) { Dispatcher.Invoke(() => refreshStatus()); return; }

            lbl_Status.Text = $"Connecor: {connection?.ToString() + Environment.NewLine}Connected: { connection?.IsConnected + Environment.NewLine }IsServer: {connection?.IsServer } ";
        }

        private void Connection_PlayerDisonnected(int playerNr)
        {
            if (!Dispatcher.CheckAccess()) { Dispatcher.Invoke(() => Connection_PlayerDisonnected(playerNr)); return; }

            listV_Players.Items.Remove(playerNr);
        }

        private void Connection_PlayerConnected(int playerNr)
        {
            if (!Dispatcher.CheckAccess()) { Dispatcher.Invoke(() => Connection_PlayerConnected(playerNr)); return; }

            listV_Players.Items.Add(playerNr);
        }

        private void Connection_MessageReceived(MessageN10 msg)
        {
            if (!Dispatcher.CheckAccess()) { Dispatcher.Invoke(() => Connection_MessageReceived(msg)); return; }

            txt_MsgReveive.AppendText(msg.ToString() + Environment.NewLine);
            txt_MsgReveive.ScrollToEnd();
        }
    }
}
