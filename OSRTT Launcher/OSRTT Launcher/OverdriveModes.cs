using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace OSRTT_Launcher
{
    public partial class OverdriveModes : UserControl
    {
        public Main mainWindow;
        private class ODMode
        {
            public string Name { get; set; }
            public List<string> Modes { get; set; }
        }
        private class ODModes
        {
            public List<ODMode> OverdriveModes { get; set; }
        }
        private ODModes odList = new ODModes();
        public ProcessData.runSettings runSetting;
        public OverdriveModes()
        {
            InitializeComponent();
            controlState(false);
            //initialiseList();
        }
        private void controlState(bool state)
        {
            label2.Enabled = state;
            odModeBox.Enabled = state;
            continueBtn.Enabled = state;
            if (state)
            {
                odModeBox.BackColor = Color.White;
                //odModeBox.Location = new Point(221, 31);
                odModeBox.BorderStyle = BorderStyle.Fixed3D;
            }
            else
            {
                odModeBox.BackColor = Color.LightGray;
                //odModeBox.Location = new Point(odModeBox.Location.X, odModeBox.Location.Y + 5);
                odModeBox.BorderStyle = BorderStyle.None;
            }
        }

        public void initialiseList(string monitorManufacturer)
        {
            string path = System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase;
            string path2 = path;
            path = new Uri(System.IO.Path.GetDirectoryName(path)).LocalPath + @"\odmodes.json";
            path2 = new Uri(System.IO.Path.GetDirectoryName(path2)).LocalPath + @"\lib\odmodes.json";
            //Console.WriteLine(path);
            string jsonData = "";
            // This text is added only once to the file.
            if (File.Exists(path))
            {
                jsonData = File.ReadAllText(path);
            }
            else if (File.Exists(path2))
            {
                jsonData = File.ReadAllText(path2);
            }
            else
            {
                // Can't find file
            }

            bool found = false;
            try
            {
                odList = JsonConvert.DeserializeObject<ODModes>(jsonData);
                //string monitorManufacturer = runSetting.MonitorName.Remove(3);
                manufacturerSelect.Items.Clear();
                foreach (ODMode o in odList.OverdriveModes)
                {
                    if (o.Name == monitorManufacturer)
                    {
                        foreach (string i in o.Modes)
                        {
                            manufacturerSelect.Items.Add(i);
                        }
                        found = true;
                    }
                }
                
            }
            catch { }
            if (!found)
            {
                manufacturerSelect.Items.Add("Enter Manually");
            }
            else
            {
                manufacturerSelect.Items.Add("Other");
            }
            odModeBox.Text = "";
            manufacturerSelect.SelectedIndex = 0;
            
        }
        private void manufacturerSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboBox box = sender as ComboBox;
            //if (box.Focused)
            //{
                if (box.Items[box.SelectedIndex].ToString().Contains("Other") || box.Items[box.SelectedIndex].ToString().Contains("Manually"))
                {
                    controlState(true);
                    continueBtn.Enabled = false;
                }
                else
                {
                    odModeBox.Text = box.Items[box.SelectedIndex].ToString();
                    continueBtn.Enabled = true;
                }
            //}
        }

        private void continueBtn_Click(object sender, EventArgs e)
        {
            if (mainWindow != null)
            {
                if (odModeBox.Text != null && odModeBox.Text != "")
                {
                    runSetting.OverdriveMode = odModeBox.Text;
                    mainWindow.runSettings = runSetting;
                }
            }
            else
            {
                // error
            }
        }

        private void odModeBox_TextChanged(object sender, EventArgs e)
        {
            continueBtn.Enabled = true;
        }
    }
}
