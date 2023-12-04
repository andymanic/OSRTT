
namespace OSRTT_Launcher
{
    partial class protest
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.graphedData = new ScottPlot.FormsPlot();
            this.SuspendLayout();
            // 
            // graphedData
            // 
            this.graphedData.Location = new System.Drawing.Point(12, 12);
            this.graphedData.Name = "graphedData";
            this.graphedData.Size = new System.Drawing.Size(1184, 666);
            this.graphedData.TabIndex = 1;
            // 
            // protest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 682);
            this.Controls.Add(this.graphedData);
            this.Name = "protest";
            this.Text = "protest";
            this.ResumeLayout(false);

        }

        #endregion

        private ScottPlot.FormsPlot graphedData;
    }
}