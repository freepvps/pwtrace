using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Magic.Data
{
    public class ConnectionData
    {
        private static Type[] emptyTypes = { };
        private static object[] emptyArgs = { };
        private static Type dataBlockType = typeof(DataBlock);
        
        public OOGHost Host { get; private set; }
        private Dictionary<Type, DataBlock> DataBlocks { get; set; }
        
        public ConnectionData(OOGHost host)
        {
            Host = host;
            DataBlocks = new Dictionary<Type, DataBlock>();
        }

        public int Count
        {
            get
            {
                return DataBlocks.Count;
            }
        }

        public static bool IsDataBlock(Type type)
        {
            if (type == null)
            {
                throw new ArgumentNullException();
            }
            return type.IsSubclassOf(dataBlockType);
        }
        private static void ValidateType(Type type)
        {
            if (!IsDataBlock(type))
            {
                throw new ArgumentException();
            }
        }

        public T Register<T>() where T : DataBlock
        {
            return Register(typeof(T)) as T;
        }
        public DataBlock Register(Type type)
        {
            ValidateType(type);

            DataBlock dataBlock;
            if (!TryGetDataBlock(type, out dataBlock))
            {
                dataBlock = type.GetConstructor(emptyTypes).Invoke(emptyArgs) as DataBlock;
                DataBlocks[type] = dataBlock;

                dataBlock.Initialize(Host);
            }
            return dataBlock;
        }

        public T GetDataBlock<T>() where T : DataBlock
        {
            return GetDataBlock(typeof(T)) as T;
        }
        public DataBlock GetDataBlock(Type type)
        {
            ValidateType(type);

            return DataBlocks[type];
        }

        public bool TryGetDataBlock<T>(out T dataBlock) where T : DataBlock
        {
            DataBlock res;
            if (TryGetDataBlock(typeof(T), out res))
            {
                dataBlock = res as T;
                return true;
            }
            else
            {
                dataBlock = default(T);
                return false;
            }
        }
        public bool TryGetDataBlock(Type type, out DataBlock dataBlock)
        {
            ValidateType(type);

            return DataBlocks.TryGetValue(type, out dataBlock);
        }

        public bool Contains<T>() where T : DataBlock
        {
            return Contains(typeof(T));
        }
        public bool Contains(Type type)
        {
            ValidateType(type);
            return DataBlocks.ContainsKey(type);
        }

        public DataBlock[] ToArray()
        {
            var res = new DataBlock[DataBlocks.Count];
            var i = 0;
            foreach (var dataBlock in DataBlocks.Values)
            {
                res[i++] = dataBlock;
            }
            return res;
        }
    }
}
