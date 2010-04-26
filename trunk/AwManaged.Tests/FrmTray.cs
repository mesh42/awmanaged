using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AwManaged.Tests
{
    public partial class FrmTray : Form
    {
        public FrmTray()
        {
            InitializeComponent();
            notifyIcon1.Tag = "Aw Managed Server Console.";
            notifyIcon1.ShowBalloonTip(1000, "blablabla", "blablabla", ToolTipIcon.Info);
            notifyIcon1.Visible = true;
            this.Show();
        }
    }
}
