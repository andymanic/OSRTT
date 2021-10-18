namespace OSRTT_Launcher
{
    partial class Main
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
            this.launchBtn = new System.Windows.Forms.Button();
            this.resultsBtn = new System.Windows.Forms.Button();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.analyseResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.verboseOutputToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.measurementsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.threePercentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tenPercentMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.noGammaCorrectionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.gammaCorrectedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.percentageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BrightnessCalBtn = new System.Windows.Forms.ToolStripMenuItem();
            this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.updateDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.testButtonToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.devStatLbl = new System.Windows.Forms.Label();
            this.devStat = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.controlsPanel = new System.Windows.Forms.Panel();
            this.fpsLimitList = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.testCount = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.refreshMonitorListBtn = new System.Windows.Forms.Button();
            this.monitorCB = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.analysePanel = new System.Windows.Forms.Panel();
            this.importRawFolder = new System.Windows.Forms.Button();
            this.rawValText = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.resetBtn = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.incPotValBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.brightnessText = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.whitePanel = new System.Windows.Forms.TableLayoutPanel();
            this.closeWindowBtn = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.brightnessPanel = new System.Windows.Forms.Panel();
            this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
            this.gamCorMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.menuStrip1.SuspendLayout();
            this.controlsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testCount)).BeginInit();
            this.analysePanel.SuspendLayout();
            this.brightnessPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // launchBtn
            // 
            this.launchBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.launchBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.launchBtn.Font = new System.Drawing.Font("Consolas", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.launchBtn.Location = new System.Drawing.Point(18, 131);
            this.launchBtn.Name = "launchBtn";
            this.launchBtn.Size = new System.Drawing.Size(551, 46);
            this.launchBtn.TabIndex = 5;
            this.launchBtn.Text = "Start Testing";
            this.launchBtn.UseVisualStyleBackColor = false;
            this.launchBtn.Click += new System.EventHandler(this.launchBtn_Click);
            // 
            // resultsBtn
            // 
            this.resultsBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.resultsBtn.FlatAppearance.BorderSize = 0;
            this.resultsBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resultsBtn.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold);
            this.resultsBtn.Location = new System.Drawing.Point(17, 47);
            this.resultsBtn.Name = "resultsBtn";
            this.resultsBtn.Size = new System.Drawing.Size(268, 75);
            this.resultsBtn.TabIndex = 6;
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
            this.analyseResultsToolStripMenuItem,
            this.resultsSettingsToolStripMenuItem,
            this.BrightnessCalBtn,
            this.debugModeToolStripMenuItem,
            this.deviceToolStripMenuItem,
            this.testButtonToolStripMenuItem,
            this.toolStripMenuItem1});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2106, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
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
            this.resultsSettingsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.verboseOutputToolStripMenuItem,
            this.measurementsToolStripMenuItem,
            this.noGammaCorrectionToolStripMenuItem});
            this.resultsSettingsToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.resultsSettingsToolStripMenuItem.Name = "resultsSettingsToolStripMenuItem";
            this.resultsSettingsToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.resultsSettingsToolStripMenuItem.Text = "Results Settings";
            // 
            // verboseOutputToolStripMenuItem
            // 
            this.verboseOutputToolStripMenuItem.CheckOnClick = true;
            this.verboseOutputToolStripMenuItem.Name = "verboseOutputToolStripMenuItem";
            this.verboseOutputToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.verboseOutputToolStripMenuItem.Text = "Verbose Output";
            this.verboseOutputToolStripMenuItem.ToolTipText = "Include all processed fields in each \"FULL\" CSV. \r\nIncludes transition start & en" +
    "d position, sample time, overshoot light level and overshoot RGB value.";
            this.verboseOutputToolStripMenuItem.Click += new System.EventHandler(this.verboseOutputToolStripMenuItem_Click);
            // 
            // measurementsToolStripMenuItem
            // 
            this.measurementsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gamCorMenuItem,
            this.toolStripSeparator1,
            this.threePercentMenuItem,
            this.tenPercentMenuItem});
            this.measurementsToolStripMenuItem.Name = "measurementsToolStripMenuItem";
            this.measurementsToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.measurementsToolStripMenuItem.Text = "GtG Measurement Style";
            // 
            // threePercentMenuItem
            // 
            this.threePercentMenuItem.Checked = true;
            this.threePercentMenuItem.CheckOnClick = true;
            this.threePercentMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.threePercentMenuItem.Name = "threePercentMenuItem";
            this.threePercentMenuItem.Size = new System.Drawing.Size(253, 22);
            this.threePercentMenuItem.Text = "3% / 97% Response Time";
            this.threePercentMenuItem.Click += new System.EventHandler(this.threePercentMenuItem_Click);
            // 
            // tenPercentMenuItem
            // 
            this.tenPercentMenuItem.CheckOnClick = true;
            this.tenPercentMenuItem.Name = "tenPercentMenuItem";
            this.tenPercentMenuItem.Size = new System.Drawing.Size(253, 22);
            this.tenPercentMenuItem.Text = "10% / 90% Response Time";
            this.tenPercentMenuItem.Click += new System.EventHandler(this.tenPercentMenuItem_Click);
            // 
            // noGammaCorrectionToolStripMenuItem
            // 
            this.noGammaCorrectionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.gammaCorrectedToolStripMenuItem,
            this.percentageToolStripMenuItem});
            this.noGammaCorrectionToolStripMenuItem.Name = "noGammaCorrectionToolStripMenuItem";
            this.noGammaCorrectionToolStripMenuItem.Size = new System.Drawing.Size(198, 22);
            this.noGammaCorrectionToolStripMenuItem.Text = "Overshoot Settings";
            // 
            // gammaCorrectedToolStripMenuItem
            // 
            this.gammaCorrectedToolStripMenuItem.Checked = true;
            this.gammaCorrectedToolStripMenuItem.CheckOnClick = true;
            this.gammaCorrectedToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gammaCorrectedToolStripMenuItem.Name = "gammaCorrectedToolStripMenuItem";
            this.gammaCorrectedToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.gammaCorrectedToolStripMenuItem.Text = "Gamma Corrected";
            this.gammaCorrectedToolStripMenuItem.Click += new System.EventHandler(this.gammaCorrectedToolStripMenuItem_Click);
            // 
            // percentageToolStripMenuItem
            // 
            this.percentageToolStripMenuItem.Checked = true;
            this.percentageToolStripMenuItem.CheckOnClick = true;
            this.percentageToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.percentageToolStripMenuItem.Name = "percentageToolStripMenuItem";
            this.percentageToolStripMenuItem.Size = new System.Drawing.Size(171, 22);
            this.percentageToolStripMenuItem.Text = "Percentage";
            this.percentageToolStripMenuItem.Click += new System.EventHandler(this.percentageToolStripMenuItem_Click);
            // 
            // BrightnessCalBtn
            // 
            this.BrightnessCalBtn.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.BrightnessCalBtn.Name = "BrightnessCalBtn";
            this.BrightnessCalBtn.Size = new System.Drawing.Size(124, 20);
            this.BrightnessCalBtn.Text = "Calibrate Brightness";
            this.BrightnessCalBtn.Click += new System.EventHandler(this.BrightnessCalBtn_Click);
            // 
            // debugModeToolStripMenuItem
            // 
            this.debugModeToolStripMenuItem.CheckOnClick = true;
            this.debugModeToolStripMenuItem.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
            this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.debugModeToolStripMenuItem.Text = "Debug mode";
            this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.debugModeToolStripMenuItem_Click);
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
            this.deviceToolStripMenuItem.Visible = false;
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
            // testButtonToolStripMenuItem
            // 
            this.testButtonToolStripMenuItem.Name = "testButtonToolStripMenuItem";
            this.testButtonToolStripMenuItem.Size = new System.Drawing.Size(78, 20);
            this.testButtonToolStripMenuItem.Text = "Test Button";
            this.testButtonToolStripMenuItem.Click += new System.EventHandler(this.testButtonToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(12, 20);
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
            // controlsPanel
            // 
            this.controlsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.controlsPanel.Controls.Add(this.fpsLimitList);
            this.controlsPanel.Controls.Add(this.label4);
            this.controlsPanel.Controls.Add(this.testCount);
            this.controlsPanel.Controls.Add(this.label2);
            this.controlsPanel.Controls.Add(this.refreshMonitorListBtn);
            this.controlsPanel.Controls.Add(this.monitorCB);
            this.controlsPanel.Controls.Add(this.label1);
            this.controlsPanel.Controls.Add(this.launchBtn);
            this.controlsPanel.Controls.Add(this.devStatLbl);
            this.controlsPanel.Controls.Add(this.devStat);
            this.controlsPanel.Location = new System.Drawing.Point(12, 36);
            this.controlsPanel.Name = "controlsPanel";
            this.controlsPanel.Size = new System.Drawing.Size(588, 192);
            this.controlsPanel.TabIndex = 15;
            this.controlsPanel.Tag = "";
            // 
            // fpsLimitList
            // 
            this.fpsLimitList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.fpsLimitList.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.fpsLimitList.FormattingEnabled = true;
            this.fpsLimitList.Location = new System.Drawing.Point(158, 94);
            this.fpsLimitList.Name = "fpsLimitList";
            this.fpsLimitList.Size = new System.Drawing.Size(145, 26);
            this.fpsLimitList.TabIndex = 3;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(14, 94);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(138, 24);
            this.label4.TabIndex = 20;
            this.label4.Text = "Framerate limit:";
            // 
            // testCount
            // 
            this.testCount.Cursor = System.Windows.Forms.Cursors.Hand;
            this.testCount.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.testCount.Location = new System.Drawing.Point(516, 94);
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
            this.testCount.Size = new System.Drawing.Size(53, 26);
            this.testCount.TabIndex = 4;
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
            this.label2.Location = new System.Drawing.Point(342, 94);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(165, 24);
            this.label2.TabIndex = 17;
            this.label2.Text = "Number of Cycles:";
            // 
            // refreshMonitorListBtn
            // 
            this.refreshMonitorListBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.refreshMonitorListBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.refreshMonitorListBtn.Font = new System.Drawing.Font("Consolas", 12.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.refreshMonitorListBtn.Location = new System.Drawing.Point(441, 59);
            this.refreshMonitorListBtn.Name = "refreshMonitorListBtn";
            this.refreshMonitorListBtn.Size = new System.Drawing.Size(128, 26);
            this.refreshMonitorListBtn.TabIndex = 2;
            this.refreshMonitorListBtn.Text = "Refresh List";
            this.refreshMonitorListBtn.UseVisualStyleBackColor = false;
            this.refreshMonitorListBtn.Click += new System.EventHandler(this.refreshMonitorListBtn_Click);
            // 
            // monitorCB
            // 
            this.monitorCB.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monitorCB.Font = new System.Drawing.Font("Arial", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.monitorCB.FormattingEnabled = true;
            this.monitorCB.Location = new System.Drawing.Point(195, 59);
            this.monitorCB.Name = "monitorCB";
            this.monitorCB.Size = new System.Drawing.Size(238, 26);
            this.monitorCB.TabIndex = 1;
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
            // analysePanel
            // 
            this.analysePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.analysePanel.Controls.Add(this.importRawFolder);
            this.analysePanel.Controls.Add(this.resultsBtn);
            this.analysePanel.Controls.Add(this.label3);
            this.analysePanel.Location = new System.Drawing.Point(12, 238);
            this.analysePanel.Name = "analysePanel";
            this.analysePanel.Size = new System.Drawing.Size(588, 138);
            this.analysePanel.TabIndex = 16;
            // 
            // importRawFolder
            // 
            this.importRawFolder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFolder.FlatAppearance.BorderSize = 0;
            this.importRawFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFolder.Font = new System.Drawing.Font("Consolas", 20F, System.Drawing.FontStyle.Bold);
            this.importRawFolder.Location = new System.Drawing.Point(301, 47);
            this.importRawFolder.Name = "importRawFolder";
            this.importRawFolder.Size = new System.Drawing.Size(268, 75);
            this.importRawFolder.TabIndex = 7;
            this.importRawFolder.Text = "Import Raw Data Folder";
            this.importRawFolder.UseVisualStyleBackColor = false;
            this.importRawFolder.Click += new System.EventHandler(this.importRawFolder_Click);
            // 
            // rawValText
            // 
            this.rawValText.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rawValText.Location = new System.Drawing.Point(160, 450);
            this.rawValText.Name = "rawValText";
            this.rawValText.Size = new System.Drawing.Size(96, 21);
            this.rawValText.TabIndex = 27;
            this.rawValText.Text = "No Result";
            this.rawValText.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(58, 450);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(96, 21);
            this.label5.TabIndex = 26;
            this.label5.Text = "Raw Result:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // resetBtn
            // 
            this.resetBtn.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.resetBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.resetBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetBtn.Font = new System.Drawing.Font("Arial Unicode MS", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.resetBtn.Location = new System.Drawing.Point(191, 612);
            this.resetBtn.Name = "resetBtn";
            this.resetBtn.Size = new System.Drawing.Size(81, 50);
            this.resetBtn.TabIndex = 25;
            this.resetBtn.Text = "Reset";
            this.resetBtn.UseVisualStyleBackColor = false;
            this.resetBtn.Click += new System.EventHandler(this.resetBtn_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.Location = new System.Drawing.Point(33, 546);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(230, 63);
            this.label6.TabIndex = 24;
            this.label6.Text = "Still too low? Try increasing the \r\nsensitivity - only use this if your\r\nmonitor " +
    "can\'t get any brighter!";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // incPotValBtn
            // 
            this.incPotValBtn.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.incPotValBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.incPotValBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.incPotValBtn.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.incPotValBtn.Location = new System.Drawing.Point(27, 612);
            this.incPotValBtn.Name = "incPotValBtn";
            this.incPotValBtn.Size = new System.Drawing.Size(158, 50);
            this.incPotValBtn.TabIndex = 23;
            this.incPotValBtn.Text = "Increase Sensitivity";
            this.incPotValBtn.UseVisualStyleBackColor = false;
            this.incPotValBtn.Click += new System.EventHandler(this.incPotValBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Arial Unicode MS", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(15, 259);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(272, 33);
            this.label7.TabIndex = 22;
            this.label7.Text = "Current brightness level:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // brightnessText
            // 
            this.brightnessText.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.brightnessText.Cursor = System.Windows.Forms.Cursors.Default;
            this.brightnessText.Font = new System.Drawing.Font("Arial Unicode MS", 42F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.brightnessText.Location = new System.Drawing.Point(12, 292);
            this.brightnessText.Margin = new System.Windows.Forms.Padding(0);
            this.brightnessText.Name = "brightnessText";
            this.brightnessText.Size = new System.Drawing.Size(275, 158);
            this.brightnessText.TabIndex = 21;
            this.brightnessText.Text = "No Results";
            this.brightnessText.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial Unicode MS", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(17, 144);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(261, 84);
            this.label8.TabIndex = 20;
            this.label8.Text = "Place the window under the sensor \r\nand set your monitor to its maximum\r\nbrightne" +
    "ss, then decrease it until it\r\nreads \"Perfect!\". Target is 160 nits.";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // whitePanel
            // 
            this.whitePanel.BackColor = System.Drawing.Color.White;
            this.whitePanel.ColumnCount = 1;
            this.whitePanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.whitePanel.Location = new System.Drawing.Point(293, 11);
            this.whitePanel.Name = "whitePanel";
            this.whitePanel.RowCount = 1;
            this.whitePanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.whitePanel.Size = new System.Drawing.Size(683, 737);
            this.whitePanel.TabIndex = 19;
            // 
            // closeWindowBtn
            // 
            this.closeWindowBtn.BackColor = System.Drawing.SystemColors.InactiveCaption;
            this.closeWindowBtn.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.closeWindowBtn.Enabled = false;
            this.closeWindowBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.closeWindowBtn.Font = new System.Drawing.Font("Arial Unicode MS", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.closeWindowBtn.Location = new System.Drawing.Point(27, 699);
            this.closeWindowBtn.Name = "closeWindowBtn";
            this.closeWindowBtn.Size = new System.Drawing.Size(245, 40);
            this.closeWindowBtn.TabIndex = 18;
            this.closeWindowBtn.Text = "Stop Calibration";
            this.closeWindowBtn.UseVisualStyleBackColor = false;
            this.closeWindowBtn.Click += new System.EventHandler(this.closeWindowBtn_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial Unicode MS", 36F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(16, 4);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(271, 128);
            this.label9.TabIndex = 17;
            this.label9.Text = "Brightness\r\nCalibration";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // brightnessPanel
            // 
            this.brightnessPanel.Controls.Add(this.whitePanel);
            this.brightnessPanel.Controls.Add(this.rawValText);
            this.brightnessPanel.Controls.Add(this.label9);
            this.brightnessPanel.Controls.Add(this.label5);
            this.brightnessPanel.Controls.Add(this.closeWindowBtn);
            this.brightnessPanel.Controls.Add(this.resetBtn);
            this.brightnessPanel.Controls.Add(this.label8);
            this.brightnessPanel.Controls.Add(this.label6);
            this.brightnessPanel.Controls.Add(this.brightnessText);
            this.brightnessPanel.Controls.Add(this.incPotValBtn);
            this.brightnessPanel.Controls.Add(this.label7);
            this.brightnessPanel.Location = new System.Drawing.Point(1115, 36);
            this.brightnessPanel.Name = "brightnessPanel";
            this.brightnessPanel.Size = new System.Drawing.Size(994, 758);
            this.brightnessPanel.TabIndex = 28;
            // 
            // notifyIcon
            // 
            this.notifyIcon.Visible = true;
            this.notifyIcon.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.notifyIcon_MouseDoubleClick);
            // 
            // gamCorMenuItem
            // 
            this.gamCorMenuItem.Checked = true;
            this.gamCorMenuItem.CheckOnClick = true;
            this.gamCorMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.gamCorMenuItem.Name = "gamCorMenuItem";
            this.gamCorMenuItem.Size = new System.Drawing.Size(253, 22);
            this.gamCorMenuItem.Text = "Gamma Corrected Response Time";
            this.gamCorMenuItem.Click += new System.EventHandler(this.gamCorMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(250, 6);
            // 
            // Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(2106, 805);
            this.Controls.Add(this.brightnessPanel);
            this.Controls.Add(this.analysePanel);
            this.Controls.Add(this.controlsPanel);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.richTextBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "Main";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "OSRTT Launcher & Analyser";
            this.Load += new System.EventHandler(this.Main_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.controlsPanel.ResumeLayout(false);
            this.controlsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.testCount)).EndInit();
            this.analysePanel.ResumeLayout(false);
            this.analysePanel.PerformLayout();
            this.brightnessPanel.ResumeLayout(false);
            this.brightnessPanel.PerformLayout();
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
        private System.Windows.Forms.Panel controlsPanel;
        private System.Windows.Forms.Panel analysePanel;
        private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
        private System.Windows.Forms.ComboBox monitorCB;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button refreshMonitorListBtn;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown testCount;
        private System.Windows.Forms.Button importRawFolder;
        private System.Windows.Forms.ToolStripMenuItem testButtonToolStripMenuItem;
        private System.Windows.Forms.ComboBox fpsLimitList;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem BrightnessCalBtn;
        private System.Windows.Forms.Label rawValText;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button resetBtn;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button incPotValBtn;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label brightnessText;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TableLayoutPanel whitePanel;
        private System.Windows.Forms.Button closeWindowBtn;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel brightnessPanel;
        private System.Windows.Forms.NotifyIcon notifyIcon;
        private System.Windows.Forms.ToolStripMenuItem verboseOutputToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem measurementsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem noGammaCorrectionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem threePercentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem tenPercentMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gammaCorrectedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem percentageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem gamCorMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}

