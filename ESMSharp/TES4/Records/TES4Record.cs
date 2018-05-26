using ESMSharp.Core;

namespace ESMSharp.TES4.Records
{
    public class TES4Record : Record
    {
        public class HEDRSubRecord : SubRecord
        {
            public float Version { get; protected set; }
            public uint NumRecords { get; protected set; }
            public uint NextObjectID { get; protected set; }

            public override void Deserialize(BetterReader reader, string name)
            {
                base.Deserialize(reader, name);
                Version = reader.ReadSingle();
                NumRecords = reader.ReadUInt32();
                NextObjectID = reader.ReadUInt32();
            }
        }

        public HEDRSubRecord Header { get; private set; }
        public BytesSubRecord Offset { get; private set; }
        public BytesSubRecord DELE { get; private set; }
        public STRSubRecord Author { get; private set; }
        public STRSubRecord Description { get; private set; }
        public STRSubRecord Master { get; private set; }
        public UInt64SubRecord FileSize { get; private set; }

        protected override void ExtractSubRecords(BetterReader reader, GameID gameId, uint size)
        {
            var endRead = reader.Position + size;
            var name = string.Empty;
     
            while (reader.Position < endRead)
            {
                name = reader.ReadString(4);

                switch (name)
                {
                    case "HEDR":
                        Header = new HEDRSubRecord();
                        Header.Deserialize(reader, name);
                        break;

                    case "OFST":
                        Offset = new BytesSubRecord();
                        Offset.Deserialize(reader, name);
                        break;

                    case "DELE":
                        DELE = new BytesSubRecord();
                        DELE.Deserialize(reader, name);
                        break;

                    case "CNAM":
                        Author = new STRSubRecord();
                        Author.Deserialize(reader, name);
                        break;

                    case "SNAM":
                        Description = new STRSubRecord();
                        Description.Deserialize(reader, name);
                        break;

                    case "MAST":
                        Master = new STRSubRecord();
                        Master.Deserialize(reader, name);
                        break;

                    case "DATA":
                        FileSize = new UInt64SubRecord();
                        FileSize.Deserialize(reader, name);
                        break;

                    default:
                        var dSize = reader.ReadInt16();
                        reader.ReadBytes(dSize);
                        break;
                }
            }
        }
    }
}
