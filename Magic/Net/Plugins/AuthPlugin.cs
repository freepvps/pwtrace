using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Net.Packets;
using Magic.Net.Packets.Client;
using Magic.Net.Packets.Server;
using Magic.Data;
using Magic.Data.Types;

namespace Magic.Net.Plugins
{
    public class AuthPlugin : Plugin
    {
        public AccountInfo Account { get; private set; }
        public ConnectionInfo Connection { get; private set; }

        public virtual void SetAuthData(string login, string password, bool force)
        {
            Account.Login = login;
            Account.Password = password;
            Account.Force = force;
        }

        public virtual void LogIn(string login, string password, bool force)
        {
            SetAuthData(login, password, force);
            LogIn();
        }//
        public virtual void LogIn()
        {
            Host.Connect();
        }

        protected internal override void Initialize()
        {
            Host.Plugins.Register<KeepAlivePlugin>();

            Account = Host.Data.Register<AccountInfo>();
            Connection = Host.Data.Register<ConnectionInfo>();

            Host.Handler.AddHandler<ServerInfoS01>(Receive_ServerInfo);
            Host.Handler.AddHandler<SMKeyS02>(Receive_SMKey);
        }

        protected virtual void Receive_ServerInfo(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var hash = Connection.MD5.GetHash(Account.Login, Account.Password, Connection.S01Key);
                Host.Send(new LogginAnnounceC03(Account.Login, hash));
            }
        }
        protected virtual void Receive_SMKey(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                Host.Send(new CMKeyC02(Account.Force));
            }
        }
    }
}
