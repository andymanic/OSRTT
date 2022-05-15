
namespace OSRTT_Launcher
{
    partial class GammaView
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
            this.gamma2 = new OSRTT_Launcher.Gamma();
            this.SuspendLayout();
            // 
            // gamma2
            // 
            this.gamma2.Location = new System.Drawing.Point(0, 1);
            this.gamma2.Name = "gamma2";
            this.gamma2.Size = new System.Drawing.Size(1286, 997);
            this.gamma2.TabIndex = 0;
            // 
            // GammaView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1233, 947);
            this.Controls.Add(this.gamma2);
            this.Name = "GammaView";
            this.Text = "GammaView";
            this.ResumeLayout(false);

        }

        #endregion

        private Gamma gamma2;
    }
}