using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AwManagedIde.ToolWindows
{
    public partial class PropertiesToolWindow : ToolWindow
    {
        public PropertiesToolWindow()
        {
            InitializeComponent();
            propertyGrid1.SelectedObject = this;
        }

        private void propertyGrid1_Click(object sender, EventArgs e)
        {

        }
    }
}
