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
            SocketClient.Start(maskedTextBox1.Text);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            treeView1.Nodes.Clear();
            treeView1.Nodes.Add(new TreeNode("root"));
            FileUtils.ListDirectory(treeView1.TopNode, @"D:\Arts");
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode.Nodes.Clear();
            FileUtils.ListDirectory(treeView1.SelectedNode, treeView1.SelectedNode.Tag.ToString());
        }
    }
}
