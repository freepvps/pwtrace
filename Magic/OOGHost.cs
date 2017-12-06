using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net;
using Magic.Data;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.IO;

namespace Magic
{
    public class OOGHost
    {
        public GameServer GameServer { get; private set; }
        public SocketFactory SocketFactory { get; private set; }
        public PacketsRegistry PacketsRegistry { get; private set; }
        public PacketsProcessor Processor { get; private set; }
        public PacketsHandler Handler { get; protected set; }
        public PluginManager Plugins { get; private set; }
        public VersionControl Version { get; set; }

        public ConnectionData Data { get; private set; }
        
        private Connection Connection { get; set; }
        public ConnectionInfo ConnectionInfo { get; private set; }

        public bool Started { get; private set; }

        public OOGHost(string server) : this(GameServer.Parse(server), new SocketFactory())
        {
            
        }
        public OOGHost(string host, int port) : this (new GameServer(host, port), new SocketFactory())
        {

        }
        public OOGHost(GameServer gameServer) : this(gameServer, new SocketFactory())
        {

        }
        public OOGHost(GameServer gameServer, SocketFactory socketFactory) : this(gameServer, socketFactory, PacketsRegistry.Create())
        {
        }
        public OOGHost(GameServer gameServer, PacketsRegistry packetsRegistry) : this(gameServer, new SocketFactory(), packetsRegistry)
        {

        }
        public OOGHost(GameServer gameServer, SocketFactory socketFactory, PacketsRegistry packetsRegistry) : this(gameServer, socketFactory, packetsRegistry, VersionControl.Any)
        {
        }
        public OOGHost(GameServer gameServer, VersionControl versionControl) : this(gameServer, new SocketFactory(), versionControl)
        {

        }
        public OOGHost(GameServer gameServer, SocketFactory socketFactory, VersionControl versionControl) : this(gameServer, socketFactory, PacketsRegistry.Create(versionControl), versionControl)
        {
        }
        public OOGHost(GameServer gameServer, PacketsRegistry packetsRegistry, VersionControl versionControl) : this(gameServer, new SocketFactory(), packetsRegistry, versionControl)
        {

        }
        public OOGHost(GameServer gameServer, SocketFactory socketFactory, PacketsRegistry packetsRegistry, VersionControl versionControl)
        {
            Version = versionControl;

            GameServer = gameServer;
            SocketFactory = socketFactory;
            PacketsRegistry = packetsRegistry;

            Handler = new PacketsHandler();
            Processor = new PacketsProcessor(this);
            Data = new ConnectionData(this);
            Plugins = new PluginManager(this);

            Connection = new Connection(this);
            ConnectionInfo = Data.Register<ConnectionInfo>();
        }
        public void Start()
        {
            lock (Connection)
            {
                if (Started)
                {
                    return;
                }
                Started = true;
            }
            Plugins.OnStart();
            Connect();
        }
        public void Stop()
        {
            lock (Connection)
            {
                if (!Started)
                {
                    return;
                }
                Started = false;
            }
            Plugins.OnStop();
            Close();
        }
        public void Connect()
        {
            lock(Connection)
            {
                if (!Started)
                {
                    Start();
                    return;
                }
            }
            Connection.Connect();
        }
        public void Close()
        {
            Connection.Close();
        }

        private DataStream sendStream = new DataStream();
        public void Send(GamePacket gamePacket, bool handleData = true)
        {
            Send(gamePacket, GamePacket.GetOnePacketIdentifier(gamePacket), handleData);
        }
        public void Send(GamePacket gamePacket, PacketIdentifier packetId, bool handleData = true)
        {
            lock(sendStream)
            {
                Processor.ProcessClientPacket(gamePacket, packetId, sendStream, handleData);

                var count = sendStream.Count;

                Send(sendStream.Buffer, 0, count);
                sendStream.Clear();
            }
        }
        public void Send(byte[] buffer, int offset, int count)
        {
            lock (Connection)
            {
                Connection.Send(buffer, offset, count);
            }
        }
    }
}
