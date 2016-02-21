using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WY.Util;
using System.Configuration;
using WY.Tasks.Base;
using System.Diagnostics;
using System.IO;

namespace WY.Tasks {
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window {

        TaskManager taskManager;
        bool IsFirstRun = true;
        string startupDir;
        const string REGISTY_KEY = "WY.Tasks";
        FileSystemWatcher watcher;

        protected bool ShowIcon {
            get { return ConfigurationManager.AppSettings["ShowIcon"] != "0"; }
            set { AppUtil.WriteSetting("ShowIcon", value ? "1" : "0"); }
        }

        public MainWindow() {
            InitializeComponent();

            this.MaxHeight = System.Windows.Forms.Screen.GetWorkingArea(new System.Drawing.Point(0, 0)).Height + 10;
            if (ShowIcon) BuildIconAbout();
            if (IsFirstRun) { IsFirstRun = false; SetVisible(false); }

            startupDir = AppDomain.CurrentDomain.BaseDirectory;
            taskManager = new TaskManager(startupDir);

            watcher = new FileSystemWatcher(Logger.GetLogPath(), Logger.GetLogName());
            watcher.Changed += delegate {
                watcher.Filter = Logger.GetLogName();
                ShowLog();
            };
            watcher.EnableRaisingEvents = true;
        }
        void BuildIconAbout() {

            menu.AddMenu("显示(&S)", delegate { SetVisible(true); }, "MICShow");
            menu.AddMenu("执行所有插件(&E)", delegate { taskManager.ExecutePluginsEvent(); }, "MICExec");
            menu.AddMenu("启用自动启动(&E)", AutoStartClick, "MICAutoStart");
            menu.AddMenu("关闭(&C)", delegate { App.Current.Shutdown(); }, "MIClose");
            menu.Icon = Properties.Resources.task;
            menu.DBClick = delegate { SetVisible(true); };

            try {
                if (RegistryHelper.IsRegisted(RegistryHelper.HKCU_RUN, REGISTY_KEY)) {
                    menu.Items["MICAutoStart"].Text = "*已启用自动启动(&D)";
                }
            }
            catch (Exception ex) {
                menu.Items["MICAutoStart"].Text = "*程序无管理员权限，无法检测启动设置";
                Logger.Write("检测注册表失败！信息为：{0}", ex.Message);
            }
        }

        void ShowLog() {
            if (this.Visibility != Visibility.Visible) return;
            logBox.Dispatcher.Invoke(new Action(delegate {
                logBox.Text = Logger.Get(DateTime.Now);
                logBoxScroller.ScrollToEnd();
            }));

        }

        void SetVisible(bool isVisible) {
            if (isVisible) { this.Show(); ShowLog(); }
            else { logBox.Text = ""; this.Hide(); }
        }
        void SetAutoStart(bool autoStart) {
            try {
                if (!autoStart) {
                    var appExtPath = System.IO.Path.Combine(startupDir, Process.GetCurrentProcess().MainModule.FileName);
                    RegistryHelper.Register(RegistryHelper.HKCU_RUN, REGISTY_KEY, appExtPath);
                    menu.Items["MICAutoStart"].Text = "*已启用自动启动(&D)";
                    Logger.Write("已启用自动启动");
                }
                else {
                    RegistryHelper.Delete(RegistryHelper.HKCU_RUN, REGISTY_KEY);
                    menu.Items["MICAutoStart"].Text = "启用自动启动(&E)";
                    Logger.Write("已禁用自动启动");
                }

            }
            catch (Exception ex) {
                Logger.Write("操作失败！错误信息为：{0}", ex.Message);
            }
        }
        protected void AutoStartClick(object sender, EventArgs e) {
            SetAutoStart(RegistryHelper.IsRegisted(RegistryHelper.HKCU_RUN, REGISTY_KEY));
        }
        private void Grid_MouseMove_1(object sender, System.Windows.Input.MouseEventArgs e) {
            if (e.LeftButton == MouseButtonState.Pressed) {
                if (this.WindowState != WindowState.Normal) {
                    this.WindowState = WindowState.Normal;
                }
                DragMove();
            }
        }
        private void Button_Click_1(object sender, RoutedEventArgs e) {
            SetVisible(false);
        }
    }
}
