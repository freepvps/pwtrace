using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using Magic.Data;
using Magic.IO;
using Magic.Threading;

namespace Magic.Net
{
    public class Connection
    {
        Socket socket;

        object socketLock = new object();

        public OOGHost Host { get; private set; }
        private ConnectionData Data { get; set; }
        private ConnectionInfo Information { get; set; }

        public bool IsWork { get; private set; }
        public ActionTimer LifeChecker { get; private set; }

        public int LifeTimeout { get; set; } = 30000;

        SocketAsyncEventArgs socketArgsRecv;
        AsyncCallback socketAsyncConnect;
        DataStream ReceiveStream;

        public Connection(OOGHost host)
        {

            Host = host;
            Data = host.Data;
            Information = host.Data.Register<ConnectionInfo>();

            ReceiveStream = new DataStream();

            socketAsyncConnect = new AsyncCallback(EndConnect);
            socketArgsRecv = new SocketAsyncEventArgs();

            socketArgsRecv.SetBuffer(new byte[1024], 0, 1024);
            socketArgsRecv.Completed += socketArgsRecv_Completed;

            LifeChecker = new ActionTimer(LifeCheck);
        }

        private DateTime LastUpdate;
        private void Pulse()
        {
            LastUpdate = DateTime.Now;
        }
        private void LifeCheck(object obj)
        {
            lock (socketLock)
            {
                if (!IsWork)
                {
                    LifeChecker.Stop();
                    return;
                }
            }
            var now = DateTime.Now;
            if (LastUpdate.AddMilliseconds(LifeTimeout) < now)
            {
                Close();
            }

        }
        public void Connect()
        {
            lock (socketLock)
            {
                if (IsWork)
                    return;
                IsWork = true;
                Pulse();
                if (LifeTimeout > 0)
                    LifeChecker.Start(LifeTimeout, LifeTimeout);
            }
            BeginConnect();
        }
        private void BeginConnect()
        {
            Information.Status = ConnectionStatus.Connecting;

            ReceiveStream.Clear();
            socket = Host.SocketFactory.BuildSocket();
            socket.BeginConnect(Host.GameServer.Host, Host.GameServer.Port, socketAsyncConnect, socket);
        }
        private void EndConnect(IAsyncResult res)
        {
            try
            {
                var skt = res.AsyncState as Socket;
                skt.EndConnect(res);

                if (!skt.Connected)
                {
                    Close();
                    return;
                }
            }
            catch
            {
                Close();
                return;
            }
            
            Information.Status = ConnectionStatus.Connected;
            StartReceive();
        }
        public void Close()
        {
            lock (socketLock)
            {
                if (!IsWork) return;
                IsWork = false;
                LifeChecker.Stop();
            }
            if (socket != null)
            {
                DisposeSocket(socket);
            }
            Information.Status = ConnectionStatus.Disconnected;
        }
        private void DisposeSocket(Socket socket)
        {
            if (socket == null) return;
            try
            {
                socket.Shutdown(SocketShutdown.Both);
            }
            catch
            {

            }
            try
            {
                socket.Close();
            }
            catch
            {

            }
            try
            {
                socket.Dispose();
            }
            catch
            {

            }
        }


        // RECEIVE ASYNC
        private void StartReceive()
        {
            try
            {
                if (!socket.ReceiveAsync(socketArgsRecv))
                {
                    ReceiveProcess(socketArgsRecv);
                }
            }
            catch
            {
                Close();
            }
        }
        private void socketArgsRecv_Completed(object sender, SocketAsyncEventArgs e)
        {
            ReceiveProcess(e);
        }
        private void ReceiveProcess(SocketAsyncEventArgs socketArgs)
        {
            if (!IsWork)
                return;
            try
            {
                if (socketArgs.SocketError == SocketError.SocketError ||
                    socketArgs.BytesTransferred <= 0)
                {
                    Close();
                    return;
                }
                Pulse();
                Information.ReceivedBytes += socketArgs.BytesTransferred;
                Information.TotalReceivedBytes += socketArgs.BytesTransferred;

                var buffer = socketArgs.Buffer;
                var count = socketArgs.BytesTransferred;
                if (Information.RC4Dec != null)
                {
                    Information.RC4Dec.Decrypt(buffer, 0, count);
                }
                if (Information.MPPC != null)
                {
                    buffer = Information.MPPC.Unpack(buffer, 0, count);
                    count = buffer.Length;
                }

                ReceiveStream.PushBack(buffer, 0, count);
            }
            catch
            {
                Close();
            }

            Host.Processor.ProcessServerStream(ReceiveStream, true);
            StartReceive();
        }
        
        public void Send(byte[] buffer)
        {
            Send(buffer, 0, buffer.Length);
        }
        public void Send(byte[] buffer, int offset, int count)
        {
            if (!IsWork) return;
            try
            {
                lock (socket)
                {
                    if (Information.RC4Enc != null)
                    {
                        Information.RC4Enc.Encrypt(buffer, offset, count);
                    }
                    
                    socket.Send(buffer, offset, count, SocketFlags.None);
                    Information.SendedBytes += count;
                    Information.TotalSendedBytes += count;
                }
            }
            catch
            {
                Close();
            }
        }
    }
}
