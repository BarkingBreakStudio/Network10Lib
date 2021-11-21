using Network10Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network10Lib.DemoWinForm
{
    public partial class TcpConnectionForm : Form
    {
        TcpConnectionAsync? connection;

        public TcpConnectionForm()
        {
            InitializeComponent();
        }

        private void TcpConnectionForm_Load(object sender, EventArgs e)
        {
            refreshStatus();
        }

        private async void cmd_OpenServer_ClickAsync(object sender, EventArgs e)
        {
            if(connection is null && IPAddress.TryParse(txt_ServerIpAdr.Text, out IPAddress? Ipadr) && Ipadr is not null && int.TryParse(txt_ServerPort.Text, out int port))
            {
                cmd_openServer.Enabled = false;
                cmd_openClient.Enabled = false;
                connection = new TcpConnectionAsync { ServerConnectionId = txt_ServerIdentifier.Text, ClientConnectionId = txt_ClientIdentifier.Text };
                connection.PlayerConnected += Connection_PlayerConnected;
                connection.PlayerDisonnected += Connection_PlayerDisonnected;
                connection.MessageReceived += Connection_MessageReceived;
                connection.Connected += refreshStatus;
                connection.Disonnected += refreshStatus;
                await connection.OpenServer(Ipadr, port);
                cmd_CloseServer.Enabled = true;
                cms_sendMessage.Enabled = true;
            }
            else
            {
                MessageBox.Show("Invalid IpAdress or port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private async void cmd_CloseServer_ClickAsync(object sender, EventArgs e)
        {
            if(connection is not null)
            {
                cmd_CloseServer.Enabled = false;
                await connection.Close();
                connection.PlayerConnected -= Connection_PlayerConnected;
                connection.PlayerDisonnected -= Connection_PlayerDisonnected;
                connection.MessageReceived -= Connection_MessageReceived;
                connection.Connected -= refreshStatus;
                connection.Disonnected -= refreshStatus;
                connection = null;
                cmd_openServer.Enabled = true;
                cmd_openClient.Enabled = true;
                cms_sendMessage.Enabled = false;
            }
        }

        private async void cmd_openClient_Click(object sender, EventArgs e)
        {
            if (connection is null && IPAddress.TryParse(txt_ClientIpAdr.Text, out IPAddress? Ipadr) && Ipadr is not null && int.TryParse(txt_ClientPort.Text, out int port))
            {
                cmd_openServer.Enabled = false;
                cmd_openClient.Enabled = false;
                connection = new TcpConnectionAsync { ServerConnectionId = txt_ServerIdentifier.Text, ClientConnectionId = txt_ClientIdentifier.Text };
                connection.PlayerConnected += Connection_PlayerConnected;
                connection.PlayerDisonnected += Connection_PlayerDisonnected;
                connection.MessageReceived += Connection_MessageReceived;
                connection.Connected += refreshStatus;
                connection.Disonnected += refreshStatus;
                await connection.OpenClient(Ipadr, port);
                cmd_closeClient.Enabled = true;
                cms_sendMessage.Enabled = true;
            }
            else
            {
                MessageBox.Show("Invalid IpAdress or port.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void cmd_closeClient_Click(object sender, EventArgs e)
        {
            if (connection is not null)
            {
                cmd_closeClient.Enabled = false;
                await connection.Close();
                connection.PlayerConnected -= Connection_PlayerConnected;
                connection.PlayerDisonnected -= Connection_PlayerDisonnected;
                connection.MessageReceived -= Connection_MessageReceived;
                connection.Connected -= refreshStatus;
                connection.Disonnected -= refreshStatus;
                connection = null;
                cmd_openServer.Enabled = true;
                cmd_openClient.Enabled = true;
                cms_sendMessage.Enabled = false;
            }
        }

        private void cms_sendMessage_Click(object sender, EventArgs e)
        {
            if(int.TryParse(txt_sendMessageDestination.Text, out int destination))
            {
                connection?.SendObject(txt_snedMessageData.Text, destination);
            }
        }

        private void refreshStatus()
        {
            if (InvokeRequired) { Invoke(() => refreshStatus()); return; }

            lblStatus.Text = $"Connecor: {connection?.ToString() + Environment.NewLine}Connected: { connection?.IsConnected + Environment.NewLine }IsServer: {connection?.IsServer + Environment.NewLine } ";
        }


        //Connection events:
        private void Connection_MessageReceived(TcpConnectionAsync.Message msg)
        {
            if (InvokeRequired) { Invoke(() => Connection_MessageReceived(msg)); return; }

            txt_MsgReveive.AppendText(msg.ToString() + Environment.NewLine);
        }


        private void Connection_PlayerConnected(int playerNr)
        {
            if (InvokeRequired) { Invoke(() => Connection_PlayerConnected(playerNr)); return; }

            listBox1.Items.Add(playerNr);
        }

        private void Connection_PlayerDisonnected(int playerNr)
        {
            if (InvokeRequired) { Invoke(() => Connection_PlayerDisonnected(playerNr)); return; }

            listBox1.Items.Remove(playerNr);
        }

       
    }
}
