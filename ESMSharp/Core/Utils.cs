using System;
using System.IO;
using System.Text;

namespace ESMSharp
{
    static class Utils
    {
#if DEBUG
        private static StreamWriter file = null;
        public static bool LogToFile = false;
        public static bool Log = true;
#endif

        public static string ReadString(BinaryReader reader, int length = 4)
        {
            return Encoding.ASCII.GetString(reader.ReadBytes(length));
        }

        public static string ReadString(MemoryStream reader, int length = 4)
        {
            byte[] temp = new byte[length];
            reader.Read(temp, 0, length);
            return Encoding.ASCII.GetString(temp);
        }

        public static string ToString(byte[] array)
        {
            return Encoding.ASCII.GetString(array);
        }

        public static void LogBuffer(string data, params object[] objects)
        {
#if DEBUG
            if (!Log)
                return;

            if (LogToFile)
            {
                if (file == null)
                {
                    if (File.Exists("dump.txt"))
                        File.Delete("dump.txt");

                    file = new StreamWriter(File.OpenWrite("dump.txt"));
                }
                file.WriteLine(string.Format(data, objects) + "\n");
            }
            else
                Console.WriteLine(data, objects);
#endif
        }
    }
}
