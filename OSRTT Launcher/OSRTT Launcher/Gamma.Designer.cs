
namespace OSRTT_Launcher
{
    partial class Gamma
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.standardResultsPanel = new System.Windows.Forms.Panel();
            this.gammaData = new ScottPlot.FormsPlot();
            this.gammaTable = new System.Windows.Forms.DataGridView();
            this.normaliseBtn = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.gammaSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.standardResultsPanel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaTable)).BeginInit();
            this.SuspendLayout();
            // 
            // standardResultsPanel
            // 
            this.standardResultsPanel.BackColor = System.Drawing.Color.Transparent;
            this.standardResultsPanel.Controls.Add(this.label1);
            this.standardResultsPanel.Controls.Add(this.label7);
            this.standardResultsPanel.Controls.Add(this.gammaSelect);
            this.standardResultsPanel.Controls.Add(this.gammaData);
            this.standardResultsPanel.Controls.Add(this.normaliseBtn);
            this.standardResultsPanel.Controls.Add(this.gammaTable);
            this.standardResultsPanel.Location = new System.Drawing.Point(0, 0);
            this.standardResultsPanel.Name = "standardResultsPanel";
            this.standardResultsPanel.Size = new System.Drawing.Size(1278, 994);
            this.standardResultsPanel.TabIndex = 3;
            // 
            // gammaData
            // 
            this.gammaData.Location = new System.Drawing.Point(3, 7);
            this.gammaData.Name = "gammaData";
            this.gammaData.Size = new System.Drawing.Size(944, 944);
            this.gammaData.TabIndex = 1;
            // 
            // gammaTable
            // 
            this.gammaTable.AllowUserToAddRows = false;
            this.gammaTable.AllowUserToDeleteRows = false;
            this.gammaTable.AllowUserToResizeColumns = false;
            this.gammaTable.AllowUserToResizeRows = false;
            this.gammaTable.BackgroundColor = System.Drawing.SystemColors.WindowFrame;
            this.gammaTable.ColumnHeadersHeight = 40;
            this.gammaTable.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.gammaTable.ColumnHeadersVisible = false;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Calibri", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.gammaTable.DefaultCellStyle = dataGridViewCellStyle1;
            this.gammaTable.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.gammaTable.EnableHeadersVisualStyles = false;
            this.gammaTable.GridColor = System.Drawing.Color.White;
            this.gammaTable.Location = new System.Drawing.Point(953, 37);
            this.gammaTable.MultiSelect = false;
            this.gammaTable.Name = "gammaTable";
            this.gammaTable.ReadOnly = true;
            this.gammaTable.RowHeadersVisible = false;
            this.gammaTable.RowHeadersWidth = 65;
            this.gammaTable.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.gammaTable.RowTemplate.Height = 35;
            this.gammaTable.RowTemplate.ReadOnly = true;
            this.gammaTable.RowTemplate.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.gammaTable.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.gammaTable.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.gammaTable.ShowCellToolTips = false;
            this.gammaTable.ShowEditingIcon = false;
            this.gammaTable.Size = new System.Drawing.Size(251, 601);
            this.gammaTable.TabIndex = 31;
            // 
            // normaliseBtn
            // 
            this.normaliseBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.normaliseBtn.FlatAppearance.BorderSize = 0;
            this.normaliseBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.normaliseBtn.Font = new System.Drawing.Font("Consolas", 16F, System.Drawing.FontStyle.Bold);
            this.normaliseBtn.Location = new System.Drawing.Point(953, 756);
            this.normaliseBtn.Name = "normaliseBtn";
            this.normaliseBtn.Size = new System.Drawing.Size(251, 36);
            this.normaliseBtn.TabIndex = 32;
            this.normaliseBtn.Text = "Normalise Gamma";
            this.normaliseBtn.UseVisualStyleBackColor = false;
            this.normaliseBtn.Click += new System.EventHandler(this.normaliseBtn_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(981, 808);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(197, 26);
            this.label7.TabIndex = 35;
            this.label7.Text = "Add a Reference:";
            // 
            // gammaSelect
            // 
            this.gammaSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.gammaSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gammaSelect.FormattingEnabled = true;
            this.gammaSelect.Location = new System.Drawing.Point(1082, 837);
            this.gammaSelect.Name = "gammaSelect";
            this.gammaSelect.Size = new System.Drawing.Size(102, 33);
            this.gammaSelect.TabIndex = 34;
            this.gammaSelect.SelectedIndexChanged += new System.EventHandler(this.gammaSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(976, 840);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(103, 26);
            this.label1.TabIndex = 36;
            this.label1.Text = "Gamma:";
            // 
            // Gamma
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.standardResultsPanel);
            this.Name = "Gamma";
            this.Size = new System.Drawing.Size(1286, 997);
            this.Load += new System.EventHandler(this.Gamma_Load);
            this.standardResultsPanel.ResumeLayout(false);
            this.standardResultsPanel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gammaTable)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel standardResultsPanel;
        private ScottPlot.FormsPlot gammaData;
        private System.Windows.Forms.DataGridView gammaTable;
        private System.Windows.Forms.Button normaliseBtn;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox gammaSelect;
    }
}
