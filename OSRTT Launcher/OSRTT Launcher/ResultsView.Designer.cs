
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsView));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardResultsPanel = new System.Windows.Forms.Panel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.allResultsMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.stdResultsMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.graphViewMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.listView1 = new System.Windows.Forms.ListView();
            this.importViewMenuButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.standardResultsPanel.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataGridView1.ColumnHeadersHeight = 31;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Arial", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataGridView1.Location = new System.Drawing.Point(499, 388);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(393, 265);
            this.dataGridView1.TabIndex = 0;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1384, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // viewToolStripMenuItem
            // 
            this.viewToolStripMenuItem.Name = "viewToolStripMenuItem";
            this.viewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.viewToolStripMenuItem.Text = "View";
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "Options";
            // 
            // standardResultsPanel
            // 
            this.standardResultsPanel.Controls.Add(this.listView1);
            this.standardResultsPanel.Controls.Add(this.dataGridView1);
            this.standardResultsPanel.Location = new System.Drawing.Point(0, 58);
            this.standardResultsPanel.Name = "standardResultsPanel";
            this.standardResultsPanel.Size = new System.Drawing.Size(1384, 704);
            this.standardResultsPanel.TabIndex = 2;
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.stdResultsMenuBtn,
            this.toolStripSeparator1,
            this.allResultsMenuBtn,
            this.toolStripSeparator2,
            this.graphViewMenuBtn,
            this.toolStripSeparator3,
            this.importViewMenuButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1384, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // allResultsMenuBtn
            // 
            this.allResultsMenuBtn.CheckOnClick = true;
            this.allResultsMenuBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.allResultsMenuBtn.Image = ((System.Drawing.Image)(resources.GetObject("allResultsMenuBtn.Image")));
            this.allResultsMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.allResultsMenuBtn.Name = "allResultsMenuBtn";
            this.allResultsMenuBtn.Size = new System.Drawing.Size(65, 22);
            this.allResultsMenuBtn.Text = "All Results";
            this.allResultsMenuBtn.Click += new System.EventHandler(this.allResultsMenuBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            this.stdResultsMenuBtn.Size = new System.Drawing.Size(98, 22);
            this.stdResultsMenuBtn.Text = "Standard Results";
            this.stdResultsMenuBtn.Click += new System.EventHandler(this.stdResultsMenuBtn_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // graphViewMenuBtn
            // 
            this.graphViewMenuBtn.CheckOnClick = true;
            this.graphViewMenuBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.graphViewMenuBtn.Image = ((System.Drawing.Image)(resources.GetObject("graphViewMenuBtn.Image")));
            this.graphViewMenuBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.graphViewMenuBtn.Name = "graphViewMenuBtn";
            this.graphViewMenuBtn.Size = new System.Drawing.Size(128, 22);
            this.graphViewMenuBtn.Text = "View Raw Data Graphs";
            this.graphViewMenuBtn.Click += new System.EventHandler(this.graphViewMenuBtn_Click);
            // 
            // listView1
            // 
            this.listView1.BackColor = System.Drawing.SystemColors.WindowFrame;
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(54, 55);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(393, 265);
            this.listView1.TabIndex = 1;
            this.listView1.UseCompatibleStateImageBehavior = false;
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
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // ResultsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1384, 761);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.standardResultsPanel);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ResultsView";
            this.Text = "ResultsView";
            this.Load += new System.EventHandler(this.ResultsView_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.standardResultsPanel.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem viewToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.Panel standardResultsPanel;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton allResultsMenuBtn;
        private System.Windows.Forms.ToolStripButton stdResultsMenuBtn;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton graphViewMenuBtn;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton importViewMenuButton;
    }
}