using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net;
using Magic.Net.Security;
using Magic.Data.Types;

namespace Magic.Data
{
    public enum ConnectionStatus
    {
        Connecting,
        Connected,
        Disconnected
    }
    public class ConnectionInfo : DataBlock
    {
        private ConnectionStatus status;
        public event EventHandler StatusChanged;
        public ConnectionStatus Status
        {
            get
            {
                return status;
            }
            set
            {
                if (value == ConnectionStatus.Connected)
                {
                    SendedBytes = 0;
                    ReceivedBytes = 0;

                    RC4Enc = null;
                    RC4Dec = null;
                    MPPC = null;
                }
                status = value;
                try
                {
                    StatusChanged(this, new EventArgs());
                }
                catch
                {

                }
            }
        }

        public GameServer GameServer { get; internal set; }
        public SocketFactory SocketFactory { get; internal set; }

        public ServerVersion ServerVersion { get; set; }
        public float ServerStatus { get; set; }
        public string CRC { get; set; }
        public byte Bonus { get; set; }

        public MD5Hash MD5 { get; set; }

        internal Rc4Encryption RC4Enc { get; set; }
        internal Rc4Encryption RC4Dec { get; set; }
        internal MppcUnpacker MPPC { get; set; }

        public byte[] S01Key { get; set; }

        public byte[] SMKey { get; set; }
        public byte[] CMKey { get; set; }

        public long SendedBytes { get; internal set; }
        public long ReceivedBytes { get; internal set; }

        public long TotalSendedBytes { get; internal set; }
        public long TotalReceivedBytes { get; internal set; }
        
        protected internal override void Initialize(OOGHost host)
        {
            StatusChanged = (a, b) => { };
            MD5 = new MD5Hash();
            GameServer = host.GameServer;
            SocketFactory = host.SocketFactory;
        }

    }
}
