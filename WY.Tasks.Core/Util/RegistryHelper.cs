using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace WY.Util {
    public class RegistryHelper {
        public const string HKCU_RUN = "hkcu/software/microsoft/windows/currentversion/run";
        public const string HKLM_RUN = "hklm/software/microsoft/windows/currentversion/run";
        public static void Register(string path, string key, object value) {
            path = FixPath(path);
            RegistryKey root = GetRoot(StringUtil.StringToArray(path, @"\\")[0]);
            var rKey = root.OpenSubKey(GetChildPath(path), true);
            rKey.SetValue(key, value);
            rKey.Close();
            root.Close();
        }
        public static void Delete(string path, string key) {
            path = FixPath(path);
            RegistryKey root = GetRoot(StringUtil.StringToArray(path, @"\\")[0]);
            var rKey = root.OpenSubKey(GetChildPath(path), true);
            rKey.DeleteValue(key);
            rKey.Close();
            root.Close();
        }
        public static object Get(string path, string key) {
            path = FixPath(path);
            RegistryKey root = GetRoot(StringUtil.StringToArray(path, @"\\")[0]);
            var rKey = root.OpenSubKey(GetChildPath(path), true);
            try {
                return rKey.GetValue(key);
            }
            catch {
                return null;
            }
            finally {
                if (rKey != null) {
                    rKey.Close();
                }
                root.Close();
            }
        }
        public static bool IsRegisted(string path, string name) {
            var value = Get(path, name);
            return value != null && !string.IsNullOrEmpty(value.ToString().Trim());
        }


        static RegistryKey GetRoot(string path) {
            switch (path.ToUpper()) {
                case "HKCR":
                    return Registry.ClassesRoot;
                case "HKLM":
                    return Registry.LocalMachine;
                case "HKCU":
                    return Registry.CurrentUser;
                case "HKCC":
                    return Registry.CurrentConfig;
                case "HKUS":
                    return Registry.Users;
            }
            throw new IOException(string.Format("{0}路径不存在！", path));
        }
        static string FixPath(string path) {
            return path.Replace("/", @"\\");
        }
        static string GetChildPath(string path) {
            return path.Substring(path.IndexOf(@"\\") + 2);
        }
    }
}
