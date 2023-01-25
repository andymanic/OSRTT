
namespace OSRTT_Launcher
{
    partial class OverdriveModes
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
            this.label6 = new System.Windows.Forms.Label();
            this.manufacturerSelect = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.odModeBox = new System.Windows.Forms.TextBox();
            this.continueBtn = new System.Windows.Forms.Button();
            this.extraInfoBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cancelButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Calibri", 32F, System.Drawing.FontStyle.Bold);
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(101, 10);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(315, 53);
            this.label6.TabIndex = 25;
            this.label6.Text = "Overdrive Mode";
            // 
            // manufacturerSelect
            // 
            this.manufacturerSelect.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.manufacturerSelect.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.manufacturerSelect.FormattingEnabled = true;
            this.manufacturerSelect.Location = new System.Drawing.Point(208, 81);
            this.manufacturerSelect.Name = "manufacturerSelect";
            this.manufacturerSelect.Size = new System.Drawing.Size(283, 33);
            this.manufacturerSelect.TabIndex = 24;
            this.manufacturerSelect.SelectedIndexChanged += new System.EventHandler(this.manufacturerSelect_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Arial", 18F);
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(15, 87);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(187, 27);
            this.label1.TabIndex = 27;
            this.label1.Text = "Overdrive Mode:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Arial", 18F);
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(15, 137);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(249, 27);
            this.label2.TabIndex = 28;
            this.label2.Text = "Enter mode manually:";
            // 
            // odModeBox
            // 
            this.odModeBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.odModeBox.Location = new System.Drawing.Point(270, 134);
            this.odModeBox.Name = "odModeBox";
            this.odModeBox.Size = new System.Drawing.Size(221, 31);
            this.odModeBox.TabIndex = 29;
            this.odModeBox.TextChanged += new System.EventHandler(this.odModeBox_TextChanged);
            // 
            // continueBtn
            // 
            this.continueBtn.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.continueBtn.FlatAppearance.BorderSize = 0;
            this.continueBtn.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.continueBtn.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.continueBtn.Location = new System.Drawing.Point(20, 231);
            this.continueBtn.Name = "continueBtn";
            this.continueBtn.Size = new System.Drawing.Size(344, 48);
            this.continueBtn.TabIndex = 30;
            this.continueBtn.Text = "Continue";
            this.continueBtn.UseVisualStyleBackColor = false;
            this.continueBtn.Click += new System.EventHandler(this.continueBtn_Click);
            // 
            // extraInfoBox
            // 
            this.extraInfoBox.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.extraInfoBox.Location = new System.Drawing.Point(142, 185);
            this.extraInfoBox.Name = "extraInfoBox";
            this.extraInfoBox.Size = new System.Drawing.Size(349, 31);
            this.extraInfoBox.TabIndex = 32;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Arial", 18F);
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(15, 188);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(121, 27);
            this.label3.TabIndex = 31;
            this.label3.Text = "Extra Info:";
            // 
            // cancelButton
            // 
            this.cancelButton.BackColor = System.Drawing.Color.DarkSalmon;
            this.cancelButton.FlatAppearance.BorderSize = 0;
            this.cancelButton.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
            this.cancelButton.Font = new System.Drawing.Font("Calibri", 24F, System.Drawing.FontStyle.Bold);
            this.cancelButton.Location = new System.Drawing.Point(370, 231);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(121, 48);
            this.cancelButton.TabIndex = 33;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseVisualStyleBackColor = false;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // OverdriveModes
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlDark;
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.extraInfoBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.continueBtn);
            this.Controls.Add(this.odModeBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.manufacturerSelect);
            this.Name = "OverdriveModes";
            this.Size = new System.Drawing.Size(511, 298);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox manufacturerSelect;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox odModeBox;
        private System.Windows.Forms.Button continueBtn;
        private System.Windows.Forms.TextBox extraInfoBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button cancelButton;
    }
}
