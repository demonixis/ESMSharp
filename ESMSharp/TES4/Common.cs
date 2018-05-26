using ESMSharp.Core;

namespace ESMSharp.TES4
{
    public class STRSubRecord : SubRecord
    {
        public string Value { get; protected set; }

        public override void Deserialize(BetterReader reader, string name)
        {
            base.Deserialize(reader, name);
            Value = reader.ReadNullTerminatedString((int)Size);
        }
    }

    public class UInt64SubRecord : SubRecord
    {
        public ulong Value { get; protected set; }

        public override void Deserialize(BetterReader reader, string name)
        {
            base.Deserialize(reader, name);
            Value = reader.ReadUInt64();
        }
    }

    public class DummySubRecord : SubRecord
    {
        public byte[] Value { get; protected set; }

        public override void Deserialize(BetterReader reader, string name)
        {
            base.Deserialize(reader, name);
            Value = reader.ReadBytes((int)Size);
        }
    }
}
