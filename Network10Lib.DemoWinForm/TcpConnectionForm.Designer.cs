namespace Network10Lib.DemoWinForm
{
    partial class TcpConnectionForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmd_openServer = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_ClientIdentifier = new System.Windows.Forms.TextBox();
            this.txt_ServerIdentifier = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.cmd_openClient = new System.Windows.Forms.Button();
            this.cmd_CloseServer = new System.Windows.Forms.Button();
            this.sdfsdf = new System.Windows.Forms.GroupBox();
            this.txt_ServerPort = new System.Windows.Forms.TextBox();
            this.txt_ServerIpAdr = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.dsfsdfsdf = new System.Windows.Forms.GroupBox();
            this.txt_ClientPort = new System.Windows.Forms.TextBox();
            this.cmd_closeClient = new System.Windows.Forms.Button();
            this.txt_ClientIpAdr = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cms_sendMessage = new System.Windows.Forms.Button();
            this.txt_sendMessageDestination = new System.Windows.Forms.TextBox();
            this.txt_snedMessageData = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txt_MsgReveive = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.groupBox5 = new System.Windows.Forms.GroupBox();
            this.lblStatus = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.sdfsdf.SuspendLayout();
            this.dsfsdfsdf.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.groupBox5.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmd_openServer
            // 
            this.cmd_openServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmd_openServer.Location = new System.Drawing.Point(10, 129);
            this.cmd_openServer.Name = "cmd_openServer";
            this.cmd_openServer.Size = new System.Drawing.Size(169, 29);
            this.cmd_openServer.TabIndex = 0;
            this.cmd_openServer.Text = "open Server";
            this.cmd_openServer.UseVisualStyleBackColor = true;
            this.cmd_openServer.Click += new System.EventHandler(this.cmd_OpenServer_ClickAsync);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txt_ClientIdentifier);
            this.groupBox1.Controls.Add(this.txt_ServerIdentifier);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(42, 29);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(380, 111);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "SharedParameter";
            // 
            // txt_ClientIdentifier
            // 
            this.txt_ClientIdentifier.Location = new System.Drawing.Point(129, 64);
            this.txt_ClientIdentifier.Name = "txt_ClientIdentifier";
            this.txt_ClientIdentifier.Size = new System.Drawing.Size(228, 27);
            this.txt_ClientIdentifier.TabIndex = 6;
            this.txt_ClientIdentifier.Text = "TestDemoClient123456";
            // 
            // txt_ServerIdentifier
            // 
            this.txt_ServerIdentifier.Location = new System.Drawing.Point(129, 33);
            this.txt_ServerIdentifier.Name = "txt_ServerIdentifier";
            this.txt_ServerIdentifier.Size = new System.Drawing.Size(228, 27);
            this.txt_ServerIdentifier.TabIndex = 5;
            this.txt_ServerIdentifier.Text = "TestDemoServer123456";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 67);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(114, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Client Identifier:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 36);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(117, 20);
            this.label1.TabIndex = 3;
            this.label1.Text = "Server Identifier:";
            // 
            // cmd_openClient
            // 
            this.cmd_openClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmd_openClient.Location = new System.Drawing.Point(11, 129);
            this.cmd_openClient.Name = "cmd_openClient";
            this.cmd_openClient.Size = new System.Drawing.Size(162, 29);
            this.cmd_openClient.TabIndex = 1;
            this.cmd_openClient.Text = "open Client";
            this.cmd_openClient.UseVisualStyleBackColor = true;
            this.cmd_openClient.Click += new System.EventHandler(this.cmd_openClient_Click);
            // 
            // cmd_CloseServer
            // 
            this.cmd_CloseServer.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmd_CloseServer.Enabled = false;
            this.cmd_CloseServer.Location = new System.Drawing.Point(185, 129);
            this.cmd_CloseServer.Name = "cmd_CloseServer";
            this.cmd_CloseServer.Size = new System.Drawing.Size(189, 29);
            this.cmd_CloseServer.TabIndex = 2;
            this.cmd_CloseServer.Text = "close Server";
            this.cmd_CloseServer.UseVisualStyleBackColor = true;
            this.cmd_CloseServer.Click += new System.EventHandler(this.cmd_CloseServer_ClickAsync);
            // 
            // sdfsdf
            // 
            this.sdfsdf.Controls.Add(this.txt_ServerPort);
            this.sdfsdf.Controls.Add(this.txt_ServerIpAdr);
            this.sdfsdf.Controls.Add(this.label4);
            this.sdfsdf.Controls.Add(this.label3);
            this.sdfsdf.Controls.Add(this.cmd_openServer);
            this.sdfsdf.Controls.Add(this.cmd_CloseServer);
            this.sdfsdf.Location = new System.Drawing.Point(42, 146);
            this.sdfsdf.Name = "sdfsdf";
            this.sdfsdf.Size = new System.Drawing.Size(380, 164);
            this.sdfsdf.TabIndex = 3;
            this.sdfsdf.TabStop = false;
            this.sdfsdf.Text = "Server";
            // 
            // txt_ServerPort
            // 
            this.txt_ServerPort.Location = new System.Drawing.Point(129, 73);
            this.txt_ServerPort.Name = "txt_ServerPort";
            this.txt_ServerPort.Size = new System.Drawing.Size(245, 27);
            this.txt_ServerPort.TabIndex = 7;
            this.txt_ServerPort.Text = "12345";
            // 
            // txt_ServerIpAdr
            // 
            this.txt_ServerIpAdr.Location = new System.Drawing.Point(129, 33);
            this.txt_ServerIpAdr.Name = "txt_ServerIpAdr";
            this.txt_ServerIpAdr.Size = new System.Drawing.Size(245, 27);
            this.txt_ServerIpAdr.TabIndex = 6;
            this.txt_ServerIpAdr.Text = "0.0.0.0";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 76);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(38, 20);
            this.label4.TabIndex = 5;
            this.label4.Text = "Port:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(10, 36);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(82, 20);
            this.label3.TabIndex = 3;
            this.label3.Text = "Ip Address:";
            // 
            // dsfsdfsdf
            // 
            this.dsfsdfsdf.Controls.Add(this.txt_ClientPort);
            this.dsfsdfsdf.Controls.Add(this.cmd_closeClient);
            this.dsfsdfsdf.Controls.Add(this.txt_ClientIpAdr);
            this.dsfsdfsdf.Controls.Add(this.label5);
            this.dsfsdfsdf.Controls.Add(this.cmd_openClient);
            this.dsfsdfsdf.Controls.Add(this.label6);
            this.dsfsdfsdf.Location = new System.Drawing.Point(428, 146);
            this.dsfsdfsdf.Name = "dsfsdfsdf";
            this.dsfsdfsdf.Size = new System.Drawing.Size(390, 164);
            this.dsfsdfsdf.TabIndex = 4;
            this.dsfsdfsdf.TabStop = false;
            this.dsfsdfsdf.Text = "Client";
            // 
            // txt_ClientPort
            // 
            this.txt_ClientPort.Location = new System.Drawing.Point(130, 73);
            this.txt_ClientPort.Name = "txt_ClientPort";
            this.txt_ClientPort.Size = new System.Drawing.Size(245, 27);
            this.txt_ClientPort.TabIndex = 11;
            this.txt_ClientPort.Text = "12345";
            // 
            // cmd_closeClient
            // 
            this.cmd_closeClient.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cmd_closeClient.Enabled = false;
            this.cmd_closeClient.Location = new System.Drawing.Point(179, 129);
            this.cmd_closeClient.Name = "cmd_closeClient";
            this.cmd_closeClient.Size = new System.Drawing.Size(196, 29);
            this.cmd_closeClient.TabIndex = 5;
            this.cmd_closeClient.Text = "close Client";
            this.cmd_closeClient.UseVisualStyleBackColor = true;
            this.cmd_closeClient.Click += new System.EventHandler(this.cmd_closeClient_Click);
            // 
            // txt_ClientIpAdr
            // 
            this.txt_ClientIpAdr.Location = new System.Drawing.Point(130, 33);
            this.txt_ClientIpAdr.Name = "txt_ClientIpAdr";
            this.txt_ClientIpAdr.Size = new System.Drawing.Size(245, 27);
            this.txt_ClientIpAdr.TabIndex = 10;
            this.txt_ClientIpAdr.Text = "127.0.0.1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(11, 76);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 20);
            this.label5.TabIndex = 9;
            this.label5.Text = "Port:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(11, 36);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(82, 20);
            this.label6.TabIndex = 8;
            this.label6.Text = "Ip Address:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cms_sendMessage);
            this.groupBox2.Controls.Add(this.txt_sendMessageDestination);
            this.groupBox2.Controls.Add(this.txt_snedMessageData);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(42, 330);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(776, 103);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Send";
            // 
            // cms_sendMessage
            // 
            this.cms_sendMessage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.cms_sendMessage.Enabled = false;
            this.cms_sendMessage.Location = new System.Drawing.Point(411, 53);
            this.cms_sendMessage.Name = "cms_sendMessage";
            this.cms_sendMessage.Size = new System.Drawing.Size(350, 29);
            this.cms_sendMessage.TabIndex = 8;
            this.cms_sendMessage.Text = "send Message";
            this.cms_sendMessage.UseVisualStyleBackColor = true;
            this.cms_sendMessage.Click += new System.EventHandler(this.cms_sendMessage_Click);
            // 
            // txt_sendMessageDestination
            // 
            this.txt_sendMessageDestination.Location = new System.Drawing.Point(275, 55);
            this.txt_sendMessageDestination.Name = "txt_sendMessageDestination";
            this.txt_sendMessageDestination.Size = new System.Drawing.Size(130, 27);
            this.txt_sendMessageDestination.TabIndex = 3;
            this.txt_sendMessageDestination.Text = "0";
            // 
            // txt_snedMessageData
            // 
            this.txt_snedMessageData.Location = new System.Drawing.Point(10, 55);
            this.txt_snedMessageData.Name = "txt_snedMessageData";
            this.txt_snedMessageData.Size = new System.Drawing.Size(258, 27);
            this.txt_snedMessageData.TabIndex = 2;
            this.txt_snedMessageData.Text = "Hallo!";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(275, 32);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(130, 20);
            this.label8.TabIndex = 1;
            this.label8.Text = "Reveiver Client Nr.";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 32);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 0;
            this.label7.Text = "MessageData";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txt_MsgReveive);
            this.groupBox3.Location = new System.Drawing.Point(42, 452);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(776, 273);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Receive";
            // 
            // txt_MsgReveive
            // 
            this.txt_MsgReveive.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txt_MsgReveive.Location = new System.Drawing.Point(10, 26);
            this.txt_MsgReveive.Multiline = true;
            this.txt_MsgReveive.Name = "txt_MsgReveive";
            this.txt_MsgReveive.ReadOnly = true;
            this.txt_MsgReveive.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txt_MsgReveive.Size = new System.Drawing.Size(751, 229);
            this.txt_MsgReveive.TabIndex = 0;
            // 
            // listBox1
            // 
            this.listBox1.Enabled = false;
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(10, 26);
            this.listBox1.Name = "listBox1";
            this.listBox1.ScrollAlwaysVisible = true;
            this.listBox1.Size = new System.Drawing.Size(751, 184);
            this.listBox1.TabIndex = 8;
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.listBox1);
            this.groupBox4.Location = new System.Drawing.Point(42, 731);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(776, 222);
            this.groupBox4.TabIndex = 9;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Connected Clients (Server Only)";
            // 
            // groupBox5
            // 
            this.groupBox5.Controls.Add(this.lblStatus);
            this.groupBox5.Location = new System.Drawing.Point(428, 29);
            this.groupBox5.Name = "groupBox5";
            this.groupBox5.Size = new System.Drawing.Size(390, 111);
            this.groupBox5.TabIndex = 10;
            this.groupBox5.TabStop = false;
            this.groupBox5.Text = "Status";
            // 
            // lblStatus
            // 
            this.lblStatus.AutoSize = true;
            this.lblStatus.Location = new System.Drawing.Point(11, 33);
            this.lblStatus.Name = "lblStatus";
            this.lblStatus.Size = new System.Drawing.Size(50, 20);
            this.lblStatus.TabIndex = 0;
            this.lblStatus.Text = "label9";
            // 
            // TcpConnectionForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(830, 957);
            this.Controls.Add(this.groupBox5);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.dsfsdfsdf);
            this.Controls.Add(this.sdfsdf);
            this.Controls.Add(this.groupBox1);
            this.Name = "TcpConnectionForm";
            this.Text = "TcpConnectionForm";
            this.Load += new System.EventHandler(this.TcpConnectionForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.sdfsdf.ResumeLayout(false);
            this.sdfsdf.PerformLayout();
            this.dsfsdfsdf.ResumeLayout(false);
            this.dsfsdfsdf.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox5.ResumeLayout(false);
            this.groupBox5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button cmd_openServer;
        private GroupBox groupBox1;
        private TextBox txt_ClientIdentifier;
        private TextBox txt_ServerIdentifier;
        private Label label2;
        private Label label1;
        private Button cmd_openClient;
        private Button cmd_CloseServer;
        private GroupBox sdfsdf;
        private GroupBox dsfsdfsdf;
        private TextBox txt_ServerPort;
        private TextBox txt_ServerIpAdr;
        private Label label4;
        private Label label3;
        private Button cmd_closeClient;
        private TextBox txt_ClientPort;
        private TextBox txt_ClientIpAdr;
        private Label label5;
        private Label label6;
        private GroupBox groupBox2;
        private Button cms_sendMessage;
        private TextBox txt_sendMessageDestination;
        private TextBox txt_snedMessageData;
        private Label label8;
        private Label label7;
        private GroupBox groupBox3;
        private TextBox txt_MsgReveive;
        private ListBox listBox1;
        private GroupBox groupBox4;
        private GroupBox groupBox5;
        private Label lblStatus;
    }
}