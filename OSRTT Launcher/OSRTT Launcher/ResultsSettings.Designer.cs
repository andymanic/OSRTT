
namespace OSRTT_Launcher
{
    partial class ResultsSettings
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
            this.panel2 = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.toleranceStyleSelect = new System.Windows.Forms.ComboBox();
            this.tolerancePanel = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.testSettingsPanel = new System.Windows.Forms.Panel();
            this.label6 = new System.Windows.Forms.Label();
            this.settingsPresetSelect = new System.Windows.Forms.ComboBox();
            this.overshootStylePanel = new System.Windows.Forms.Panel();
            this.label10 = new System.Windows.Forms.Label();
            this.osPercentSelect = new System.Windows.Forms.ComboBox();
            this.overshootSourcePanel = new System.Windows.Forms.Panel();
            this.label9 = new System.Windows.Forms.Label();
            this.osGammaSelect = new System.Windows.Forms.ComboBox();
            this.toleranceLevelPanel = new System.Windows.Forms.Panel();
            this.Per10Btn = new OSRTT_Launcher.RoundButton();
            this.Per3Btn = new OSRTT_Launcher.RoundButton();
            this.RGB10Btn = new OSRTT_Launcher.RoundButton();
            this.RGB5Btn = new OSRTT_Launcher.RoundButton();
            this.label5 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.label8 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.gammaPanel = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.saveGammaTableSelect = new System.Windows.Forms.ComboBox();
            this.errorsPanel = new System.Windows.Forms.Panel();
            this.label12 = new System.Windows.Forms.Label();
            this.ignoreErrorsSelect = new System.Windows.Forms.ComboBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel10 = new System.Windows.Forms.Panel();
            this.label14 = new System.Windows.Forms.Label();
            this.suppressMessageBoxPanel = new System.Windows.Forms.Panel();
            this.label15 = new System.Windows.Forms.Label();
            this.suppressMsgBoxSelect = new System.Windows.Forms.ComboBox();
            this.saveToExcelPanel = new System.Windows.Forms.Panel();
            this.label16 = new System.Windows.Forms.Label();
            this.saveToExcelSelect = new System.Windows.Forms.ComboBox();
            this.saveLabel = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label17 = new System.Windows.Forms.Label();
            this.shareDataSelect = new System.Windows.Forms.ComboBox();
            this.panel2.SuspendLayout();
            this.tolerancePanel.SuspendLayout();
            this.testSettingsPanel.SuspendLayout();
            this.overshootStylePanel.SuspendLayout();
            this.overshootSourcePanel.SuspendLayout();
            this.toleranceLevelPanel.SuspendLayout();
            this.panel1.SuspendLayout();
            this.gammaPanel.SuspendLayout();
            this.errorsPanel.SuspendLayout();
            this.panel10.SuspendLayout();
            this.suppressMessageBoxPanel.SuspendLayout();
            this.saveToExcelPanel.SuspendLayout();
            this.panel3.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.Khaki;
            this.panel2.Controls.Add(this.label1);
            this.panel2.Location = new System.Drawing.Point(12, 340);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(356, 54);
            this.panel2.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.Black;
            this.label1.Location = new System.Drawing.Point(4, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(337, 32);
            this.label1.TabIndex = 11;
            this.label1.Text = "Response Time Settings";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(16, 403);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(348, 130);
            this.label3.TabIndex = 11;
            this.label3.Text = "Adjust how you would like data\r\nfrom the Response Time Test to\r\nbe processed. Gam" +
    "ma Correction\r\nis required for measurements to\r\nuse RGB values as their toleranc" +
    "e.";
            // 
            // toleranceStyleSelect
            // 
            this.toleranceStyleSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.toleranceStyleSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.toleranceStyleSelect.FormattingEnabled = true;
            this.toleranceStyleSelect.Location = new System.Drawing.Point(198, 8);
            this.toleranceStyleSelect.Name = "toleranceStyleSelect";
            this.toleranceStyleSelect.Size = new System.Drawing.Size(402, 33);
            this.toleranceStyleSelect.TabIndex = 21;
            this.toleranceStyleSelect.SelectedIndexChanged += new System.EventHandler(this.toleranceStyleSelect_SelectedIndexChanged);
            // 
            // tolerancePanel
            // 
            this.tolerancePanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.tolerancePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.tolerancePanel.Controls.Add(this.label4);
            this.tolerancePanel.Controls.Add(this.toleranceStyleSelect);
            this.tolerancePanel.Location = new System.Drawing.Point(386, 340);
            this.tolerancePanel.Name = "tolerancePanel";
            this.tolerancePanel.Size = new System.Drawing.Size(615, 50);
            this.tolerancePanel.TabIndex = 22;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Arial", 18F);
            this.label4.Location = new System.Drawing.Point(9, 11);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(183, 27);
            this.label4.TabIndex = 22;
            this.label4.Text = "Tolerance Style:";
            // 
            // testSettingsPanel
            // 
            this.testSettingsPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.testSettingsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.testSettingsPanel.Controls.Add(this.label6);
            this.testSettingsPanel.Controls.Add(this.settingsPresetSelect);
            this.testSettingsPanel.Location = new System.Drawing.Point(386, 57);
            this.testSettingsPanel.Name = "testSettingsPanel";
            this.testSettingsPanel.Size = new System.Drawing.Size(615, 54);
            this.testSettingsPanel.TabIndex = 23;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Arial", 18F);
            this.label6.Location = new System.Drawing.Point(10, 12);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(233, 27);
            this.label6.TabIndex = 23;
            this.label6.Text = "Test Settings Preset:";
            // 
            // settingsPresetSelect
            // 
            this.settingsPresetSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.settingsPresetSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.settingsPresetSelect.FormattingEnabled = true;
            this.settingsPresetSelect.Location = new System.Drawing.Point(249, 9);
            this.settingsPresetSelect.Name = "settingsPresetSelect";
            this.settingsPresetSelect.Size = new System.Drawing.Size(351, 33);
            this.settingsPresetSelect.TabIndex = 21;
            this.settingsPresetSelect.SelectedIndexChanged += new System.EventHandler(this.settingsPresetSelect_SelectedIndexChanged);
            // 
            // overshootStylePanel
            // 
            this.overshootStylePanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.overshootStylePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overshootStylePanel.Controls.Add(this.label10);
            this.overshootStylePanel.Controls.Add(this.osPercentSelect);
            this.overshootStylePanel.Location = new System.Drawing.Point(386, 616);
            this.overshootStylePanel.Name = "overshootStylePanel";
            this.overshootStylePanel.Size = new System.Drawing.Size(615, 50);
            this.overshootStylePanel.TabIndex = 24;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Arial", 18F);
            this.label10.Location = new System.Drawing.Point(10, 11);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(190, 27);
            this.label10.TabIndex = 24;
            this.label10.Text = "Overshoot Style:";
            // 
            // osPercentSelect
            // 
            this.osPercentSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.osPercentSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osPercentSelect.FormattingEnabled = true;
            this.osPercentSelect.Location = new System.Drawing.Point(206, 8);
            this.osPercentSelect.Name = "osPercentSelect";
            this.osPercentSelect.Size = new System.Drawing.Size(394, 32);
            this.osPercentSelect.TabIndex = 21;
            this.osPercentSelect.SelectedIndexChanged += new System.EventHandler(this.osPercentSelect_SelectedIndexChanged);
            // 
            // overshootSourcePanel
            // 
            this.overshootSourcePanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.overshootSourcePanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.overshootSourcePanel.Controls.Add(this.label9);
            this.overshootSourcePanel.Controls.Add(this.osGammaSelect);
            this.overshootSourcePanel.Location = new System.Drawing.Point(386, 567);
            this.overshootSourcePanel.Name = "overshootSourcePanel";
            this.overshootSourcePanel.Size = new System.Drawing.Size(615, 50);
            this.overshootSourcePanel.TabIndex = 25;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Arial", 18F);
            this.label9.Location = new System.Drawing.Point(10, 11);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(212, 27);
            this.label9.TabIndex = 23;
            this.label9.Text = "Overshoot Source:";
            // 
            // osGammaSelect
            // 
            this.osGammaSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.osGammaSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 14F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osGammaSelect.FormattingEnabled = true;
            this.osGammaSelect.Location = new System.Drawing.Point(228, 8);
            this.osGammaSelect.Name = "osGammaSelect";
            this.osGammaSelect.Size = new System.Drawing.Size(372, 32);
            this.osGammaSelect.TabIndex = 21;
            this.osGammaSelect.SelectedIndexChanged += new System.EventHandler(this.osGammaSelect_SelectedIndexChanged);
            // 
            // toleranceLevelPanel
            // 
            this.toleranceLevelPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.toleranceLevelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.toleranceLevelPanel.Controls.Add(this.Per10Btn);
            this.toleranceLevelPanel.Controls.Add(this.Per3Btn);
            this.toleranceLevelPanel.Controls.Add(this.RGB10Btn);
            this.toleranceLevelPanel.Controls.Add(this.RGB5Btn);
            this.toleranceLevelPanel.Controls.Add(this.label5);
            this.toleranceLevelPanel.Location = new System.Drawing.Point(386, 389);
            this.toleranceLevelPanel.Name = "toleranceLevelPanel";
            this.toleranceLevelPanel.Size = new System.Drawing.Size(615, 100);
            this.toleranceLevelPanel.TabIndex = 25;
            // 
            // Per10Btn
            // 
            this.Per10Btn.BackColor = System.Drawing.Color.SteelBlue;
            this.Per10Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Per10Btn.ForeColor = System.Drawing.Color.White;
            this.Per10Btn.Location = new System.Drawing.Point(465, 43);
            this.Per10Btn.Name = "Per10Btn";
            this.Per10Btn.Size = new System.Drawing.Size(135, 45);
            this.Per10Btn.TabIndex = 34;
            this.Per10Btn.Text = "10% Light Level";
            this.Per10Btn.UseVisualStyleBackColor = false;
            this.Per10Btn.Click += new System.EventHandler(this.Per10Btn_Click);
            // 
            // Per3Btn
            // 
            this.Per3Btn.BackColor = System.Drawing.Color.SteelBlue;
            this.Per3Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Per3Btn.ForeColor = System.Drawing.Color.White;
            this.Per3Btn.Location = new System.Drawing.Point(315, 43);
            this.Per3Btn.Name = "Per3Btn";
            this.Per3Btn.Size = new System.Drawing.Size(135, 45);
            this.Per3Btn.TabIndex = 33;
            this.Per3Btn.Text = "3% RGB Value";
            this.Per3Btn.UseVisualStyleBackColor = false;
            this.Per3Btn.Click += new System.EventHandler(this.Per3Btn_Click);
            // 
            // RGB10Btn
            // 
            this.RGB10Btn.BackColor = System.Drawing.Color.SteelBlue;
            this.RGB10Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RGB10Btn.ForeColor = System.Drawing.Color.White;
            this.RGB10Btn.Location = new System.Drawing.Point(165, 43);
            this.RGB10Btn.Name = "RGB10Btn";
            this.RGB10Btn.Size = new System.Drawing.Size(135, 45);
            this.RGB10Btn.TabIndex = 32;
            this.RGB10Btn.Text = "Fixed RGB 10";
            this.RGB10Btn.UseVisualStyleBackColor = false;
            this.RGB10Btn.Click += new System.EventHandler(this.RGB10Btn_Click);
            // 
            // RGB5Btn
            // 
            this.RGB5Btn.BackColor = System.Drawing.Color.LimeGreen;
            this.RGB5Btn.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.RGB5Btn.ForeColor = System.Drawing.Color.White;
            this.RGB5Btn.Location = new System.Drawing.Point(15, 43);
            this.RGB5Btn.Name = "RGB5Btn";
            this.RGB5Btn.Size = new System.Drawing.Size(135, 45);
            this.RGB5Btn.TabIndex = 31;
            this.RGB5Btn.Text = "Fixed RGB 5";
            this.RGB5Btn.UseVisualStyleBackColor = false;
            this.RGB5Btn.Click += new System.EventHandler(this.RGB5Btn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Arial", 18F);
            this.label5.Location = new System.Drawing.Point(9, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(185, 27);
            this.label5.TabIndex = 23;
            this.label5.Text = "Tolerance Level:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(16, 630);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(335, 130);
            this.label7.TabIndex = 13;
            this.label7.Text = "Adjust how you would like the\r\nOvershoot data to be calculated\r\nand reported. Gam" +
    "ma corrected\r\nmeans using RGB values instead\r\nof raw light level.";
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Khaki;
            this.panel1.Controls.Add(this.label8);
            this.panel1.Location = new System.Drawing.Point(12, 567);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(356, 54);
            this.panel1.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.Color.Black;
            this.label8.Location = new System.Drawing.Point(4, 10);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(268, 32);
            this.label8.TabIndex = 11;
            this.label8.Text = "Overshoot Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(12, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(121, 31);
            this.label2.TabIndex = 10;
            this.label2.Text = "Settings";
            // 
            // gammaPanel
            // 
            this.gammaPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.gammaPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.gammaPanel.Controls.Add(this.label11);
            this.gammaPanel.Controls.Add(this.saveGammaTableSelect);
            this.gammaPanel.Location = new System.Drawing.Point(386, 488);
            this.gammaPanel.Name = "gammaPanel";
            this.gammaPanel.Size = new System.Drawing.Size(615, 50);
            this.gammaPanel.TabIndex = 26;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Arial", 18F);
            this.label11.Location = new System.Drawing.Point(9, 10);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(228, 27);
            this.label11.TabIndex = 22;
            this.label11.Text = "Save Gamma Table:";
            // 
            // saveGammaTableSelect
            // 
            this.saveGammaTableSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.saveGammaTableSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveGammaTableSelect.FormattingEnabled = true;
            this.saveGammaTableSelect.Location = new System.Drawing.Point(249, 7);
            this.saveGammaTableSelect.Name = "saveGammaTableSelect";
            this.saveGammaTableSelect.Size = new System.Drawing.Size(351, 33);
            this.saveGammaTableSelect.TabIndex = 21;
            this.saveGammaTableSelect.SelectedIndexChanged += new System.EventHandler(this.saveGammaTableSelect_SelectedIndexChanged);
            // 
            // errorsPanel
            // 
            this.errorsPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.errorsPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.errorsPanel.Controls.Add(this.label12);
            this.errorsPanel.Controls.Add(this.ignoreErrorsSelect);
            this.errorsPanel.Location = new System.Drawing.Point(386, 171);
            this.errorsPanel.Name = "errorsPanel";
            this.errorsPanel.Size = new System.Drawing.Size(615, 50);
            this.errorsPanel.TabIndex = 27;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Font = new System.Drawing.Font("Arial", 18F);
            this.label12.Location = new System.Drawing.Point(9, 11);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(256, 27);
            this.label12.TabIndex = 22;
            this.label12.Text = "Ignore Mid Run Errors:";
            // 
            // ignoreErrorsSelect
            // 
            this.ignoreErrorsSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ignoreErrorsSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ignoreErrorsSelect.FormattingEnabled = true;
            this.ignoreErrorsSelect.Location = new System.Drawing.Point(271, 8);
            this.ignoreErrorsSelect.Name = "ignoreErrorsSelect";
            this.ignoreErrorsSelect.Size = new System.Drawing.Size(329, 33);
            this.ignoreErrorsSelect.TabIndex = 21;
            this.ignoreErrorsSelect.SelectedIndexChanged += new System.EventHandler(this.ignoreErrorsSelect_SelectedIndexChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 13F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(14, 114);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(364, 88);
            this.label13.TabIndex = 29;
            this.label13.Text = "Recommended Settings include a fixed \r\nRGB 5 tolerance and RGB value overshoot. \r" +
    "\nRunning into errors? Try enabling the\r\nignore mid run errors setting!";
            // 
            // panel10
            // 
            this.panel10.BackColor = System.Drawing.Color.Khaki;
            this.panel10.Controls.Add(this.label14);
            this.panel10.Location = new System.Drawing.Point(12, 57);
            this.panel10.Name = "panel10";
            this.panel10.Size = new System.Drawing.Size(356, 54);
            this.panel10.TabIndex = 28;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Font = new System.Drawing.Font("Arial", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label14.ForeColor = System.Drawing.Color.Black;
            this.label14.Location = new System.Drawing.Point(4, 11);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(186, 32);
            this.label14.TabIndex = 11;
            this.label14.Text = "Test Settings";
            // 
            // suppressMessageBoxPanel
            // 
            this.suppressMessageBoxPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.suppressMessageBoxPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.suppressMessageBoxPanel.Controls.Add(this.label15);
            this.suppressMessageBoxPanel.Controls.Add(this.suppressMsgBoxSelect);
            this.suppressMessageBoxPanel.Location = new System.Drawing.Point(386, 220);
            this.suppressMessageBoxPanel.Name = "suppressMessageBoxPanel";
            this.suppressMessageBoxPanel.Size = new System.Drawing.Size(615, 50);
            this.suppressMessageBoxPanel.TabIndex = 30;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Font = new System.Drawing.Font("Arial", 18F);
            this.label15.Location = new System.Drawing.Point(10, 10);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(297, 27);
            this.label15.TabIndex = 22;
            this.label15.Text = "Suppress Message Boxes:";
            // 
            // suppressMsgBoxSelect
            // 
            this.suppressMsgBoxSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.suppressMsgBoxSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.suppressMsgBoxSelect.FormattingEnabled = true;
            this.suppressMsgBoxSelect.Location = new System.Drawing.Point(312, 7);
            this.suppressMsgBoxSelect.Name = "suppressMsgBoxSelect";
            this.suppressMsgBoxSelect.Size = new System.Drawing.Size(288, 33);
            this.suppressMsgBoxSelect.TabIndex = 21;
            this.suppressMsgBoxSelect.SelectedIndexChanged += new System.EventHandler(this.suppressMsgBoxSelect_SelectedIndexChanged);
            // 
            // saveToExcelPanel
            // 
            this.saveToExcelPanel.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.saveToExcelPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.saveToExcelPanel.Controls.Add(this.label16);
            this.saveToExcelPanel.Controls.Add(this.saveToExcelSelect);
            this.saveToExcelPanel.Location = new System.Drawing.Point(386, 269);
            this.saveToExcelPanel.Name = "saveToExcelPanel";
            this.saveToExcelPanel.Size = new System.Drawing.Size(615, 50);
            this.saveToExcelPanel.TabIndex = 31;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Font = new System.Drawing.Font("Arial", 18F);
            this.label16.Location = new System.Drawing.Point(9, 10);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(280, 27);
            this.label16.TabIndex = 22;
            this.label16.Text = "Save to Excel Heatmaps:";
            // 
            // saveToExcelSelect
            // 
            this.saveToExcelSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.saveToExcelSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveToExcelSelect.FormattingEnabled = true;
            this.saveToExcelSelect.Location = new System.Drawing.Point(293, 7);
            this.saveToExcelSelect.Name = "saveToExcelSelect";
            this.saveToExcelSelect.Size = new System.Drawing.Size(307, 33);
            this.saveToExcelSelect.TabIndex = 21;
            this.saveToExcelSelect.SelectedIndexChanged += new System.EventHandler(this.saveToExcelSelect_SelectedIndexChanged);
            // 
            // saveLabel
            // 
            this.saveLabel.AutoSize = true;
            this.saveLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.saveLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.saveLabel.Location = new System.Drawing.Point(805, 14);
            this.saveLabel.Name = "saveLabel";
            this.saveLabel.Size = new System.Drawing.Size(196, 29);
            this.saveLabel.TabIndex = 32;
            this.saveLabel.Text = "Changes Saved";
            // 
            // panel3
            // 
            this.panel3.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label17);
            this.panel3.Controls.Add(this.shareDataSelect);
            this.panel3.Location = new System.Drawing.Point(386, 122);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(615, 50);
            this.panel3.TabIndex = 33;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Font = new System.Drawing.Font("Arial", 18F);
            this.label17.Location = new System.Drawing.Point(9, 11);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(270, 27);
            this.label17.TabIndex = 22;
            this.label17.Text = "Share Raw Result Data:";
            // 
            // shareDataSelect
            // 
            this.shareDataSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.shareDataSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.shareDataSelect.FormattingEnabled = true;
            this.shareDataSelect.Location = new System.Drawing.Point(285, 8);
            this.shareDataSelect.Name = "shareDataSelect";
            this.shareDataSelect.Size = new System.Drawing.Size(315, 33);
            this.shareDataSelect.TabIndex = 21;
            this.shareDataSelect.SelectedIndexChanged += new System.EventHandler(this.shareDataSelect_SelectedIndexChanged);
            // 
            // ResultsSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1018, 775);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.saveLabel);
            this.Controls.Add(this.saveToExcelPanel);
            this.Controls.Add(this.suppressMessageBoxPanel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.panel10);
            this.Controls.Add(this.errorsPanel);
            this.Controls.Add(this.gammaPanel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.toleranceLevelPanel);
            this.Controls.Add(this.tolerancePanel);
            this.Controls.Add(this.testSettingsPanel);
            this.Controls.Add(this.overshootSourcePanel);
            this.Controls.Add(this.overshootStylePanel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.label2);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.Name = "ResultsSettings";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Test & Results Settings";
            this.Load += new System.EventHandler(this.ResultsSettings_Load);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.tolerancePanel.ResumeLayout(false);
            this.tolerancePanel.PerformLayout();
            this.testSettingsPanel.ResumeLayout(false);
            this.testSettingsPanel.PerformLayout();
            this.overshootStylePanel.ResumeLayout(false);
            this.overshootStylePanel.PerformLayout();
            this.overshootSourcePanel.ResumeLayout(false);
            this.overshootSourcePanel.PerformLayout();
            this.toleranceLevelPanel.ResumeLayout(false);
            this.toleranceLevelPanel.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.gammaPanel.ResumeLayout(false);
            this.gammaPanel.PerformLayout();
            this.errorsPanel.ResumeLayout(false);
            this.errorsPanel.PerformLayout();
            this.panel10.ResumeLayout(false);
            this.panel10.PerformLayout();
            this.suppressMessageBoxPanel.ResumeLayout(false);
            this.suppressMessageBoxPanel.PerformLayout();
            this.saveToExcelPanel.ResumeLayout(false);
            this.saveToExcelPanel.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox toleranceStyleSelect;
        private System.Windows.Forms.Panel tolerancePanel;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel testSettingsPanel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox settingsPresetSelect;
        private System.Windows.Forms.Panel overshootStylePanel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox osPercentSelect;
        private System.Windows.Forms.Panel overshootSourcePanel;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox osGammaSelect;
        private System.Windows.Forms.Panel toleranceLevelPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Panel gammaPanel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.ComboBox saveGammaTableSelect;
        private System.Windows.Forms.Panel errorsPanel;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox ignoreErrorsSelect;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel panel10;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Panel suppressMessageBoxPanel;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.ComboBox suppressMsgBoxSelect;
        private RoundButton RGB5Btn;
        private RoundButton Per10Btn;
        private RoundButton Per3Btn;
        private RoundButton RGB10Btn;
        private System.Windows.Forms.Panel saveToExcelPanel;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ComboBox saveToExcelSelect;
        private System.Windows.Forms.Label saveLabel;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.ComboBox shareDataSelect;
    }
}