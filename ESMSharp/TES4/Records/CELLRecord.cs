using ESMSharp.Core;

namespace ESMSharp.TES4.Records
{
    public class CELLRecord : Record
    {
        public STRSubRecord EDID { get; private set; }
        public STRSubRecord FULL { get; private set; }
        public ByteSubRecord DATA { get; private set; }
        public BytesSubRecord XCLL { get; private set; }
        public ByteSubRecord XCMT { get; private set; }
        public UInt32SubRecord XOWN { get; private set; }
        public UInt32SubRecord XGLB { get; private set; }
        public UInt32SubRecord XRNK { get; private set; }
        public UInt32SubRecord XCCM { get; private set; }
        public UInt32SubRecord XCWT { get; private set; }
        public FloatSubRecord XCLW { get; private set; }
        public UInt32SubRecord XCLR { get; private set; }
        public Vector2iSubRecord XCLC { get; private set; }

        protected override void ExtractSubRecords(BetterReader reader, GameID gameID, uint size)
        {
            var bytes = reader.ReadBytes((int)size);
            var name = string.Empty;

            using (var stream = new BetterMemoryReader(bytes))
            {
                var end = stream.Length;

                while (stream.Position < end)
                {
                    name = stream.ReadString(4);

                    switch (name)
                    {
                        case "EDID":
                            EDID = new STRSubRecord();
                            EDID.Deserialize(stream, name);
                            break;

                        case "FULL":
                            FULL = new STRSubRecord();
                            FULL.Deserialize(stream, name);
                            break;

                        case "DATA":
                            DATA = new ByteSubRecord();
                            DATA.Deserialize(stream, name);
                            break;

                        case "XCLL":
                            XCLL = new BytesSubRecord();
                            XCLL.Deserialize(stream, name);
                            break;

                        case "XCMT":
                            XCMT = new ByteSubRecord();
                            XCMT.Deserialize(stream, name);
                            break;

                        case "XOWN":
                            XOWN = new UInt32SubRecord();
                            XOWN.Deserialize(stream, name);
                            break;

                        case "XGLB":
                            XGLB = new UInt32SubRecord();
                            XGLB.Deserialize(stream, name);
                            break;

                        case "XRNK":
                            XRNK = new UInt32SubRecord();
                            XRNK.Deserialize(stream, name);
                            break;

                        case "XCCM":
                            XCCM = new UInt32SubRecord();
                            XCCM.Deserialize(stream, name);
                            break;

                        case "XCWT":
                            XCWT = new UInt32SubRecord();
                            XCWT.Deserialize(stream, name);
                            break;

                        case "XCLW":
                            XCLW = new FloatSubRecord();
                            XCLW.Deserialize(stream, name);
                            break;

                        case "XCLR":
                            XCLR = new UInt32SubRecord();
                            XCLR.Deserialize(stream, name);
                            break;

                        case "XCLC":
                            XCLC = new Vector2iSubRecord();
                            XCLC.Deserialize(stream, name);
                            break;
                            
                        default:
                            var rest = stream.ReadUInt16();
                            stream.ReadBytes(rest);
                            break;
                    }
                }
            }            
        }
    }
}
