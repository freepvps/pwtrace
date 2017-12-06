using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Net
{
    public class Plugin
    {
        public OOGHost Host { get; internal set; }

        public virtual bool Enabled { get; set; }

        internal protected virtual void Initialize()
        {
        }
        internal protected virtual void OnStart()
        {

        }
        internal protected virtual void OnStop()
        {

        }
    }
}
