using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;

namespace WY.Tasks.Api.Generic {
    public sealed class AppSetting {
        public string ModuleName { get; set; }
        public AppSetting(string moduleName) {
            this.ModuleName = moduleName;
        }
        public string Get(string key, string defualtValue = "") {
            var value = ConfigurationManager.AppSettings[string.Format("{0}.{1}", ModuleName, key)];
            if (value == null || string.IsNullOrEmpty(value.Trim())) {
                return defualtValue;
            }
            return value.Trim();
        }
        public void Set(string key, string value) {
            WriteSetting(string.Format("{0}.{1}", ModuleName, key), value);
        }

        public string this[string key] {
            get { return this.Get(key); }
            set { this.Set(key, value); }
        }


        #region 配置文件
        /// <summary>
        /// 批量设置配置文件AppSetting
        /// </summary>
        /// <param name="ass">键与值</param>
        internal static void WriteStting(params KeyValuePair<string, object>[] parms) {
            WriteSettings(null, parms);
        }
        /// <summary>
        /// 批量设置配置文件AppSetting
        /// </summary>
        /// <param name="configPath">配置文件路径</param>
        /// <param name="ass">键与值</param>
        internal static void WriteSettings(string configPath, params KeyValuePair<string, object>[] parms) {
            Configuration config;
            if (string.IsNullOrEmpty(configPath)) {
                config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            }
            else {
                config = ConfigurationManager.OpenExeConfiguration(configPath);
            }
            foreach (var parm in parms) {
                if (!string.IsNullOrEmpty(parm.Key) &&
                    !string.IsNullOrEmpty(parm.Value.ToString())) {
                    var element = config.AppSettings.Settings[parm.Key];
                    if (element != null) {
                        config.AppSettings.Settings[parm.Key].Value = parm.Value.ToString();
                    }
                    else {
                        config.AppSettings.Settings.Add(parm.Key, parm.Value.ToString());
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
        internal static void WriteSetting(string key, string value) {
            WriteStting(new KeyValuePair<string, object>(key, value));
        }

        #endregion
    }
}
