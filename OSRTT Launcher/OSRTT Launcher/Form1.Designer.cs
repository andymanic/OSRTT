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
            this.reconnectDeviceToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.checkDeviceFirmwareVersionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.resultsSettingsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.devStatLbl = new System.Windows.Forms.Label();
            this.devStat = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.measFrm = new System.Windows.Forms.TextBox();
            this.measTo = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.updateTable = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.analyseResultsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.debugModeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menuStrip1.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // launchBtn
            // 
            this.launchBtn.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.launchBtn.Location = new System.Drawing.Point(18, 59);
            this.launchBtn.Name = "launchBtn";
            this.launchBtn.Size = new System.Drawing.Size(551, 46);
            this.launchBtn.TabIndex = 0;
            this.launchBtn.Text = "Launch Test";
            this.launchBtn.UseVisualStyleBackColor = true;
            this.launchBtn.Click += new System.EventHandler(this.launchBtn_Click);
            // 
            // resultsBtn
            // 
            this.resultsBtn.Font = new System.Drawing.Font("Arial Black", 16F, System.Drawing.FontStyle.Bold);
            this.resultsBtn.Location = new System.Drawing.Point(17, 89);
            this.resultsBtn.Name = "resultsBtn";
            this.resultsBtn.Size = new System.Drawing.Size(552, 75);
            this.resultsBtn.TabIndex = 1;
            this.resultsBtn.Text = "Analyse Results";
            this.resultsBtn.UseVisualStyleBackColor = true;
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
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deviceToolStripMenuItem,
            this.analyseResultsToolStripMenuItem,
            this.resultsSettingsToolStripMenuItem,
            this.debugModeToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1104, 24);
            this.menuStrip1.TabIndex = 4;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // deviceToolStripMenuItem
            // 
            this.deviceToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.reconnectDeviceToolStripMenuItem,
            this.checkDeviceFirmwareVersionToolStripMenuItem});
            this.deviceToolStripMenuItem.Name = "deviceToolStripMenuItem";
            this.deviceToolStripMenuItem.Size = new System.Drawing.Size(54, 20);
            this.deviceToolStripMenuItem.Text = "Device";
            // 
            // reconnectDeviceToolStripMenuItem
            // 
            this.reconnectDeviceToolStripMenuItem.Name = "reconnectDeviceToolStripMenuItem";
            this.reconnectDeviceToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.reconnectDeviceToolStripMenuItem.Text = "Reconnect Device";
            this.reconnectDeviceToolStripMenuItem.Click += new System.EventHandler(this.reconnectDeviceToolStripMenuItem_Click);
            // 
            // checkDeviceFirmwareVersionToolStripMenuItem
            // 
            this.checkDeviceFirmwareVersionToolStripMenuItem.Name = "checkDeviceFirmwareVersionToolStripMenuItem";
            this.checkDeviceFirmwareVersionToolStripMenuItem.Size = new System.Drawing.Size(238, 22);
            this.checkDeviceFirmwareVersionToolStripMenuItem.Text = "Check Device Firmware Version";
            this.checkDeviceFirmwareVersionToolStripMenuItem.Click += new System.EventHandler(this.checkDeviceFirmwareVersionToolStripMenuItem_Click);
            // 
            // resultsSettingsToolStripMenuItem
            // 
            this.resultsSettingsToolStripMenuItem.Name = "resultsSettingsToolStripMenuItem";
            this.resultsSettingsToolStripMenuItem.Size = new System.Drawing.Size(101, 20);
            this.resultsSettingsToolStripMenuItem.Text = "Results Settings";
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
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(77, 13);
            this.label1.TabIndex = 8;
            this.label1.Text = "Measure From:";
            // 
            // measFrm
            // 
            this.measFrm.Location = new System.Drawing.Point(96, 52);
            this.measFrm.Name = "measFrm";
            this.measFrm.Size = new System.Drawing.Size(100, 20);
            this.measFrm.TabIndex = 9;
            // 
            // measTo
            // 
            this.measTo.Location = new System.Drawing.Point(290, 52);
            this.measTo.Name = "measTo";
            this.measTo.Size = new System.Drawing.Size(100, 20);
            this.measTo.TabIndex = 11;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(217, 55);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(67, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Measure To:";
            // 
            // updateTable
            // 
            this.updateTable.Location = new System.Drawing.Point(422, 50);
            this.updateTable.Name = "updateTable";
            this.updateTable.Size = new System.Drawing.Size(145, 23);
            this.updateTable.TabIndex = 12;
            this.updateTable.Text = "Update Results Table";
            this.updateTable.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(192, 12);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(200, 24);
            this.label3.TabIndex = 13;
            this.label3.Text = "Measurement Controls";
            // 
            // analyseResultsToolStripMenuItem
            // 
            this.analyseResultsToolStripMenuItem.Name = "analyseResultsToolStripMenuItem";
            this.analyseResultsToolStripMenuItem.Size = new System.Drawing.Size(100, 20);
            this.analyseResultsToolStripMenuItem.Text = "Analyse Results";
            this.analyseResultsToolStripMenuItem.Click += new System.EventHandler(this.analyseResultsToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.launchBtn);
            this.panel1.Controls.Add(this.devStatLbl);
            this.panel1.Controls.Add(this.devStat);
            this.panel1.Location = new System.Drawing.Point(12, 36);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(588, 120);
            this.panel1.TabIndex = 15;
            this.panel1.Tag = "";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.resultsBtn);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Controls.Add(this.measFrm);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.label2);
            this.panel2.Controls.Add(this.updateTable);
            this.panel2.Controls.Add(this.measTo);
            this.panel2.Location = new System.Drawing.Point(12, 173);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(588, 183);
            this.panel2.TabIndex = 16;
            // 
            // debugModeToolStripMenuItem
            // 
            this.debugModeToolStripMenuItem.Name = "debugModeToolStripMenuItem";
            this.debugModeToolStripMenuItem.Size = new System.Drawing.Size(88, 20);
            this.debugModeToolStripMenuItem.Text = "Debug mode";
            this.debugModeToolStripMenuItem.Click += new System.EventHandler(this.debugModeToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1104, 811);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.menuStrip1);
            this.Controls.Add(this.richTextBox1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "OSRTT Launcher";
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
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
        private System.Windows.Forms.ToolStripMenuItem reconnectDeviceToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem checkDeviceFirmwareVersionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem resultsSettingsToolStripMenuItem;
        private System.Windows.Forms.Label devStatLbl;
        private System.Windows.Forms.Label devStat;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox measFrm;
        private System.Windows.Forms.TextBox measTo;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button updateTable;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ToolStripMenuItem analyseResultsToolStripMenuItem;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.ToolStripMenuItem debugModeToolStripMenuItem;
    }
}

