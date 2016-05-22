using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC
{
    class CommandUtils
    {
        public static String GetDrives()
        {
            string[] drives = Directory.GetLogicalDrives();
            StringBuilder builder = new StringBuilder();
            foreach(string drive in drives)
            {
                builder.Append(drive + ";");
            }
            return builder.Remove(builder.Length - 1, 1).ToString();
        }
    }
}
