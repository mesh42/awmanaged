using SharedMemory;using System;

namespace AwManagedIde.ToolWindows
{
    /// <summary>
    /// Script Control Document Window.
    /// </summary>
    public partial class ScriptControlDocumentWindow : ToolWindow
    {
        public ScriptControlDocumentWindow()
        {
            InitializeComponent();
            scriptControl1.CodeEditor.Text =
@"public class hello {
	public string Hi;
	public hello()
	{
		// ok
		Hi = ""hello World"";
	}
}";
            // TODO: Add references, such as System and AwManaged, so the intellisense works using reflection.
        }

        /// <summary>
        /// Executes the script.
        /// </summary>
        private void ExecuteScript()
        {
            engine.AddCode(scriptControl1.CodeEditor.Text);
            engine.DebugOn = true;
            var errors = engine.Compile();
        }

        /// <summary>
        /// Handles the Click event of the button1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        private void button1_Click(object sender, EventArgs e)
        {
            ExecuteScript();
        }
    }
}
