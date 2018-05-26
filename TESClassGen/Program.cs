using System.IO;
using System.Text;

namespace TESClassGen
{
    class Program
    {
        public static void Main(string[] args)
        {
            var tpl = new StringBuilder();
            tpl.Append("namespace ESPSharp.TES4.Records\r\n");
            tpl.Append("{\r\n");
            tpl.Append("\tpublic class [NAME]Record : Record\r\n");
            tpl.Append("\t{\r\n");
            tpl.Append("\t\tprotected override void ExtractSubRecords(BetterBinaryReader reader, GameID gameID, uint size)\r\n");
            tpl.Append("\t\t{\r\n");
            tpl.Append("\t\t\t reader.ReadBytes((int)size);\r\n");
            tpl.Append("\t\t}\r\n");
            tpl.Append("\t}\r\n");
            tpl.Append("}");

            var recordNames = new string[]
            {
                "ACHR", "ACRE", "ACTI", "ALCH", "AMMO", "ANIO", "APPA", "ARMO", "BOOK",
                "BSGN", "CELL", "CLAS", "CLMT", "CLOT", "CONT", "CREA", "CSTY", "DIAL",
                "DOOR", "EFSH", "ENCH", "EYES", "FACT", "FLOR", "FURN", "GLOB", "GMST",
                "GRAS", "HAIR", "IDLE", "INFO", "INGR", "KEYM", "LAND", "LIGH", "LSCR",
                "LTEX", "LVLC", "LVLI", "LVSP", "MGEF", "MISC", "NPC_", "PACK", "PGRD",
                "QUST", "RACE", "REFR", "REGN", "ROAD", "SBSP", "SCPT", "SGST", "SKIL",
                "SLGM", "SOUN", "SPEL", "STAT", "TES4", "TREE", "WATR", "WEAP", "WRLD",
                "WTHR"
            };

            var str = new StringBuilder();
            str.Append("public static Record GetRecord(string name)\n");
            str.Append("{\r\n");

            for (var i = 0; i < recordNames.Length; i++)
            {
                str.Append("\t");

                if (i > 0)
                    str.Append("else ");

                str.Append("if (name == \"" + recordNames[i] + "\")\r\n");
                str.Append("\t\t return new " + recordNames[i] + "Record();\r\n");

                File.WriteAllText(recordNames[i] + "Record.cs", tpl.ToString().Replace("[NAME]", recordNames[i]));
            }

            str.Append("\r\n");
            str.Append("\treturn new Record();\r\n");
            str.Append("}");

            File.WriteAllText("Function.cs", str.ToString());
        }
    }
}