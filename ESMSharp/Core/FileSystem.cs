using System.Collections.Generic;
using System.IO;

namespace ESMSharp.Core
{
    public class FileSystem
    {
        private Dictionary<string, BSAArchive> _files;
        private string _path;

        public FileSystem(string path)
        {
            _path = path;
            _files = new Dictionary<string, BSAArchive>();

            var files = Directory.GetFiles(_path);
            BSAArchive bsaArchive;

            for (var i = 0; i < files.Length; i++)
            {
                if (Path.GetExtension(files[i]) == ".bsa" || Path.GetExtension(files[i]) == ".ba2")
                {
                    bsaArchive = new BSAArchive(files[i]);
                    bsaArchive.Load();
                    _files.Add(Path.GetFileNameWithoutExtension(files[i]), bsaArchive);
                }
            }
        }

        public byte[] GetRawData(string bsa, string path)
        {
            return _files[bsa].GetFile(path);
        }

        public byte[] GetRawData(string path)
        {
            byte[] data = null;

            foreach (var keyValue in _files)
            {
                data = keyValue.Value.GetFile(path);

                if (data != null)
                    break;
            }

            return data;
        }
    }
}
