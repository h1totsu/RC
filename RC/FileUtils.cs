using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC
{
    class FileUtils
    {
        public static void CreateDirectoryNode(Object[] dir, TreeView view)
        {
            if (dir is DirectoryInfo[])
            {
                foreach (DirectoryInfo dr in dir)
                {
                    view.SelectedNode.Nodes.Add(new TreeNode(dr.Name) { Tag = dr.FullName, ImageIndex = 1, SelectedImageIndex = 1});
                }

            }
            else
            {
                foreach (FileInfo dr in dir)
                    view.SelectedNode.Nodes.Add(new TreeNode(dr.Name) { Tag = dr.FullName, ImageIndex = 0 });
            }
            
        }


    }
}
