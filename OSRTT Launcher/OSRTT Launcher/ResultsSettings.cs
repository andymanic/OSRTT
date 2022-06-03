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
    public partial class ResultsSettings : Form
    {
        private ResourceManager rm = OSRTT_Launcher.Properties.Resources.ResourceManager;
        private Thread saveThread;
        public ResultsSettings()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.SupportsTransparentBackColor, true);
            this.Icon = (Icon)rm.GetObject("osrttIcon");
            
            initSettingsPreset();
            initIgnoreErrors();
            initSupressErrors();
            initShareData();
            initToleranceStyle();
            initGammaTable();
            initSavetoExcelTable();
            initOvershootStyle();
            initHeatmapsPreset();
            initKeys();
            initTextColour();
            saveLabel.Visible = false;
        }

        private void ResultsSettings_Load(object sender, EventArgs e)
        {

        }

        private void SavingLabel()
        {
            this.saveLabel.Invoke((MethodInvoker)(() => this.saveLabel.Visible = true));
            Thread.Sleep(2500);
            this.saveLabel.Invoke((MethodInvoker)(() => this.saveLabel.Visible = false));
            saveThread.Abort(); // I probably don't need to do this. Remind me to look up the documentation on this...
        }
        
        private void initSettingsPreset()
        {
            settingsPresetSelect.Items.Clear();
            settingsPresetSelect.Items.Add("Recommended");
            settingsPresetSelect.Items.Add("Advanced");
            if (Properties.Settings.Default.advancedSettings)
            {
                settingsPresetSelect.SelectedIndex = 1;
            }
            else
            {
                settingsPresetSelect.SelectedIndex = 0;
                tolerancePanel.Enabled = false;
                toleranceLevelPanel.Enabled = false;
                gammaPanel.Enabled = false;
                overshootSourcePanel.Enabled = false;
                overshootStylePanel.Enabled = false;
                RGB5Btn.BackColor = Color.DarkSeaGreen;
                RGB10Btn.BackColor = Color.SlateGray;
                Per3Btn.BackColor = Color.SlateGray;
                Per10Btn.BackColor = Color.SlateGray;
            }
        }
        private void initIgnoreErrors()
        {
            ignoreErrorsSelect.Items.Clear();
            ignoreErrorsSelect.Items.Add("Yes");
            ignoreErrorsSelect.Items.Add("No");
            if (Properties.Settings.Default.ignoreErrors)
            {
                ignoreErrorsSelect.SelectedIndex = 0;
            }
            else
            {
                ignoreErrorsSelect.SelectedIndex = 1;
            }
        }
        private void initSupressErrors()
        {
            suppressMsgBoxSelect.Items.Clear();
            suppressMsgBoxSelect.Items.Add("Yes");
            suppressMsgBoxSelect.Items.Add("No");
            if (Properties.Settings.Default.SuppressDiagBox)
            {
                suppressMsgBoxSelect.SelectedIndex = 0;
            }
            else
            {
                suppressMsgBoxSelect.SelectedIndex = 1;
            }
        }
        private void initShareData()
        {
            shareDataSelect.Items.Clear();
            shareDataSelect.Items.Add("Yes");
            shareDataSelect.Items.Add("No");
            if (Properties.Settings.Default.shareResults)
            {
                shareDataSelect.SelectedIndex = 0;
            }
            else
            {
                shareDataSelect.SelectedIndex = 1;
            }
        }
        private void initToleranceStyle()
        {
            toleranceStyleSelect.Items.Clear();
            toleranceStyleSelect.Items.Add("RGB Values (Gamma Corrected)");
            toleranceStyleSelect.Items.Add("Light Level");
            Color selected = Color.DarkSeaGreen;
            Color notSelected = Color.SlateGray;
            if (Properties.Settings.Default.advancedSettings)
            {
                selected = Color.LimeGreen;
                notSelected = Color.SteelBlue;
            }
            RGB5Btn.BackColor = notSelected;
            RGB10Btn.BackColor = notSelected;
            Per3Btn.BackColor = notSelected;
            Per10Btn.BackColor = notSelected;
            if (Properties.Settings.Default.rtGammaCorrected)
            {
                toleranceStyleSelect.SelectedIndex = 0;
                Per3Btn.Text = "3% RGB Value";
                Per10Btn.Text = "10% RGB Value";
                RGB5Btn.Enabled = true;
                RGB10Btn.Enabled = true;
                if (Properties.Settings.Default.rtTolerance == 5)
                {
                    RGB5Btn.BackColor = selected;
                    RGB5Btn.ForeColor = Color.White;
                }
                else if (Properties.Settings.Default.rtTolerance == 10 && Properties.Settings.Default.rtPercentage)
                {
                    Per10Btn.BackColor = selected;
                    Per10Btn.ForeColor = Color.White;
                }
                else if (Properties.Settings.Default.rtTolerance == 10 && !Properties.Settings.Default.rtPercentage)
                {
                    RGB10Btn.BackColor = selected;
                    RGB10Btn.ForeColor = Color.White;
                }
                else
                {
                    Per3Btn.BackColor = selected;
                    Per3Btn.ForeColor = Color.White;
                }
            }
            else
            {
                toleranceStyleSelect.SelectedIndex = 1;
                RGB5Btn.Enabled = false;
                RGB5Btn.BackColor = Color.LightSlateGray;
                RGB10Btn.Enabled = false;
                RGB10Btn.BackColor = Color.LightSlateGray;
                Per3Btn.Text = "3% Light Level";
                Per10Btn.Text = "10% Light Level";
                if (Properties.Settings.Default.rtTolerance == 3)
                {
                    Per3Btn.BackColor = Color.LimeGreen;
                    Per3Btn.ForeColor = Color.White;
                }
                else
                {
                    Per10Btn.BackColor = Color.LimeGreen;
                    Per10Btn.ForeColor = Color.White;
                }
            }
        }
        private void initGammaTable()
        {
            saveGammaTableSelect.Items.Clear();
            saveGammaTableSelect.Items.Add("Yes");
            saveGammaTableSelect.Items.Add("No");
            if (Properties.Settings.Default.saveGammaTable)
            {
                saveGammaTableSelect.SelectedIndex = 0;
            }
            else
            {
                saveGammaTableSelect.SelectedIndex = 1;
            }
        }
        private void initSavetoExcelTable()
        {
            saveToExcelSelect.Items.Clear();
            saveToExcelSelect.Items.Add("Yes");
            saveToExcelSelect.Items.Add("No");
            if (Properties.Settings.Default.saveXLSX)
            {
                saveToExcelSelect.SelectedIndex = 0;
            }
            else
            {
                saveToExcelSelect.SelectedIndex = 1;
            }
        }
        private void initOvershootStyle()
        {
            osPercentSelect.Items.Clear();
            osGammaSelect.Items.Clear();
            osGammaSelect.Items.Add("RGB Values (Gamma Corrected)");
            osGammaSelect.Items.Add("Light Level");
            if (Properties.Settings.Default.osGammaCorrected)
            {
                osGammaSelect.SelectedIndex = 0;
                osPercentSelect.Items.Add("Raw RGB Values");
                osPercentSelect.Items.Add("Percent Over End RGB");
                osPercentSelect.Items.Add("Percent Over Transition Range");
                if (!Properties.Settings.Default.osRangePercent && !Properties.Settings.Default.osEndPercent)
                {
                    osPercentSelect.SelectedIndex = 0;
                }
                else if (!Properties.Settings.Default.osRangePercent && Properties.Settings.Default.osEndPercent)
                {
                    osPercentSelect.SelectedIndex = 1;
                }
                else
                {
                    osPercentSelect.SelectedIndex = 2;
                }
            }
            else
            {
                osGammaSelect.SelectedIndex = 1;
                osPercentSelect.Items.Add("Percent Over End Light Level");
                osPercentSelect.Items.Add("Percent Over Transition Range");
                if (!Properties.Settings.Default.osRangePercent && Properties.Settings.Default.osEndPercent)
                {
                    osPercentSelect.SelectedIndex = 0;
                }
                else
                {
                    osPercentSelect.SelectedIndex = 1;
                }
            }
        }
        private void initHeatmapsPreset()
        {
            heatmapPresetSelect.Items.Clear();
            heatmapPresetSelect.Items.Add("Recommended");
            heatmapPresetSelect.Items.Add("Custom");
            if (Properties.Settings.Default.advancedHeatmaps)
            {
                heatmapPresetSelect.SelectedIndex = 1;
                rtKeyPanel.Enabled = true;
                osKeyPanel.Enabled = true;
                vrrKeyPanel.Enabled = true;
            }
            else
            {
                heatmapPresetSelect.SelectedIndex = 0;
                rtKeyPanel.Enabled = false;
                osKeyPanel.Enabled = false;
                vrrKeyPanel.Enabled = false;
            }
        }
        private void initKeys()
        {
            string[] rtKey = Properties.Settings.Default.rtKey.Split(',');
            if (rtKey.Length == 3)
            {
                rtLowNum.Value = decimal.Parse(rtKey[0]);
                rtMidNum.Value = decimal.Parse(rtKey[1]);
                rtHighNum.Value = decimal.Parse(rtKey[2]);
            }
            string[] osKey = Properties.Settings.Default.osKey.Split(',');
            if (osKey.Length == 3)
            {
                osLowNum.Value = decimal.Parse(osKey[0]);
                osMidNum.Value = decimal.Parse(osKey[1]);
                osHighNum.Value = decimal.Parse(osKey[2]);
            }
            string[] vrrKey = Properties.Settings.Default.vrrKey.Split(',');
            if (vrrKey.Length == 3)
            {
                vrrLowNum.Value = decimal.Parse(vrrKey[2]);
                vrrMidNum.Value = decimal.Parse(vrrKey[1]);
                vrrHighNum.Value = decimal.Parse(vrrKey[0]);
            }
        }
        private void initTextColour()
        {
            textColourSelect.Items.Clear();
            textColourSelect.Items.Add("Black");
            textColourSelect.Items.Add("White");
            if (Properties.Settings.Default.heatmapTextColour == Color.Black)
            {
                textColourSelect.SelectedIndex = 0;
            }
            else
            {
                textColourSelect.SelectedIndex = 1;
            }
        }
        private void settingsPresetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (settingsPresetSelect.SelectedIndex == 0 && Properties.Settings.Default.advancedSettings)
                {
                    Properties.Settings.Default.advancedSettings = false;
                    Properties.Settings.Default.rtName = "RGB 5 Tolerance";
                    Properties.Settings.Default.rtTolerance = 5 ;
                    Properties.Settings.Default.rtGammaCorrected = true;
                    Properties.Settings.Default.rtPercentage = false;
                    Properties.Settings.Default.saveGammaTable = false;
                    Properties.Settings.Default.osName = "RGB Values";
                    Properties.Settings.Default.osGammaCorrected = true;
                    Properties.Settings.Default.osEndPercent = false;
                    Properties.Settings.Default.osRangePercent = false;
                    tolerancePanel.Enabled = false;
                    toleranceLevelPanel.Enabled = false;
                    gammaPanel.Enabled = false;
                    overshootSourcePanel.Enabled = false;
                    overshootStylePanel.Enabled = false;
                    initOvershootStyle();
                    initGammaTable();
                    initSavetoExcelTable();
                }
                else
                {
                    Properties.Settings.Default.advancedSettings = true;
                    tolerancePanel.Enabled = true;
                    toleranceLevelPanel.Enabled = true;
                    gammaPanel.Enabled = true;
                    overshootSourcePanel.Enabled = true;
                    overshootStylePanel.Enabled = true;
                }
                Properties.Settings.Default.Save();
                initToleranceStyle();
            }
        }

        private void ignoreErrorsSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (ignoreErrorsSelect.SelectedIndex == 0 && !Properties.Settings.Default.ignoreErrors)
                {
                    Properties.Settings.Default.ignoreErrors = true;
                }
                else
                {
                    Properties.Settings.Default.ignoreErrors = false;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void suppressMsgBoxSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (suppressMsgBoxSelect.SelectedIndex == 0 && !Properties.Settings.Default.SuppressDiagBox)
                {
                    Properties.Settings.Default.SuppressDiagBox = true;
                }
                else
                {
                    Properties.Settings.Default.SuppressDiagBox = false;
                }
                Properties.Settings.Default.Save();
            }
        }


        private void toleranceStyleSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (toleranceStyleSelect.SelectedIndex == 0 && !Properties.Settings.Default.rtGammaCorrected)
                {
                    Per3Btn.Text = "3% RGB Value";
                    Per10Btn.Text = "10% RGB Value";
                    RGB5Btn.Enabled = true;
                    RGB10Btn.Enabled = true;
                    RGB5Btn.BackColor = Color.LimeGreen;
                    RGB10Btn.BackColor = Color.SteelBlue;
                    Per3Btn.BackColor = Color.SteelBlue;
                    Per10Btn.BackColor = Color.SteelBlue;
                    Properties.Settings.Default.rtGammaCorrected = true;
                    Properties.Settings.Default.rtTolerance = 5;
                    Properties.Settings.Default.rtPercentage = false;
                    Properties.Settings.Default.rtName = "RGB 5 Tolerance";
                }
                else
                {
                    Per3Btn.Text = "3% Light Level";
                    Per10Btn.Text = "10% Light Level";
                    RGB5Btn.Enabled = false;
                    RGB10Btn.Enabled = false;
                    Per3Btn.BackColor = Color.LimeGreen;
                    RGB5Btn.BackColor = Color.SlateGray;
                    RGB10Btn.BackColor = Color.SlateGray;
                    Per10Btn.BackColor = Color.SteelBlue;
                    Properties.Settings.Default.rtGammaCorrected = false;
                    Properties.Settings.Default.rtTolerance = 3;
                    Properties.Settings.Default.rtPercentage = true;
                    Properties.Settings.Default.rtName = "3% of Light Level Tolerance";
                }
                Properties.Settings.Default.Save();
            }
        }
        private void RGB5Btn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.rtTolerance != 5)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                RGB5Btn.BackColor = Color.LimeGreen;
                RGB10Btn.BackColor = Color.SteelBlue;
                Per3Btn.BackColor = Color.SteelBlue;
                Per10Btn.BackColor = Color.SteelBlue;
                Properties.Settings.Default.rtGammaCorrected = true;
                Properties.Settings.Default.rtTolerance = 5;
                Properties.Settings.Default.rtPercentage = false;
                Properties.Settings.Default.rtName = "RGB 5 Tolerance";
                Properties.Settings.Default.Save();
            }
        }

        private void RGB10Btn_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.rtName.Contains("RGB 10"))
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                RGB10Btn.BackColor = Color.LimeGreen;
                RGB5Btn.BackColor = Color.SteelBlue;
                Per3Btn.BackColor = Color.SteelBlue;
                Per10Btn.BackColor = Color.SteelBlue;
                Properties.Settings.Default.rtGammaCorrected = true;
                Properties.Settings.Default.rtTolerance = 10;
                Properties.Settings.Default.rtPercentage = false;
                Properties.Settings.Default.rtName = "RGB 10 Tolerance";
                Properties.Settings.Default.Save();
            }
        }

        private void Per3Btn_Click(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.rtTolerance != 3)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                Per3Btn.BackColor = Color.LimeGreen;
                Per10Btn.BackColor = Color.SteelBlue;
                if (Properties.Settings.Default.rtGammaCorrected)
                {
                    RGB5Btn.BackColor = Color.SteelBlue;
                    RGB10Btn.BackColor = Color.SteelBlue;
                }
                Properties.Settings.Default.rtTolerance = 3;
                Properties.Settings.Default.rtPercentage = true;
                Properties.Settings.Default.rtName = "3% of Light Level Tolerance";
                Properties.Settings.Default.Save();
            }
        }

        private void Per10Btn_Click(object sender, EventArgs e)
        {
            if (!Properties.Settings.Default.rtName.Contains("10%"))
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                Per3Btn.BackColor = Color.SteelBlue;
                Per10Btn.BackColor = Color.LimeGreen;
                if (Properties.Settings.Default.rtGammaCorrected)
                {
                    RGB5Btn.BackColor = Color.SteelBlue;
                    RGB10Btn.BackColor = Color.SteelBlue;
                }
                Properties.Settings.Default.rtTolerance = 10;
                Properties.Settings.Default.rtPercentage = true;
                if (Properties.Settings.Default.rtGammaCorrected)
                {
                    Properties.Settings.Default.rtName = "10% of RGB Tolerance";
                }
                else
                {
                    Properties.Settings.Default.rtName = "10% of Light Level Tolerance";
                }
                Properties.Settings.Default.Save();
            }
        }

        private void saveGammaTableSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (saveGammaTableSelect.SelectedIndex == 0 && !Properties.Settings.Default.saveGammaTable)
                {
                    Properties.Settings.Default.saveGammaTable = true;
                }
                else
                {
                    Properties.Settings.Default.saveGammaTable = false;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void osGammaSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (osGammaSelect.SelectedIndex == 0 && !Properties.Settings.Default.osGammaCorrected)
                {
                    Properties.Settings.Default.osGammaCorrected = true;
                    osPercentSelect.Items.Clear();
                    osPercentSelect.Items.Add("Raw RGB Values");
                    osPercentSelect.Items.Add("Percent Over End RGB");
                    osPercentSelect.Items.Add("Percent Over Transition Range");
                    if (Properties.Settings.Default.osRangePercent )
                    {
                        osPercentSelect.SelectedIndex = 2;
                    }
                    else if (Properties.Settings.Default.osRangePercent)
                    {
                        osPercentSelect.SelectedIndex = 1;
                    }
                    else
                    {
                        osPercentSelect.SelectedIndex = 0;
                    }
                }
                else
                {
                    Properties.Settings.Default.osGammaCorrected = false;
                    osPercentSelect.Items.Clear();
                    osPercentSelect.Items.Add("Percent Over End Light Level");
                    osPercentSelect.Items.Add("Percent Over Transition Range");
                    if (Properties.Settings.Default.osRangePercent)
                    {
                        osPercentSelect.SelectedIndex = 1;
                    }
                    else
                    {
                        osPercentSelect.SelectedIndex = 0;
                    }
                }
                Properties.Settings.Default.Save();
            }
        }

        private void osPercentSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (osPercentSelect.SelectedIndex == 0 && (Properties.Settings.Default.osEndPercent || Properties.Settings.Default.osRangePercent))
                {
                    Properties.Settings.Default.osEndPercent = false;
                    Properties.Settings.Default.osRangePercent = false;
                }
                else if (osPercentSelect.SelectedIndex == 1 && !Properties.Settings.Default.osEndPercent)
                {
                    Properties.Settings.Default.osEndPercent = true;
                    Properties.Settings.Default.osRangePercent = false;
                }
                else
                {
                    Properties.Settings.Default.osEndPercent = false;
                    Properties.Settings.Default.osRangePercent = true;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void saveToExcelSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (saveToExcelSelect.SelectedIndex == 0 && !Properties.Settings.Default.saveXLSX)
                {
                    Properties.Settings.Default.saveXLSX = true;
                }
                else
                {
                    Properties.Settings.Default.saveXLSX = false;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void shareDataSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (shareDataSelect.SelectedIndex == 0 && !Properties.Settings.Default.shareResults)
                {
                    Properties.Settings.Default.shareResults = true;
                }
                else
                {
                    Properties.Settings.Default.shareResults = false;
                }
                Properties.Settings.Default.Save();
            }
        }

        private void openColourPickerBtn_Click(object sender, EventArgs e)
        {
            DialogResult d = colorDialog1.ShowDialog();
            if (d == DialogResult.OK)
            {
                Color newColour = colorDialog1.Color;
                float brightness = newColour.GetBrightness();
                if (brightness > 0.5)
                {
                    openColourPickerBtn.ForeColor = Color.Black;
                }
                else
                {
                    openColourPickerBtn.ForeColor = Color.White;
                }
                Console.WriteLine(newColour.GetBrightness());
                openColourPickerBtn.BackColor = newColour;
            }
        }

        private void rtLowNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value >= rtMidNum.Value)
                {
                    rtMidNum.Value = n.Value + 1;
                }
                if (n.Value >= rtHighNum.Value)
                {
                    rtHighNum.Value = n.Value + 2;
                }
                Properties.Settings.Default.rtKey = rtLowNum.Value.ToString() + "," + rtMidNum.Value.ToString() + "," + rtHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void rtMidNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= rtLowNum.Value)
                {
                    rtLowNum.Value = n.Value - 1;
                }
                if (n.Value >= rtHighNum.Value)
                {
                    rtHighNum.Value = n.Value + 1;
                }
                Properties.Settings.Default.rtKey = rtLowNum.Value.ToString() + "," + rtMidNum.Value.ToString() + "," + rtHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void rtHighNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= rtMidNum.Value)
                {
                    rtMidNum.Value = n.Value - 1;
                }
                if (n.Value <= rtLowNum.Value)
                {
                    rtLowNum.Value = n.Value - 2;
                }
                Properties.Settings.Default.rtKey = rtLowNum.Value.ToString() + "," + rtMidNum.Value.ToString() + "," + rtHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void osLowNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value >= osMidNum.Value)
                {
                    osMidNum.Value = n.Value + 1;
                }
                if (n.Value >= osHighNum.Value)
                {
                    osHighNum.Value = n.Value + 2;
                }
                Properties.Settings.Default.osKey = osLowNum.Value.ToString() + "," + osMidNum.Value.ToString() + "," + osHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void osMidNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= osLowNum.Value)
                {
                    osLowNum.Value = n.Value - 1;
                }
                if (n.Value >= osHighNum.Value)
                {
                    osHighNum.Value = n.Value + 1;
                }
                Properties.Settings.Default.osKey = osLowNum.Value.ToString() + "," + osMidNum.Value.ToString() + "," + osHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void osHighNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= osMidNum.Value)
                {
                    osMidNum.Value = n.Value - 1;
                }
                if (n.Value <= osLowNum.Value)
                {
                    osLowNum.Value = n.Value - 2;
                }
                Properties.Settings.Default.osKey = osLowNum.Value.ToString() + "," + osMidNum.Value.ToString() + "," + osHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void vrrLowNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value >= vrrMidNum.Value)
                {
                    vrrMidNum.Value = n.Value + 1;
                }
                if (n.Value >= vrrHighNum.Value)
                {
                    vrrHighNum.Value = n.Value + 2;
                }
                Properties.Settings.Default.vrrKey = vrrLowNum.Value.ToString() + "," + vrrMidNum.Value.ToString() + "," + vrrHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void vrrMidNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= vrrLowNum.Value)
                {
                    vrrLowNum.Value = n.Value - 1;
                }
                if (n.Value >= vrrHighNum.Value)
                {
                    vrrHighNum.Value = n.Value + 1;
                }
                Properties.Settings.Default.vrrKey = vrrLowNum.Value.ToString() + "," + vrrMidNum.Value.ToString() + "," + vrrHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void vrrHighNum_ValueChanged(object sender, EventArgs e)
        {
            NumericUpDown n = sender as NumericUpDown;
            if (n.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (n.Value <= vrrMidNum.Value)
                {
                    vrrMidNum.Value = n.Value - 1;
                }
                if (n.Value <= vrrLowNum.Value)
                {
                    vrrLowNum.Value = n.Value - 2;
                }
                Properties.Settings.Default.vrrKey = vrrLowNum.Value.ToString() + "," + vrrMidNum.Value.ToString() + "," + vrrHighNum.Value.ToString();
                Properties.Settings.Default.Save();
            }
        }

        private void heatmapPresetSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (ctrl.SelectedIndex == 0)
                {
                    Properties.Settings.Default.advancedHeatmaps = false;
                    Properties.Settings.Default.rtKey = "1,5,10";
                    Properties.Settings.Default.osKey = "5,15,20";
                    Properties.Settings.Default.vrrKey = "75,85,95";
                    rtKeyPanel.Enabled = false;
                    osKeyPanel.Enabled = false;
                    vrrKeyPanel.Enabled = false;
                    Properties.Settings.Default.Save();
                    initKeys();
                }
                else
                {
                    rtKeyPanel.Enabled = true;
                    osKeyPanel.Enabled = true;
                    vrrKeyPanel.Enabled = true;
                    Properties.Settings.Default.advancedHeatmaps = true;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void textColourSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            var ctrl = sender as ComboBox;
            if (ctrl.Focused)
            {
                if (saveThread == null || !saveThread.IsAlive)
                {
                    saveThread = new Thread(new ThreadStart(this.SavingLabel));
                    saveThread.Start();
                }
                if (ctrl.SelectedIndex == 0)
                {
                    Properties.Settings.Default.heatmapTextColour = Color.Black;
                }
                else
                {
                    Properties.Settings.Default.heatmapTextColour = Color.White;
                }
                Properties.Settings.Default.Save();
            }
        }
    }

    public class RoundButton : Button
    {
        GraphicsPath GetRoundPath(RectangleF Rect, int radius)
        {
            float m = 2.75F;
            float r2 = radius / 2f;
            GraphicsPath GraphPath = new GraphicsPath();

            GraphPath.AddArc(Rect.X + m, Rect.Y + m, radius, radius, 180, 90);
            GraphPath.AddLine(Rect.X + r2 + m, Rect.Y + m, Rect.Width - r2 - m, Rect.Y + m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m, Rect.Y + m, radius, radius, 270, 90);
            GraphPath.AddLine(Rect.Width - m, Rect.Y + r2, Rect.Width - m, Rect.Height - r2 - m);
            GraphPath.AddArc(Rect.X + Rect.Width - radius - m,
                           Rect.Y + Rect.Height - radius - m, radius, radius, 0, 90);
            GraphPath.AddLine(Rect.Width - r2 - m, Rect.Height - m, Rect.X + r2 - m, Rect.Height - m);
            GraphPath.AddArc(Rect.X + m, Rect.Y + Rect.Height - radius - m, radius, radius, 90, 90);
            GraphPath.AddLine(Rect.X + m, Rect.Height - r2 - m, Rect.X + m, Rect.Y + r2 + m);

            GraphPath.CloseFigure();
            return GraphPath;
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            int borderRadius = 25;
            float borderThickness = 1.75f;
            base.OnPaint(e);
            RectangleF Rect = new RectangleF(0, 0, this.Width, this.Height);
            GraphicsPath GraphPath = GetRoundPath(Rect, borderRadius);

            this.Region = new Region(GraphPath);
            using (Pen pen = new Pen(Color.Silver, borderThickness))
            {
                pen.Alignment = PenAlignment.Inset;
                e.Graphics.DrawPath(pen, GraphPath);
            }
        }
    }
}
