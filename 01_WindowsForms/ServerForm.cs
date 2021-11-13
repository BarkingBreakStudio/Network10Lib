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

namespace _01_WindowsForms
{
    public partial class ServerForm : Form
    {

        //TcpListener? tcpListener = null;

        TcpServerAsync? tcpServerAsync;

        public ServerForm()
        {
            InitializeComponent();
        }

        private async void cmd_serverStart_Click(object sender, EventArgs e)
        {
            if (tcpServerAsync is null)
            {
                Log("Server starting ...");
                tcpServerAsync = new TcpServerAsync();
                tcpServerAsync.MessageReceived += TcpServerAsync_MessageReceived;
                tcpServerAsync.ClientConnected += TcpServerAsync_ClientConnected;
                tcpServerAsync.ClientDisconnected += TcpServerAsync_ClientDisconnected;
                tcpServerAsync.Start();
                Log("Server started");
                cmd_serverStart.Text = "Stop";
            }
            else
            {
                Log("Server stopping ...");
                await tcpServerAsync.Stop();
                tcpServerAsync = null;
                Log("Server stopped ...");
                cmd_serverStart.Text = "Start";
            }
            /*
            if (tcpServerAsync is null)
            {
                tcpServerAsync = new TcpServerAsync();
                await tcpServerAsync.StartAsync();
            }*/

        }

        private void TcpServerAsync_ClientDisconnected(TcpServerAsync sender, int clientNr, TcpClient client)
        {
            Log($"Client disconnected: {clientNr}");
        }

        private void TcpServerAsync_ClientConnected(TcpServerAsync sender, int clientNr, TcpClient client)
        {
            Log($"Client connected: {clientNr}");
        }

        private void TcpServerAsync_MessageReceived(TcpServerAsync sender, int clientNr, TcpClient client, string message)
        {
            Log($"Client: {clientNr} sent: {message}");
        }

        /*
        public static void DoAcceptTcpClientCallback(IAsyncResult ar)
        {
            ServerForm? form = (ServerForm?)ar.AsyncState;
            TcpListener? listener = form?.tcpListener;
            if (listener is not null && form is not null)
            {
                TcpClient client = listener.EndAcceptTcpClient(ar);

                ReceiveParam rp = new ReceiveParam(form, client);
                client.GetStream().BeginRead(rp.Buffer, 0, rp.Buffer.Length, new AsyncCallback(DoReadTcpClientCallback), rp);

                listener.BeginAcceptTcpClient(new AsyncCallback(DoAcceptTcpClientCallback), form);
            }

        }

        public static void DoReadTcpClientCallback(IAsyncResult ar)
        {
            ReceiveParam? param = (ReceiveParam?)ar.AsyncState;
          
            if (param is not null)
            {
                TcpClient client = param.Client;
                int numberOfBytesRead = client.GetStream().EndRead(ar);
                if (numberOfBytesRead > 0)
                {
                    client.GetStream().Write(param.Buffer, 0, numberOfBytesRead);

                    client.GetStream().BeginRead(param.Buffer, 0, param.Buffer.Length, new AsyncCallback(DoReadTcpClientCallback), param);
                }
                else
                {
                    client.Dispose();
                }

            }
        }

        public class ReceiveParam
        {
            public ServerForm Form;
            public TcpClient Client;
            public byte[] Buffer = new byte[1024];

            public ReceiveParam(ServerForm form, TcpClient tcpClient)
            {
                this.Client = tcpClient;
                this.Form = form;
            }
        }*/

        private void ServerForm_Load(object sender, EventArgs e)
        {

        }

        private void ServerForm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void cmd_StopServer_Click(object sender, EventArgs e)
        {
        /*
            if (tcpServerAsync is not null)
            {
                tcpServerAsync.Stop();
                tcpServerAsync = null;
            }
        */
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
                await tcpServerAsync.Write(clientNr, txt_send.Text);
            }
        }
    }

   

}
