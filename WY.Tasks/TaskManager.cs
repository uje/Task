using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Threading;
using WY.Tasks.Api;
using WY.Tasks.Api.Generic;
using WY.Tasks.Base;
using WY.Util;

namespace WY.Tasks {
    internal class TaskManager : IPluginWatch, IPluginServer<ITask> {


        private string pluginDir;
        private FileSystemWatcher fileWatcher;
        private Dictionary<string, string> plugins;
        private const string PLUGIN_PATTERN = "WY.Tasks.Plugin.*.dll";

        public string PluginDir {
            get {
                if (pluginDir.IsNullOrWhiteSpace()) {
                    return AppDomain.CurrentDomain.BaseDirectory;
                }
                return pluginDir;
            }
            set { pluginDir = value; }
        }

        public Action Callback { get; set; }

        public TaskManager() : this("", null) { }
        public TaskManager(string pluginDir) : this(pluginDir, null) { }
        public TaskManager(string pluginDir, Action callback) {
            if (!pluginDir.IsNullOrWhiteSpace()) {
                this.PluginDir = pluginDir;
            }
            this.Callback = callback;
            plugins = new Dictionary<string, string>();

            this.Initialize();
            this.PluginDirWatch();
        }

        void Initialize() {

            Logger.Write("[程序设定]插件目录：{0}", PluginDir);
            var files = Directory.GetFiles(PluginDir, PLUGIN_PATTERN);
            foreach (var file in files) {
                AssemblyAdd(null, new FileSystemEventArgs(WatcherChangeTypes.Created, PluginDir, Path.GetFileName(file)));
            }
            foreach (var plugin in plugins) {
                AppSetting appSetting = new AppSetting(plugin.Key);
                var interval = int.Parse(appSetting.Get("interval", "1"));
                if (appSetting.Get("interval").IsNullOrWhiteSpace()) {
                    appSetting.Set("interval", "1");
                }
                DispatcherTimer pluginTimer = new DispatcherTimer();
                pluginTimer.Interval = TimeSpan.FromHours(interval);
                pluginTimer.IsEnabled = true;
                pluginTimer.Tick += delegate {
                    ExecutePluginEvent(plugin.Key);
                };
                pluginTimer.Start();
                Logger.Write("{0}定时器启动，时间间隔为{1}小时", plugin.Key, interval);
            }
        }

        private void AssemblyRemove(object sender, FileSystemEventArgs e) {

            Logger.Write("检查文件：{0}", Path.GetFileName(e.FullPath));
            RemovePlugins(e.FullPath);
        }

        private void AssemblyAdd(object sender, FileSystemEventArgs e) {
            try {
                Logger.Write("检查文件：{0}", Path.GetFileName(e.FullPath));
                RegisterPlugins(e.FullPath);
                ExecutePluginsEvent(e.FullPath);
            }
            catch (Exception ex) {
                Logger.Write("出现异常：{0}", ex.Message);
            }
        }

        #region IPluginWatch 成员

        public void PluginDirWatch() {
            fileWatcher = new FileSystemWatcher();
            fileWatcher.Path = PluginDir;
            fileWatcher.Filter = PLUGIN_PATTERN;
            fileWatcher.Deleted += AssemblyRemove;
            fileWatcher.Created += AssemblyAdd;
            fileWatcher.EnableRaisingEvents = true;
            fileWatcher.IncludeSubdirectories = false;
        }


        #endregion

        #region IPluginServer<ITask> 成员

        public void ExecutePluginsEvent() {
            foreach (var p in plugins) {
                ExecutePluginEvent(p.Key);
            }
        }

        public void ExecutePluginEvent(string pluginKey) {

            if (!plugins.ContainsKey(pluginKey)) return;

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += delegate {
                Logger.Write("执行插件：{0}", pluginKey);
                DateTime startTime = DateTime.Now;
                TaskHelper.Execute(plugins[pluginKey], pluginKey);
                //AppUtil.CreateInstance<ITask>(plugins[pluginKey],pluginKey).Execute();
                DateTime endTime = DateTime.Now;
                Logger.Write("{1}执行完成，用时：{0}秒", (endTime - startTime).TotalSeconds, pluginKey);
                if (Callback != null) {
                    Callback();
                }
            };

            bw.RunWorkerAsync();
        }

        /// <summary>
        /// 根据指定文件名执行插件
        /// </summary>
        /// <param name="name"></param>
        public void ExecutePluginsEvent(string name) {
            foreach (var plugin in plugins) {
                if (plugin.Value == name) {
                    ExecutePluginEvent(plugin.Key);
                }
            }
        }
        public void RegisterPlugins(string path) {
            var plugins = AppUtil.GetClassNamesByInterface<ITask>(path);
            foreach (var plugin in plugins) {
                RegisterPlugin(plugin.Key, plugin.Value);
            }
        }
        public void RegisterPlugin(string pluginKey, string path) {
            try {
                if (!plugins.ContainsKey(pluginKey) && AppUtil.IsInterface<ITask>(path, pluginKey)) {
                    plugins.Add(pluginKey, path);
                    Logger.Write("注册插件：{0},名称：{1}", pluginKey, Path.GetFileName(path));
                }
                else {
                    Logger.Write("不是插件或插件已存在！插件名称：{0},名称：{1}", pluginKey, Path.GetFileName(path));
                }
            }
            catch (Exception ex) {
                Logger.Write("注册插件失败！插件名称：{0},路径：{1},信息：{2}", pluginKey, Path.GetFileName(path), ex.Message);
            }
        }

        public List<ITask> GetPlugins() {
            return plugins.Select(t => AppUtil.CreateInstance<ITask>(t.Value, t.Key)).ToList();
        }

        public void RemovePlugin(string pluginKey) {
            if (plugins.ContainsKey(pluginKey)) {
                plugins.Remove(pluginKey);
                Logger.Write("移除插件：{0}", pluginKey);
            }
        }
        private void RemovePlugins(string fileName) {
            var _plugins = new Dictionary<string, string>(plugins);
            foreach (var plugin in _plugins) {
                if (plugin.Value == fileName) {
                    RemovePlugin(plugin.Key);
                }
            }
        }

        public void ClearPlugins() {
            plugins.Clear();
        }

        #endregion

    }
}
