
namespace OSRTT_Launcher
{
    partial class InputLagProcRV
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.switchGraphTypeBtn = new System.Windows.Forms.Button();
            this.graphedData = new ScottPlot.FormsPlot();
            this.rtTitle = new System.Windows.Forms.Label();
            this.saveIMGBtn = new System.Windows.Forms.Button();
            this.barPlot = new ScottPlot.FormsPlot();
            this.saveWhitePNGBtn = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // switchGraphTypeBtn
            // 
            this.switchGraphTypeBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.switchGraphTypeBtn.FlatAppearance.BorderSize = 0;
            this.switchGraphTypeBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.switchGraphTypeBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.switchGraphTypeBtn.Location = new System.Drawing.Point(806, 16);
            this.switchGraphTypeBtn.Name = "switchGraphTypeBtn";
            this.switchGraphTypeBtn.Size = new System.Drawing.Size(375, 36);
            this.switchGraphTypeBtn.TabIndex = 29;
            this.switchGraphTypeBtn.Text = "Switch to Individual Results";
            this.switchGraphTypeBtn.UseVisualStyleBackColor = false;
            this.switchGraphTypeBtn.Click += new System.EventHandler(this.switchGraphTypeBtn_Click);
            // 
            // graphedData
            // 
            this.graphedData.Location = new System.Drawing.Point(18, 67);
            this.graphedData.Name = "graphedData";
            this.graphedData.Size = new System.Drawing.Size(1184, 666);
            this.graphedData.TabIndex = 30;
            // 
            // rtTitle
            // 
            this.rtTitle.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtTitle.ForeColor = System.Drawing.Color.Black;
            this.rtTitle.Location = new System.Drawing.Point(30, 16);
            this.rtTitle.Name = "rtTitle";
            this.rtTitle.Size = new System.Drawing.Size(339, 47);
            this.rtTitle.TabIndex = 31;
            this.rtTitle.Text = "Latency Results";
            // 
            // saveIMGBtn
            // 
            this.saveIMGBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveIMGBtn.FlatAppearance.BorderSize = 0;
            this.saveIMGBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveIMGBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.saveIMGBtn.Location = new System.Drawing.Point(391, 16);
            this.saveIMGBtn.Name = "saveIMGBtn";
            this.saveIMGBtn.Size = new System.Drawing.Size(140, 36);
            this.saveIMGBtn.TabIndex = 32;
            this.saveIMGBtn.Text = "Save PNG";
            this.saveIMGBtn.UseVisualStyleBackColor = false;
            this.saveIMGBtn.Click += new System.EventHandler(this.saveIMGBtn_Click);
            // 
            // barPlot
            // 
            this.barPlot.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barPlot.Location = new System.Drawing.Point(18, 67);
            this.barPlot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.barPlot.Name = "barPlot";
            this.barPlot.Size = new System.Drawing.Size(1184, 666);
            this.barPlot.TabIndex = 33;
            // 
            // saveWhitePNGBtn
            // 
            this.saveWhitePNGBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveWhitePNGBtn.FlatAppearance.BorderSize = 0;
            this.saveWhitePNGBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveWhitePNGBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.saveWhitePNGBtn.Location = new System.Drawing.Point(537, 16);
            this.saveWhitePNGBtn.Name = "saveWhitePNGBtn";
            this.saveWhitePNGBtn.Size = new System.Drawing.Size(196, 36);
            this.saveWhitePNGBtn.TabIndex = 34;
            this.saveWhitePNGBtn.Text = "Save White PNG";
            this.saveWhitePNGBtn.UseVisualStyleBackColor = false;
            this.saveWhitePNGBtn.Click += new System.EventHandler(this.saveWhitePNGBtn_Click);
            // 
            // InputLagProcRV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.saveWhitePNGBtn);
            this.Controls.Add(this.barPlot);
            this.Controls.Add(this.saveIMGBtn);
            this.Controls.Add(this.rtTitle);
            this.Controls.Add(this.graphedData);
            this.Controls.Add(this.switchGraphTypeBtn);
            this.Name = "InputLagProcRV";
            this.Size = new System.Drawing.Size(1232, 756);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button switchGraphTypeBtn;
        private ScottPlot.FormsPlot graphedData;
        private System.Windows.Forms.Label rtTitle;
        private System.Windows.Forms.Button saveIMGBtn;
        private ScottPlot.FormsPlot barPlot;
        private System.Windows.Forms.Button saveWhitePNGBtn;
    }
}
