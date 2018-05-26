using ESMSharp.Core;

namespace ESMSharp.TES4.Records
{
    public class WRLDRecord : Record
    {
        public STRSubRecord EDID { get; private set; }
        public STRSubRecord FULL { get; private set; }
        public STRSubRecord WNAM { get; private set; }
        public STRSubRecord SNAM { get; private set; }
        public STRSubRecord ICON { get; private set; }
        public UInt32SubRecord CNAM { get; private set; }
        public UInt32SubRecord NAM2 { get; private set; }
        public ByteSubRecord DATA { get; private set; }
        public Vector2iSubRecord NAM0 { get; private set; }
        public Vector2iSubRecord NAM9 { get; private set; }

        protected override void ExtractSubRecords(BetterReader reader, GameID gameID, uint size)
        {
            base.ExtractSubRecords(reader, gameID, size);
            
            /*
            var name = string.Empty;
            var end = reader.Position + size;

            while (reader.Position < end)
            {
                name = reader.ReadString(4);

                switch (name)
                {
                    case "EDID":
                        EDID = new STRSubRecord();
                        EDID.Deserialize(reader, name);
                        break;

                    case "FULL":
                        FULL = new STRSubRecord();
                        FULL.Deserialize(reader, name);
                        break;

                    case "WNAM":
                        WNAM = new STRSubRecord();
                        WNAM.Deserialize(reader, name);
                        break;

                    case "SNAM":
                        SNAM = new STRSubRecord();
                        SNAM.Deserialize(reader, name);
                        break;

                    case "DATA":
                        DATA = new ByteSubRecord();
                        DATA.Deserialize(reader, name);
                        break;

                    case "NAM0":
                        NAM0 = new Vector2iSubRecord();
                        NAM0.Deserialize(reader, name);
                        break;

                    case "NAM9":
                        NAM9 = new Vector2iSubRecord();
                        NAM9.Deserialize(reader, name);
                        break;

                    default:
                        var rest = reader.ReadUInt16();
                        reader.ReadBytes(rest);
                        break;
                }
            }*/
        }
    }
}
