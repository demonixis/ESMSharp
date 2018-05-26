using ESMSharp.Core;
using ESMSharp.TES4.Records;
using System.IO;

namespace ESMSharp.TES4
{
    public class Record
    {
        protected string type;
        protected uint dataSize;
        protected uint flags;
        protected uint id;
        protected uint revision;
        protected uint version;
        protected uint unknow;

        public string Type
        {
            get { return type; }
        }

        public bool Compressed
        {
            get { return (flags & 0x00040000) != 0; }
        }

        public bool Deleted
        {
            get { return (flags & 0x20) != 0; }
        }

        public bool Ignored
        {
            get { return (flags & 0x1000) != 0; }
        }

        public virtual void Deserialize(BetterReader reader, string name, GameID gameID)
        {
            type = name;
            dataSize = reader.ReadUInt32();
            flags = reader.ReadUInt32();
            id = reader.ReadUInt32();
            revision = reader.ReadUInt32();

            if (gameID != GameID.Oblivion)
            {
                version = reader.ReadUInt16();
                unknow = reader.ReadUInt16();
            }

            if (Deleted)
            {
                reader.ReadBytes((int)dataSize);
                return;
            }

            if (Compressed)
            {
                var decompSize = (int)reader.ReadUInt32();
                var compressedData = reader.ReadBytes((int)dataSize - 4);

                Utils.LogBuffer("\t\tCompressed Data {0}", type);

                var decompressedData = Decompress(compressedData);
                using (var betterReader = new BetterMemoryReader(decompressedData))
                    ExtractSubRecords(betterReader, gameID, (uint)betterReader.Length);
            }
            else
                ExtractSubRecords(reader, gameID, dataSize);
        }

        protected virtual void ExtractSubRecords(BetterReader reader, GameID gameID, uint size)
        {
            reader.ReadBytes((int)size);
        }

        private static byte[] Decompress(byte[] data)
        {
            using (var compressedStream = new MemoryStream(data))
            {
                using (var outZStream = new zlib.ZOutputStream(compressedStream, 6))
                {
                    using (var resultStream = new MemoryStream())
                    {
                        CopyStream(resultStream, outZStream);
                        return resultStream.ToArray();
                    }
                }
            }
        }

        public static void CopyStream(Stream input, Stream output)
        {
            byte[] buffer = new byte[2000];
            int len;
            while ((len = input.Read(buffer, 0, 2000)) > 0)
            {
                output.Write(buffer, 0, len);
            }
            output.Flush();
        }

        public static Record GetRecord(string name)
        {
            if (name == "ACHR")
                return new ACHRRecord();
            else if (name == "ACRE")
                return new ACRERecord();
            else if (name == "ACTI")
                return new ACTIRecord();
            else if (name == "ALCH")
                return new ALCHRecord();
            else if (name == "AMMO")
                return new AMMORecord();
            else if (name == "ANIO")
                return new ANIORecord();
            else if (name == "APPA")
                return new APPARecord();
            else if (name == "ARMO")
                return new ARMORecord();
            else if (name == "BOOK")
                return new BOOKRecord();
            else if (name == "BSGN")
                return new BSGNRecord();
            else if (name == "CELL")
                return new CELLRecord();
            else if (name == "CLAS")
                return new CLASRecord();
            else if (name == "CLMT")
                return new CLMTRecord();
            else if (name == "CLOT")
                return new CLOTRecord();
            else if (name == "CONT")
                return new CONTRecord();
            else if (name == "CREA")
                return new CREARecord();
            else if (name == "CSTY")
                return new CSTYRecord();
            else if (name == "DIAL")
                return new DIALRecord();
            else if (name == "DOOR")
                return new DOORRecord();
            else if (name == "EFSH")
                return new EFSHRecord();
            else if (name == "ENCH")
                return new ENCHRecord();
            else if (name == "EYES")
                return new EYESRecord();
            else if (name == "FACT")
                return new FACTRecord();
            else if (name == "FLOR")
                return new FLORRecord();
            else if (name == "FURN")
                return new FURNRecord();
            else if (name == "GLOB")
                return new GLOBRecord();
            else if (name == "GMST")
                return new GMSTRecord();
            else if (name == "GRAS")
                return new GRASRecord();
            else if (name == "HAIR")
                return new HAIRRecord();
            else if (name == "IDLE")
                return new IDLERecord();
            else if (name == "INFO")
                return new INFORecord();
            else if (name == "INGR")
                return new INGRRecord();
            else if (name == "KEYM")
                return new KEYMRecord();
            else if (name == "LAND")
                return new LANDRecord();
            else if (name == "LIGH")
                return new LIGHRecord();
            else if (name == "LSCR")
                return new LSCRRecord();
            else if (name == "LTEX")
                return new LTEXRecord();
            else if (name == "LVLC")
                return new LVLCRecord();
            else if (name == "LVLI")
                return new LVLIRecord();
            else if (name == "LVSP")
                return new LVSPRecord();
            else if (name == "MGEF")
                return new MGEFRecord();
            else if (name == "MISC")
                return new MISCRecord();
            else if (name == "NPC_")
                return new NPC_Record();
            else if (name == "PACK")
                return new PACKRecord();
            else if (name == "PGRD")
                return new PGRDRecord();
            else if (name == "QUST")
                return new QUSTRecord();
            else if (name == "RACE")
                return new RACERecord();
            else if (name == "REFR")
                return new REFRRecord();
            else if (name == "REGN")
                return new REGNRecord();
            else if (name == "ROAD")
                return new ROADRecord();
            else if (name == "SBSP")
                return new SBSPRecord();
            else if (name == "SCPT")
                return new SCPTRecord();
            else if (name == "SGST")
                return new SGSTRecord();
            else if (name == "SKIL")
                return new SKILRecord();
            else if (name == "SLGM")
                return new SLGMRecord();
            else if (name == "SOUN")
                return new SOUNRecord();
            else if (name == "SPEL")
                return new SPELRecord();
            else if (name == "STAT")
                return new STATRecord();
            else if (name == "TES4")
                return new TES4Record();
            else if (name == "TREE")
                return new TREERecord();
            else if (name == "WATR")
                return new WATRRecord();
            else if (name == "WEAP")
                return new WEAPRecord();
            else if (name == "WRLD")
                return new WRLDRecord();
            else if (name == "WTHR")
                return new WTHRRecord();

            return new Record();
        }
    }
}
