using Network10Lib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Network10Lib.DemoWinForm
{
    public partial class ClientForm : Form
    {

        TcpClientN10? client;
      

        public ClientForm()
        {
            InitializeComponent();
        }

        private async void cmd_connect_Click(object sender, EventArgs e)
        {
            if (client is null)
            {
                Log("Connecting...");
                client = new TcpClientN10();
                client.StringReceived += Client_MessageReceived;
                client.Disconnected += Client_Disconnected;
                await client.Connect();
                Log("Connected");
                cmd_connect.Text = "disconnect";
            }
            else
            {
                Log("Disconnecting...");
                client.StringReceived -= Client_MessageReceived;
                client.Disconnected -= Client_Disconnected;
                await client.Disconnect();
                client = null;
                Log("Disconnected");
                cmd_connect.Text = "connect";
            }
        }

        private void Client_Disconnected(TcpClientN10 sender)
        {
            Log($"Client disconnected");
        }

        private void Client_MessageReceived(TcpClientN10 sender, string message)
        {
            Log($"Message received: {message}");
        }

        private async void cmd_Send_Click(object sender, EventArgs e)
        {
            if (client is not null)
            {
                Log($"Message sending: {txt_send.Text} ...");
                await client.WriteString(txt_send.Text);
                Log($"Message sent");
            }
        }

        private void ClientForm_Load(object sender, EventArgs e)
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
    }
}
