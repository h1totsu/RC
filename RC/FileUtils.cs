using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC
{
    class FileUtils
    {
        public static void CreateDirectoryNode(Object[] dir, TreeNode directoryNode)
        {
            if (dir is DirectoryInfo[])
            {
                foreach (DirectoryInfo dr in dir)
                    directoryNode.Nodes.Add(new TreeNode(dr.Name) { Tag = dr.FullName });
            }
            else
            {
                foreach (FileInfo dr in dir)
                    directoryNode.Nodes.Add(new TreeNode(dr.Name) { Tag = dr.FullName });
            }
            
        }


    }
}
