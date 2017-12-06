using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.Data;
using Magic.Net.Packets;
using Magic.Net.Packets.Server;
using Magic.Net.Plugins.Sub;

namespace Magic.Net.Plugins
{
    public class RandomRolesPlugin : Plugin
    {
        public byte[] Face { get; set; }
        public Gender Gender { get; set; }
        public Occupation Occupation { get; set; }
        public AccountRolesPlugin AccountRoles { get; private set; }
        public RandomStringGenerator RandomString { get; private set; }


        private object lockObj = new object();
        
        public int QueueSize { get; private set; }

        public void CreateRandomRole()
        {
            if (Host.ConnectionInfo.Status != ConnectionStatus.Connected) return;

            lock (lockObj)
            {
                var name = RandomString.GetRandomString();
                if (Face == null) Face = AccountRolesPlugin.DefaultFace;
                if (Gender == null) Gender = Gender.Male;
                if (Occupation == null) Occupation = Occupation.Warrior;

                QueueSize++;
                AccountRoles.CreateRole(name, Gender, Occupation, Face);
            }
        }

        protected internal override void Initialize()
        {
            RandomString = new RandomStringGenerator();

            AccountRoles = Host.Plugins.Register<AccountRolesPlugin>();
            Host.Handler.AddHandler<CreateRole_ReS55>(OnCreateRole);
            Host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            base.Initialize();
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            if (Host.ConnectionInfo.Status == ConnectionStatus.Disconnected)
            {
                lock (lockObj)
                {
                    QueueSize = 0;
                }
            }
        }

        private void OnCreateRole(object sender, PacketEventArgs e)
        {
            if (Enabled)
            {
                var role = (CreateRole_ReS55)e.Packet;

                lock (lockObj)
                {
                    if (role.ResultCode.ResultCode != 0)
                    {
                        if (QueueSize != 0)
                        {
                            QueueSize--;
                            CreateRandomRole();
                        }
                    }
                    else
                    {
                        if (QueueSize != 0)
                        {
                            QueueSize--;
                        }
                    }
                }
            }
        }
    }
}
