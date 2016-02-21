using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WY.Tasks.Api.Generic;

namespace WY.Tasks.Api {
    public abstract class Task:ITask {
        AppSetting appSetting;
        public Task() {
            AppSettings = new AppSetting(this.GetType().FullName);
        }
        /// <summary>
        /// 配置
        /// </summary>
        protected AppSetting AppSettings {
            get { return appSetting; }
            set { appSetting = value; }
        }
     
        #region ITask 成员
        public abstract void Execute();
        #endregion
    }
}
