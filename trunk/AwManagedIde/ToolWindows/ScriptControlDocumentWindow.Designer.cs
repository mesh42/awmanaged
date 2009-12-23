namespace AwManagedIde.ToolWindows
{
    partial class ScriptControlDocumentWindow
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.scriptControl1 = new AIMS.Libraries.Scripting.ScriptControl.ScriptControl();
            this.engine = new AIMS.Libraries.Scripting.Engine.Engine(this.components);
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // scriptControl1
            // 
            this.scriptControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scriptControl1.Location = new System.Drawing.Point(0, 16);
            this.scriptControl1.Name = "scriptControl1";
            this.scriptControl1.ScriptLanguage = AIMS.Libraries.Scripting.ScriptControl.ScriptLanguage.CSharp;
            this.scriptControl1.Size = new System.Drawing.Size(819, 478);
            this.scriptControl1.TabIndex = 0;
            // 
            // engine
            // 
            this.engine.DebugOn = true;
            this.engine.Language = AIMS.Libraries.Scripting.Engine.LanguageType.CSharp;
            this.engine.OutputAssembly = "";
            this.engine.WarningLevel = 4;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(705, 470);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ScriptControlDocumentWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(819, 494);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.scriptControl1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "ScriptControlDocumentWindow";
            this.Text = "ScriptControlDocumentWindow";
            this.ResumeLayout(false);

        }

        #endregion

        private AIMS.Libraries.Scripting.ScriptControl.ScriptControl scriptControl1;
        private AIMS.Libraries.Scripting.Engine.Engine engine;
        private System.Windows.Forms.Button button1;
    }
}