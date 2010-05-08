using SharedMemory;using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using AW;
using AwManaged;
using AwManaged.Configuration;
using AwManaged.Core.Interfaces;
using AwManaged.Core.Patterns.Tree;
using AwManaged.Math;
using AwManaged.Scene;
using AwManagedIde.ToolWindows;
using WeifenLuo.WinFormsUI.Docking;

namespace AwManagedIde
{
    public partial class Mdi : Form
    {
        #region Fields

        private bool m_bSaveLayout = true;
        private DeserializeDockContent m_deserializeDockContent;
        private PropertiesToolWindow m_propertiesToolWindow = new PropertiesToolWindow();
        private ProjectToolWindow m_projectToolWindow = new ProjectToolWindow();
        private ScriptControlDocumentWindow m_scriptControlDocumentWindow = new ScriptControlDocumentWindow();

        #endregion

        public Tree Tree = new Tree();

        public Mdi()
        {
            InitializeComponent();
           // showRightToLeft.Checked = (RightToLeft == RightToLeft.Yes);
           // RightToLeftLayout = showRightToLeft.Checked;
           // m_solutionExplorer = new DummySolutionExplorer();
            m_propertiesToolWindow = new PropertiesToolWindow();
            dockPanel.DocumentStyle = DocumentStyle.DockingWindow;
            m_propertiesToolWindow.Dock = DockStyle.Right;
            m_projectToolWindow.Dock = DockStyle.Right;
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            m_deserializeDockContent = new DeserializeDockContent(GetContentFromPersistString);
            m_projectToolWindow.PropertiesWindow = m_propertiesToolWindow;
            var engine = new BotEngine() {IdentifyableDisplayName = "Masterbot", IdentifyableId = Guid.NewGuid()};
            var connection = new UniverseConnectionProperties() { LoginName = "awmanaged", World = "zebrakey", IdentifyableId = Guid.NewGuid() };
            connection.IdentifyableDisplayName = connection.LoginName + "@" + connection.World;
            m_projectToolWindow.AddRoot(engine);
            m_projectToolWindow.AddChild(engine,connection);

            var pluginFolder = new FolderNode("Plugins");

            m_projectToolWindow.AddChild(connection, pluginFolder);
            m_projectToolWindow.AddChild(connection, new FolderNode("Scene"));

            //List<Vector3> cells = new List<Vector3>();
            var cellFolders = new List<CellFolder>();
            for (int i=0;i<5000;i++)
            {
                var m = new Model(i, i, DateTime.Now, ObjectType.V3, "rwx01", new Vector3(i*10, i*10, i*10), new Vector3(i*10, i*10, i*10),
                                  "description for " + i, "action for " + i) {IdentifyableId = Guid.NewGuid()};

                var cellFolder = cellFolders.Find(p => p.Cell.x == m.Cell.x && p.Cell.z == m.Cell.z);
                // check if the cell folder exists: m.Cell 
                if (cellFolder ==null)
                {
                    var newCellFolder = new CellFolder(m.Cell);
                    cellFolders.Add(newCellFolder);
                    m_projectToolWindow.AddChild(pluginFolder,newCellFolder.FolderNode);
                    m_projectToolWindow.AddChild(newCellFolder.FolderNode,m);
                }
                else
                {
                    m_projectToolWindow.AddChild(cellFolder.FolderNode, m);
                }
            }
        }

        private class CellFolder : IIdentifiable
        {
            public Vector3 Cell;
            public FolderNode FolderNode;

            public CellFolder(Vector3 cell)
            {
                Cell = cell;
                FolderNode = new FolderNode(string.Format("cell {0},{1}", Cell.x, Cell.z));
                //IdentifyableId = Guid.NewGuid();
            }

            #region IIdentifiable Members

            public string IdentifyableDisplayName
            {
                get { return "Cell Folder"; }
            }

            public Guid IdentifyableId
            { 
                get; set;
            }

            public string IdentifyableTechnicalName
            {
                get { return "CellFolder"; }
            }

            #endregion
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PropertiesToolWindow).ToString())
                return m_propertiesToolWindow;

            return null;
        }

        private void toolStripContainer1_ContentPanel_Load(object sender, EventArgs e)
        {

        }

        private void Mdi_Load(object sender, EventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (File.Exists(configFile))
                dockPanel.LoadFromXml(configFile, m_deserializeDockContent);
            m_propertiesToolWindow.Show(dockPanel);
            m_projectToolWindow.Show(dockPanel);
            m_scriptControlDocumentWindow.Show(dockPanel);
        }

        private void Mdi_FormClosing(object sender, FormClosingEventArgs e)
        {
            string configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "DockPanel.config");
            if (m_bSaveLayout)
                dockPanel.SaveAsXml(configFile);
            else if (File.Exists(configFile))
                File.Delete(configFile);
        }

        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

    }
}
