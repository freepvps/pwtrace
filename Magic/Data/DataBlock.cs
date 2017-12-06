using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Data
{
    public class DataBlock
    {
        public OOGHost Host { get; private set; }

        internal protected virtual void Initialize(OOGHost host)
        {
            Host = host;
        }
    }
}
