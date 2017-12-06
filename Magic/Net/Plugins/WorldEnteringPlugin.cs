using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;
using Magic.Net.Packets.Client;
using Magic.Data;
using Magic.Net.Packets.Server;

namespace Magic.Net.Plugins
{
    public class WorldEnteringPlugin : Plugin
    {
        private int selectedIndex;

        public uint SelectedRoleId { get; set; }
        public int SelectedIndex
        {
            get
            {
                return selectedIndex;
            }
            set
            {
                selectedIndex = value;
                SelectedRoleId = 0;
            }
        }

        public AccountInfo Account;
        public AccountRolesPlugin AccountRoles { get; private set; }

        protected internal override void Initialize()
        {
            Account = Host.Data.Register<AccountInfo>();
            AccountRoles = Host.Plugins.Register<AccountRolesPlugin>();

            Account.RolesListChanged += Account_RolesListChanged;
        }

        private void Account_RolesListChanged(object sender, EventArgs e)
        {
            if (Account.RolesLoaded && Enabled)
            {
                if (SelectedRoleId == 0)
                {
                    if (selectedIndex < 0 || selectedIndex >= Account.Roles.Count)
                    {
                        return;
                    }
                    SelectedRoleId = Account.Roles[SelectedIndex].RoleId;
                }

                AccountRoles.EnterWorld(SelectedRoleId);
            }
        }
    }
}
