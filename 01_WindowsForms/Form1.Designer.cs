namespace _01_WindowsForms
{
    partial class Form1
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
            this.cmd_TestAsync = new System.Windows.Forms.Button();
            this.txt_TestLog = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // cmd_openServerForm
            // 
            this.cmd_openServerForm.Location = new System.Drawing.Point(42, 135);
            this.cmd_openServerForm.Name = "cmd_openServerForm";
            this.cmd_openServerForm.Size = new System.Drawing.Size(94, 29);
            this.cmd_openServerForm.TabIndex = 0;
            this.cmd_openServerForm.Text = "new Server";
            this.cmd_openServerForm.UseVisualStyleBackColor = true;
            this.cmd_openServerForm.Click += new System.EventHandler(this.cmd_openServerForm_Click);
            // 
            // cmd_openClientForm
            // 
            this.cmd_openClientForm.Location = new System.Drawing.Point(164, 135);
            this.cmd_openClientForm.Name = "cmd_openClientForm";
            this.cmd_openClientForm.Size = new System.Drawing.Size(94, 29);
            this.cmd_openClientForm.TabIndex = 1;
            this.cmd_openClientForm.Text = "new Client";
            this.cmd_openClientForm.UseVisualStyleBackColor = true;
            this.cmd_openClientForm.Click += new System.EventHandler(this.cmd_openClientForm_Click);
            // 
            // cmd_TestAsync
            // 
            this.cmd_TestAsync.Location = new System.Drawing.Point(586, 135);
            this.cmd_TestAsync.Name = "cmd_TestAsync";
            this.cmd_TestAsync.Size = new System.Drawing.Size(94, 29);
            this.cmd_TestAsync.TabIndex = 2;
            this.cmd_TestAsync.Text = "test Async";
            this.cmd_TestAsync.UseVisualStyleBackColor = true;
            this.cmd_TestAsync.Click += new System.EventHandler(this.cmd_TestAsync_Click);
            // 
            // txt_TestLog
            // 
            this.txt_TestLog.Location = new System.Drawing.Point(461, 207);
            this.txt_TestLog.Multiline = true;
            this.txt_TestLog.Name = "txt_TestLog";
            this.txt_TestLog.Size = new System.Drawing.Size(304, 154);
            this.txt_TestLog.TabIndex = 3;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.txt_TestLog);
            this.Controls.Add(this.cmd_TestAsync);
            this.Controls.Add(this.cmd_openClientForm);
            this.Controls.Add(this.cmd_openServerForm);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button cmd_openServerForm;
        private Button cmd_openClientForm;
        private Button cmd_TestAsync;
        private TextBox txt_TestLog;
    }
}