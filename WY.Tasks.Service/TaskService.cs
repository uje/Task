using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using WY.Tasks.Util;

namespace WY.Tasks.Service {
    partial class TaskService : ServiceBase {
        public TaskService() {
            InitializeComponent();
        }

        protected override void OnStart(string[] args) {
            ApplicationHelper.WriteLog("当前运行模式：{0}", ApplicationHelper.IsAdminExecute() ? "管理员" : "来宾");
            new Tasks.Core.TaskManager();
        }

        protected override void OnStop() {
            // TODO: 在此处添加代码以执行停止服务所需的关闭操作。
        }
    }
}
