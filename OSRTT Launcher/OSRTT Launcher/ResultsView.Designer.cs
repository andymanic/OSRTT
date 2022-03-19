
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ResultsView));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.viewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.standardResultsPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.from3 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.from2 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.from1 = new System.Windows.Forms.Label();
            this.vrrGridView = new System.Windows.Forms.DataGridView();
            this.osGridView = new System.Windows.Forms.DataGridView();
            this.rtGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.stdResultsMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.allResultsMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.graphViewMenuBtn = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.importViewMenuButton = new System.Windows.Forms.ToolStripButton();
            this.graphViewPanel = new System.Windows.Forms.Panel();
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
            this.importResultsViewBtn = new System.Windows.Forms.Button();
            this.importRawFolder = new System.Windows.Forms.Button();
            this.importGraphBtn = new System.Windows.Forms.Button();
            this.titleLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.rtStatsGridView = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.menuStrip1.SuspendLayout();
            this.standardResultsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vrrGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.osGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtGridView)).BeginInit();
            this.toolStrip1.SuspendLayout();
            this.graphViewPanel.SuspendLayout();
            this.importPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtStatsGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.viewToolStripMenuItem,
            this.optionsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(2850, 24);
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
            this.standardResultsPanel.Controls.Add(this.rtStatsGridView);
            this.standardResultsPanel.Controls.Add(this.pictureBox1);
            this.standardResultsPanel.Controls.Add(this.titleLabel);
            this.standardResultsPanel.Controls.Add(this.vrrGridView);
            this.standardResultsPanel.Controls.Add(this.osGridView);
            this.standardResultsPanel.Controls.Add(this.rtGridView);
            this.standardResultsPanel.Controls.Add(this.from2);
            this.standardResultsPanel.Controls.Add(this.from3);
            this.standardResultsPanel.Controls.Add(this.from1);
            this.standardResultsPanel.Controls.Add(this.label12);
            this.standardResultsPanel.Controls.Add(this.label10);
            this.standardResultsPanel.Controls.Add(this.label9);
            this.standardResultsPanel.Location = new System.Drawing.Point(1439, 52);
            this.standardResultsPanel.Name = "standardResultsPanel";
            this.standardResultsPanel.Size = new System.Drawing.Size(1384, 704);
            this.standardResultsPanel.TabIndex = 2;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label12.ForeColor = System.Drawing.Color.Black;
            this.label12.Location = new System.Drawing.Point(1329, 99);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(38, 26);
            this.label12.TabIndex = 27;
            this.label12.Text = "To";
            // 
            // from3
            // 
            this.from3.AutoSize = true;
            this.from3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.from3.ForeColor = System.Drawing.Color.Black;
            this.from3.Location = new System.Drawing.Point(933, 352);
            this.from3.Name = "from3";
            this.from3.Size = new System.Drawing.Size(67, 26);
            this.from3.TabIndex = 26;
            this.from3.Text = "From";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.Color.Black;
            this.label10.Location = new System.Drawing.Point(885, 99);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(38, 26);
            this.label10.TabIndex = 25;
            this.label10.Text = "To";
            // 
            // from2
            // 
            this.from2.AutoSize = true;
            this.from2.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.from2.ForeColor = System.Drawing.Color.Black;
            this.from2.Location = new System.Drawing.Point(489, 352);
            this.from2.Name = "from2";
            this.from2.Size = new System.Drawing.Size(67, 26);
            this.from2.TabIndex = 24;
            this.from2.Text = "From";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.ForeColor = System.Drawing.Color.Black;
            this.label9.Location = new System.Drawing.Point(435, 99);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 26);
            this.label9.TabIndex = 23;
            this.label9.Text = "To";
            // 
            // from1
            // 
            this.from1.AutoSize = true;
            this.from1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.from1.ForeColor = System.Drawing.Color.Black;
            this.from1.Location = new System.Drawing.Point(39, 352);
            this.from1.Name = "from1";
            this.from1.Size = new System.Drawing.Size(67, 26);
            this.from1.TabIndex = 22;
            this.from1.Text = "From";
            // 
            // vrrGridView
            // 
            this.vrrGridView.AllowUserToAddRows = false;
            this.vrrGridView.AllowUserToDeleteRows = false;
            this.vrrGridView.AllowUserToResizeColumns = false;
            this.vrrGridView.AllowUserToResizeRows = false;
            this.vrrGridView.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.vrrGridView.ColumnHeadersHeight = 40;
            this.vrrGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.vrrGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.vrrGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.vrrGridView.EnableHeadersVisualStyles = false;
            this.vrrGridView.GridColor = System.Drawing.Color.White;
            this.vrrGridView.Location = new System.Drawing.Point(939, 103);
            this.vrrGridView.MultiSelect = false;
            this.vrrGridView.Name = "vrrGridView";
            this.vrrGridView.ReadOnly = true;
            this.vrrGridView.RowHeadersWidth = 65;
            this.vrrGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.vrrGridView.RowTemplate.Height = 35;
            this.vrrGridView.RowTemplate.ReadOnly = true;
            this.vrrGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.vrrGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.vrrGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.vrrGridView.ShowCellToolTips = false;
            this.vrrGridView.ShowEditingIcon = false;
            this.vrrGridView.Size = new System.Drawing.Size(392, 250);
            this.vrrGridView.TabIndex = 9;
            // 
            // osGridView
            // 
            this.osGridView.AllowUserToAddRows = false;
            this.osGridView.AllowUserToDeleteRows = false;
            this.osGridView.AllowUserToResizeColumns = false;
            this.osGridView.AllowUserToResizeRows = false;
            this.osGridView.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.osGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.osGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.osGridView.ColumnHeadersHeight = 40;
            this.osGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.osGridView.DefaultCellStyle = dataGridViewCellStyle3;
            this.osGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.osGridView.EnableHeadersVisualStyles = false;
            this.osGridView.GridColor = System.Drawing.Color.White;
            this.osGridView.Location = new System.Drawing.Point(494, 103);
            this.osGridView.MultiSelect = false;
            this.osGridView.Name = "osGridView";
            this.osGridView.ReadOnly = true;
            this.osGridView.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            this.osGridView.RowHeadersWidth = 65;
            this.osGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.osGridView.RowTemplate.Height = 35;
            this.osGridView.RowTemplate.ReadOnly = true;
            this.osGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.osGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.osGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.osGridView.ShowCellToolTips = false;
            this.osGridView.ShowEditingIcon = false;
            this.osGridView.Size = new System.Drawing.Size(392, 250);
            this.osGridView.TabIndex = 8;
            // 
            // rtGridView
            // 
            this.rtGridView.AllowUserToAddRows = false;
            this.rtGridView.AllowUserToDeleteRows = false;
            this.rtGridView.AllowUserToResizeColumns = false;
            this.rtGridView.AllowUserToResizeRows = false;
            this.rtGridView.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.rtGridView.ColumnHeadersHeight = 40;
            this.rtGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.rtGridView.DefaultCellStyle = dataGridViewCellStyle4;
            this.rtGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.rtGridView.EnableHeadersVisualStyles = false;
            this.rtGridView.GridColor = System.Drawing.Color.White;
            this.rtGridView.Location = new System.Drawing.Point(45, 103);
            this.rtGridView.MultiSelect = false;
            this.rtGridView.Name = "rtGridView";
            this.rtGridView.ReadOnly = true;
            this.rtGridView.RowHeadersWidth = 65;
            this.rtGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.rtGridView.RowTemplate.Height = 35;
            this.rtGridView.RowTemplate.ReadOnly = true;
            this.rtGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.rtGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.rtGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.rtGridView.ShowCellToolTips = false;
            this.rtGridView.ShowEditingIcon = false;
            this.rtGridView.Size = new System.Drawing.Size(392, 250);
            this.rtGridView.TabIndex = 7;
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
            this.toolStrip1.Size = new System.Drawing.Size(2850, 25);
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
            this.stdResultsMenuBtn.Size = new System.Drawing.Size(98, 22);
            this.stdResultsMenuBtn.Text = "Standard Results";
            this.stdResultsMenuBtn.Click += new System.EventHandler(this.stdResultsMenuBtn_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
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
            this.graphViewPanel.Location = new System.Drawing.Point(1439, 762);
            this.graphViewPanel.Name = "graphViewPanel";
            this.graphViewPanel.Size = new System.Drawing.Size(1384, 704);
            this.graphViewPanel.TabIndex = 4;
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
            this.runSelectBox.Location = new System.Drawing.Point(180, 10);
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
            this.overshootStyleListBox.Location = new System.Drawing.Point(1187, 411);
            this.overshootStyleListBox.Name = "overshootStyleListBox";
            this.overshootStyleListBox.Size = new System.Drawing.Size(182, 24);
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
            this.processTypeListBox.Location = new System.Drawing.Point(1187, 232);
            this.processTypeListBox.Name = "processTypeListBox";
            this.processTypeListBox.Size = new System.Drawing.Size(182, 24);
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
            this.label1.Location = new System.Drawing.Point(376, 13);
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
            this.transSelect1.Location = new System.Drawing.Point(599, 10);
            this.transSelect1.Name = "transSelect1";
            this.transSelect1.Size = new System.Drawing.Size(306, 33);
            this.transSelect1.TabIndex = 1;
            this.transSelect1.SelectedIndexChanged += new System.EventHandler(this.transSelect1_SelectedIndexChanged);
            // 
            // graphedData
            // 
            this.graphedData.Location = new System.Drawing.Point(7, 51);
            this.graphedData.Name = "graphedData";
            this.graphedData.Size = new System.Drawing.Size(1183, 650);
            this.graphedData.TabIndex = 0;
            // 
            // importPanel
            // 
            this.importPanel.Controls.Add(this.importResultsViewBtn);
            this.importPanel.Controls.Add(this.importRawFolder);
            this.importPanel.Controls.Add(this.importGraphBtn);
            this.importPanel.Location = new System.Drawing.Point(5, 52);
            this.importPanel.Name = "importPanel";
            this.importPanel.Size = new System.Drawing.Size(1384, 704);
            this.importPanel.TabIndex = 5;
            // 
            // importResultsViewBtn
            // 
            this.importResultsViewBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importResultsViewBtn.FlatAppearance.BorderSize = 0;
            this.importResultsViewBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importResultsViewBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importResultsViewBtn.Location = new System.Drawing.Point(41, 102);
            this.importResultsViewBtn.Name = "importResultsViewBtn";
            this.importResultsViewBtn.Size = new System.Drawing.Size(309, 93);
            this.importResultsViewBtn.TabIndex = 9;
            this.importResultsViewBtn.Text = "Import Processed Data File for Heatmaps";
            this.importResultsViewBtn.UseVisualStyleBackColor = false;
            this.importResultsViewBtn.Click += new System.EventHandler(this.importResultsViewBtn_Click);
            // 
            // importRawFolder
            // 
            this.importRawFolder.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importRawFolder.FlatAppearance.BorderSize = 0;
            this.importRawFolder.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importRawFolder.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importRawFolder.Location = new System.Drawing.Point(428, 201);
            this.importRawFolder.Name = "importRawFolder";
            this.importRawFolder.Size = new System.Drawing.Size(231, 93);
            this.importRawFolder.TabIndex = 8;
            this.importRawFolder.Text = "Import Raw Data Folder to Graph";
            this.importRawFolder.UseVisualStyleBackColor = false;
            this.importRawFolder.Click += new System.EventHandler(this.importRawFolder_Click);
            // 
            // importGraphBtn
            // 
            this.importGraphBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.importGraphBtn.FlatAppearance.BorderSize = 0;
            this.importGraphBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.importGraphBtn.Font = new System.Drawing.Font("Consolas", 18F, System.Drawing.FontStyle.Bold);
            this.importGraphBtn.Location = new System.Drawing.Point(428, 102);
            this.importGraphBtn.Name = "importGraphBtn";
            this.importGraphBtn.Size = new System.Drawing.Size(231, 93);
            this.importGraphBtn.TabIndex = 7;
            this.importGraphBtn.Text = "Import Raw Data File to Graph";
            this.importGraphBtn.UseVisualStyleBackColor = false;
            this.importGraphBtn.Click += new System.EventHandler(this.importGraphBtn_Click);
            // 
            // titleLabel
            // 
            this.titleLabel.Font = new System.Drawing.Font("Calibri", 28F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.titleLabel.ForeColor = System.Drawing.Color.Black;
            this.titleLabel.Location = new System.Drawing.Point(45, 8);
            this.titleLabel.Name = "titleLabel";
            this.titleLabel.Size = new System.Drawing.Size(1295, 52);
            this.titleLabel.TabIndex = 28;
            this.titleLabel.Text = "Monitor Name - Freqency - FPS Limit";
            this.titleLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::OSRTT_Launcher.Properties.Resources.icon_wide;
            this.pictureBox1.Location = new System.Drawing.Point(1237, 614);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(118, 66);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 29;
            this.pictureBox1.TabStop = false;
            // 
            // rtStatsGridView
            // 
            this.rtStatsGridView.AllowUserToAddRows = false;
            this.rtStatsGridView.AllowUserToDeleteRows = false;
            this.rtStatsGridView.AllowUserToResizeColumns = false;
            this.rtStatsGridView.AllowUserToResizeRows = false;
            this.rtStatsGridView.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.rtStatsGridView.ColumnHeadersHeight = 40;
            this.rtStatsGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.rtStatsGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1});
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.rtStatsGridView.DefaultCellStyle = dataGridViewCellStyle1;
            this.rtStatsGridView.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.rtStatsGridView.EnableHeadersVisualStyles = false;
            this.rtStatsGridView.GridColor = System.Drawing.Color.White;
            this.rtStatsGridView.Location = new System.Drawing.Point(98, 396);
            this.rtStatsGridView.MultiSelect = false;
            this.rtStatsGridView.Name = "rtStatsGridView";
            this.rtStatsGridView.ReadOnly = true;
            this.rtStatsGridView.RowHeadersWidth = 65;
            this.rtStatsGridView.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.rtStatsGridView.RowTemplate.Height = 35;
            this.rtStatsGridView.RowTemplate.ReadOnly = true;
            this.rtStatsGridView.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.rtStatsGridView.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.rtStatsGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.rtStatsGridView.ShowCellToolTips = false;
            this.rtStatsGridView.ShowEditingIcon = false;
            this.rtStatsGridView.Size = new System.Drawing.Size(277, 250);
            this.rtStatsGridView.TabIndex = 30;
            // 
            // Column1
            // 
            this.Column1.HeaderText = "";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            // 
            // ResultsView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(2850, 1689);
            this.Controls.Add(this.importPanel);
            this.Controls.Add(this.graphViewPanel);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.standardResultsPanel);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.Name = "ResultsView";
            this.Text = "ResultsView";
            this.Load += new System.EventHandler(this.ResultsView_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.standardResultsPanel.ResumeLayout(false);
            this.standardResultsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.vrrGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.osGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtGridView)).EndInit();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.graphViewPanel.ResumeLayout(false);
            this.graphViewPanel.PerformLayout();
            this.importPanel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rtStatsGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
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
        private System.Windows.Forms.DataGridView rtGridView;
        private System.Windows.Forms.DataGridView vrrGridView;
        private System.Windows.Forms.DataGridView osGridView;
        private System.Windows.Forms.Button importResultsViewBtn;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label from3;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label from2;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label from1;
        private System.Windows.Forms.Label titleLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.DataGridView rtStatsGridView;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
    }
}