namespace TwinFinder.Sample
{
    partial class FormMatch
    {
        /// <summary>
        /// Erforderliche Designervariable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Verwendete Ressourcen bereinigen.
        /// </summary>
        /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Vom Windows Form-Designer generierter Code

        /// <summary>
        /// Erforderliche Methode für die Designerunterstützung.
        /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonCompare = new System.Windows.Forms.Button();
            this.txtSimiliarity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExplainPlan = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label3 = new System.Windows.Forms.Label();
            this.chkToogleMode = new System.Windows.Forms.CheckBox();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.txtMatchKeyResult = new System.Windows.Forms.TextBox();
            this.lblSoundex = new System.Windows.Forms.Label();
            this.btnGenerateMatchKey = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMatchKeySample = new System.Windows.Forms.TextBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonCompare
            // 
            this.buttonCompare.Location = new System.Drawing.Point(16, 271);
            this.buttonCompare.Name = "buttonCompare";
            this.buttonCompare.Size = new System.Drawing.Size(160, 31);
            this.buttonCompare.TabIndex = 0;
            this.buttonCompare.Text = "Compare";
            this.buttonCompare.UseVisualStyleBackColor = true;
            this.buttonCompare.Click += new System.EventHandler(this.buttonMatch_Click);
            // 
            // txtSimiliarity
            // 
            this.txtSimiliarity.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimiliarity.Location = new System.Drawing.Point(278, 275);
            this.txtSimiliarity.Name = "txtSimiliarity";
            this.txtSimiliarity.ReadOnly = true;
            this.txtSimiliarity.Size = new System.Drawing.Size(46, 22);
            this.txtSimiliarity.TabIndex = 19;
            this.txtSimiliarity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(182, 278);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Similarity (%)";
            // 
            // txtExplainPlan
            // 
            this.txtExplainPlan.AcceptsReturn = true;
            this.txtExplainPlan.Location = new System.Drawing.Point(16, 329);
            this.txtExplainPlan.Multiline = true;
            this.txtExplainPlan.Name = "txtExplainPlan";
            this.txtExplainPlan.ReadOnly = true;
            this.txtExplainPlan.Size = new System.Drawing.Size(621, 298);
            this.txtExplainPlan.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(35, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(667, 673);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.chkToogleMode);
            this.tabPage2.Controls.Add(this.propertyGrid2);
            this.tabPage2.Controls.Add(this.propertyGrid1);
            this.tabPage2.Controls.Add(this.txtExplainPlan);
            this.tabPage2.Controls.Add(this.label1);
            this.tabPage2.Controls.Add(this.buttonCompare);
            this.tabPage2.Controls.Add(this.txtSimiliarity);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(659, 644);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Compare";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(80, 17);
            this.label3.TabIndex = 24;
            this.label3.Text = "Explainplan";
            // 
            // chkToogleMode
            // 
            this.chkToogleMode.AutoSize = true;
            this.chkToogleMode.Location = new System.Drawing.Point(356, 275);
            this.chkToogleMode.Name = "chkToogleMode";
            this.chkToogleMode.Size = new System.Drawing.Size(212, 21);
            this.chkToogleMode.TabIndex = 23;
            this.chkToogleMode.Text = "Use classes with annotations";
            this.chkToogleMode.UseVisualStyleBackColor = true;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.HelpVisible = false;
            this.propertyGrid2.Location = new System.Drawing.Point(336, 18);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid2.Size = new System.Drawing.Size(301, 245);
            this.propertyGrid2.TabIndex = 1;
            this.propertyGrid2.ToolbarVisible = false;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(16, 18);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.PropertySort = System.Windows.Forms.PropertySort.Categorized;
            this.propertyGrid1.Size = new System.Drawing.Size(301, 245);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.txtMatchKeyResult);
            this.tabPage1.Controls.Add(this.lblSoundex);
            this.tabPage1.Controls.Add(this.btnGenerateMatchKey);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.txtMatchKeySample);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Size = new System.Drawing.Size(659, 644);
            this.tabPage1.TabIndex = 2;
            this.tabPage1.Text = "MatchKey";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // txtMatchKeyResult
            // 
            this.txtMatchKeyResult.Location = new System.Drawing.Point(18, 132);
            this.txtMatchKeyResult.Multiline = true;
            this.txtMatchKeyResult.Name = "txtMatchKeyResult";
            this.txtMatchKeyResult.ReadOnly = true;
            this.txtMatchKeyResult.Size = new System.Drawing.Size(618, 331);
            this.txtMatchKeyResult.TabIndex = 4;
            // 
            // lblSoundex
            // 
            this.lblSoundex.AutoSize = true;
            this.lblSoundex.Location = new System.Drawing.Point(15, 112);
            this.lblSoundex.Name = "lblSoundex";
            this.lblSoundex.Size = new System.Drawing.Size(48, 17);
            this.lblSoundex.TabIndex = 3;
            this.lblSoundex.Text = "Result";
            // 
            // btnGenerateMatchKey
            // 
            this.btnGenerateMatchKey.Location = new System.Drawing.Point(18, 68);
            this.btnGenerateMatchKey.Name = "btnGenerateMatchKey";
            this.btnGenerateMatchKey.Size = new System.Drawing.Size(159, 32);
            this.btnGenerateMatchKey.TabIndex = 2;
            this.btnGenerateMatchKey.Text = "Generate MatchKey";
            this.btnGenerateMatchKey.UseVisualStyleBackColor = true;
            this.btnGenerateMatchKey.Click += new System.EventHandler(this.btnGenerateMatchKey_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(35, 17);
            this.label2.TabIndex = 1;
            this.label2.Text = "Text";
            // 
            // txtMatchKeySample
            // 
            this.txtMatchKeySample.Location = new System.Drawing.Point(18, 40);
            this.txtMatchKeySample.Name = "txtMatchKeySample";
            this.txtMatchKeySample.Size = new System.Drawing.Size(618, 22);
            this.txtMatchKeySample.TabIndex = 0;
            // 
            // FormMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(741, 699);
            this.Controls.Add(this.tabControl1);
            this.Name = "FormMatch";
            this.Text = "Test";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonCompare;
        private System.Windows.Forms.TextBox txtSimiliarity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExplainPlan;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.CheckBox chkToogleMode;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMatchKeySample;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtMatchKeyResult;
        private System.Windows.Forms.Label lblSoundex;
        private System.Windows.Forms.Button btnGenerateMatchKey;
    }
}

