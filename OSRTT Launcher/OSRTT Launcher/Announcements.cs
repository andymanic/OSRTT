using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net.Http;
using System.Net;

namespace OSRTT_Launcher
{
    public partial class Announcements : UserControl
    {
        public class updateJson
        {
            public string Title { get; set; }
            public string MainText { get; set; }
        }
        public class updateList
        {
            public List<updateJson> Updates { get; set; }
        }

        public Announcements()
        {
            InitializeComponent();
            TitleText.UseMnemonic = false;      
            updateList newthing = new updateList();
            newthing.Updates = new List<updateJson>();
            updateJson update = new updateJson
            {
                Title = "NEW FEATURES & BUG FIXES!",
                MainText = "This update contains a whole load of new stuff, including: \n " +
                "- Input lag clicks can now go up to 990. I don't know why either. \n" +
                "- New option to add extra info to the heatmaps. Useful for tracking extra settings changes. \n" +
                "- New option to display the date and time of the result, again good for distinguishing results \n" +
                "- Save as PNG button is now working on the input lag mode."
            };
            newthing.Updates.Add(update);
            string updateText = JsonConvert.SerializeObject(newthing);
            //Console.WriteLine(updateText);
            processJSON(getAnnouncementData());
        }

        private string getAnnouncementData()
        {
            string contents;
            using (var wc = new System.Net.WebClient())
            {
                contents = wc.DownloadString("https://github.com/andymanic/OSRTT/raw/rt-to-directx/OSRTT%20Launcher/OSRTT%20Launcher/announcements.json");
            }
            Console.WriteLine(contents);
            return contents;
        }

        private void processJSON(string jsonData)
        {
            updateList data = JsonConvert.DeserializeObject<updateList>(jsonData);
            TitleText.Text = data.Updates.Last().Title;
            mainText.Text = data.Updates.Last().MainText;
        }
    }

    
}
