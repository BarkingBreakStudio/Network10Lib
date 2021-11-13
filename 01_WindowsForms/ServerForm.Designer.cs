namespace _01_WindowsForms
{
    partial class ServerForm
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
            this.cmd_serverStart = new System.Windows.Forms.Button();
            this.txt_log = new System.Windows.Forms.TextBox();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.cmd_Send = new System.Windows.Forms.Button();
            this.txt_clientNr = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmd_serverStart
            // 
            this.cmd_serverStart.Location = new System.Drawing.Point(437, 40);
            this.cmd_serverStart.Name = "cmd_serverStart";
            this.cmd_serverStart.Size = new System.Drawing.Size(127, 29);
            this.cmd_serverStart.TabIndex = 0;
            this.cmd_serverStart.Text = " Start Server";
            this.cmd_serverStart.UseVisualStyleBackColor = true;
            this.cmd_serverStart.Click += new System.EventHandler(this.cmd_serverStart_Click);
            // 
            // txt_log
            // 
            this.txt_log.Location = new System.Drawing.Point(38, 126);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(526, 312);
            this.txt_log.TabIndex = 4;
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(38, 93);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(353, 27);
            this.txt_send.TabIndex = 6;
            this.txt_send.TextChanged += new System.EventHandler(this.txt_send_TextChanged);
            // 
            // cmd_Send
            // 
            this.cmd_Send.Location = new System.Drawing.Point(470, 93);
            this.cmd_Send.Name = "cmd_Send";
            this.cmd_Send.Size = new System.Drawing.Size(94, 29);
            this.cmd_Send.TabIndex = 5;
            this.cmd_Send.Text = "Send";
            this.cmd_Send.UseVisualStyleBackColor = true;
            this.cmd_Send.Click += new System.EventHandler(this.cmd_Send_ClickAsync);
            // 
            // txt_clientNr
            // 
            this.txt_clientNr.Location = new System.Drawing.Point(397, 93);
            this.txt_clientNr.Name = "txt_clientNr";
            this.txt_clientNr.Size = new System.Drawing.Size(67, 27);
            this.txt_clientNr.TabIndex = 7;
            this.txt_clientNr.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_clientNr);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.cmd_Send);
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.cmd_serverStart);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ServerForm_FormClosing);
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cmd_serverStart;
        private TextBox txt_log;
        private TextBox txt_send;
        private Button cmd_Send;
        private TextBox txt_clientNr;
    }
}