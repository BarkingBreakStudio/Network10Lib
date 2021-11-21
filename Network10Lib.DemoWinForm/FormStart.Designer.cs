namespace Network10Lib.DemoWinForm
{
    partial class FormStart
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.cmd_openServerForm = new System.Windows.Forms.Button();
            this.cmd_openClientForm = new System.Windows.Forms.Button();
            this.cmd_TcpConnection = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // cmd_openServerForm
            // 
            this.cmd_openServerForm.Location = new System.Drawing.Point(18, 45);
            this.cmd_openServerForm.Name = "cmd_openServerForm";
            this.cmd_openServerForm.Size = new System.Drawing.Size(94, 29);
            this.cmd_openServerForm.TabIndex = 0;
            this.cmd_openServerForm.Text = "new Server";
            this.cmd_openServerForm.UseVisualStyleBackColor = true;
            this.cmd_openServerForm.Click += new System.EventHandler(this.cmd_openServerForm_Click);
            // 
            // cmd_openClientForm
            // 
            this.cmd_openClientForm.Location = new System.Drawing.Point(136, 45);
            this.cmd_openClientForm.Name = "cmd_openClientForm";
            this.cmd_openClientForm.Size = new System.Drawing.Size(94, 29);
            this.cmd_openClientForm.TabIndex = 1;
            this.cmd_openClientForm.Text = "new Client";
            this.cmd_openClientForm.UseVisualStyleBackColor = true;
            this.cmd_openClientForm.Click += new System.EventHandler(this.cmd_openClientForm_Click);
            // 
            // cmd_TcpConnection
            // 
            this.cmd_TcpConnection.Location = new System.Drawing.Point(18, 37);
            this.cmd_TcpConnection.Name = "cmd_TcpConnection";
            this.cmd_TcpConnection.Size = new System.Drawing.Size(212, 29);
            this.cmd_TcpConnection.TabIndex = 4;
            this.cmd_TcpConnection.Text = "new Tcp Connection";
            this.cmd_TcpConnection.UseVisualStyleBackColor = true;
            this.cmd_TcpConnection.Click += new System.EventHandler(this.cmd_TcpConnection_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.cmd_openClientForm);
            this.groupBox1.Controls.Add(this.cmd_openServerForm);
            this.groupBox1.Location = new System.Drawing.Point(36, 171);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(250, 103);
            this.groupBox1.TabIndex = 5;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Demo low level Components";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cmd_TcpConnection);
            this.groupBox2.Location = new System.Drawing.Point(36, 40);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(250, 103);
            this.groupBox2.TabIndex = 6;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Demo high level Component";
            // 
            // FormStart
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(333, 305);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Name = "FormStart";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private Button cmd_openServerForm;
        private Button cmd_openClientForm;
        private Button cmd_TcpConnection;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
    }
}