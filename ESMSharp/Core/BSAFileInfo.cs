using System.IO;

namespace ESMSharp.Core
{
    public class BSARecord
    {
        public string Path { get; set; }
        public ulong Hash { get; private set; }
        public int Count { get; private set; }
        public int Offset { get; set; }

        public BSARecord(BetterBinaryReader reader)
        {
            Hash = reader.ReadUInt64();
            Count = reader.ReadInt32();
            Offset = reader.ReadInt32();
        }
    }

    public class BSAFileInfo
    {
        private bool _compressed;
        private long _offset;
        private long _size;
        private bool _skipNames;

        public BSAFileInfo(int offset, int size, bool skipName, bool compressed)
        {
            _compressed = compressed;
            _offset = offset;
            _size = size;
            _skipNames = skipName;

            if ((_size & (1 << 30)) != 0)
            {
                _size ^= 1 << 30;
                _compressed = !compressed;
            }    
        }

        public byte[] GetRawData(BetterBinaryReader reader)
        {
            reader.Seek(_offset, SeekOrigin.Begin);

            if (_skipNames)
                reader.Position += 2;

            if (_compressed)
            {
                var size = reader.ReadUInt32();
                var b = new byte[_size - 4];
                var output = new byte[size];

                reader.Read(b, 0, (int)_size - 4);
                // Uncompress

                return output;
            }

            return reader.ReadBytes((int)_size);
        }
    }
}
