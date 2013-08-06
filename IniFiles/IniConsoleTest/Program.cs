using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IniFiles;

namespace IniConsoleTest
{
    class Program
    {
        static void Main(string[] args)
        {
            var ini = new IniFile(@"F:\Drive D\LPL\Source Codes (CSharp)\General Use\IniFiles\testfile.ini");

            var sections = ini.ReadSections();
            
            int cnt = 1;
            foreach (string sectionName in sections)
            {
                Console.WriteLine("#" + cnt.ToString() + " - " + sectionName);

                var keys = ini.ReadSectionKeys(sectionName);
                foreach (string keyName in keys)
                {
                    Console.WriteLine(keyName + " = " + ini.ReadString(sectionName, keyName, "UNKNOWN"));
                }
                Console.WriteLine("");

                cnt++;
            }

            var b = ini.ReadBool("valueTests", "boolTrue", false);
            Console.WriteLine("True=" + b.ToString());
            b = ini.ReadBool("valueTests", "boolFalse", true);
            Console.WriteLine("False=" + b.ToString());

            short s = ini.ReadShort("valueTests", "short", 255);
            Console.WriteLine("Short=" + s.ToString());

            int i = ini.ReadInteger("valueTests", "integer", 0);
            Console.WriteLine("Integer=" + i.ToString());

            long i64 = ini.ReadInteger64("valueTests", "integer64", 0);
            Console.WriteLine("Integer64=" + i64.ToString());

            double d = ini.ReadFloat("valueTests", "float", 0.0);
            Console.WriteLine("Float=" + d.ToString());

            Console.WriteLine("Saving file");
            ini.Save();

            Console.ReadLine();
        }
    }
}
