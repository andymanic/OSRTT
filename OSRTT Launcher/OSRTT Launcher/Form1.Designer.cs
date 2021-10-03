namespace OSRTT_Launcher
{
    partial class Form1
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
            this.launchBtn = new System.Windows.Forms.Button();
            this.resultsBtn = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.analyseResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devStatLbl = new System.Windows.Forms.Label();
            this.devStat = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.setRepeatBtn = new System.Windows.Forms.Button();
            this.testCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.refreshMonitorListBtn = new System.Windows.Forms.Button();
            this.monitorCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.importRawFolder = new System.Windows.Forms.Button();
            this.fpsLimitBtn = new System.Windows.Forms.Button();
            this.fpsLimitList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testCount)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // launchBtn
            // 
            this.launchBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.launchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.launchBtn.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.launchBtn.Location = new System.Drawing.Point(18, 172);
            this.launchBtn.Name = "launchBtn";
            this.launchBtn.Size = new System.Drawing.Size(551, 46);
            this.launchBtn.TabIndex = 0;
            this.launchBtn.Text = "Launch Test Program";
            this.launchBtn.UseVisualStyleBackColor = false;
            this.launchBtn.Click += new System.EventHandler(this.launchBtn_Click);
            // 
            // resultsBtn
            // 
            this.resultsBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.resultsBtn.FlatAppearance.BorderSize = 0;
            this.resultsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resultsBtn.Font = new System.Drawing.Font("Arial Black", 18F, System.Drawing.FontStyle.Bold);
            this.resultsBtn.Location = new System.Drawing.Point(17, 47);
            this.resultsBtn.Name = "resultsBtn";
            this.resultsBtn.Size = new System.Drawing.Size(268, 75);
            this.resultsBtn.TabIndex = 1;
            this.resultsBtn.Text = "Import Raw Data File";
            this.resultsBtn.UseVisualStyleBackColor = false;
            this.resultsBtn.Click += new System.EventHandler(this.resultsBtn_Click);
            // 
            // richTextBox1
            // 
            this.richTextBox1.Location = new System.Drawing.Point(723, 36);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(369, 760);
            this.richTextBox1.TabIndex = 2;
            this.richTextBox1.Text = "";
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceToolStripMenuItem,
            this.analyseResultsToolStripMenuItem,
            this.resultsSettingsToolStripMenuItem,
            this.debugModeToolStripMenuItem,
            this.testButtonToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1107, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.updateDeviceToolStripMenuItem});
            this.deviceToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // updateDeviceToolStripMenuItem
            // 
            this.updateDeviceToolStripMenuItem.BackColor = System.Drawing.SystemColors.ControlDarkDark;
            this.updateDeviceToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.updateDeviceToolStripMenuItem.Name = "updateDeviceToolStripMenuItem";
            this.updateDeviceToolStripMenuItem.Size = new System.Drawing.Size(202, 22);
            this.updateDeviceToolStripMenuItem.Text = "Update Device Firmware";
            this.updateDeviceToolStripMenuItem.Click += new System.EventHandler(this.updateDeviceToolStripMenuItem_Click);
            // 
            // analyseResultsToolStripMenuItem
            // 
            this.analyseResultsToolStripMenuItem.CheckOnClick = true;
            this.analyseResultsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.analyseResultsToolStripMenuItem.Name = "analyseResultsToolStripMenuItem";
            this.analyseResultsToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.analyseResultsToolStripMenuItem.Text = "Analyse Results";
            this.analyseResultsToolStripMenuItem.Click += new System.EventHandler(this.analyseResultsToolStripMenuItem_Click);
            // 
            // resultsSettingsToolStripMenuItem
            // 
            this.resultsSettingsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.resultsSettingsToolStripMenuItem.Name = "resultsSettingsToolStripMenuItem";
            this.resultsSettingsToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.resultsSettingsToolStripMenuItem.Text = "Results Settings";
            // 
            // debugModeToolStripMenuItem
            // 
            this.debugModeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
            this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.debugModeToolStripMenuItem.Text = "Debug mode";
            this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.debugModeToolStripMenuItem_Click);
            // 
            // testButtonToolStripMenuItem
            // 
            this.testButtonToolStripMenuItem.Name = "testButtonToolStripMenuItem";
            this.testButtonToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.testButtonToolStripMenuItem.Text = "Test Button";
            this.testButtonToolStripMenuItem.Click += new System.EventHandler(this.testButtonToolStripMenuItem_Click);
            // 
            // devStatLbl
            // 
            this.devStatLbl.AutoSize = true;
            this.devStatLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.devStatLbl.Location = new System.Drawing.Point(13, 12);
            this.devStatLbl.Name = "devStatLbl";
            this.devStatLbl.Size = new System.Drawing.Size(298, 25);
            this.devStatLbl.TabIndex = 5;
            this.devStatLbl.Text = "Device Connection Status: ";
            // 
            // devStat
            // 
            this.devStat.AutoSize = true;
            this.devStat.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.devStat.Location = new System.Drawing.Point(317, 12);
            this.devStat.Name = "devStat";
            this.devStat.Size = new System.Drawing.Size(252, 25);
            this.devStat.TabIndex = 6;
            this.devStat.Text = "Waiting for Connection";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(228, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(143, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Analyse Results";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.fpsLimitBtn);
            this.panel1.Controls.Add(this.fpsLimitList);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.setRepeatBtn);
            this.panel1.Controls.Add(this.testCount);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.refreshMonitorListBtn);
            this.panel1.Controls.Add(this.monitorCB);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Controls.Add(this.launchBtn);
            this.panel1.Controls.Add(this.devStatLbl);
            this.panel1.Controls.Add(this.devStat);
            this.panel1.Location = new System.Drawing.Point(12, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 241);
            this.panel1.TabIndex = 15;
            this.panel1.Tag = "";
            // 
            // setRepeatBtn
            // 
            this.setRepeatBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.setRepeatBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.setRepeatBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.setRepeatBtn.Location = new System.Drawing.Point(387, 133);
            this.setRepeatBtn.Name = "setRepeatBtn";
            this.setRepeatBtn.Size = new System.Drawing.Size(182, 26);
            this.setRepeatBtn.TabIndex = 19;
            this.setRepeatBtn.Text = "Save to Device";
            this.setRepeatBtn.UseVisualStyleBackColor = false;
            this.setRepeatBtn.Click += new System.EventHandler(this.setRepeatBtn_Click);
            // 
            // testCount
            // 
            this.testCount.Font = new System.Drawing.Font("Arial Narrow", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testCount.Location = new System.Drawing.Point(326, 133);
            this.testCount.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.testCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.testCount.Name = "testCount";
            this.testCount.Size = new System.Drawing.Size(45, 26);
            this.testCount.TabIndex = 18;
            this.testCount.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(14, 135);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(301, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Averaging - Number of tests to run:";
            // 
            // refreshMonitorListBtn
            // 
            this.refreshMonitorListBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.refreshMonitorListBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.refreshMonitorListBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshMonitorListBtn.Location = new System.Drawing.Point(428, 59);
            this.refreshMonitorListBtn.Name = "refreshMonitorListBtn";
            this.refreshMonitorListBtn.Size = new System.Drawing.Size(141, 26);
            this.refreshMonitorListBtn.TabIndex = 16;
            this.refreshMonitorListBtn.Text = "Refresh List";
            this.refreshMonitorListBtn.UseVisualStyleBackColor = false;
            this.refreshMonitorListBtn.Click += new System.EventHandler(this.refreshMonitorListBtn_Click);
            // 
            // monitorCB
            // 
            this.monitorCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monitorCB.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monitorCB.FormattingEnabled = true;
            this.monitorCB.Location = new System.Drawing.Point(202, 59);
            this.monitorCB.Name = "monitorCB";
            this.monitorCB.Size = new System.Drawing.Size(220, 26);
            this.monitorCB.TabIndex = 15;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 59);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(182, 24);
            this.label1.TabIndex = 14;
            this.label1.Text = "Select your monitor: ";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.importRawFolder);
            this.panel2.Controls.Add(this.resultsBtn);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Location = new System.Drawing.Point(12, 283);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 138);
            this.panel2.TabIndex = 16;
            // 
            // importRawFolder
            // 
            this.importRawFolder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFolder.FlatAppearance.BorderSize = 0;
            this.importRawFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFolder.Font = new System.Drawing.Font("Arial Black", 18F, System.Drawing.FontStyle.Bold);
            this.importRawFolder.Location = new System.Drawing.Point(301, 47);
            this.importRawFolder.Name = "importRawFolder";
            this.importRawFolder.Size = new System.Drawing.Size(268, 75);
            this.importRawFolder.TabIndex = 14;
            this.importRawFolder.Text = "Import Raw Data Folder";
            this.importRawFolder.UseVisualStyleBackColor = false;
            this.importRawFolder.Click += new System.EventHandler(this.importRawFolder_Click);
            // 
            // fpsLimitBtn
            // 
            this.fpsLimitBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.fpsLimitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.fpsLimitBtn.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsLimitBtn.Location = new System.Drawing.Point(428, 96);
            this.fpsLimitBtn.Name = "fpsLimitBtn";
            this.fpsLimitBtn.Size = new System.Drawing.Size(141, 26);
            this.fpsLimitBtn.TabIndex = 22;
            this.fpsLimitBtn.Text = "Save to Device";
            this.fpsLimitBtn.UseVisualStyleBackColor = false;
            this.fpsLimitBtn.Click += new System.EventHandler(this.fpsLimitBtn_Click);
            // 
            // fpsLimitList
            // 
            this.fpsLimitList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpsLimitList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsLimitList.FormattingEnabled = true;
            this.fpsLimitList.Location = new System.Drawing.Point(215, 96);
            this.fpsLimitList.Name = "fpsLimitList";
            this.fpsLimitList.Size = new System.Drawing.Size(207, 26);
            this.fpsLimitList.TabIndex = 21;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(195, 24);
            this.label4.TabIndex = 20;
            this.label4.Text = "Set test framerate limit:";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1107, 808);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.richTextBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "OSRTT Launcher & Analyser";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testCount)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button launchBtn;
        private System.Windows.Forms.Button resultsBtn;
        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem updateDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultsSettingsToolStripMenuItem;
        private System.Windows.Forms.Label devStatLbl;
        private System.Windows.Forms.Label devStat;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem analyseResultsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
        private System.Windows.Forms.ComboBox monitorCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button refreshMonitorListBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button setRepeatBtn;
        private System.Windows.Forms.NumericUpDown testCount;
        private System.Windows.Forms.Button importRawFolder;
        private System.Windows.Forms.ToolStripMenuItem testButtonToolStripMenuItem;
        private System.Windows.Forms.Button fpsLimitBtn;
        private System.Windows.Forms.ComboBox fpsLimitList;
        private System.Windows.Forms.Label label4;
    }
}

