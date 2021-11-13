namespace _01_WindowsForms
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

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private async void cmd_TestAsync_Click(object sender, EventArgs e)
        {
            txt_TestLog.Text = "";

            txt_TestLog.Text += "Start some Async Methos" + Environment.NewLine;
            Task t = SomeAsyncMethod();
            
            //t.Wait();
            t.GetAwaiter().GetResult();
            await t;
            txt_TestLog.Text += "Finished Main" + Environment.NewLine;
        }

        private async Task SomeAsyncMethod()
        {
            txt_TestLog.Text += "Start heavy work" + Environment.NewLine;
            await Task.Delay(1000).ConfigureAwait(false);
            txt_TestLog.Text += "End heavy work" + Environment.NewLine;
        }
    }
}