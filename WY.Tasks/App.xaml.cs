using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows;
using WY.Util;

namespace WY.Tasks {
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application {
        public App() {
            Logger.Write("程序已运行！");
            Logger.Write(AppUtil.IsAdminExecute() ? "当前以管理员模式运行" : "非管理员模式运行");
            AppUtil.AppIsOn("wy.tasks");
        }
    }
}
