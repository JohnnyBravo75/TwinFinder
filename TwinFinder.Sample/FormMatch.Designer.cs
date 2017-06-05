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
            this.buttonMatch = new System.Windows.Forms.Button();
            this.txtSimiliarity = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtExplainPlan = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.propertyGrid1 = new System.Windows.Forms.PropertyGrid();
            this.propertyGrid2 = new System.Windows.Forms.PropertyGrid();
            this.chkToogleMode = new System.Windows.Forms.CheckBox();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonMatch
            // 
            this.buttonMatch.Location = new System.Drawing.Point(35, 323);
            this.buttonMatch.Name = "buttonMatch";
            this.buttonMatch.Size = new System.Drawing.Size(120, 31);
            this.buttonMatch.TabIndex = 0;
            this.buttonMatch.Text = "Match";
            this.buttonMatch.UseVisualStyleBackColor = true;
            this.buttonMatch.Click += new System.EventHandler(this.buttonMatch_Click);
            // 
            // txtSimiliarity
            // 
            this.txtSimiliarity.Font = new System.Drawing.Font("Microsoft Sans Serif", 7.8F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSimiliarity.Location = new System.Drawing.Point(297, 327);
            this.txtSimiliarity.Name = "txtSimiliarity";
            this.txtSimiliarity.ReadOnly = true;
            this.txtSimiliarity.Size = new System.Drawing.Size(46, 22);
            this.txtSimiliarity.TabIndex = 19;
            this.txtSimiliarity.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(201, 330);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(90, 17);
            this.label1.TabIndex = 20;
            this.label1.Text = "Similarity (%)";
            // 
            // txtExplainPlan
            // 
            this.txtExplainPlan.AcceptsReturn = true;
            this.txtExplainPlan.Location = new System.Drawing.Point(35, 360);
            this.txtExplainPlan.Multiline = true;
            this.txtExplainPlan.Name = "txtExplainPlan";
            this.txtExplainPlan.ReadOnly = true;
            this.txtExplainPlan.Size = new System.Drawing.Size(664, 319);
            this.txtExplainPlan.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(35, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(668, 311);
            this.tabControl1.TabIndex = 22;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.propertyGrid2);
            this.tabPage2.Controls.Add(this.propertyGrid1);
            this.tabPage2.Location = new System.Drawing.Point(4, 25);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(660, 282);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "tabPage2";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // propertyGrid1
            // 
            this.propertyGrid1.HelpVisible = false;
            this.propertyGrid1.Location = new System.Drawing.Point(16, 18);
            this.propertyGrid1.Name = "propertyGrid1";
            this.propertyGrid1.Size = new System.Drawing.Size(301, 245);
            this.propertyGrid1.TabIndex = 0;
            this.propertyGrid1.ToolbarVisible = false;
            // 
            // propertyGrid2
            // 
            this.propertyGrid2.HelpVisible = false;
            this.propertyGrid2.Location = new System.Drawing.Point(336, 18);
            this.propertyGrid2.Name = "propertyGrid2";
            this.propertyGrid2.Size = new System.Drawing.Size(301, 245);
            this.propertyGrid2.TabIndex = 1;
            this.propertyGrid2.ToolbarVisible = false;
            // 
            // chkToogleMode
            // 
            this.chkToogleMode.AutoSize = true;
            this.chkToogleMode.Location = new System.Drawing.Point(375, 327);
            this.chkToogleMode.Name = "chkToogleMode";
            this.chkToogleMode.Size = new System.Drawing.Size(143, 21);
            this.chkToogleMode.TabIndex = 23;
            this.chkToogleMode.Text = "Use Static objects";
            this.chkToogleMode.UseVisualStyleBackColor = true;
            // 
            // FormMatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(756, 691);
            this.Controls.Add(this.chkToogleMode);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.txtExplainPlan);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtSimiliarity);
            this.Controls.Add(this.buttonMatch);
            this.Name = "FormMatch";
            this.Text = "Form1";
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonMatch;
        private System.Windows.Forms.TextBox txtSimiliarity;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtExplainPlan;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.PropertyGrid propertyGrid2;
        private System.Windows.Forms.PropertyGrid propertyGrid1;
        private System.Windows.Forms.CheckBox chkToogleMode;
    }
}

