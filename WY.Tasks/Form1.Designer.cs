namespace WY.Tasks {
    partial class Form1 {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent() {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.CMSSHow = new System.Windows.Forms.ToolStripMenuItem();
            this.CMSAutoStart = new System.Windows.Forms.ToolStripMenuItem();
            this.CMSClose = new System.Windows.Forms.ToolStripMenuItem();
            this.ConsoleBox = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // notifyIcon1
            // 
            this.notifyIcon1.ContextMenuStrip = this.contextMenuStrip1;
            this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon1.Icon")));
            this.notifyIcon1.Text = "工具";
            this.notifyIcon1.Visible = true;
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.CMSSHow,
            this.CMSAutoStart,
            this.CMSClose});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(162, 70);
            // 
            // CMSSHow
            // 
            this.CMSSHow.Name = "CMSSHow";
            this.CMSSHow.Size = new System.Drawing.Size(161, 22);
            this.CMSSHow.Text = "显示窗口(&S)";
            this.CMSSHow.Click += new System.EventHandler(this.CMSSHow_Click);
            // 
            // CMSAutoStart
            // 
            this.CMSAutoStart.Name = "CMSAutoStart";
            this.CMSAutoStart.Size = new System.Drawing.Size(161, 22);
            this.CMSAutoStart.Text = "启用自动启动(&E)";
            this.CMSAutoStart.Click += new System.EventHandler(this.CMSAutoStart_Click);
            // 
            // CMSClose
            // 
            this.CMSClose.Name = "CMSClose";
            this.CMSClose.Size = new System.Drawing.Size(161, 22);
            this.CMSClose.Text = "关闭(&C)";
            this.CMSClose.Click += new System.EventHandler(this.CMSClose_Click);
            // 
            // ConsoleBox
            // 
            this.ConsoleBox.Location = new System.Drawing.Point(13, 39);
            this.ConsoleBox.Multiline = true;
            this.ConsoleBox.Name = "ConsoleBox";
            this.ConsoleBox.ReadOnly = true;
            this.ConsoleBox.Size = new System.Drawing.Size(782, 404);
            this.ConsoleBox.TabIndex = 1;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(13, 13);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 20);
            this.comboBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 455);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.ConsoleBox);
            this.Name = "Form1";
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.TextBox ConsoleBox;
        private System.Windows.Forms.ToolStripMenuItem CMSSHow;
        private System.Windows.Forms.ToolStripMenuItem CMSAutoStart;
        private System.Windows.Forms.ToolStripMenuItem CMSClose;
        private System.Windows.Forms.ComboBox comboBox1;
    }
}

