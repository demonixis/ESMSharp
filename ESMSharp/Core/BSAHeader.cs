namespace ESMSharp.Core
{
    class BSAHeader
    {
        public string FileID { get; private set; }
        public uint Version { get; private set; }
        public uint Offset { get; private set; }
        public uint Flags { get; private set; }
        public uint FolderCount { get; private set; }
        public uint FileCount { get; private set; }
        public uint FolderNameLength { get; private set; }
        public uint FileNameLength { get; private set; }
        public uint FileFlags { get; private set; }

        public bool ArchiveHasNameForDirectories
        {
            get { return (Flags & 0x1) != 0; }
        }

        public bool ArhiveHasNameForFiles
        {
            get { return (Flags & 0x2) != 0; }
        }

        public bool Compressed
        {
            get { return (Flags & 0x4) != 0; }
        }

        public bool Xbox360Archive
        {
            get { return (Flags & 0x40) != 0; }
        }

        public bool ContainsMeshes
        {
            get { return (FileFlags & 0x1) != 0; }
        } 

        public bool ContainsTextures
        {
            get { return (FileFlags & 0x2) != 0; }
        }

        public bool ContainsMenus
        {
            get { return (FileFlags & 0x4) != 0; }
        }

        public bool ContainsSounds
        {
            get { return (FileFlags & 0x8) != 0; }
        }

        public bool ContainsVoices
        {
            get { return (FileFlags & 0x10) != 0; }
        }

        public bool ContainsShaders
        {
            get { return (FileFlags & 0x20) != 0; }
        }

        public bool ContainsTrees
        {
            get { return (FileFlags & 0x40) != 0; }
        }

        public bool ContainsFonts
        {
            get { return (FileFlags & 0x80) != 0; }
        }

        public bool ContainsMisc
        {
            get { return (FileFlags & 0x100) != 0; }
        }

        public void Deserialize(BetterBinaryReader reader)
        {
            FileID = reader.ReadString(4);
            Version = reader.ReadUInt32();
            Offset = reader.ReadUInt32();
            Flags = reader.ReadUInt32();
            FolderCount = reader.ReadUInt32();
            FileCount = reader.ReadUInt32();
            FolderNameLength = reader.ReadUInt32();
            FileNameLength = reader.ReadUInt32();
            FileFlags = reader.ReadUInt32();
        }
    }
}
