﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RC
{
    public enum Type
    {
        TEXT, DIR
    }

    [Serializable]
    class Message
    {
        public DirectoryInfo[] Directories { get; set; }
        public FileInfo[] Files { get; set; }
        public String Text { get; set; }
    }
}
