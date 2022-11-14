
namespace OSRTT_Launcher
{
    partial class Announcements
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
            this.TitleText = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.mainText = new System.Windows.Forms.RichTextBox();
            this.SuspendLayout();
            // 
            // TitleText
            // 
            this.TitleText.AutoSize = true;
            this.TitleText.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TitleText.Location = new System.Drawing.Point(21, 18);
            this.TitleText.Name = "TitleText";
            this.TitleText.Size = new System.Drawing.Size(98, 39);
            this.TitleText.TabIndex = 0;
            this.TitleText.Text = "label1";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.SystemColors.AppWorkspace;
            this.panel1.Location = new System.Drawing.Point(12, 60);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(784, 10);
            this.panel1.TabIndex = 2;
            // 
            // mainText
            // 
            this.mainText.BackColor = System.Drawing.SystemColors.Control;
            this.mainText.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.mainText.Font = new System.Drawing.Font("Calibri", 20F);
            this.mainText.Location = new System.Drawing.Point(28, 87);
            this.mainText.Name = "mainText";
            this.mainText.ReadOnly = true;
            this.mainText.Size = new System.Drawing.Size(749, 597);
            this.mainText.TabIndex = 3;
            this.mainText.Text = "";
            // 
            // Announcements
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.mainText);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.TitleText);
            this.Name = "Announcements";
            this.Size = new System.Drawing.Size(814, 717);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label TitleText;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RichTextBox mainText;
    }
}
