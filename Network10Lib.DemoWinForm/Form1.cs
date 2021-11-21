namespace Network10Lib.DemoWinForm
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void cmd_openServerForm_Click(object sender, EventArgs e)
        {
            ServerForm form1 = new ServerForm();
            form1.Show();
        }

        private void cmd_openClientForm_Click(object sender, EventArgs e)
        {
            ClientForm form1 = new ClientForm();
            form1.Show();
        }

        private void cmd_TcpConnection_Click(object sender, EventArgs e)
        {
            TcpConnectionForm form1 = new TcpConnectionForm();
            form1.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

    }
}