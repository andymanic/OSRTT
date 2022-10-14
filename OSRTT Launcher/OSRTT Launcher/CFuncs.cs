using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OSRTT_Launcher
{
    class CFuncs
    {
        public DialogResult showMessageBox(string title, string message, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            if (!Properties.Settings.Default.SuppressDiagBox)
            {
                DialogResult d = MessageBox.Show(title, message, buttons, icon);
                return d;
            }
            else
            {
                return DialogResult.None;
            }
        }

        public static string createFileName(string resultsFolderPath, string searchParams)
        {
            decimal fileNumber = 001;
            // search /Results folder for existing file names, pick new name
            string[] existingFiles = Directory.GetFiles(resultsFolderPath, "*" + searchParams);
            // Search \Results folder for existing results to not overwrite existing or have save conflict errors
            foreach (var s in existingFiles)
            {
                decimal num = 0;
                try
                { num = decimal.Parse(Path.GetFileNameWithoutExtension(s).Remove(3)); }
                catch
                { Console.WriteLine("Non-standard file name found"); }
                if (num >= fileNumber)
                {
                    fileNumber = num + 1;
                }
            }
            return fileNumber.ToString("000") + searchParams;
        }

    }
}
