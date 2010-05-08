using SharedMemory;using System;
using System.Collections.Generic;
using System.Linq;
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Patterns.Tree;

namespace AwManagedIde.ToolWindows
{

    public partial class ProjectToolWindow : ToolWindow
    {
        private List<IIdentifiable> _nodeTypes;

        public PropertiesToolWindow PropertiesWindow;

        public ProjectToolWindow()
        {
            InitializeComponent();
            _nodeTypes = new List<IIdentifiable>();
        }

        public void AddRoot(IIdentifiable root)
        {
            _nodeTypes.Add(root);
            treeView1.Nodes.Add(root.IdentifyableId.ToString(), root.IdentifyableDisplayName);
            //todo: resolve icon to type.
        }

        public void AddChild(IIdentifiable parent, IIdentifiable node)
        {
            _nodeTypes.Add(node);
            //todo: resolve icon from type.
            var p = treeView1.Nodes.Find(parent.IdentifyableId.ToString(), true)[0];
            p.Nodes.Add(node.IdentifyableId.ToString(), node.IdentifyableDisplayName);
        }

        private void treeView1_NodeMouseClick(object sender, System.Windows.Forms.TreeNodeMouseClickEventArgs e)
        {
            var guid = new Guid(e.Node.Name);
            treeView1.SelectedNode = e.Node;



            var q = from IIdentifiable p in _nodeTypes where p.IdentifyableId == guid select p;
            if (q.Count() == 1)
            {
                var result = q.Single();
                PropertiesWindow.SetPropertyGrid(result);
                if (result.GetType() == typeof(UniverseConnectionProperties))
                {
                    contextMenuStrip1.Items.Clear();
                    contextMenuStrip1.Items.Add("Disconnect");
                    contextMenuStrip1.Items.Add("Reconnect");
                    contextMenuStrip1.Show(treeView1,e.X,e.Y);
                    return;
                }
            }
        }

        private void treeView1_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {

        }
    }
}
