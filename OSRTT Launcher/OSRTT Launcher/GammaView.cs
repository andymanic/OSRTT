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
    public partial class GammaView : Form
    {
        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;
        
        public GammaView()
        {
            InitializeComponent();
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            BackColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
        }

        public void setGammaData(List<ProcessData.gammaResult> g)
        {
            gamma2.setGammaData(g);
            gamma2.drawGraph();
            gamma2.initList();
        }
    }
}
