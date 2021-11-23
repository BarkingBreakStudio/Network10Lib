using Network10Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network10Lib.DemoWinForm
{
    public partial class ServerForm : Form
    {

        TcpServerN10? tcpServerAsync;

        public ServerForm()
        {
            InitializeComponent();
        }

        private async void cmd_serverStart_Click(object sender, EventArgs e)
        {
            if (tcpServerAsync is null)
            {
                Log("Server starting ...");
                tcpServerAsync = new TcpServerN10();
                tcpServerAsync.StringReceived += TcpServerAsync_MessageReceived;
                tcpServerAsync.ClientConnected += TcpServerAsync_ClientConnected;
                tcpServerAsync.ClientDisconnected += TcpServerAsync_ClientDisconnected;
                await tcpServerAsync.Connect();
                Log("Server started");
                cmd_serverStart.Text = "Stop";
            }
            else
            {
                Log("Server stopping ...");
                tcpServerAsync.StringReceived -= TcpServerAsync_MessageReceived;
                tcpServerAsync.ClientConnected -= TcpServerAsync_ClientConnected;
                tcpServerAsync.ClientDisconnected -= TcpServerAsync_ClientDisconnected;
                await tcpServerAsync.Disconnect();
                tcpServerAsync = null;
                Log("Server stopped");
                cmd_serverStart.Text = "Start";
            }
        }

        private void TcpServerAsync_ClientDisconnected(TcpServerN10 sender, int clientNr)
        {
            Log($"Client disconnected: {clientNr}");
        }

        private void TcpServerAsync_ClientConnected(TcpServerN10 sender, int clientNr)
        {
            Log($"Client connected: {clientNr}");
        }

        private void TcpServerAsync_MessageReceived(TcpServerN10 sender, int clientNr, string message)
        {
            Log($"Client: {clientNr} sent: {message}");
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void cmd_StopServer_Click(object sender, EventArgs e)
        {

        }
        


        private void Log(string log)
        {
            if (this.InvokeRequired)
            {
                Invoke(() => Log(log));
            }
            else
            {
                txt_log.Text += log + Environment.NewLine;
            }
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void txt_send_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private async void cmd_Send_ClickAsync(object sender, EventArgs e)
        {
            if (tcpServerAsync is not null && int.TryParse(txt_clientNr.Text,out int clientNr))
            {
                await tcpServerAsync.WriteString(clientNr, txt_send.Text);
            }
        }
    }

   

}
