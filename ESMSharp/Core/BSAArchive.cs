using System;
using System.Collections.Generic;
using System.IO;

namespace ESMSharp.Core
{
    public class BSAArchive
    {
        BetterBinaryReader _reader;
        Dictionary<ulong, BSAFileInfo> _files;

        public BSAArchive(string filename)
        {
            _files = new Dictionary<ulong, BSAFileInfo>();
            _reader = new BetterBinaryReader(File.OpenRead(filename));
        }

        public void Close()
        {
            _reader.Dispose();
            _files.Clear();
        }

        public void Load()
        {
            var header = new BSAHeader();
            header.Deserialize(_reader);

            var skipNames = (header.Flags & 0x100) != 0 && header.Version == 0x68;
            var folderInfos = new BSARecord[header.FolderCount];
            var fileInfos = new BSARecord[header.FileCount];
            var fileNames = new string[header.FileCount];

            var c = '0';
            var i = 0;
            var j = 0;
            var count = 0;
            var pathSize = 0;

            for (i = 0; i < header.FolderCount; i++)
                folderInfos[i] = new BSARecord(_reader);

            for (i = 0; i < header.FolderCount; i++)
            {
                pathSize = _reader.ReadByte() - 1;
                folderInfos[i].Path = _reader.ReadString(pathSize);
                _reader.Position++;
                folderInfos[i].Offset = count;

                for (j = 0; j < folderInfos[i].Count; j++)
                    fileInfos[count + j] = new BSARecord(_reader);

                count += folderInfos[i].Count;
            }

            for (i = 0; i < header.FileCount; i++)
            {
                fileInfos[i].Path = String.Empty;
                c = _reader.ReadChar();

                while (c != '\0')
                {
                    fileInfos[i].Path += c;
                    c = _reader.ReadChar();
                }
            }

            for (i = 0; i < header.FolderCount; i++)
            {
                for (j = 0; j < folderInfos[i].Count; j++)
                {
                    var fileInfo = fileInfos[folderInfos[i].Offset + j];
                    var extension = Path.GetExtension(fileInfo.Path);
                    var bsaFileInfo = new BSAFileInfo((int)fileInfo.Offset, fileInfo.Count, skipNames, header.Compressed);
                    var filePath = Path.Combine(folderInfos[i].Path, Path.GetFileNameWithoutExtension(fileInfo.Path));
                    var hash = GenHash(filePath, extension);
                    _files.Add(hash, bsaFileInfo);
                    fileNames[folderInfos[i].Offset + j] = filePath + extension;
                }
            }

            Array.Sort(fileNames);
        }

        public byte[] GetFile(string path)
        {
            var hash = GenHash(path);

            if (_files.ContainsKey(hash))
                return _files[hash].GetRawData(_reader);

            return null;
        }

        private static ulong GenHash(string file)
        {
            file = file.ToLower().Replace('/', '\\');
            return GenHash(Path.ChangeExtension(file, null), Path.GetExtension(file));
        }

        private static ulong GenHash(string file, string ext)
        {
            file = file.ToLower();
            ext = ext.ToLower();
            ulong hash = 0;

            if (file.Length > 0)
            {
                hash = (ulong)(
                   (((byte)file[file.Length - 1]) * 0x1) +
                    ((file.Length > 2 ? (byte)file[file.Length - 2] : (byte)0) * 0x100) +
                     (file.Length * 0x10000) +
                    (((byte)file[0]) * 0x1000000)
                );
            }

            if (file.Length > 3)
                hash += (ulong)(GenHash2(file.Substring(1, file.Length - 3)) * 0x100000000);

            if (ext.Length > 0)
            {
                hash += (ulong)(GenHash2(ext) * 0x100000000);
                byte i = 0;

                switch (ext)
                {
                    case ".nif": i = 1; break;
                    //case ".kf": i=2; break;
                    case ".dds": i = 3; break;
                        //case ".wav": i=4; break;
                }

                if (i != 0)
                {
                    byte a = (byte)(((i & 0xfc) << 5) + (byte)((hash & 0xff000000) >> 24));
                    byte b = (byte)(((i & 0xfe) << 6) + (byte)(hash & 0xff));
                    byte c = (byte)((i << 7) + (byte)((hash & 0xff00) >> 8));
                    hash -= hash & 0xFF00FFFF;
                    hash += (uint)((a << 24) + b + (c << 8));
                }
            }
            return hash;
        }

        private static uint GenHash2(string s)
        {
            uint hash = 0;
            for (int i = 0; i < s.Length; i++)
            {
                hash *= 0x1003f;
                hash += (byte)s[i];
            }
            return hash;
        }
    }
}
