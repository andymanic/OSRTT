using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public partial class HelpView : Form
    {
        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;
        public HelpView()
        {
            InitializeComponent();
            this.Icon = (Icon)rm.GetObject("osrttIcon");
        }

        private void ResultsSettings_Load(object sender, EventArgs e)
        {

        }

    }
}
