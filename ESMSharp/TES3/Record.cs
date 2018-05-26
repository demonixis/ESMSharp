using ESMSharp.Core;
using System;

namespace ESMSharp.TES3
{
    public class Record
    {
        protected string type;
        protected uint dataSize;
        protected uint flags;
        protected uint unknow;

        public string Type
        {
            get { return type; }
        }

        public bool Blocked
        {
            get { return (flags & 0x00002000) != 0; }
        }

        public bool Persistant
        {
            get { return (flags & 0x00000400) != 0; }
        }

        public virtual void Deserialize(BetterReader reader, string name)
        {
            type = name;
            dataSize = reader.ReadUInt32();
            unknow = reader.ReadUInt32();
            flags = reader.ReadUInt32();
            ExtractSubRecords(reader, dataSize);
        }

        protected virtual void ExtractSubRecords(BetterReader reader, uint size)
        {
            reader.ReadBytes((int)size);
        }
    }
}
