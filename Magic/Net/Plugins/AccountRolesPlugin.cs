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
    public class AccountRolesPlugin : Plugin
    {
        public AccountInfo Account { get; private set; }
        public ConnectionInfo Connection { get; private set; }
        
        public virtual void EnterWorld(int slot)
        {
            Host.Send(new SelectRoleC46(slot));
        }
        public virtual void EnterWorld(uint roleId)
        {
            Host.Send(new SelectRoleC46(roleId));
        }

        protected internal override void Initialize()
        {
            Host.Plugins.Register<KeepAlivePlugin>();

            Account = Host.Data.Register<AccountInfo>();
            Connection = Host.Data.Register<ConnectionInfo>();
            
            Host.Handler.AddHandler<OnlineAnnounceS04>(Receive_RoleSelectionPage);
            Host.Handler.AddHandler<RoleLogoutS45>(Receive_RoleSelectionPage);
            Host.Handler.AddHandler<RoleList_ReS53>(Receive_RoleList_Re);
            Host.Handler.AddHandler<SelectRole_ReS47>(Receive_SelectRole_Re);
        }

        public static readonly byte[] EmptyFace = { 0x01, 0x70, 0x00, 0x10 };
        public static byte[] DefaultFace
        {
            get
            {
                return EmptyFace;
            }
        }

        public virtual void CreateRole(string name, Gender gender, Occupation occupation)
        {
            CreateRole(name, gender, occupation, EmptyFace);
        }
        public virtual void CreateRole(string name, Gender gender, Occupation occupation, byte[] face)
        {
            Host.Send(new CreateRoleC54(name, gender, occupation, face));
        }
        public virtual void Logout()
        {
            Host.Send(new LogoutC01());
        }

        protected virtual void Receive_RoleSelectionPage(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                Host.Send(new RoleListC52(-1));
            }
        }
        protected virtual void Receive_RoleList_Re(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                RoleList_ReS53 roleListRe = e.Packet as RoleList_ReS53;
                if (roleListRe.IsChar)
                {
                    Host.Send(new RoleListC52(roleListRe.NextSlot));
                }
                else
                {
                    Account.RolesLoaded = true;
                }
            }
        }
        protected virtual void Receive_SelectRole_Re(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                Host.Send(new EnterWorldC48());
            }
        }
    }
}
