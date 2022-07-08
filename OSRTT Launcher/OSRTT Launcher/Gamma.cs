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
    public partial class Gamma : UserControl
    {
        public List<ProcessData.gammaResult> gamma = new List<ProcessData.gammaResult>();
        public List<double[]> normalisedGamma = new List<double[]>();

        public void setGammaData(List<ProcessData.gammaResult> ad)
        {
            if (ad != null)
            {
                gamma.Clear();
                gamma.AddRange(ad);
                double last = gamma.Last().LightLevel;
                foreach (ProcessData.gammaResult g in gamma)
                {
                    double rgb = g.RGB;
                    rgb /= 255;
                    double light = g.LightLevel;
                    light /= last;
                    normalisedGamma.Add(new double[]{ rgb, light });
                }
            }
        }
        
        public Gamma()
        {
            Graphics g = this.CreateGraphics();
            g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAliasGridFit;
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            initGammaList();
        }

        private void Gamma_Load(object sender, EventArgs e)
        {

        }

        public void drawGraph(bool normalise = false)
        {
            if (gamma.Count != 0)
            {
                double[] lightData = new double[gamma.Count];
                double[] rgbData = new double[gamma.Count];
                if (!normalise)
                {
                    for (int i = 0; i < 256; i++)
                    {
                        lightData[i] = gamma[i].LightLevel;
                        rgbData[i] = gamma[i].RGB;
                    }
                }
                else
                {
                    for (int i = 0; i < 256; i++)
                    {
                        rgbData[i] = normalisedGamma[i][0];
                        lightData[i] = normalisedGamma[i][1];
                    }
                }
                gammaData.Plot.Clear();
                gammaData.Configuration.Zoom = false;
                gammaData.Configuration.LeftClickDragPan = false;
                gammaData.Configuration.MiddleClickDragZoom = false;
                gammaData.Configuration.RightClickDragZoom = false;
                gammaData.Plot.Palette = ScottPlot.Palette.OneHalfDark;
                gammaData.Plot.AxisAuto(0, 0.1);
                gammaData.Plot.Style(ScottPlot.Style.Gray1);
                var bnColor = System.Drawing.ColorTranslator.FromHtml("#2e3440");
                gammaData.Plot.Style(figureBackground: bnColor, dataBackground: bnColor);
                gammaData.Plot.Title("Gamma Curve");
                gammaData.Plot.YLabel("Light level (16 bit integer)");
                gammaData.Plot.XLabel("RGB Value");
                
                gammaData.Plot.AddScatter(rgbData, lightData, lineWidth: 3, markerSize: 4);
                gammaData.Plot.Render();
                gammaData.Refresh();
            }
            else
            {
                MessageBox.Show("Gamma Data Empty", "Gamma Data Empty", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        private void initStatsGridViewColumns(DataGridView dgv)
        {
            if (dgv.Columns.Count != 0)
            {
                dgv.Columns.Clear();
            }
            if (dgv.Rows.Count != 0)
            {
                dgv.Rows.Clear();
            }
            dgv.SelectionChanged += gridView_SelectionChanged;
            dgv.ColumnCount = 2;
            dgv.BorderStyle = BorderStyle.None;
            dgv.ColumnHeadersVisible = true;
            dgv.RowHeadersVisible = false;
            dgv.AdvancedColumnHeadersBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgv.AdvancedCellBorderStyle.All = DataGridViewAdvancedCellBorderStyle.Single;
            dgv.ColumnHeadersDefaultCellStyle.BackColor = ColorTranslator.FromHtml("#2d5875");
            dgv.ColumnHeadersDefaultCellStyle.ForeColor = Color.FromArgb(255, 255, 255, 255);
            dgv.ColumnHeadersDefaultCellStyle.Font = new Font("Calibri", 18, FontStyle.Bold);
            dgv.RowsDefaultCellStyle.ForeColor = Color.White;
            dgv.RowsDefaultCellStyle.BackColor = Color.SteelBlue;
            dgv.RowsDefaultCellStyle.Font = new Font("Calibri", 22);
            

            // rtGridView.RowHeadersDefaultCellStyle.Padding = new Padding(rtGridView.RowHeadersWidth / 2 );
            for (int k = 0; k < dgv.Columns.Count; k++)
            {
                dgv.Columns[k].Width = 125;       
                dgv.Columns[k].DefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;
                dgv.Columns[k].SortMode = DataGridViewColumnSortMode.NotSortable;
            }
            
            dgv.Columns[0].Name = "RGB Value";
            dgv.Columns[1].Name = "Light Level";


        }
        private void gridView_SelectionChanged(Object sender, EventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            dgv.ClearSelection();
            dgv.CurrentRow.Selected = false;
        }

        public void initList()
        {
            initStatsGridViewColumns(gammaTable);
            drawStatsTable(gammaTable);
        }

        private void drawStatsTable(DataGridView dgv)
        {     
            List<string[]> data = new List<string[]>();
            for (int i = 0; i < 16; i++)    
            {
                string[] line = new string[2];    
                data.Add(line);    
            }
            int count = 0;
            for (int l = 0; l < 256; l += 17)    
            {
                data[count][0] = gamma[l].RGB.ToString();
                data[count][1] = gamma[l].LightLevel.ToString();
                count++;
            }    
            foreach (var item in data)
            {
                dgv.Rows.Add(item);
            }

        }

        private void addReference(double reference)
        {
            double[] lightData = new double[gamma.Count];
            double[] rgbData = new double[gamma.Count];
            for (int i = 0; i < normalisedGamma.Count; i++)
            {
                rgbData[i] = normalisedGamma[i][0];
                lightData[i] = Math.Pow(normalisedGamma[i][0],reference);
            }
            gammaData.Plot.AddScatter(rgbData, lightData, lineWidth: 3, markerSize: 4);
            gammaData.Plot.Render();
            gammaData.Refresh();
        }

        private void normaliseBtn_Click(object sender, EventArgs e)
        {
            if (normaliseBtn.Text.Contains("Normalise"))
            {
                drawGraph(true);
                normaliseBtn.Text = "View Raw Gamma";
            }
            else
            {
                drawGraph();
                normaliseBtn.Text = "Normalise Gamma";
            }
            
        }

        private void initGammaList()
        {
            gammaSelect.Items.Clear();

            gammaSelect.Items.Add("1.8");
            gammaSelect.Items.Add("2.0");
            gammaSelect.Items.Add("2.2");
            gammaSelect.Items.Add("2.4");
            gammaSelect.Items.Add("2.6");

            //gammaSelect.SelectedIndex = 2;
        }

        private void gammaSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            { 
                drawGraph(true);
                normaliseBtn.Text = "View Raw Gamma";
                double val = double.Parse(gammaSelect.SelectedItem.ToString());
                addReference(val);
            }
        }
    }
}
