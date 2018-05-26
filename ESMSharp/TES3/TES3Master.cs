using ESMSharp.Core;
using System;
using System.Collections.Generic;
using System.IO;

namespace ESMSharp.TES3
{
    public class TES3Master
    {
        private Record[] _records;

        public TES3Master(string filename)
        {
            using (var reader = new BetterBinaryReader(File.OpenRead(filename)))
            {
                var tes3 = new Record();
                tes3.Deserialize(reader, reader.ReadString(4));

                if (tes3.Type != "TES3")
                    throw new Exception("That's not a Morrowind master file.");

                Utils.LogBuffer("# Loading Morrowind");
                Utils.LogBuffer("\t- Record: {0}", tes3.Type);

                var mDico = new List<string>();
                var mRecords = new List<Record>();
                Record mRecord = null;

                while (reader.Position < reader.Length)
                {
                    mRecord = new Record();
                    mRecord.Deserialize(reader, reader.ReadString(4));
                    mRecords.Add(mRecord);

                    if (!mDico.Contains(mRecord.Type))
                    {
                        mDico.Add(mRecord.Type);
                        Utils.LogBuffer("\t- Record: {0}", mRecord.Type);
                    }
                }

                _records = mRecords.ToArray();
            }
        }
    }
}
