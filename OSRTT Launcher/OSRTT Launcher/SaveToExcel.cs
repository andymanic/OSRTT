using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Excel = Microsoft.Office.Interop.Excel;

namespace OSRTT_Launcher
{
    class SaveToExcel
    {
        private Excel.Application resultsTemplate;
        private Excel._Workbook resultsTemplateWorkbook;
        public bool SaveDataToHeatmap(List<ProcessData.processedResult> averageData, ProcessData.runSettings runSettings, string path, string excelFilePath, string[] headers)
        {
            CFuncs cf = new CFuncs();
            bool failed = false;
            if (Properties.Settings.Default.saveXLSX)      
            {
                try   
                {
                    File.Copy(path + "\\Results Template.xlsx", excelFilePath);
                    Console.WriteLine(path);
                    Console.WriteLine(excelFilePath);
                }
                catch (IOException ioe)
                {
                    if (ioe.StackTrace.Contains("exists"))   
                    {
                        Console.WriteLine("File exists, skipping writing.");   
                    }
                }
                catch (Exception ex)
                {
                    cf.showMessageBox(ex.Message + ex.StackTrace, ex.Message, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                resultsTemplate = new Excel.Application();
                try
                {
                    resultsTemplateWorkbook = resultsTemplate.Workbooks.Open(excelFilePath);
                }
                catch   
                {
                    DialogResult d = cf.showMessageBox("Error writing data to XLSX results file, file may be open already. Would you like to try again?", "Unable to Save to XLSX File", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                    if (d == DialogResult.Yes)
                    {
                        try
                        {
                            resultsTemplateWorkbook = resultsTemplate.Workbooks.Open(excelFilePath);
                        }
                        catch (Exception ex)
                        {
                            cf.showMessageBox(ex.Message + ex.StackTrace, "Unable to Save to XLSX File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            failed = true;
                        }
                    }
                    else
                    {               
                        failed = true;
                    }
                }
                if (!failed)
                {
                    Excel._Worksheet resTempSheet = resultsTemplateWorkbook.Sheets[1];
                    Excel._Worksheet resTempSheet2 = resultsTemplateWorkbook.Sheets[2];
                    Excel._Worksheet resTempSheet3 = resultsTemplateWorkbook.Sheets[3];
                    try
                    {
                        for (int h = 0; h < headers.Length; h++)
                        {
                            resTempSheet.Cells[1, h + 1] = headers[h];
                        }
                        //Console.WriteLine("AverageData Count: " + averageData.Count);
                        for (int p = 0; p < averageData.Count; p++)
                        {
                            resTempSheet.Cells[p + 2, 1] = averageData[p].StartingRGB;
                            resTempSheet.Cells[p + 2, 2] = averageData[p].EndRGB;
                            resTempSheet.Cells[p + 2, 3] = averageData[p].compTime;
                            resTempSheet.Cells[p + 2, 4] = averageData[p].initTime;
                            resTempSheet.Cells[p + 2, 5] = averageData[p].perTime;
                            resTempSheet.Cells[p + 2, 6] = averageData[p].Overshoot;
                            resTempSheet.Cells[p + 2, 7] = averageData[p].visualResponseRating;
                            resTempSheet.Cells[p + 2, 8] = averageData[p].inputLag;
                        }
                        resultsTemplateWorkbook.Save();
                    }
                    catch (Exception ex)
                    {
                        cf.showMessageBox(ex.Message + ex.StackTrace, "Unable to Save to XLSX File", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        failed = true;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Marshal.ReleaseComObject(resTempSheet);
                    try
                    {
                        resTempSheet2.Cells[4, 12] = runSettings.RefreshRate;
                        resTempSheet3.Activate();
                        resultsTemplateWorkbook.Save();
                    }
                    catch (Exception ex)
                    {
                        cf.showMessageBox(ex.Message + ex.StackTrace, "Unable to Save to XLSX File", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                        failed = true;
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Marshal.ReleaseComObject(resTempSheet2);
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    Marshal.ReleaseComObject(resTempSheet3);
                }
                resultsTemplateWorkbook.Close();
                Marshal.ReleaseComObject(resultsTemplateWorkbook);
                resultsTemplate.Quit();
                Marshal.ReleaseComObject(resultsTemplate);

                if (failed)
                {
                    File.Delete(excelFilePath);
                }
            
            }
            return failed;
        }
    }
}
