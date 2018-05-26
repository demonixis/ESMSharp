using ESMSharp.Core;
using System.Collections.Generic;

namespace ESMSharp.TES4
{
    public enum GroupType
    {
        Top = 0,
        WorldChildren,
        InteriorCellBlock,
        InteriorCellSubBlock,
        ExteriorCellBlock,
        ExteriorCellSubBlock,
        CellChildren,
        TopicChildren,
        CellPersistentChildren,
        CellTemporaryChildren,
        CellVisibleDistantChildren
    }

    public class Group
    {
        protected int _parentId;
        protected string _type;
        protected uint _groupSize;
        protected byte[] _label;
        protected int _groupType;
        protected ushort _stamp;
        protected ushort _unknow;
        protected ushort _version;
        protected ushort _unknow2;
        protected Dictionary<GroupType, List<Group>> _subGroups;
        protected Dictionary<string, List<Record>> _records;

        public int ParentID => _parentId;

        public string Label
        {
            get; protected set;
        }

        public Dictionary<GroupType, List<Group>> SubGroups => _subGroups;

        public Dictionary<string, List<Record>> Records => _records;

        public int Type => _groupType;

        public virtual void Deserialize(BetterReader reader, string name, GameID gameID)
        {
            _type = name;
            _subGroups = new Dictionary<GroupType, List<Group>>();
            _records = new Dictionary<string, List<Record>>();

            var headerSize = 24;

            if (gameID == GameID.Oblivion)
                headerSize = 20;

            _groupSize = reader.ReadUInt32();
            _label = reader.ReadBytes(4);
            _groupType = reader.ReadInt32();

            switch ((GroupType)_groupType)
            {
                case GroupType.Top:
                    //if (label[0] >= 32)
                    Label = Utils.ToString(_label);
                    break;
                case GroupType.WorldChildren:
                case GroupType.CellChildren:
                case GroupType.TopicChildren:
                case GroupType.CellPersistentChildren:
                case GroupType.CellTemporaryChildren:
                case GroupType.CellVisibleDistantChildren:
                    _parentId = _label[0];
                    break;
            }

            _stamp = reader.ReadUInt16();
            _unknow = reader.ReadUInt16();

            if (gameID != GameID.Oblivion)
            {
                _version = reader.ReadUInt16();
                _unknow2 = reader.ReadUInt16();
            }

            if (Label != null)
                Utils.LogBuffer("{0} > {1}", _type, Label);

            var endRead = reader.Position + (_groupSize - headerSize);
            var fname = string.Empty;

            Group group = null;
            Record record = null;

            // Only used for debug helping.
            var logGroupDebugDico = new List<int>();
            var logRecordDebugDico = new List<string>();

            while (reader.Position < endRead)
            {
                fname = reader.ReadString(4);

                if (fname == "GRUP")
                {
                    group = new Group();
                    group.Deserialize(reader, fname, gameID);

                    var groupType = (GroupType)group.Type;

                    if (!_subGroups.ContainsKey(groupType))
                    {
                        var list = new List<Group>();
                        list.Add(group);
                        _subGroups.Add(groupType, list);
                    }
                    else
                        _subGroups[groupType].Add(group);

                    if (!logGroupDebugDico.Contains(group._groupType))
                    {
                        logGroupDebugDico.Add(group._groupType);
                        Utils.LogBuffer("\t# SubGroup: {0}", (GroupType)group._groupType);
                    }
                }
                else
                {
                    record = Record.GetRecord(fname);
                    record.Deserialize(reader, fname, gameID);

                    if (!_records.ContainsKey(record.Type))
                    {
                        var list = new List<Record>();
                        list.Add(record);
                        _records.Add(record.Type, list);
                    }
                    else
                        _records[record.Type].Add(record);

                    if (!logRecordDebugDico.Contains(record.Type))
                    {
                        logRecordDebugDico.Add(record.Type);
                        Utils.LogBuffer("\t- Record: {0}", record.Type);
                    }
                }
            }
        }
    }
}
