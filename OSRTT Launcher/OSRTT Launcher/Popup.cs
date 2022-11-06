using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public partial class Popup : Form
    {
        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;
        public Popup()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Icon = (Icon)rm.GetObject("osrttIcon");
        }

        public void OverdriveModeWindow(Main mainWindow, ProcessData.runSettings rs)
        {
            OverdriveModes od = new OverdriveModes
            {
                mainWindow = mainWindow,
                Location = new Point(0, 0),
                Size = new Size(511, 295),
                Name = "OverdriveModes",
                runSetting = rs,
            };
            Size = new Size(522, 291);
            Text = "Overdrive Mode";
            od.runSetting = rs;
            od.initialiseList("");
            od.Show();
            this.Controls.Add(od);
            this.Activate();
        }
    }
}
