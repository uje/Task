using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WY.Tasks.Api;

namespace WY.Tasks.Base {
    public interface IPluginServer<T> where T : IPlugin {
        void ExecutePluginsEvent();
        void ExecutePluginEvent(string pluginKey);
        void RegisterPlugin(string pluginName, string path);
        List<T> GetPlugins();
        void RemovePlugin(string pluginKey);
        void ClearPlugins();
    }
}
