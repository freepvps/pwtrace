using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.IO;
using Magic.Data.Types;

namespace Magic.Data
{
    public class AccountInfo : DataBlock
    {
        private static EventHandler emptyHandler = (a, b) => { };

        private ConnectionInfo ConnectionInfo;

        public string Login { get; set; }
        public string Password { get; set; }
        public bool Force { get; set; }

        public uint AccountId { get; set; }
        public uint UnkId { get; set; }
        public UnixTime RemainTime { get; set; }
        public byte ZoneId { get; set; }

        public UnixTime LastLoginTime { get; set; }
        public string LastLoginIP { get; set; }
        public string CurrentIP { get; set; }

        public RoleInfo SelectedRole { get; set; }
        public List<RoleInfo> Roles { get; set; }

        public void AddRole(RoleInfo role)
        {
            Roles.Add(role);
            RolesListChanged(this, new EventArgs());
        }
        public void SetRolesPage()
        {
            Roles.Clear();
            SelectedRole = null;

            RolesLoaded = false;
            EnteredWorld = false;
            RolesListChanged(this, new EventArgs());
        }


        private bool rolesLoaded = false;
        private bool enteredWorld = false;

        public event EventHandler RolesLoadedChanged = emptyHandler;
        public event EventHandler RolesListChanged = emptyHandler;
        public event EventHandler EnteredWorldChanged = emptyHandler;

        public bool RolesLoaded
        {
            get
            {
                return rolesLoaded;
            }
            set
            {
                rolesLoaded = value;
                RolesLoadedChanged(this, new EventArgs());
                RolesListChanged(this, new EventArgs());
            }
        }
        public bool EnteredWorld
        {
            get
            {
                return enteredWorld;
            }
            set
            {
                enteredWorld = value;
                EnteredWorldChanged(this, new EventArgs());
            }
        }

        protected internal override void Initialize(OOGHost host)
        {
            Roles = new List<RoleInfo>();

            ConnectionInfo = host.Data.Register<ConnectionInfo>();
            ConnectionInfo.StatusChanged += ConnectionInformation_StatusChanged;

            base.Initialize(host);
        }

        private void ConnectionInformation_StatusChanged(object sender, EventArgs e)
        {
            switch(ConnectionInfo.Status)
            {
                case ConnectionStatus.Disconnected:
                    EnteredWorld = false;
                    RolesLoaded = false;
                    return;
            }
        }
    }
}
