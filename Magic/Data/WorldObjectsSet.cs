using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Magic.Data.Types;

namespace Magic.Data
{
    public class WorldObjectsSet : DataBlock
    {
        public Dictionary<uint, WorldObject> Objects = new Dictionary<uint, WorldObject>();


        public void Add(WorldObject worldObject)
        {
            lock(Objects)
                Objects[worldObject.WorldId] = worldObject;
        }
        public void Remove(uint worldId)
        {
            lock (Objects)
                Objects.Remove(worldId);
        }
        public void Clear()
        {
            lock (Objects)
                Objects.Clear();
        }

        protected internal override void Initialize(OOGHost host)
        {
            host.ConnectionInfo.StatusChanged += ConnectionInfo_StatusChanged;

            base.Initialize(host);
        }

        private void ConnectionInfo_StatusChanged(object sender, EventArgs e)
        {
            Clear();
        }
    }
}
