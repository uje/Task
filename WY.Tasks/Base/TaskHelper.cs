using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Shapes;
using WY.Tasks.Api;

namespace WY.Tasks.Base {
    internal class TaskHelper {
        public static void Execute(string path, string typeName) {
            AppDomainSetup domainInfo = new AppDomainSetup();
            domainInfo.ApplicationName = "WY.Tasks.Plugin";
            domainInfo.ApplicationBase = AppDomain.CurrentDomain.BaseDirectory;
            domainInfo.ConfigurationFile = string.Format("{0}.config", typeName);
            AppDomain appDomain = AppDomain.CreateDomain(typeName, null, domainInfo);

            Loader loader = (Loader)appDomain.CreateInstanceAndUnwrap(
                typeof(Loader).Assembly.FullName,
                typeof(Loader).FullName);

            loader.Execute(path, typeName);
            loader = null;

            AppDomain.Unload(appDomain);
        }
    }


    internal class Loader : MarshalByRefObject {
        public override object InitializeLifetimeService() {
            return null;
        }

        public void Execute(string path, string typeName) {
            var ass = Assembly.Load(File.ReadAllBytes(path));
            var task = (ITask)ass.CreateInstance(typeName);
            task.Execute();
            ass = null;
        }
    }
}
