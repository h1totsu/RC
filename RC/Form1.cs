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
    public partial class frmMain : Form
    {
        SocketClient client;
        public delegate void ChangeView();
        public ChangeView del;

        public frmMain()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.tabPage2.Parent = null;
            treeView1.ImageList = imageList1;
            new SocketServer(this).Start();
            del = new ChangeView(ChangeViewMethod);
        }



        private void button2_Click(object sender, EventArgs e)
        {
            if ("CONNECT".Equals(btnConnect.Text))
            {
                client = new SocketClient(mtbxServerIp.Text);
                Message data = client.Execute(Command.CONNECT);
                if (data != null && data.Text == Command.SUCCESS)
                {
                    btnConnect.Text = "DISCONNECT";
                    mtbxServerIp.Enabled = false;
                    data = client.Execute(Command.GET_DRIVES);
                    string[] drives = data.Text.Split(';');

                    foreach (string drive in drives)
                    {
                        treeView1.Nodes.Add(new TreeNode(drive) { Tag = drive, ImageIndex = 1, SelectedImageIndex = 1 });
                    }
                }
            }
            else
            {
                mtbxServerIp.Enabled = true;
                btnConnect.Text = "CONNECT";
                treeView1.Nodes.Clear();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
        }

        private void treeView1_NodeMouseDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            treeView1.SelectedNode.Nodes.Clear();
            Message data = client.Execute(Command.GET_DIR_INFO + ";" + treeView1.SelectedNode.Tag.ToString());
            FileUtils.CreateDirectoryNode(data.Directories, treeView1);
            FileUtils.CreateDirectoryNode(data.Files, treeView1);
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
            Message data = client.Execute(Command.RENAME + ";" + treeView1.SelectedNode.Tag.ToString() + ";" + toolStripTextBox1.Text);
            treeView1.SelectedNode.Text = toolStripTextBox1.Text;
        }

        private void btnDissconnect_Click(object sender, EventArgs e)
        {
            mtbxServerIp.Enabled = true;
            btnConnect.Text = "CONNECT";
            treeView1.Nodes.Clear();

            this.tabPage2.Parent = null;
            this.tabPage1.Parent = this.tbcMenu;
            SocketServer.Stop();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        public void ChangeViewMethod()
        {
            this.tabPage1.Parent = null;
            this.tabPage2.Parent = this.tbcMenu;
        }
    }
}
