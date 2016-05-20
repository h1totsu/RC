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
        public static void ListDirectory(TreeNode node, string path)
        {
            node.Nodes.Clear();
            DirectoryInfo rootDirectoryInfo = new DirectoryInfo(path);
            CreateDirectoryNode(rootDirectoryInfo, node);
        }

        private static void CreateDirectoryNode(DirectoryInfo directoryInfo, TreeNode directoryNode)
        {
            foreach (DirectoryInfo directory in directoryInfo.GetDirectories())
                directoryNode.Nodes.Add(new TreeNode(directory.Name) { Tag = directory.FullName });
            foreach (FileInfo file in directoryInfo.GetFiles())
                directoryNode.Nodes.Add(new TreeNode(file.Name) { Tag = file.FullName });
        }
    }
}
