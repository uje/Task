using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WY.Tasks.Core;
using WY.Tasks.Util;
using System.Threading;

namespace WY.Tasks {
    public partial class Form1 : Form {

        bool isFirstRun = true;
        bool isHandleClose = false;
        TaskManager taskManager;

        protected bool AutoStart {
            get { return ConfigurationManager.AppSettings["AutoStart"] == "1"; }
            set { ApplicationHelper.WriteConfigKey("AutoStart", value ? "1" : "0"); }
        }
        public Form1() {
            InitializeComponent();
            Console.SetOut(new ConsoleStringWriter(ConsoleBox));
            taskManager = new TaskManager();
            if (AutoStart) {
                SetAutoStart(true);
            }
        }

        private void Form1_Load(object sender, EventArgs e) {

            Console.WriteLine("程序启动！");
            Console.WriteLine(AutoStart ? "当前为自动启动模式！" : "当前为手动模式");
            SetAutoStart(AutoStart);
            Thread th = new Thread(taskManager.Initialize);
            th.Start();
        }
        protected override void SetVisibleCore(bool value) {
            if (isFirstRun) {
                base.SetVisibleCore(true);
                this.ShowInTaskbar = false;
                value = false;
                isFirstRun = false;
            }
            base.SetVisibleCore(value);
        }

        private void CMSClose_Click(object sender, EventArgs e) {
            isHandleClose = true;
            Application.Exit();
        }

        private void CMSSHow_Click(object sender, EventArgs e) {
            this.Show();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            if (!isHandleClose) {
                base.SetVisibleCore(false);
                e.Cancel = true;
            }
        }

        private void CMSAutoStart_Click(object sender, EventArgs e) {
            SetAutoStart(CMSAutoStart.Text == "启用自动启动(&E)");
        }

        protected void SetAutoStart(bool autoStart) {
            AutoStart = autoStart;
            if (autoStart) {
                try {
                    ApplicationHelper.AddOnStart();
                    CMSAutoStart.Text = "*已启用自动启动(&D)";
                }
                catch (Exception ex) {
                    Console.WriteLine("添加到启动项失败！错误信息为：{0}", ex.Message);
                }
            }
            else {

                try {
                    ApplicationHelper.DelStartApp();
                    CMSAutoStart.Text = "启用自动启动(&E)";
                }
                catch (Exception ex) {
                    Console.WriteLine("删除启动项失败！错误信息为：{0}", ex.Message);
                }

            }
        }
    }
}
