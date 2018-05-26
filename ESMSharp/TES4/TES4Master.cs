using ESMSharp.Core;
using ESMSharp.TES4.Records;
using System;
using System.Collections.Generic;
using System.IO;

namespace ESMSharp.TES4
{
    public enum GameID
    {
        Oblivion, Skyrim, Fallout3, FalloutNV, Fallout4
    }

    public class TES4Master
    {
        private Dictionary<string, Group> _groups;

        public Dictionary<string, Group> Groups => _groups;

        public TES4Master(string filename)
        {
            var gameID = GameID.Oblivion;

            switch (Path.GetFileNameWithoutExtension(filename).ToLower())
            {
                case "oblivion": gameID = GameID.Oblivion; break;
                case "skyrim": gameID = GameID.Skyrim; break;
                case "fallout3": gameID = GameID.Fallout3; break;
                case "falloutNV": gameID = GameID.FalloutNV; break;
                case "fallout4": gameID = GameID.Fallout4; break;
            }

            _groups = new Dictionary<string, Group>();

            using (var reader = new BetterBinaryReader(File.OpenRead(filename)))
            {
                var tes4 = new TES4Record();
                tes4.Deserialize(reader, reader.ReadString(4), gameID);

                if (tes4.Type != "TES4")
                    throw new Exception("That's not a TES4/5 compatible master file.");

                Utils.LogBuffer("# Loading {0}", gameID);
                Utils.LogBuffer("\t- Record: {0}", tes4.Type);

                string groupName = string.Empty;
                Group group = null;

                while (reader.Position < reader.Length)
                {
                    groupName = reader.ReadString(4);
                    group = new Group();
                    group.Deserialize(reader, groupName, gameID);

                    if (_groups.ContainsKey(group.Label))
                        continue;

                    _groups.Add(group.Label, group);
                }
            }
        }

        public Group GetParent(Group group)
        {
            foreach (var child in _groups)
                if (group.ParentID == child.Value.ParentID)
                    return child.Value;

            return null;
        }
    }
}
