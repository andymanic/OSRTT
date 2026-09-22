
namespace OSRTT_Launcher
{
    partial class TestSettingsPane
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
            this.startTestBtn = new MaterialSkin.Controls.MaterialButton();
            this.framerateSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel1 = new MaterialSkin.Controls.MaterialLabel();
            this.materialCard1 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel3 = new MaterialSkin.Controls.MaterialLabel();
            this.captureSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel4 = new MaterialSkin.Controls.MaterialLabel();
            this.vsyncSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.materialLabel5 = new MaterialSkin.Controls.MaterialLabel();
            this.windowSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.runSlider = new MaterialSkin.Controls.MaterialSlider();
            this.materialCard2 = new MaterialSkin.Controls.MaterialCard();
            this.clickSlider = new MaterialSkin.Controls.MaterialSlider();
            this.startLatencyBtn = new MaterialSkin.Controls.MaterialButton();
            this.materialLabel8 = new MaterialSkin.Controls.MaterialLabel();
            this.timeSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.materialCard3 = new MaterialSkin.Controls.MaterialCard();
            this.liveViewBtn = new MaterialSkin.Controls.MaterialButton();
            this.materialCard4 = new MaterialSkin.Controls.MaterialCard();
            this.materialLabel2 = new MaterialSkin.Controls.MaterialLabel();
            this.monitorSelect = new MaterialSkin.Controls.MaterialComboBox();
            this.refreshDisplayListBtn = new MaterialSkin.Controls.MaterialFloatingActionButton();
            this.materialCard5 = new MaterialSkin.Controls.MaterialCard();
            this.materialCard1.SuspendLayout();
            this.materialCard2.SuspendLayout();
            this.materialCard3.SuspendLayout();
            this.materialCard4.SuspendLayout();
            this.SuspendLayout();
            // 
            // startTestBtn
            // 
            this.startTestBtn.AutoSize = false;
            this.startTestBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startTestBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.startTestBtn.Depth = 0;
            this.startTestBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startTestBtn.HighEmphasis = true;
            this.startTestBtn.Icon = null;
            this.startTestBtn.Location = new System.Drawing.Point(18, 20);
            this.startTestBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.startTestBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.startTestBtn.Name = "startTestBtn";
            this.startTestBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.startTestBtn.Size = new System.Drawing.Size(396, 54);
            this.startTestBtn.TabIndex = 57;
            this.startTestBtn.Text = "Start Response Time Test";
            this.startTestBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.startTestBtn.UseAccentColor = true;
            this.startTestBtn.UseVisualStyleBackColor = true;
            this.startTestBtn.Click += new System.EventHandler(this.startTestBtn_Click);
            // 
            // framerateSelect
            // 
            this.framerateSelect.AutoResize = false;
            this.framerateSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.framerateSelect.Depth = 0;
            this.framerateSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.framerateSelect.DropDownHeight = 174;
            this.framerateSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.framerateSelect.DropDownWidth = 121;
            this.framerateSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.framerateSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.framerateSelect.FormattingEnabled = true;
            this.framerateSelect.IntegralHeight = false;
            this.framerateSelect.ItemHeight = 43;
            this.framerateSelect.Location = new System.Drawing.Point(150, 89);
            this.framerateSelect.MaxDropDownItems = 4;
            this.framerateSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.framerateSelect.Name = "framerateSelect";
            this.framerateSelect.Size = new System.Drawing.Size(264, 49);
            this.framerateSelect.StartIndex = 0;
            this.framerateSelect.TabIndex = 58;
            // 
            // materialLabel1
            // 
            this.materialLabel1.AutoSize = true;
            this.materialLabel1.Depth = 0;
            this.materialLabel1.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel1.Location = new System.Drawing.Point(26, 107);
            this.materialLabel1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel1.Name = "materialLabel1";
            this.materialLabel1.Size = new System.Drawing.Size(113, 19);
            this.materialLabel1.TabIndex = 59;
            this.materialLabel1.Text = "Framerate Limit";
            // 
            // materialCard1
            // 
            this.materialCard1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard1.Controls.Add(this.runSlider);
            this.materialCard1.Controls.Add(this.materialLabel5);
            this.materialCard1.Controls.Add(this.windowSelect);
            this.materialCard1.Controls.Add(this.materialLabel4);
            this.materialCard1.Controls.Add(this.vsyncSelect);
            this.materialCard1.Controls.Add(this.materialLabel3);
            this.materialCard1.Controls.Add(this.captureSelect);
            this.materialCard1.Controls.Add(this.startTestBtn);
            this.materialCard1.Controls.Add(this.materialLabel1);
            this.materialCard1.Controls.Add(this.framerateSelect);
            this.materialCard1.Depth = 0;
            this.materialCard1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard1.Location = new System.Drawing.Point(9, 9);
            this.materialCard1.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard1.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard1.Name = "materialCard1";
            this.materialCard1.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard1.Size = new System.Drawing.Size(433, 447);
            this.materialCard1.TabIndex = 60;
            // 
            // materialLabel3
            // 
            this.materialLabel3.AutoSize = true;
            this.materialLabel3.Depth = 0;
            this.materialLabel3.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel3.Location = new System.Drawing.Point(26, 249);
            this.materialLabel3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel3.Name = "materialLabel3";
            this.materialLabel3.Size = new System.Drawing.Size(96, 19);
            this.materialLabel3.TabIndex = 63;
            this.materialLabel3.Text = "Capture Time";
            // 
            // captureSelect
            // 
            this.captureSelect.AutoResize = false;
            this.captureSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.captureSelect.Depth = 0;
            this.captureSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.captureSelect.DropDownHeight = 174;
            this.captureSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.captureSelect.DropDownWidth = 121;
            this.captureSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.captureSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.captureSelect.FormattingEnabled = true;
            this.captureSelect.IntegralHeight = false;
            this.captureSelect.ItemHeight = 43;
            this.captureSelect.Items.AddRange(new object[] {
            "100ms",
            "150ms",
            "200ms",
            "250ms"});
            this.captureSelect.Location = new System.Drawing.Point(150, 231);
            this.captureSelect.MaxDropDownItems = 4;
            this.captureSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.captureSelect.Name = "captureSelect";
            this.captureSelect.Size = new System.Drawing.Size(264, 49);
            this.captureSelect.StartIndex = 0;
            this.captureSelect.TabIndex = 62;
            // 
            // materialLabel4
            // 
            this.materialLabel4.AutoSize = true;
            this.materialLabel4.Depth = 0;
            this.materialLabel4.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel4.Location = new System.Drawing.Point(26, 322);
            this.materialLabel4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel4.Name = "materialLabel4";
            this.materialLabel4.Size = new System.Drawing.Size(87, 19);
            this.materialLabel4.TabIndex = 65;
            this.materialLabel4.Text = "VSync State";
            // 
            // vsyncSelect
            // 
            this.vsyncSelect.AutoResize = false;
            this.vsyncSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.vsyncSelect.Depth = 0;
            this.vsyncSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.vsyncSelect.DropDownHeight = 174;
            this.vsyncSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.vsyncSelect.DropDownWidth = 121;
            this.vsyncSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.vsyncSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.vsyncSelect.FormattingEnabled = true;
            this.vsyncSelect.IntegralHeight = false;
            this.vsyncSelect.ItemHeight = 43;
            this.vsyncSelect.Items.AddRange(new object[] {
            "On",
            "Off"});
            this.vsyncSelect.Location = new System.Drawing.Point(150, 304);
            this.vsyncSelect.MaxDropDownItems = 4;
            this.vsyncSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.vsyncSelect.Name = "vsyncSelect";
            this.vsyncSelect.Size = new System.Drawing.Size(264, 49);
            this.vsyncSelect.StartIndex = 0;
            this.vsyncSelect.TabIndex = 64;
            // 
            // materialLabel5
            // 
            this.materialLabel5.AutoSize = true;
            this.materialLabel5.Depth = 0;
            this.materialLabel5.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel5.Location = new System.Drawing.Point(28, 393);
            this.materialLabel5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel5.Name = "materialLabel5";
            this.materialLabel5.Size = new System.Drawing.Size(99, 19);
            this.materialLabel5.TabIndex = 67;
            this.materialLabel5.Text = "Window State";
            // 
            // windowSelect
            // 
            this.windowSelect.AutoResize = false;
            this.windowSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.windowSelect.Depth = 0;
            this.windowSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.windowSelect.DropDownHeight = 174;
            this.windowSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.windowSelect.DropDownWidth = 121;
            this.windowSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.windowSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.windowSelect.FormattingEnabled = true;
            this.windowSelect.IntegralHeight = false;
            this.windowSelect.ItemHeight = 43;
            this.windowSelect.Items.AddRange(new object[] {
            "Fullscreen",
            "Windowed"});
            this.windowSelect.Location = new System.Drawing.Point(150, 375);
            this.windowSelect.MaxDropDownItems = 4;
            this.windowSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.windowSelect.Name = "windowSelect";
            this.windowSelect.Size = new System.Drawing.Size(266, 49);
            this.windowSelect.StartIndex = 0;
            this.windowSelect.TabIndex = 66;
            // 
            // runSlider
            // 
            this.runSlider.Depth = 0;
            this.runSlider.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.runSlider.Location = new System.Drawing.Point(24, 161);
            this.runSlider.MouseState = MaterialSkin.MouseState.HOVER;
            this.runSlider.Name = "runSlider";
            this.runSlider.RangeMax = 11;
            this.runSlider.RangeMin = 1;
            this.runSlider.Size = new System.Drawing.Size(392, 40);
            this.runSlider.TabIndex = 68;
            this.runSlider.Text = "Number of Test Runs";
            this.runSlider.Value = 5;
            // 
            // materialCard2
            // 
            this.materialCard2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard2.Controls.Add(this.clickSlider);
            this.materialCard2.Controls.Add(this.startLatencyBtn);
            this.materialCard2.Controls.Add(this.materialLabel8);
            this.materialCard2.Controls.Add(this.timeSelect);
            this.materialCard2.Depth = 0;
            this.materialCard2.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard2.Location = new System.Drawing.Point(454, 98);
            this.materialCard2.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard2.Name = "materialCard2";
            this.materialCard2.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard2.Size = new System.Drawing.Size(433, 237);
            this.materialCard2.TabIndex = 69;
            // 
            // clickSlider
            // 
            this.clickSlider.Depth = 0;
            this.clickSlider.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.clickSlider.Location = new System.Drawing.Point(24, 161);
            this.clickSlider.MouseState = MaterialSkin.MouseState.HOVER;
            this.clickSlider.Name = "clickSlider";
            this.clickSlider.RangeMax = 500;
            this.clickSlider.RangeMin = 1;
            this.clickSlider.Size = new System.Drawing.Size(392, 40);
            this.clickSlider.TabIndex = 68;
            this.clickSlider.Text = "Number of Clicks";
            this.clickSlider.Value = 100;
            // 
            // startLatencyBtn
            // 
            this.startLatencyBtn.AutoSize = false;
            this.startLatencyBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.startLatencyBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.startLatencyBtn.Depth = 0;
            this.startLatencyBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLatencyBtn.HighEmphasis = true;
            this.startLatencyBtn.Icon = null;
            this.startLatencyBtn.Location = new System.Drawing.Point(18, 20);
            this.startLatencyBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.startLatencyBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.startLatencyBtn.Name = "startLatencyBtn";
            this.startLatencyBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.startLatencyBtn.Size = new System.Drawing.Size(396, 54);
            this.startLatencyBtn.TabIndex = 57;
            this.startLatencyBtn.Text = "Start Latency Test";
            this.startLatencyBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.startLatencyBtn.UseAccentColor = true;
            this.startLatencyBtn.UseVisualStyleBackColor = true;
            // 
            // materialLabel8
            // 
            this.materialLabel8.AutoSize = true;
            this.materialLabel8.Depth = 0;
            this.materialLabel8.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel8.Location = new System.Drawing.Point(26, 107);
            this.materialLabel8.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel8.Name = "materialLabel8";
            this.materialLabel8.Size = new System.Drawing.Size(147, 19);
            this.materialLabel8.TabIndex = 59;
            this.materialLabel8.Text = "Time Between Clicks";
            // 
            // timeSelect
            // 
            this.timeSelect.AutoResize = false;
            this.timeSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.timeSelect.Depth = 0;
            this.timeSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.timeSelect.DropDownHeight = 174;
            this.timeSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.timeSelect.DropDownWidth = 121;
            this.timeSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.timeSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.timeSelect.FormattingEnabled = true;
            this.timeSelect.IntegralHeight = false;
            this.timeSelect.ItemHeight = 43;
            this.timeSelect.Items.AddRange(new object[] {
            "0.5",
            "1",
            "1.5",
            "2",
            "2.5",
            "3",
            "3.5",
            "4",
            "4.5",
            "5"});
            this.timeSelect.Location = new System.Drawing.Point(200, 89);
            this.timeSelect.MaxDropDownItems = 4;
            this.timeSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.timeSelect.Name = "timeSelect";
            this.timeSelect.Size = new System.Drawing.Size(214, 49);
            this.timeSelect.StartIndex = 0;
            this.timeSelect.TabIndex = 58;
            // 
            // materialCard3
            // 
            this.materialCard3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard3.Controls.Add(this.liveViewBtn);
            this.materialCard3.Depth = 0;
            this.materialCard3.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard3.Location = new System.Drawing.Point(454, 352);
            this.materialCard3.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard3.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard3.Name = "materialCard3";
            this.materialCard3.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard3.Size = new System.Drawing.Size(433, 104);
            this.materialCard3.TabIndex = 70;
            // 
            // liveViewBtn
            // 
            this.liveViewBtn.AutoSize = false;
            this.liveViewBtn.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.liveViewBtn.Density = MaterialSkin.Controls.MaterialButton.MaterialButtonDensity.Default;
            this.liveViewBtn.Depth = 0;
            this.liveViewBtn.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.liveViewBtn.HighEmphasis = true;
            this.liveViewBtn.Icon = null;
            this.liveViewBtn.Location = new System.Drawing.Point(18, 24);
            this.liveViewBtn.Margin = new System.Windows.Forms.Padding(4, 6, 4, 6);
            this.liveViewBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.liveViewBtn.Name = "liveViewBtn";
            this.liveViewBtn.NoAccentTextColor = System.Drawing.Color.Empty;
            this.liveViewBtn.Size = new System.Drawing.Size(396, 54);
            this.liveViewBtn.TabIndex = 57;
            this.liveViewBtn.Text = "Launch Live View Mode";
            this.liveViewBtn.Type = MaterialSkin.Controls.MaterialButton.MaterialButtonType.Contained;
            this.liveViewBtn.UseAccentColor = true;
            this.liveViewBtn.UseVisualStyleBackColor = true;
            // 
            // materialCard4
            // 
            this.materialCard4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard4.Controls.Add(this.refreshDisplayListBtn);
            this.materialCard4.Controls.Add(this.materialLabel2);
            this.materialCard4.Controls.Add(this.monitorSelect);
            this.materialCard4.Depth = 0;
            this.materialCard4.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard4.Location = new System.Drawing.Point(454, 9);
            this.materialCard4.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard4.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard4.Name = "materialCard4";
            this.materialCard4.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard4.Size = new System.Drawing.Size(433, 74);
            this.materialCard4.TabIndex = 71;
            // 
            // materialLabel2
            // 
            this.materialLabel2.AutoSize = true;
            this.materialLabel2.Depth = 0;
            this.materialLabel2.Font = new System.Drawing.Font("Roboto", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Pixel);
            this.materialLabel2.Location = new System.Drawing.Point(22, 31);
            this.materialLabel2.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialLabel2.Name = "materialLabel2";
            this.materialLabel2.Size = new System.Drawing.Size(107, 19);
            this.materialLabel2.TabIndex = 61;
            this.materialLabel2.Text = "Select Monitor:";
            // 
            // monitorSelect
            // 
            this.monitorSelect.AutoResize = false;
            this.monitorSelect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.monitorSelect.Depth = 0;
            this.monitorSelect.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.monitorSelect.DropDownHeight = 174;
            this.monitorSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.monitorSelect.DropDownWidth = 121;
            this.monitorSelect.Font = new System.Drawing.Font("Roboto Medium", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Pixel);
            this.monitorSelect.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.monitorSelect.FormattingEnabled = true;
            this.monitorSelect.IntegralHeight = false;
            this.monitorSelect.ItemHeight = 43;
            this.monitorSelect.Location = new System.Drawing.Point(146, 13);
            this.monitorSelect.MaxDropDownItems = 4;
            this.monitorSelect.MouseState = MaterialSkin.MouseState.OUT;
            this.monitorSelect.Name = "monitorSelect";
            this.monitorSelect.Size = new System.Drawing.Size(199, 49);
            this.monitorSelect.StartIndex = 0;
            this.monitorSelect.TabIndex = 60;
            // 
            // refreshDisplayListBtn
            // 
            this.refreshDisplayListBtn.Depth = 0;
            this.refreshDisplayListBtn.Icon = global::OSRTT_Launcher.Properties.Resources.arrow_rotate_right;
            this.refreshDisplayListBtn.Location = new System.Drawing.Point(362, 8);
            this.refreshDisplayListBtn.MouseState = MaterialSkin.MouseState.HOVER;
            this.refreshDisplayListBtn.Name = "refreshDisplayListBtn";
            this.refreshDisplayListBtn.Size = new System.Drawing.Size(56, 56);
            this.refreshDisplayListBtn.TabIndex = 69;
            this.refreshDisplayListBtn.UseVisualStyleBackColor = true;
            // 
            // materialCard5
            // 
            this.materialCard5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.materialCard5.Depth = 0;
            this.materialCard5.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(222)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
            this.materialCard5.Location = new System.Drawing.Point(899, 9);
            this.materialCard5.Margin = new System.Windows.Forms.Padding(14);
            this.materialCard5.MouseState = MaterialSkin.MouseState.HOVER;
            this.materialCard5.Name = "materialCard5";
            this.materialCard5.Padding = new System.Windows.Forms.Padding(14);
            this.materialCard5.Size = new System.Drawing.Size(98, 447);
            this.materialCard5.TabIndex = 72;
            // 
            // TestSettingsPane
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.materialCard5);
            this.Controls.Add(this.materialCard4);
            this.Controls.Add(this.materialCard3);
            this.Controls.Add(this.materialCard2);
            this.Controls.Add(this.materialCard1);
            this.Name = "TestSettingsPane";
            this.Size = new System.Drawing.Size(1022, 470);
            this.materialCard1.ResumeLayout(false);
            this.materialCard1.PerformLayout();
            this.materialCard2.ResumeLayout(false);
            this.materialCard2.PerformLayout();
            this.materialCard3.ResumeLayout(false);
            this.materialCard4.ResumeLayout(false);
            this.materialCard4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private MaterialSkin.Controls.MaterialButton startTestBtn;
        private MaterialSkin.Controls.MaterialComboBox framerateSelect;
        private MaterialSkin.Controls.MaterialLabel materialLabel1;
        private MaterialSkin.Controls.MaterialCard materialCard1;
        private MaterialSkin.Controls.MaterialSlider runSlider;
        private MaterialSkin.Controls.MaterialLabel materialLabel5;
        private MaterialSkin.Controls.MaterialComboBox windowSelect;
        private MaterialSkin.Controls.MaterialLabel materialLabel4;
        private MaterialSkin.Controls.MaterialComboBox vsyncSelect;
        private MaterialSkin.Controls.MaterialLabel materialLabel3;
        private MaterialSkin.Controls.MaterialComboBox captureSelect;
        private MaterialSkin.Controls.MaterialCard materialCard2;
        private MaterialSkin.Controls.MaterialSlider clickSlider;
        private MaterialSkin.Controls.MaterialButton startLatencyBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel8;
        private MaterialSkin.Controls.MaterialComboBox timeSelect;
        private MaterialSkin.Controls.MaterialCard materialCard3;
        private MaterialSkin.Controls.MaterialButton liveViewBtn;
        private MaterialSkin.Controls.MaterialCard materialCard4;
        private MaterialSkin.Controls.MaterialFloatingActionButton refreshDisplayListBtn;
        private MaterialSkin.Controls.MaterialLabel materialLabel2;
        private MaterialSkin.Controls.MaterialComboBox monitorSelect;
        private MaterialSkin.Controls.MaterialCard materialCard5;
    }
}
