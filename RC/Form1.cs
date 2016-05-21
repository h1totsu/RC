using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RC
{
    public partial class Form1 : Form
    {
        SocketClient client;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SocketServer.Start();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            client = new SocketClient(maskedTextBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            Message data = client.Execute(Command.GET_DRIVES);
            string[] drives = data.Text.Split(';');
            foreach (string drive in drives)
            {
                treeView1.Nodes.Add(new TreeNode(drive) { Tag = drive });
            }
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode.Nodes.Clear();
            Message data = client.Execute(Command.GET_DIR_INFO + ";" + treeView1.SelectedNode.Tag.ToString());
            FileUtils.CreateDirectoryNode(data.Directories, treeView1.SelectedNode);
            FileUtils.CreateDirectoryNode(data.Files, treeView1.SelectedNode);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            client.Execute(Command.DELETE + ";" + treeView1.SelectedNode.Tag.ToString());
            treeView1.SelectedNode.Remove();
        }

        private void treeView1_NodeMouseClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                treeView1.SelectedNode = treeView1.GetNodeAt(e.X, e.Y);

                if (treeView1.SelectedNode != null)
                {
                    contextMenuStrip1.Show(treeView1, e.Location);
                }
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            client.Execute(Command.RENAME + ";" + treeView1.SelectedNode.Tag.ToString() + ";" + toolStripTextBox1.Text);
        }
    }
}
