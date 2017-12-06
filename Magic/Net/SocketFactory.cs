using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

namespace Magic.Net
{
    public class SocketFactory
    {
        public EndPoint LocalEndPoint { get; private set; }

        public virtual Socket BuildSocket()
        {
            var skt = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            BindSocket(skt);
            skt.ReceiveTimeout = 30000;
            return skt;
        }
        protected virtual void BindSocket(Socket skt)
        {
            if (LocalEndPoint != null)
            {
                skt.Bind(LocalEndPoint);
            }
        }
        public virtual void Bind(EndPoint endPoint)
        {
            LocalEndPoint = endPoint;
        }
    }
}
