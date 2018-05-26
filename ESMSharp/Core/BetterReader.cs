using System;
using System.IO;

namespace ESMSharp.Core
{
    public abstract class BetterReader : IDisposable
    {
        public abstract long Position { get; set; }
        public abstract long Length { get; }
        public abstract long Seek(long offset, SeekOrigin origin);
        public abstract int Read(byte[] buffer, int offset, int count);
        public abstract char ReadChar();
        public abstract byte ReadByte();
        public abstract byte[] ReadBytes(int count);
        public abstract bool ReadBool32();
        public abstract ushort ReadUInt16();
        public abstract uint ReadUInt32();
        public abstract ulong ReadUInt64();
        public abstract short ReadInt16();
        public abstract int ReadInt32();
        public abstract long ReadInt64();
        public abstract float ReadSingle();
        public abstract double ReadDouble();
        public abstract string ReadString(int length);
        public abstract string ReadNullTerminatedString(int length);

        public void Dispose()
        {
            Dispose(false);
        }

        protected abstract void Dispose(bool disposed);
    }
}
