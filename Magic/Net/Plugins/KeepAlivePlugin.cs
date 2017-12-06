using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;
using Magic.Threading;
using Magic.Net.Packets;
using Magic.Net.Packets.Server;
using Magic.Net.Packets.Client;
using System.Threading;

namespace Magic.Net.Plugins
{
    public class KeepAlivePlugin : Plugin
    {
        private ActionTimer Timer;
        private int period = 15000;

        private static KeepAliveC5A keepAlive = new KeepAliveC5A();

        public ConnectionInfo ConnectionInfo { get; private set; }
        public int KeepAlivePeriod
        {
            get
            {
                return period;
            }
            set
            {
                if (value <= 0)
                {
                    throw new ArgumentException();
                }
                period = value;
            }
        }

        protected internal override void Initialize()
        {
            Timer = new ActionTimer(KeepAliveTick);

            ConnectionInfo = Host.Data.Register<ConnectionInfo>();

            ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;
            Host.Handler.AddHandler<OnlineAnnounceS04>(Receive_OnlineAnnounce);
        }
        protected internal override void OnStop()
        {
            Timer.Stop();
            base.OnStop();
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            Timer.Stop();
        }
        private void Receive_OnlineAnnounce(object sender, PacketEventArgs e)
        {
            Timer.Start(0, period);
        }

        private void KeepAliveTick(object state)
        {
            if (Enabled)
            {
                Host.Send(keepAlive);
            }
        }
    }
}
