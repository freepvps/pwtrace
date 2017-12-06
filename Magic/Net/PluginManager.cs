using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net
{
    public class PluginManager
    {
        private static Type[] emptyTypes = { };
        private static object[] emptyArgs = { };
        private static Type pluginType = typeof(Plugin);

        public OOGHost Host { get; private set; }

        private Dictionary<Type, Plugin> Plugins { get; set; }

        public PluginManager(OOGHost host)
        {
            Host = host;
            Plugins = new Dictionary<Type, Plugin>();
        }


        public int Count
        {
            get
            {
                return Plugins.Count;
            }
        }

        public static bool IsPlugin(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            return type.IsSubclassOf(pluginType);
        }
        private static void ValidateType(Type type)
        {
            if (!IsPlugin(type))
            {
                throw new ArgumentException();
            }
        }

        public T Register<T>() where T : Plugin
        {
            return Register(typeof(T)) as T;
        }
        public Plugin Register(Type type)
        {
            ValidateType(type);

            Plugin plugin;
            if (!TryGetPlugin(type, out plugin))
            {
                plugin = type.GetConstructor(emptyTypes).Invoke(emptyArgs) as Plugin;
                Plugins[type] = plugin;

                plugin.Host = Host;
                plugin.Enabled = true;
                plugin.Initialize();
            }
            return plugin;
        }

        public T GetPlugin<T>() where T : Plugin
        {
            return GetPlugin(typeof(T)) as T;
        }
        public Plugin GetPlugin(Type type)
        {
            ValidateType(type);

            return Plugins[type];
        }

        public bool TryGetPlugin<T>(out T plugin) where T : Plugin
        {
            Plugin res;
            if (TryGetPlugin(typeof(T), out res))
            {
                plugin = res as T;
                return true;
            }
            else
            {
                plugin = default(T);
                return false;
            }
        }
        public bool TryGetPlugin(Type type, out Plugin plugin)
        {
            ValidateType(type);

            return Plugins.TryGetValue(type, out plugin);
        }

        public bool Contains<T>() where T : Plugin
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type type)
        {
            ValidateType(type);
            return Plugins.ContainsKey(type);
        }

        public Plugin[] ToArray()
        {
            var res = new Plugin[Plugins.Count];
            var i = 0;
            foreach(var plugin in Plugins.Values)
            {
                res[i++] = plugin;
            }
            return res;
        }

        internal void OnStart()
        {
            foreach(var plugin in Plugins.Values)
            {
                plugin.OnStart();
            }
        }
        internal void OnStop()
        {
            foreach (var plugin in Plugins.Values)
            {
                plugin.OnStop();
            }
        }
    }
}
