using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using WY.Tasks.Api;
using WY.Util;

namespace WY.Tasks.Plugin.Combine {

    public class HostSync : Task {

        const string defaultHostUrl = "http://storage.live.com/items/B932F88AF10993EC!8708?host";
        const string LOCAL_HOST_PATH = @"c:\windows\system32\drivers\etc\hosts";
        static WebUtil web;
        public HostSync() {
            web = Util.GetWebUtil();
        }

        public string HostUrl {
            get {
                if (ConfigurationManager.AppSettings["HostUrl"].IsNullOrWhiteSpace()) {
                    AppUtil.WriteSetting("HostUrl", defaultHostUrl);
                }
                return ConfigurationManager.AppSettings["HostUrl"];
            }
            set {
                AppUtil.WriteSetting("HostUrl", value);
            }
        }


        public long GetLocalFileLength(string path) {
            var bytes = File.ReadAllBytes(path);
            return bytes.Length;
        }


        #region ITask 成员

        public override void Execute() {

            if (!File.Exists(LOCAL_HOST_PATH) || web.GetFileLength(HostUrl) != GetLocalFileLength(LOCAL_HOST_PATH)) {
                var host = web.Get(HostUrl);
                if (File.Exists(LOCAL_HOST_PATH)) {
                    var bakName = string.Format("HOST_BAK_DATE_{0}", DateTime.Now.ToString("yyyy-MM-dd"));
                    FileInfo fi = new FileInfo(LOCAL_HOST_PATH);
                    fi.MoveTo(bakName);
                    Logger.Write("本地host文件已备份，文件名称为{0}", bakName);
                }
                File.WriteAllText(LOCAL_HOST_PATH, host);
                Logger.Write("已更新host文件,文件大小为：{0}", web.GetFileLength(HostUrl));
            }
            else {
                Logger.Write("host文件无更新");
            }

        }

        #endregion
    }
}
