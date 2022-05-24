
namespace OSRTT_Launcher
{
    partial class LiveView
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
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.recordToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.clearDataToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.zoomToFitToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.saveAsPNGToolStripBtn = new System.Windows.Forms.ToolStripButton();
            this.graphViewPanel = new System.Windows.Forms.Panel();
            this.mainLabel = new System.Windows.Forms.Label();
            this.zoomToFitBtn = new System.Windows.Forms.Button();
            this.potValPanel = new System.Windows.Forms.Panel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.clearBtn = new System.Windows.Forms.Button();
            this.osPanel = new System.Windows.Forms.Panel();
            this.cleanVSpanBtn = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.osStartLbl = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.verticalSpanBtn = new System.Windows.Forms.Button();
            this.osEndLbl = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.osMeasureLbl = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.rtPanel = new System.Windows.Forms.Panel();
            this.clearHSpanBtn = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.startLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.addRemoveSpanBtn = new System.Windows.Forms.Button();
            this.endLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.rtLabel = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.saveGraphNoHSpanBtn = new System.Windows.Forms.Button();
            this.saveAsPNGBtn = new System.Windows.Forms.Button();
            this.startStopBtn = new System.Windows.Forms.Button();
            this.graphedData = new ScottPlot.FormsPlot();
            this.toolStrip1.SuspendLayout();
            this.graphViewPanel.SuspendLayout();
            this.potValPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            this.osPanel.SuspendLayout();
            this.rtPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordToolStripBtn,
            this.clearDataToolStripBtn,
            this.zoomToFitToolStripBtn,
            this.saveAsPNGToolStripBtn});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1646, 25);
            this.toolStrip1.TabIndex = 3;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // recordToolStripBtn
            // 
            this.recordToolStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.recordToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.recordToolStripBtn.Name = "recordToolStripBtn";
            this.recordToolStripBtn.Size = new System.Drawing.Size(75, 22);
            this.recordToolStripBtn.Text = "Record Data";
            this.recordToolStripBtn.Click += new System.EventHandler(this.recordToolStripBtn_Click);
            // 
            // clearDataToolStripBtn
            // 
            this.clearDataToolStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.clearDataToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.clearDataToolStripBtn.Name = "clearDataToolStripBtn";
            this.clearDataToolStripBtn.Size = new System.Drawing.Size(73, 22);
            this.clearDataToolStripBtn.Text = "Clear Graph";
            this.clearDataToolStripBtn.Click += new System.EventHandler(this.clearDataToolStripBtn_Click);
            // 
            // zoomToFitToolStripBtn
            // 
            this.zoomToFitToolStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.zoomToFitToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.zoomToFitToolStripBtn.Name = "zoomToFitToolStripBtn";
            this.zoomToFitToolStripBtn.Size = new System.Drawing.Size(73, 22);
            this.zoomToFitToolStripBtn.Text = "Zoom to Fit";
            this.zoomToFitToolStripBtn.Click += new System.EventHandler(this.zoomToFitToolStripBtn_Click);
            // 
            // saveAsPNGToolStripBtn
            // 
            this.saveAsPNGToolStripBtn.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.saveAsPNGToolStripBtn.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.saveAsPNGToolStripBtn.Name = "saveAsPNGToolStripBtn";
            this.saveAsPNGToolStripBtn.Size = new System.Drawing.Size(76, 22);
            this.saveAsPNGToolStripBtn.Text = "Save as PNG";
            this.saveAsPNGToolStripBtn.Click += new System.EventHandler(this.saveAsPNGToolStripBtn_Click);
            // 
            // graphViewPanel
            // 
            this.graphViewPanel.Controls.Add(this.mainLabel);
            this.graphViewPanel.Controls.Add(this.zoomToFitBtn);
            this.graphViewPanel.Controls.Add(this.potValPanel);
            this.graphViewPanel.Controls.Add(this.clearBtn);
            this.graphViewPanel.Controls.Add(this.osPanel);
            this.graphViewPanel.Controls.Add(this.rtPanel);
            this.graphViewPanel.Controls.Add(this.saveGraphNoHSpanBtn);
            this.graphViewPanel.Controls.Add(this.saveAsPNGBtn);
            this.graphViewPanel.Controls.Add(this.startStopBtn);
            this.graphViewPanel.Controls.Add(this.graphedData);
            this.graphViewPanel.Location = new System.Drawing.Point(3, 28);
            this.graphViewPanel.Name = "graphViewPanel";
            this.graphViewPanel.Size = new System.Drawing.Size(1641, 813);
            this.graphViewPanel.TabIndex = 4;
            // 
            // mainLabel
            // 
            this.mainLabel.BackColor = System.Drawing.Color.Transparent;
            this.mainLabel.Font = new System.Drawing.Font("Calibri", 40F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.mainLabel.ForeColor = System.Drawing.Color.White;
            this.mainLabel.Location = new System.Drawing.Point(55, 362);
            this.mainLabel.Name = "mainLabel";
            this.mainLabel.Size = new System.Drawing.Size(1260, 69);
            this.mainLabel.TabIndex = 36;
            this.mainLabel.Text = "Press RECORD or F10 to capture 3s of data";
            this.mainLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // zoomToFitBtn
            // 
            this.zoomToFitBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.zoomToFitBtn.FlatAppearance.BorderSize = 0;
            this.zoomToFitBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.zoomToFitBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.zoomToFitBtn.Location = new System.Drawing.Point(1326, 698);
            this.zoomToFitBtn.Name = "zoomToFitBtn";
            this.zoomToFitBtn.Size = new System.Drawing.Size(303, 37);
            this.zoomToFitBtn.TabIndex = 35;
            this.zoomToFitBtn.Text = "Zoom To Fit Data";
            this.zoomToFitBtn.UseVisualStyleBackColor = false;
            this.zoomToFitBtn.Click += new System.EventHandler(this.zoomToFitBtn_Click);
            // 
            // potValPanel
            // 
            this.potValPanel.BackColor = System.Drawing.Color.Transparent;
            this.potValPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.potValPanel.Controls.Add(this.label1);
            this.potValPanel.Controls.Add(this.numericUpDown1);
            this.potValPanel.Location = new System.Drawing.Point(1326, 563);
            this.potValPanel.Name = "potValPanel";
            this.potValPanel.Size = new System.Drawing.Size(303, 57);
            this.potValPanel.TabIndex = 34;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 24F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(16, 10);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(179, 37);
            this.label1.TabIndex = 30;
            this.label1.Text = "Sensitivity:";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.numericUpDown1.Font = new System.Drawing.Font("Consolas", 22F, System.Drawing.FontStyle.Bold);
            this.numericUpDown1.Location = new System.Drawing.Point(201, 9);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            15,
            0,
            0,
            0});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.ReadOnly = true;
            this.numericUpDown1.Size = new System.Drawing.Size(82, 38);
            this.numericUpDown1.TabIndex = 29;
            this.numericUpDown1.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.numericUpDown1.ValueChanged += new System.EventHandler(this.numericUpDown1_ValueChanged);
            // 
            // clearBtn
            // 
            this.clearBtn.BackColor = System.Drawing.Color.Orange;
            this.clearBtn.FlatAppearance.BorderSize = 0;
            this.clearBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.clearBtn.Font = new System.Drawing.Font("Consolas", 26F, System.Drawing.FontStyle.Bold);
            this.clearBtn.ForeColor = System.Drawing.Color.White;
            this.clearBtn.Location = new System.Drawing.Point(1498, 626);
            this.clearBtn.Name = "clearBtn";
            this.clearBtn.Size = new System.Drawing.Size(131, 66);
            this.clearBtn.TabIndex = 33;
            this.clearBtn.Text = "Clear";
            this.clearBtn.UseVisualStyleBackColor = false;
            this.clearBtn.Click += new System.EventHandler(this.clearBtn_Click);
            // 
            // osPanel
            // 
            this.osPanel.BackColor = System.Drawing.Color.Transparent;
            this.osPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.osPanel.Controls.Add(this.cleanVSpanBtn);
            this.osPanel.Controls.Add(this.label3);
            this.osPanel.Controls.Add(this.osStartLbl);
            this.osPanel.Controls.Add(this.label8);
            this.osPanel.Controls.Add(this.verticalSpanBtn);
            this.osPanel.Controls.Add(this.osEndLbl);
            this.osPanel.Controls.Add(this.label11);
            this.osPanel.Controls.Add(this.osMeasureLbl);
            this.osPanel.Controls.Add(this.label13);
            this.osPanel.Location = new System.Drawing.Point(1326, 308);
            this.osPanel.Name = "osPanel";
            this.osPanel.Size = new System.Drawing.Size(303, 250);
            this.osPanel.TabIndex = 32;
            // 
            // cleanVSpanBtn
            // 
            this.cleanVSpanBtn.BackColor = System.Drawing.Color.Orange;
            this.cleanVSpanBtn.FlatAppearance.BorderSize = 0;
            this.cleanVSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cleanVSpanBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.cleanVSpanBtn.ForeColor = System.Drawing.Color.White;
            this.cleanVSpanBtn.Location = new System.Drawing.Point(205, 200);
            this.cleanVSpanBtn.Name = "cleanVSpanBtn";
            this.cleanVSpanBtn.Size = new System.Drawing.Size(87, 34);
            this.cleanVSpanBtn.TabIndex = 34;
            this.cleanVSpanBtn.Text = "Clear";
            this.cleanVSpanBtn.UseVisualStyleBackColor = false;
            this.cleanVSpanBtn.Click += new System.EventHandler(this.cleanVSpanBtn_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(2, 10);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(298, 31);
            this.label3.TabIndex = 32;
            this.label3.Text = "Height Measurements";
            // 
            // osStartLbl
            // 
            this.osStartLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.osStartLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osStartLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.osStartLbl.Location = new System.Drawing.Point(16, 75);
            this.osStartLbl.Name = "osStartLbl";
            this.osStartLbl.Size = new System.Drawing.Size(123, 39);
            this.osStartLbl.TabIndex = 31;
            this.osStartLbl.Text = "0";
            this.osStartLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label8.Location = new System.Drawing.Point(43, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(70, 26);
            this.label8.TabIndex = 30;
            this.label8.Text = "Start:";
            // 
            // verticalSpanBtn
            // 
            this.verticalSpanBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.verticalSpanBtn.FlatAppearance.BorderSize = 0;
            this.verticalSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.verticalSpanBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.verticalSpanBtn.ForeColor = System.Drawing.Color.White;
            this.verticalSpanBtn.Location = new System.Drawing.Point(8, 200);
            this.verticalSpanBtn.Name = "verticalSpanBtn";
            this.verticalSpanBtn.Size = new System.Drawing.Size(190, 34);
            this.verticalSpanBtn.TabIndex = 29;
            this.verticalSpanBtn.Text = "Add Measuring Block";
            this.verticalSpanBtn.UseVisualStyleBackColor = false;
            this.verticalSpanBtn.Click += new System.EventHandler(this.verticalSpanBtn_Click);
            // 
            // osEndLbl
            // 
            this.osEndLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.osEndLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osEndLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.osEndLbl.Location = new System.Drawing.Point(164, 75);
            this.osEndLbl.Name = "osEndLbl";
            this.osEndLbl.Size = new System.Drawing.Size(123, 39);
            this.osEndLbl.TabIndex = 12;
            this.osEndLbl.Text = "0";
            this.osEndLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label11.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label11.Location = new System.Drawing.Point(194, 49);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(61, 26);
            this.label11.TabIndex = 11;
            this.label11.Text = "End:";
            // 
            // osMeasureLbl
            // 
            this.osMeasureLbl.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.osMeasureLbl.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.osMeasureLbl.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.osMeasureLbl.Location = new System.Drawing.Point(62, 142);
            this.osMeasureLbl.Name = "osMeasureLbl";
            this.osMeasureLbl.Size = new System.Drawing.Size(179, 50);
            this.osMeasureLbl.TabIndex = 10;
            this.osMeasureLbl.Text = "0%";
            this.osMeasureLbl.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label13.Location = new System.Drawing.Point(66, 116);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(163, 26);
            this.label13.TabIndex = 9;
            this.label13.Text = "Measurement:";
            // 
            // rtPanel
            // 
            this.rtPanel.BackColor = System.Drawing.Color.Transparent;
            this.rtPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.rtPanel.Controls.Add(this.clearHSpanBtn);
            this.rtPanel.Controls.Add(this.label5);
            this.rtPanel.Controls.Add(this.startLabel);
            this.rtPanel.Controls.Add(this.label10);
            this.rtPanel.Controls.Add(this.addRemoveSpanBtn);
            this.rtPanel.Controls.Add(this.endLabel);
            this.rtPanel.Controls.Add(this.label7);
            this.rtPanel.Controls.Add(this.rtLabel);
            this.rtPanel.Controls.Add(this.label2);
            this.rtPanel.Location = new System.Drawing.Point(1326, 31);
            this.rtPanel.Name = "rtPanel";
            this.rtPanel.Size = new System.Drawing.Size(303, 264);
            this.rtPanel.TabIndex = 31;
            // 
            // clearHSpanBtn
            // 
            this.clearHSpanBtn.BackColor = System.Drawing.Color.Orange;
            this.clearHSpanBtn.FlatAppearance.BorderSize = 0;
            this.clearHSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.clearHSpanBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.clearHSpanBtn.ForeColor = System.Drawing.Color.White;
            this.clearHSpanBtn.Location = new System.Drawing.Point(205, 217);
            this.clearHSpanBtn.Name = "clearHSpanBtn";
            this.clearHSpanBtn.Size = new System.Drawing.Size(87, 34);
            this.clearHSpanBtn.TabIndex = 33;
            this.clearHSpanBtn.Text = "Clear";
            this.clearHSpanBtn.UseVisualStyleBackColor = false;
            this.clearHSpanBtn.Click += new System.EventHandler(this.clearHSpanBtn_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(13, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(277, 31);
            this.label5.TabIndex = 32;
            this.label5.Text = "Time Measurements";
            // 
            // startLabel
            // 
            this.startLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.startLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.startLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.startLabel.Location = new System.Drawing.Point(15, 81);
            this.startLabel.Name = "startLabel";
            this.startLabel.Size = new System.Drawing.Size(123, 39);
            this.startLabel.TabIndex = 31;
            this.startLabel.Text = "0";
            this.startLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label10.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label10.Location = new System.Drawing.Point(42, 55);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(70, 26);
            this.label10.TabIndex = 30;
            this.label10.Text = "Start:";
            // 
            // addRemoveSpanBtn
            // 
            this.addRemoveSpanBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.addRemoveSpanBtn.FlatAppearance.BorderSize = 0;
            this.addRemoveSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.addRemoveSpanBtn.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold);
            this.addRemoveSpanBtn.ForeColor = System.Drawing.Color.White;
            this.addRemoveSpanBtn.Location = new System.Drawing.Point(8, 217);
            this.addRemoveSpanBtn.Name = "addRemoveSpanBtn";
            this.addRemoveSpanBtn.Size = new System.Drawing.Size(190, 34);
            this.addRemoveSpanBtn.TabIndex = 29;
            this.addRemoveSpanBtn.Text = "Add Measuring Block";
            this.addRemoveSpanBtn.UseVisualStyleBackColor = false;
            this.addRemoveSpanBtn.Click += new System.EventHandler(this.addRemoveSpanBtn_Click);
            // 
            // endLabel
            // 
            this.endLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.endLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.endLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.endLabel.Location = new System.Drawing.Point(163, 81);
            this.endLabel.Name = "endLabel";
            this.endLabel.Size = new System.Drawing.Size(123, 39);
            this.endLabel.TabIndex = 12;
            this.endLabel.Text = "0";
            this.endLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(193, 55);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 26);
            this.label7.TabIndex = 11;
            this.label7.Text = "End:";
            // 
            // rtLabel
            // 
            this.rtLabel.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.rtLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.rtLabel.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.rtLabel.Location = new System.Drawing.Point(62, 159);
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
            this.label2.Location = new System.Drawing.Point(66, 133);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(163, 26);
            this.label2.TabIndex = 9;
            this.label2.Text = "Measurement:";
            // 
            // saveGraphNoHSpanBtn
            // 
            this.saveGraphNoHSpanBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveGraphNoHSpanBtn.FlatAppearance.BorderSize = 0;
            this.saveGraphNoHSpanBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveGraphNoHSpanBtn.Font = new System.Drawing.Font("Consolas", 14F, System.Drawing.FontStyle.Bold);
            this.saveGraphNoHSpanBtn.Location = new System.Drawing.Point(1464, 741);
            this.saveGraphNoHSpanBtn.Name = "saveGraphNoHSpanBtn";
            this.saveGraphNoHSpanBtn.Size = new System.Drawing.Size(165, 67);
            this.saveGraphNoHSpanBtn.TabIndex = 25;
            this.saveGraphNoHSpanBtn.Text = "Save as PNG\r\nWithout Blocks";
            this.saveGraphNoHSpanBtn.UseVisualStyleBackColor = false;
            this.saveGraphNoHSpanBtn.Click += new System.EventHandler(this.saveGraphNoHSpanBtn_Click);
            // 
            // saveAsPNGBtn
            // 
            this.saveAsPNGBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.saveAsPNGBtn.FlatAppearance.BorderSize = 0;
            this.saveAsPNGBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.saveAsPNGBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.saveAsPNGBtn.Location = new System.Drawing.Point(1326, 741);
            this.saveAsPNGBtn.Name = "saveAsPNGBtn";
            this.saveAsPNGBtn.Size = new System.Drawing.Size(132, 67);
            this.saveAsPNGBtn.TabIndex = 24;
            this.saveAsPNGBtn.Text = "Save as PNG";
            this.saveAsPNGBtn.UseVisualStyleBackColor = false;
            this.saveAsPNGBtn.Click += new System.EventHandler(this.saveAsPNGBtn_Click);
            // 
            // startStopBtn
            // 
            this.startStopBtn.BackColor = System.Drawing.Color.LimeGreen;
            this.startStopBtn.FlatAppearance.BorderSize = 0;
            this.startStopBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.startStopBtn.Font = new System.Drawing.Font("Consolas", 26F, System.Drawing.FontStyle.Bold);
            this.startStopBtn.ForeColor = System.Drawing.Color.White;
            this.startStopBtn.Location = new System.Drawing.Point(1326, 626);
            this.startStopBtn.Name = "startStopBtn";
            this.startStopBtn.Size = new System.Drawing.Size(165, 66);
            this.startStopBtn.TabIndex = 8;
            this.startStopBtn.Text = "RECORD";
            this.startStopBtn.UseVisualStyleBackColor = false;
            this.startStopBtn.Click += new System.EventHandler(this.startStopBtn_Click);
            // 
            // graphedData
            // 
            this.graphedData.Location = new System.Drawing.Point(3, 3);
            this.graphedData.Name = "graphedData";
            this.graphedData.Size = new System.Drawing.Size(1334, 801);
            this.graphedData.TabIndex = 0;
            // 
            // LiveView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.ClientSize = new System.Drawing.Size(1646, 844);
            this.Controls.Add(this.graphViewPanel);
            this.Controls.Add(this.toolStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "LiveView";
            this.Text = "Live View";
            this.Load += new System.EventHandler(this.ResultsView_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.graphViewPanel.ResumeLayout(false);
            this.potValPanel.ResumeLayout(false);
            this.potValPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            this.osPanel.ResumeLayout(false);
            this.osPanel.PerformLayout();
            this.rtPanel.ResumeLayout(false);
            this.rtPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.Panel graphViewPanel;
        private ScottPlot.FormsPlot graphedData;
        private System.Windows.Forms.Button startStopBtn;
        private System.Windows.Forms.Label rtLabel;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button saveAsPNGBtn;
        private System.Windows.Forms.Button saveGraphNoHSpanBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Panel osPanel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label osStartLbl;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button verticalSpanBtn;
        private System.Windows.Forms.Label osEndLbl;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label osMeasureLbl;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Panel rtPanel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label startLabel;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button addRemoveSpanBtn;
        private System.Windows.Forms.Label endLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Button clearBtn;
        private System.Windows.Forms.Panel potValPanel;
        private System.Windows.Forms.Button clearHSpanBtn;
        private System.Windows.Forms.Button cleanVSpanBtn;
        private System.Windows.Forms.Button zoomToFitBtn;
        private System.Windows.Forms.Label mainLabel;
        private System.Windows.Forms.ToolStripButton recordToolStripBtn;
        private System.Windows.Forms.ToolStripButton clearDataToolStripBtn;
        private System.Windows.Forms.ToolStripButton zoomToFitToolStripBtn;
        private System.Windows.Forms.ToolStripButton saveAsPNGToolStripBtn;
    }
}