using System;
using System.IO;
using System.Windows.Forms;
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
        }

        private void toolStripContainer1_TopToolStripPanel_Click(object sender, EventArgs e)
        {

        }

        private IDockContent GetContentFromPersistString(string persistString)
        {
            if (persistString == typeof(PropertiesToolWindow).ToString())
                return m_propertiesToolWindow;

            return null;

            //else
            //{
            //    // DummyDoc overrides GetPersistString to add extra information into persistString.
            //    // Any DockContent may override this value to add any needed information for deserialization.

            //    //string[] parsedStrings = persistString.Split(new char[] { ',' });
            //    //if (parsedStrings.Length != 3)
            //    //    return null;

            //    //if (parsedStrings[0] != typeof(DummyDoc).ToString())
            //    //    return null;

            //    //DummyDoc dummyDoc = new DummyDoc();
            //    //if (parsedStrings[1] != string.Empty)
            //    //    dummyDoc.FileName = parsedStrings[1];
            //    //if (parsedStrings[2] != string.Empty)
            //    //    dummyDoc.Text = parsedStrings[2];

            //    //return dummyDoc;
            //}
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

    }
}
