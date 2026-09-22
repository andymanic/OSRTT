
namespace OSRTT_Launcher
{
    partial class MainWindow
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
            this.deviceStatusPanel = new System.Windows.Forms.Panel();
            this.materialButton2 = new MaterialSkin.Controls.MaterialButton();
            this.fwLbl = new MaterialSkin.Controls.MaterialLabel();
            this.fwLblTitle = new MaterialSkin.Controls.MaterialLabel();
            this.devStat = new MaterialSkin.Controls.MaterialLabel();
            this.materialButton1 = new MaterialSkin.Controls.MaterialButton();
            this.menuImageList = new System.Windows.Forms.ImageList(this.components);
            this.testSettingsPane1 = new OSRTT_Launcher.TestSettingsPane();
            this.deviceStatusPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // deviceStatusPanel
            // 
            this.deviceStatusPanel.BackColor = System.Drawing.Color.White;
            this.deviceStatusPanel.Controls.Add(this.materialButton2);
            this.deviceStatusPanel.Controls.Add(this.fwLbl);
            this.deviceStatusPanel.Controls.Add(this.fwLblTitle);
            this.deviceStatusPanel.Controls.Add(this.devStat);
            this.deviceStatusPanel.Controls.Add(this.materialButton1);
            this.deviceStatusPanel.Location = new System.Drawing.Point(177, 25);
            this.deviceStatusPanel.Name = "deviceStatusPanel";
            this.deviceStatusPanel.Size = new System.Drawing.Size(2436, 40);
            this.deviceStatusPanel.TabIndex = 32;
            // 
            // materialButton2
            // 
            this.materialButton2.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton2.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton2.Depth = 0;
            this.materialButton2.HighEmphasis = true;
            this.materialButton2.Icon = null;
            this.materialButton2.Location = new System.Drawing.Point(409, 2);
            this.materialButton2.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton2.Name = "materialButton2";
            this.materialButton2.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton2.Size = new System.Drawing.Size(150, 36);
            this.materialButton2.TabIndex = 59;
            this.materialButton2.Text = "(Dev) Test Board";
            this.materialButton2.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton2.UseAccentColor = false;
            this.materialButton2.UseVisualStyleBackColor = true;
            this.materialButton2.Click += new System.EventHandler(this.materialButton2_Click_1);
            // 
            // fwLbl
            // 
            this.fwLbl.AutoSize = true;
            this.fwLbl.BackColor = System.Drawing.Color.Transparent;
            this.fwLbl.Depth = 0;
            this.fwLbl.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.fwLbl.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.fwLbl.Location = new System.Drawing.Point(741, 8);
            this.fwLbl.MouseState = MaterialSkin.MouseState.HOVER;
            this.fwLbl.Name = "fwLbl";
            this.fwLbl.Size = new System.Drawing.Size(53, 24);
            this.fwLbl.TabIndex = 12;
            this.fwLbl.Text = "V0.00";
            // 
            // fwLblTitle
            // 
            this.fwLblTitle.AutoSize = true;
            this.fwLblTitle.BackColor = System.Drawing.Color.Transparent;
            this.fwLblTitle.Depth = 0;
            this.fwLblTitle.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.fwLblTitle.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.fwLblTitle.Location = new System.Drawing.Point(700, 8);
            this.fwLblTitle.MouseState = MaterialSkin.MouseState.HOVER;
            this.fwLblTitle.Name = "fwLblTitle";
            this.fwLblTitle.Size = new System.Drawing.Size(35, 24);
            this.fwLblTitle.TabIndex = 11;
            this.fwLblTitle.Text = "FW:";
            // 
            // devStat
            // 
            this.devStat.AutoSize = true;
            this.devStat.BackColor = System.Drawing.Color.Transparent;
            this.devStat.Depth = 0;
            this.devStat.Font = new System.Drawing.Font("Roboto Medium", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.devStat.FontType = MaterialSkin.MaterialSkinManager.fontType.H6;
            this.devStat.Location = new System.Drawing.Point(13, 8);
            this.devStat.MouseState = MaterialSkin.MouseState.HOVER;
            this.devStat.Name = "devStat";
            this.devStat.Size = new System.Drawing.Size(162, 24);
            this.devStat.TabIndex = 10;
            this.devStat.Text = "Device Connected";
            // 
            // materialButton1
            // 
            this.materialButton1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.materialButton1.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.materialButton1.Depth = 0;
            this.materialButton1.HighEmphasis = true;
            this.materialButton1.Icon = null;
            this.materialButton1.Location = new System.Drawing.Point(567, 2);
            this.materialButton1.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.materialButton1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialButton1.Name = "materialButton1";
            this.materialButton1.NoAccentTextColor = System.Drawing.Color.Empty;
            this.materialButton1.Size = new System.Drawing.Size(98, 36);
            this.materialButton1.TabIndex = 58;
            this.materialButton1.Text = "(Dev) Test";
            this.materialButton1.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.materialButton1.UseAccentColor = false;
            this.materialButton1.UseVisualStyleBackColor = true;
            this.materialButton1.Click += new System.EventHandler(this.materialButton1_Click_1);
            // 
            // menuImageList
            // 
            this.menuImageList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("menuImageList.ImageStream")));
            this.menuImageList.TransparentColor = System.Drawing.Color.Transparent;
            this.menuImageList.Images.SetKeyName(0, "results.png");
            this.menuImageList.Images.SetKeyName(1, "test.png");
            this.menuImageList.Images.SetKeyName(2, "monitor.png");
            this.menuImageList.Images.SetKeyName(3, "play.png");
            this.menuImageList.Images.SetKeyName(4, "timer.png");
            // 
            // testSettingsPane1
            // 
            this.testSettingsPane1.Location = new System.Drawing.Point(10, 140);
            this.testSettingsPane1.Name = "testSettingsPane1";
            this.testSettingsPane1.Size = new System.Drawing.Size(902, 471);
            this.testSettingsPane1.TabIndex = 33;
            // 
            // MainWindow
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(998, 652);
            this.Controls.Add(this.testSettingsPane1);
            this.Controls.Add(this.deviceStatusPanel);
            this.DrawerShowIconsWhenHidden = true;
            this.DrawerUseColors = true;
            this.Name = "MainWindow";
            this.Text = "OSRTT V";
            this.deviceStatusPanel.ResumeLayout(false);
            this.deviceStatusPanel.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel deviceStatusPanel;
        private MaterialSkin.Controls.MaterialButton materialButton2;
        private MaterialSkin.Controls.MaterialLabel fwLbl;
        private MaterialSkin.Controls.MaterialLabel fwLblTitle;
        private MaterialSkin.Controls.MaterialLabel devStat;
        private MaterialSkin.Controls.MaterialButton materialButton1;
        private System.Windows.Forms.ImageList menuImageList;
        private TestSettingsPane testSettingsPane1;
    }
}