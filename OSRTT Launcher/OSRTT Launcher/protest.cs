using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    public partial class protest : Form
    {
        public List<int> adcres = new List<int>();
        public protest()
        {
            InitializeComponent();
            
            
        }
        public void drawScatter()
        {
            List<double> xs = new List<double>();
            for (int i = 0; i < adcres.Count(); i++)
            {
                xs.Add(i);
            }
            List<double> ys = new List<double>();
            for (int i = 0; i < adcres.Count(); i++)
            {
                ys.Add(Convert.ToDouble(adcres[i]));
            }
            graphedData.Plot.AddScatter(xs.ToArray(), ys.ToArray());
            graphedData.RefreshRequest();
        }
    }
}
