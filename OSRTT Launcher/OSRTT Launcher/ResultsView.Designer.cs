
namespace OSRTT_Launcher
{
    partial class ResultsView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsView));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.perceivedResponseTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.initialResponseTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.completeResponseTimeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.deNoisedRawDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveHeatmapsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.asTransparentPNGToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.stdResultsMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.runSelectToolStrip = new System.Windows.Forms.ToolStripComboBox();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.graphViewMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.rtViewMenuList = new System.Windows.Forms.ToolStripComboBox();
            this.denoiseToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.importViewMenuButton = new System.Windows.Forms.ToolStripButton();
            this.graphViewPanel = new System.Windows.Forms.Panel();
            this.viewGammaBtn = new System.Windows.Forms.Button();
            this.saveGraphNoHSpanBtn = new System.Windows.Forms.Button();
            this.saveAsPNGBtn = new System.Windows.Forms.Button();
            this.latencyLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.runSelectBox = new System.Windows.Forms.ComboBox();
            this.rtTypeLabel = new System.Windows.Forms.Label();
            this.osLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.overshootStyleListBox = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.processTypeListBox = new System.Windows.Forms.ComboBox();
            this.rtLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.resetGraphBtn = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.transSelect1 = new System.Windows.Forms.ComboBox();
            this.graphedData = new ScottPlot.FormsPlot();
            this.importPanel = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label13 = new System.Windows.Forms.Label();
            this.importGraphBtn = new System.Windows.Forms.Button();
            this.importRawFolder = new System.Windows.Forms.Button();
            this.panel2 = new System.Windows.Forms.Panel();
            this.importRawFolderBtn = new System.Windows.Forms.Button();
            this.label11 = new System.Windows.Forms.Label();
            this.importRawFileBtn = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.importResultsViewBtn = new System.Windows.Forms.Button();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.heatmaps1 = new OSRTT_Launcher.Heatmaps();
            this.button1 = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.graphViewPanel.SuspendLayout();
            this.importPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.saveHeatmapsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2577, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.perceivedResponseTimeToolStripMenuItem,
            this.initialResponseTimeToolStripMenuItem,
            this.completeResponseTimeToolStripMenuItem,
            this.toolStripSeparator2,
            this.deNoisedRawDataToolStripMenuItem});
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // perceivedResponseTimeToolStripMenuItem
            // 
            this.perceivedResponseTimeToolStripMenuItem.Checked = true;
            this.perceivedResponseTimeToolStripMenuItem.CheckOnClick = true;
            this.perceivedResponseTimeToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.perceivedResponseTimeToolStripMenuItem.Name = "perceivedResponseTimeToolStripMenuItem";
            this.perceivedResponseTimeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.perceivedResponseTimeToolStripMenuItem.Text = "Perceived Response Time";
            this.perceivedResponseTimeToolStripMenuItem.Click += new System.EventHandler(this.perceivedResponseTimeToolStripMenuItem_Click);
            // 
            // initialResponseTimeToolStripMenuItem
            // 
            this.initialResponseTimeToolStripMenuItem.CheckOnClick = true;
            this.initialResponseTimeToolStripMenuItem.Name = "initialResponseTimeToolStripMenuItem";
            this.initialResponseTimeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.initialResponseTimeToolStripMenuItem.Text = "Initial Response Time";
            this.initialResponseTimeToolStripMenuItem.Click += new System.EventHandler(this.initialResponseTimeToolStripMenuItem_Click);
            // 
            // completeResponseTimeToolStripMenuItem
            // 
            this.completeResponseTimeToolStripMenuItem.CheckOnClick = true;
            this.completeResponseTimeToolStripMenuItem.Name = "completeResponseTimeToolStripMenuItem";
            this.completeResponseTimeToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.completeResponseTimeToolStripMenuItem.Text = "Complete Response Time";
            this.completeResponseTimeToolStripMenuItem.Click += new System.EventHandler(this.completeResponseTimeToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(205, 6);
            // 
            // deNoisedRawDataToolStripMenuItem
            // 
            this.deNoisedRawDataToolStripMenuItem.CheckOnClick = true;
            this.deNoisedRawDataToolStripMenuItem.Name = "deNoisedRawDataToolStripMenuItem";
            this.deNoisedRawDataToolStripMenuItem.Size = new System.Drawing.Size(208, 22);
            this.deNoisedRawDataToolStripMenuItem.Text = "De-Noised Raw Data";
            this.deNoisedRawDataToolStripMenuItem.Click += new System.EventHandler(this.deNoisedRawDataToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            this.optionsToolStripMenuItem.Click += new System.EventHandler(this.optionsToolStripMenuItem_Click);
            // 
            // saveHeatmapsToolStripMenuItem
            // 
            this.saveHeatmapsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.asPNGToolStripMenuItem,
            this.asTransparentPNGToolStripMenuItem});
            this.saveHeatmapsToolStripMenuItem.Name = "saveHeatmapsToolStripMenuItem";
            this.saveHeatmapsToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.saveHeatmapsToolStripMenuItem.Text = "Save Heatmaps";
            // 
            // asPNGToolStripMenuItem
            // 
            this.asPNGToolStripMenuItem.Name = "asPNGToolStripMenuItem";
            this.asPNGToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.asPNGToolStripMenuItem.Text = "As PNG";
            this.asPNGToolStripMenuItem.Click += new System.EventHandler(this.asPNGToolStripMenuItem_Click);
            // 
            // asTransparentPNGToolStripMenuItem
            // 
            this.asTransparentPNGToolStripMenuItem.Name = "asTransparentPNGToolStripMenuItem";
            this.asTransparentPNGToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.asTransparentPNGToolStripMenuItem.Text = "As Transparent PNG";
            this.asTransparentPNGToolStripMenuItem.Click += new System.EventHandler(this.asTransparentPNGToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stdResultsMenuBtn,
            this.runSelectToolStrip,
            this.toolStripSeparator1,
            this.graphViewMenuBtn,
            this.rtViewMenuList,
            this.denoiseToolStripBtn,
            this.toolStripSeparator3,
            this.importViewMenuButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(2577, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // stdResultsMenuBtn
            // 
            this.stdResultsMenuBtn.Checked = true;
            this.stdResultsMenuBtn.CheckOnClick = true;
            this.stdResultsMenuBtn.CheckState = System.Windows.Forms.CheckState.Checked;
            this.stdResultsMenuBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.stdResultsMenuBtn.Image = ((System.Drawing.Image)(resources.GetObject("stdResultsMenuBtn.Image")));
            this.stdResultsMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.stdResultsMenuBtn.Name = "stdResultsMenuBtn";
            this.stdResultsMenuBtn.Size = new System.Drawing.Size(65, 22);
            this.stdResultsMenuBtn.Text = "Heatmaps";
            this.stdResultsMenuBtn.Click += new System.EventHandler(this.stdResultsMenuBtn_Click);
            // 
            // runSelectToolStrip
            // 
            this.runSelectToolStrip.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runSelectToolStrip.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runSelectToolStrip.Margin = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.runSelectToolStrip.Name = "runSelectToolStrip";
            this.runSelectToolStrip.Size = new System.Drawing.Size(121, 25);
            this.runSelectToolStrip.SelectedIndexChanged += new System.EventHandler(this.runSelectToolStrip_SelectedIndexChanged);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // graphViewMenuBtn
            // 
            this.graphViewMenuBtn.CheckOnClick = true;
            this.graphViewMenuBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.graphViewMenuBtn.Image = ((System.Drawing.Image)(resources.GetObject("graphViewMenuBtn.Image")));
            this.graphViewMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphViewMenuBtn.Name = "graphViewMenuBtn";
            this.graphViewMenuBtn.Size = new System.Drawing.Size(100, 22);
            this.graphViewMenuBtn.Text = "Raw Data Graphs";
            this.graphViewMenuBtn.Click += new System.EventHandler(this.graphViewMenuBtn_Click);
            // 
            // rtViewMenuList
            // 
            this.rtViewMenuList.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.rtViewMenuList.DropDownWidth = 175;
            this.rtViewMenuList.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.rtViewMenuList.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtViewMenuList.Margin = new System.Windows.Forms.Padding(5, 0, 1, 0);
            this.rtViewMenuList.Name = "rtViewMenuList";
            this.rtViewMenuList.Size = new System.Drawing.Size(175, 25);
            this.rtViewMenuList.SelectedIndexChanged += new System.EventHandler(this.rtViewMenuList_SelectedIndexChanged);
            // 
            // denoiseToolStripBtn
            // 
            this.denoiseToolStripBtn.CheckOnClick = true;
            this.denoiseToolStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.denoiseToolStripBtn.Image = ((System.Drawing.Image)(resources.GetObject("denoiseToolStripBtn.Image")));
            this.denoiseToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.denoiseToolStripBtn.Name = "denoiseToolStripBtn";
            this.denoiseToolStripBtn.Size = new System.Drawing.Size(88, 22);
            this.denoiseToolStripBtn.Text = "Denoise Graph";
            this.denoiseToolStripBtn.Click += new System.EventHandler(this.denoiseToolStripBtn_Click);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // importViewMenuButton
            // 
            this.importViewMenuButton.CheckOnClick = true;
            this.importViewMenuButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.importViewMenuButton.Image = ((System.Drawing.Image)(resources.GetObject("importViewMenuButton.Image")));
            this.importViewMenuButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.importViewMenuButton.Name = "importViewMenuButton";
            this.importViewMenuButton.Size = new System.Drawing.Size(74, 22);
            this.importViewMenuButton.Text = "Import Data";
            this.importViewMenuButton.Click += new System.EventHandler(this.importViewMenuButton_Click);
            // 
            // graphViewPanel
            // 
            this.graphViewPanel.Controls.Add(this.button1);
            this.graphViewPanel.Controls.Add(this.viewGammaBtn);
            this.graphViewPanel.Controls.Add(this.saveGraphNoHSpanBtn);
            this.graphViewPanel.Controls.Add(this.saveAsPNGBtn);
            this.graphViewPanel.Controls.Add(this.latencyLabel);
            this.graphViewPanel.Controls.Add(this.label8);
            this.graphViewPanel.Controls.Add(this.label7);
            this.graphViewPanel.Controls.Add(this.runSelectBox);
            this.graphViewPanel.Controls.Add(this.rtTypeLabel);
            this.graphViewPanel.Controls.Add(this.osLabel);
            this.graphViewPanel.Controls.Add(this.label6);
            this.graphViewPanel.Controls.Add(this.label4);
            this.graphViewPanel.Controls.Add(this.overshootStyleListBox);
            this.graphViewPanel.Controls.Add(this.label3);
            this.graphViewPanel.Controls.Add(this.processTypeListBox);
            this.graphViewPanel.Controls.Add(this.rtLabel);
            this.graphViewPanel.Controls.Add(this.label2);
            this.graphViewPanel.Controls.Add(this.resetGraphBtn);
            this.graphViewPanel.Controls.Add(this.label1);
            this.graphViewPanel.Controls.Add(this.transSelect1);
            this.graphViewPanel.Controls.Add(this.graphedData);
            this.graphViewPanel.Location = new System.Drawing.Point(5, 442);
            this.graphViewPanel.Name = "graphViewPanel";
            this.graphViewPanel.Size = new System.Drawing.Size(1384, 719);
            this.graphViewPanel.TabIndex = 4;
            // 
            // viewGammaBtn
            // 
            this.viewGammaBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.viewGammaBtn.FlatAppearance.BorderSize = 0;
            this.viewGammaBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.viewGammaBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.viewGammaBtn.Location = new System.Drawing.Point(1180, 9);
            this.viewGammaBtn.Name = "viewGammaBtn";
            this.viewGammaBtn.Size = new System.Drawing.Size(193, 36);
            this.viewGammaBtn.TabIndex = 28;
            this.viewGammaBtn.Text = "Gamma Curve";
            this.viewGammaBtn.UseVisualStyleBackColor = false;
            this.viewGammaBtn.Click += new System.EventHandler(this.viewGammaBtn_Click);
            // 
            // saveGraphNoHSpanBtn
            // 
            this.saveGraphNoHSpanBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveGraphNoHSpanBtn.FlatAppearance.BorderSize = 0;
            this.saveGraphNoHSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveGraphNoHSpanBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.saveGraphNoHSpanBtn.Location = new System.Drawing.Point(1187, 646);
            this.saveGraphNoHSpanBtn.Name = "saveGraphNoHSpanBtn";
            this.saveGraphNoHSpanBtn.Size = new System.Drawing.Size(182, 53);
            this.saveGraphNoHSpanBtn.TabIndex = 25;
            this.saveGraphNoHSpanBtn.Text = "Save as PNG\r\nWithout Block";
            this.saveGraphNoHSpanBtn.UseVisualStyleBackColor = false;
            this.saveGraphNoHSpanBtn.Click += new System.EventHandler(this.saveGraphNoHSpanBtn_Click);
            // 
            // saveAsPNGBtn
            // 
            this.saveAsPNGBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveAsPNGBtn.FlatAppearance.BorderSize = 0;
            this.saveAsPNGBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveAsPNGBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.saveAsPNGBtn.Location = new System.Drawing.Point(1187, 606);
            this.saveAsPNGBtn.Name = "saveAsPNGBtn";
            this.saveAsPNGBtn.Size = new System.Drawing.Size(182, 34);
            this.saveAsPNGBtn.TabIndex = 24;
            this.saveAsPNGBtn.Text = "Save as PNG";
            this.saveAsPNGBtn.UseVisualStyleBackColor = false;
            this.saveAsPNGBtn.Click += new System.EventHandler(this.saveAsPNGBtn_Click);
            // 
            // latencyLabel
            // 
            this.latencyLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.latencyLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.latencyLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.latencyLabel.Location = new System.Drawing.Point(1190, 504);
            this.latencyLabel.Name = "latencyLabel";
            this.latencyLabel.Size = new System.Drawing.Size(179, 50);
            this.latencyLabel.TabIndex = 23;
            this.latencyLabel.Text = "0 ms";
            this.latencyLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(1192, 477);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(177, 26);
            this.label8.TabIndex = 22;
            this.label8.Text = "Result Latency:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(18, 13);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(156, 26);
            this.label7.TabIndex = 21;
            this.label7.Text = "Select a Run:";
            // 
            // runSelectBox
            // 
            this.runSelectBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.runSelectBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.runSelectBox.FormattingEnabled = true;
            this.runSelectBox.Location = new System.Drawing.Point(176, 10);
            this.runSelectBox.Name = "runSelectBox";
            this.runSelectBox.Size = new System.Drawing.Size(140, 33);
            this.runSelectBox.TabIndex = 20;
            this.runSelectBox.SelectedIndexChanged += new System.EventHandler(this.runSelectBox_SelectedIndexChanged);
            // 
            // rtTypeLabel
            // 
            this.rtTypeLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtTypeLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtTypeLabel.Location = new System.Drawing.Point(1203, 83);
            this.rtTypeLabel.Name = "rtTypeLabel";
            this.rtTypeLabel.Size = new System.Drawing.Size(152, 26);
            this.rtTypeLabel.TabIndex = 19;
            this.rtTypeLabel.Text = "Perceived";
            this.rtTypeLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // osLabel
            // 
            this.osLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.osLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.osLabel.Location = new System.Drawing.Point(1189, 327);
            this.osLabel.Name = "osLabel";
            this.osLabel.Size = new System.Drawing.Size(179, 50);
            this.osLabel.TabIndex = 18;
            this.osLabel.Text = "0 RGB";
            this.osLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(1197, 296);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(158, 31);
            this.label6.TabIndex = 17;
            this.label6.Text = "Overshoot:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(1184, 392);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(189, 17);
            this.label4.TabIndex = 16;
            this.label4.Text = "Change Overshoot Style:";
            // 
            // overshootStyleListBox
            // 
            this.overshootStyleListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.overshootStyleListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.overshootStyleListBox.FormattingEnabled = true;
            this.overshootStyleListBox.Location = new System.Drawing.Point(1180, 411);
            this.overshootStyleListBox.Name = "overshootStyleListBox";
            this.overshootStyleListBox.Size = new System.Drawing.Size(193, 24);
            this.overshootStyleListBox.TabIndex = 15;
            this.overshootStyleListBox.SelectedIndexChanged += new System.EventHandler(this.overshootStyleListBox_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(1184, 213);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(187, 15);
            this.label3.TabIndex = 14;
            this.label3.Text = "Change Measurement Style:";
            // 
            // processTypeListBox
            // 
            this.processTypeListBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.processTypeListBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.processTypeListBox.FormattingEnabled = true;
            this.processTypeListBox.Location = new System.Drawing.Point(1180, 232);
            this.processTypeListBox.Name = "processTypeListBox";
            this.processTypeListBox.Size = new System.Drawing.Size(193, 24);
            this.processTypeListBox.TabIndex = 13;
            this.processTypeListBox.SelectedIndexChanged += new System.EventHandler(this.processTypeListBox_SelectedIndexChanged);
            // 
            // rtLabel
            // 
            this.rtLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtLabel.Location = new System.Drawing.Point(1190, 141);
            this.rtLabel.Name = "rtLabel";
            this.rtLabel.Size = new System.Drawing.Size(179, 50);
            this.rtLabel.TabIndex = 10;
            this.rtLabel.Text = "0 ms";
            this.rtLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(1185, 109);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(184, 26);
            this.label2.TabIndex = 9;
            this.label2.Text = "Response Time:";
            // 
            // resetGraphBtn
            // 
            this.resetGraphBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.resetGraphBtn.FlatAppearance.BorderSize = 0;
            this.resetGraphBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.resetGraphBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.resetGraphBtn.Location = new System.Drawing.Point(974, 9);
            this.resetGraphBtn.Name = "resetGraphBtn";
            this.resetGraphBtn.Size = new System.Drawing.Size(193, 34);
            this.resetGraphBtn.TabIndex = 8;
            this.resetGraphBtn.Text = "Reset Graph";
            this.resetGraphBtn.UseVisualStyleBackColor = false;
            this.resetGraphBtn.Click += new System.EventHandler(this.resetGraphBtn_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(376, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(217, 26);
            this.label1.TabIndex = 2;
            this.label1.Text = "Select a Transition:";
            // 
            // transSelect1
            // 
            this.transSelect1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.transSelect1.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.transSelect1.FormattingEnabled = true;
            this.transSelect1.Location = new System.Drawing.Point(595, 10);
            this.transSelect1.Name = "transSelect1";
            this.transSelect1.Size = new System.Drawing.Size(328, 33);
            this.transSelect1.TabIndex = 1;
            this.transSelect1.SelectedIndexChanged += new System.EventHandler(this.transSelect1_SelectedIndexChanged);
            // 
            // graphedData
            // 
            this.graphedData.Location = new System.Drawing.Point(6, 51);
            this.graphedData.Name = "graphedData";
            this.graphedData.Size = new System.Drawing.Size(1184, 666);
            this.graphedData.TabIndex = 0;
            // 
            // importPanel
            // 
            this.importPanel.Controls.Add(this.panel3);
            this.importPanel.Controls.Add(this.panel2);
            this.importPanel.Controls.Add(this.panel1);
            this.importPanel.Location = new System.Drawing.Point(5, 52);
            this.importPanel.Name = "importPanel";
            this.importPanel.Size = new System.Drawing.Size(1082, 290);
            this.importPanel.TabIndex = 5;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.importGraphBtn);
            this.panel3.Controls.Add(this.importRawFolder);
            this.panel3.Location = new System.Drawing.Point(755, 3);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(314, 280);
            this.panel3.TabIndex = 25;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.BackColor = System.Drawing.Color.Transparent;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.Color.Black;
            this.label13.Location = new System.Drawing.Point(23, 9);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(270, 37);
            this.label13.TabIndex = 23;
            this.label13.Text = "Graph Raw Data";
            // 
            // importGraphBtn
            // 
            this.importGraphBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importGraphBtn.FlatAppearance.BorderSize = 0;
            this.importGraphBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importGraphBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importGraphBtn.Location = new System.Drawing.Point(43, 59);
            this.importGraphBtn.Name = "importGraphBtn";
            this.importGraphBtn.Size = new System.Drawing.Size(231, 93);
            this.importGraphBtn.TabIndex = 7;
            this.importGraphBtn.Text = "Import Raw Data File to Graph";
            this.importGraphBtn.UseVisualStyleBackColor = false;
            this.importGraphBtn.Click += new System.EventHandler(this.importGraphBtn_Click);
            // 
            // importRawFolder
            // 
            this.importRawFolder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFolder.FlatAppearance.BorderSize = 0;
            this.importRawFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFolder.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importRawFolder.Location = new System.Drawing.Point(43, 158);
            this.importRawFolder.Name = "importRawFolder";
            this.importRawFolder.Size = new System.Drawing.Size(231, 93);
            this.importRawFolder.TabIndex = 8;
            this.importRawFolder.Text = "Import Raw Data Folder to Graph";
            this.importRawFolder.UseVisualStyleBackColor = false;
            this.importRawFolder.Click += new System.EventHandler(this.importRawFolder_Click);
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.importRawFolderBtn);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.importRawFileBtn);
            this.panel2.Location = new System.Drawing.Point(7, 3);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(359, 239);
            this.panel2.TabIndex = 24;
            // 
            // importRawFolderBtn
            // 
            this.importRawFolderBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFolderBtn.FlatAppearance.BorderSize = 0;
            this.importRawFolderBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFolderBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importRawFolderBtn.Location = new System.Drawing.Point(22, 142);
            this.importRawFolderBtn.Name = "importRawFolderBtn";
            this.importRawFolderBtn.Size = new System.Drawing.Size(309, 75);
            this.importRawFolderBtn.TabIndex = 24;
            this.importRawFolderBtn.Text = "Import Raw Data Folder to Process";
            this.importRawFolderBtn.UseVisualStyleBackColor = false;
            this.importRawFolderBtn.Click += new System.EventHandler(this.importRawFolderBtn_Click);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.BackColor = System.Drawing.Color.Transparent;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.Color.Black;
            this.label11.Location = new System.Drawing.Point(29, 9);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(297, 37);
            this.label11.TabIndex = 23;
            this.label11.Text = "Process Raw Data";
            // 
            // importRawFileBtn
            // 
            this.importRawFileBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFileBtn.FlatAppearance.BorderSize = 0;
            this.importRawFileBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFileBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importRawFileBtn.Location = new System.Drawing.Point(22, 59);
            this.importRawFileBtn.Name = "importRawFileBtn";
            this.importRawFileBtn.Size = new System.Drawing.Size(309, 75);
            this.importRawFileBtn.TabIndex = 9;
            this.importRawFileBtn.Text = "Import Raw Data File to Process";
            this.importRawFileBtn.UseVisualStyleBackColor = false;
            this.importRawFileBtn.Click += new System.EventHandler(this.importRawFileBtn_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.importResultsViewBtn);
            this.panel1.Location = new System.Drawing.Point(381, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(359, 174);
            this.panel1.TabIndex = 10;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.Black;
            this.label5.Location = new System.Drawing.Point(37, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(282, 37);
            this.label5.TabIndex = 23;
            this.label5.Text = "Create Heatmaps";
            // 
            // importResultsViewBtn
            // 
            this.importResultsViewBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importResultsViewBtn.FlatAppearance.BorderSize = 0;
            this.importResultsViewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importResultsViewBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importResultsViewBtn.Location = new System.Drawing.Point(24, 59);
            this.importResultsViewBtn.Name = "importResultsViewBtn";
            this.importResultsViewBtn.Size = new System.Drawing.Size(309, 93);
            this.importResultsViewBtn.TabIndex = 9;
            this.importResultsViewBtn.Text = "Import Processed Data File for Heatmaps";
            this.importResultsViewBtn.UseVisualStyleBackColor = false;
            this.importResultsViewBtn.Click += new System.EventHandler(this.importResultsViewBtn_Click);
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(0, 348);
            this.progressBar1.MarqueeAnimationSpeed = 30;
            this.progressBar1.Maximum = 50;
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(1087, 23);
            this.progressBar1.Step = 50;
            this.progressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressBar1.TabIndex = 34;
            this.progressBar1.Visible = false;
            // 
            // heatmaps1
            // 
            this.heatmaps1.Location = new System.Drawing.Point(1395, 55);
            this.heatmaps1.Name = "heatmaps1";
            this.heatmaps1.Size = new System.Drawing.Size(1775, 950);
            this.heatmaps1.TabIndex = 6;
            // 
            // button1
            // 
            this.button1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.button1.FlatAppearance.BorderSize = 0;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.button1.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.button1.Location = new System.Drawing.Point(1180, 48);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(193, 36);
            this.button1.TabIndex = 29;
            this.button1.Text = "Test";
            this.button1.UseVisualStyleBackColor = false;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // ResultsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(2577, 1329);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.heatmaps1);
            this.Controls.Add(this.importPanel);
            this.Controls.Add(this.graphViewPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ResultsView";
            this.Text = "ResultsView";
            this.Load += new System.EventHandler(this.ResultsView_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.graphViewPanel.ResumeLayout(false);
            this.graphViewPanel.PerformLayout();
            this.importPanel.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton stdResultsMenuBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton graphViewMenuBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton importViewMenuButton;
        private System.Windows.Forms.Panel graphViewPanel;
        private ScottPlot.FormsPlot graphedData;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox transSelect1;
        private System.Windows.Forms.Panel importPanel;
        private System.Windows.Forms.Button importGraphBtn;
        private System.Windows.Forms.Button resetGraphBtn;
        private System.Windows.Forms.Label rtLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox processTypeListBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox overshootStyleListBox;
        private System.Windows.Forms.Label osLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label rtTypeLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox runSelectBox;
        private System.Windows.Forms.Button importRawFolder;
        private System.Windows.Forms.Label latencyLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button importResultsViewBtn;
        private System.Windows.Forms.Button saveAsPNGBtn;
        private System.Windows.Forms.Button saveGraphNoHSpanBtn;
        private System.Windows.Forms.ToolStripMenuItem saveHeatmapsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem asTransparentPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem perceivedResponseTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem initialResponseTimeToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem completeResponseTimeToolStripMenuItem;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button importRawFileBtn;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button importRawFolderBtn;
        private Heatmaps heatmaps1;
        private System.Windows.Forms.ToolStripComboBox runSelectToolStrip;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.ToolStripMenuItem asPNGToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem deNoisedRawDataToolStripMenuItem;
        private System.Windows.Forms.Button viewGammaBtn;
        private System.Windows.Forms.ToolStripComboBox rtViewMenuList;
        private System.Windows.Forms.ToolStripButton denoiseToolStripBtn;
        private System.Windows.Forms.Button button1;
    }
}