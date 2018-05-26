using ESMSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace ESMSharp.TES3
{
    public class Master
    {
        public class Record
        {
            public Record(string name)
            {

            }

            public virtual void Deserialize(BetterReader reader)
            {

            }
        }

        public class SubRecord { }

        private Record[] _records;

        public Master(string filename)
        {
            if (Path.GetFileNameWithoutExtension(filename).ToLower() != "morrowind")
                throw new Exception("Morrowind not present.");

            using (var reader = new BetterBinaryReader(File.OpenRead(filename)))
            {
                var tes3 = new Record(reader.ReadString(4));
                tes3.Deserialize(reader);

                Utils.LogBuffer("# Loading Morrowind");

                Record record;
                var records = new List<Record>();
                var name = string.Empty;

                while (reader.Position < reader.Length)
                {
                    name = reader.ReadString(4);
                    record = new Record(name);
                    record.Deserialize(reader);
                    records.Add(record);
                }

                _records = records.ToArray();
            }
        }
    }
}
