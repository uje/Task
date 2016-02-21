using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Win32;
using System.Windows.Forms;
using System.Xml.Linq;
using System.IO;
using System.Configuration;
using System.Reflection;

namespace WY.Util {
    public class AppUtil {
        #region 窗口
        /// <summary>
        /// 警告对话框
        /// </summary>
        /// <param name="msg"></param>
        public static void Alert(string msg) {
            MessageBox.Show(msg, "程序提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        public static void Success(string msg) {
            MessageBox.Show(msg, "程序提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        /// <summary>
        /// 询问对话框
        /// </summary>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static bool Confirm(string msg) {
            return MessageBox.Show(msg, "程序提示", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
        }

        /// <summary>
        /// 检查是否管理员权限
        /// </summary>
        public static bool IsAdminExecute() {
            System.Security.Principal.WindowsIdentity wid = System.Security.Principal.WindowsIdentity.GetCurrent();
            System.Security.Principal.WindowsPrincipal p = new System.Security.Principal.WindowsPrincipal(wid);

            bool isAdmin = (p.IsInRole(System.Security.Principal.WindowsBuiltInRole.Administrator));

            return isAdmin;
        }

        #endregion

        #region 配置文件
        /// <summary>
        /// 批量设置配置文件AppSetting
        /// </summary>
        /// <param name="ass">键与值</param>
        public static void WriteSetting(params KeyValuePair<string, object>[] parms) {
            WriteSetting(null, parms);
        }
        /// <summary>
        /// 批量设置配置文件AppSetting
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="ass">键与值</param>
        public static void WriteSetting(string configPath, params KeyValuePair<string, object>[] parms) {
            Configuration config;
            if (string.IsNullOrEmpty(configPath)) {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else {
                config = ConfigurationManager.OpenExeConfiguration(configPath);
            }
            foreach (var appStringElement in parms) {
                if (!string.IsNullOrEmpty(appStringElement.Key) &&
                    !string.IsNullOrEmpty(appStringElement.Value.ToString())) {
                    try {
                        config.AppSettings.Settings[appStringElement.Key].Value = appStringElement.Value.ToString();
                    }
                    catch {
                        config.AppSettings.Settings.Add(appStringElement.Key, appStringElement.Value.ToString());
                    }
                }
            }
            config.Save();
        }
        /// <summary>
        /// 设置配置文件
        /// </summary>
        /// <param name="key">键</param>
        /// <param name="value">值</param>
        public static void WriteSetting(string key, string value) {
            WriteSetting(new KeyValuePair<string, object>(key, value));
        }

        #endregion


        #region 进程
        /// <summary>
        /// 关闭指定名字的进程
        /// </summary>
        /// <param name="name"></param>
        public static void KillProcess(string name) {
            var p = Process.GetProcessesByName(name);
            p.ToList().ForEach(t => {
                try {
                    t.Kill();
                }
                catch {
                }
            });
        }

        /// <summary>
        /// 执行应用程序 
        /// </summary>
        /// <param name="appPath">程序路径</param>
        /// <param name="param">参数</param>
        public static void Execute(string appPath, string param = null) {
            var p = new Process();
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.FileName = appPath;
            if (!string.IsNullOrEmpty(param)) {
                p.StartInfo.Arguments = param;
            }
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardInput = false;
            p.StartInfo.RedirectStandardOutput = true;
            try {
                p.Start();
            }
            catch {
            }
            finally {
                p.WaitForExit();
            }

        }

        public static void AppIsOn(string appName) {
            var apps = Process.GetProcesses().Where(t => appName.Equals(t.ProcessName, StringComparison.CurrentCultureIgnoreCase));
            if (apps.Count() > 1) {
                System.Environment.Exit(0);
            }
        }
        #endregion

        #region 系统

        /// <summary>
        /// 关机
        /// </summary>
        public static void CloseWindow() {
            Execute("shutdown", " -s -t 1");
        }
        /// <summary>
        /// 重启
        /// </summary>
        public static void RestartWindow() {
            Execute("shutdown", " -s -r");
        }
        /// <summary>
        /// 取消关机操作
        /// </summary>
        public static void AbortCloseWindow() {
            Execute("shutdown", "-a");
        }
        #endregion
        public static T CreateInstance<T>(string path, string fullName) {
            var ass = Assembly.Load(File.ReadAllBytes(path));
            var instance = ass.CreateInstance(fullName);
            ass = null;
            return (T)instance;
        }
        public static bool IsInterface<T>(string path, string typeName) {
            var ass = Assembly.Load(File.ReadAllBytes(path));

            foreach (var t in ass.GetExportedTypes()) {
                if (t.GetInterface(typeof(T).Name) != null && !t.IsAbstract && t.FullName==typeName) {
                    return true;
                }
            }
            ass = null;
            return false;
        }
        public static Dictionary<string, string> GetClassNamesByInterface<T>(string path) {
            var dicts = new Dictionary<string, string>();
            var ass = Assembly.Load(File.ReadAllBytes(path));

            foreach (var t in ass.GetExportedTypes()) {
                if (t.GetInterface(typeof(T).Name) != null && !t.IsAbstract) {
                    dicts.Add(t.FullName, path);
                }
            }
            ass = null;
            return dicts;
        }
        public static List<T> GetInstacesByInterface<T>(string path) {
            var list = new List<T>();
            var ass = Assembly.Load(File.ReadAllBytes(path));

            foreach (var t in ass.GetExportedTypes()) {
                if (t.GetInterface(typeof(T).Name) != null && !t.IsAbstract) {
                    list.Add((T)ass.CreateInstance(t.FullName));
                }
            }
            ass = null;
            return list;
        }
    }
}
