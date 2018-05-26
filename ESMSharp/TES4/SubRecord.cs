using ESMSharp.Core;

namespace ESMSharp.TES4
{
    public class SubRecord
    {
        public string Name { get; protected set; }
        public uint Size { get; protected set; }

        public virtual void Deserialize(BetterReader reader, string name)
        {
            Name = name;
            Size = reader.ReadUInt16();
        }
    }
}
