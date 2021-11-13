namespace _01_WindowsForms
{
    partial class ClientForm
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
            this.cmd_Send = new System.Windows.Forms.Button();
            this.cmd_connect = new System.Windows.Forms.Button();
            this.txt_send = new System.Windows.Forms.TextBox();
            this.txt_log = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmd_Send
            // 
            this.cmd_Send.Location = new System.Drawing.Point(435, 73);
            this.cmd_Send.Name = "cmd_Send";
            this.cmd_Send.Size = new System.Drawing.Size(94, 29);
            this.cmd_Send.TabIndex = 0;
            this.cmd_Send.Text = "Send";
            this.cmd_Send.UseVisualStyleBackColor = true;
            this.cmd_Send.Click += new System.EventHandler(this.cmd_Send_Click);
            // 
            // cmd_connect
            // 
            this.cmd_connect.Location = new System.Drawing.Point(435, 38);
            this.cmd_connect.Name = "cmd_connect";
            this.cmd_connect.Size = new System.Drawing.Size(94, 29);
            this.cmd_connect.TabIndex = 1;
            this.cmd_connect.Text = "Connect";
            this.cmd_connect.UseVisualStyleBackColor = true;
            this.cmd_connect.Click += new System.EventHandler(this.cmd_connect_Click);
            // 
            // txt_send
            // 
            this.txt_send.Location = new System.Drawing.Point(22, 73);
            this.txt_send.Name = "txt_send";
            this.txt_send.Size = new System.Drawing.Size(379, 27);
            this.txt_send.TabIndex = 2;
            // 
            // txt_log
            // 
            this.txt_log.Location = new System.Drawing.Point(22, 117);
            this.txt_log.Multiline = true;
            this.txt_log.Name = "txt_log";
            this.txt_log.Size = new System.Drawing.Size(379, 312);
            this.txt_log.TabIndex = 3;
            // 
            // ClientForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_log);
            this.Controls.Add(this.txt_send);
            this.Controls.Add(this.cmd_connect);
            this.Controls.Add(this.cmd_Send);
            this.Name = "ClientForm";
            this.Text = "ClientForm";
            this.Load += new System.EventHandler(this.ClientForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cmd_Send;
        private Button cmd_connect;
        private TextBox txt_send;
        private TextBox txt_log;
    }
}