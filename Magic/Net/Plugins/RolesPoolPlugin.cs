using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data;

namespace Magic.Net.Plugins
{
    public class RolesPoolPlugin : Plugin
    {
        public RandomRolesPlugin RandomRoles { get; private set; }
        public AccountInfo Account { get; private set; }

        public const int MaxPoolSize = 10;
        public int PoolSize { get; set; }

        protected internal override void Initialize()
        {
            PoolSize = 1;

            RandomRoles = Host.Plugins.Register<RandomRolesPlugin>();
            Account = Host.Data.Register<AccountInfo>();

            Account.RolesListChanged += Account_RolesListChanged;  

            base.Initialize();
        }

        private void Account_RolesListChanged(object sender, EventArgs e)
        {
            if (Enabled && Account.RolesLoaded && Account.Roles.Count < PoolSize)
            {
                RandomRoles.CreateRandomRole();
            }
        }
    }
}
