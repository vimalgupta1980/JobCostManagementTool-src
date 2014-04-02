namespace Syscon.JobCostManagementTool
{
    partial class JobCostManagement
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(JobCostManagement));
            this.txtDataDir = new System.Windows.Forms.TextBox();
            this.btnUpdateJobCost = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.optionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.selectTMJobTypesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.onlineHelpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.activateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.demoLabel = new System.Windows.Forms.Label();
            this.radioShowTMJobs = new System.Windows.Forms.RadioButton();
            this.radioShowAllJobs = new System.Windows.Forms.RadioButton();
            this.dteStartDate = new System.Windows.Forms.DateTimePicker();
            this.label1 = new System.Windows.Forms.Label();
            this.chkUnbilled = new System.Windows.Forms.CheckBox();
            this.btnSMBDir = new System.Windows.Forms.Button();
            this.cmbStartingPeriod = new SysconCommon.GUI.SearchableComboBox();
            this.cmbEndPeriod = new SysconCommon.GUI.SearchableComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.chkSumCustomer = new SysconCommon.GUI.SysconCheckBox();
            this.chkSumPeriod = new SysconCommon.GUI.SysconCheckBox();
            this.chkSumJob = new SysconCommon.GUI.SysconCheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.cboTaxPartClass = new SysconCommon.GUI.SearchableComboBox();
            this.grpBoxSelOptions = new System.Windows.Forms.GroupBox();
            this.radCombineForBilling = new System.Windows.Forms.RadioButton();
            this.radScanJobForTax = new System.Windows.Forms.RadioButton();
            this.cboPhaseNum = new SysconCommon.GUI.SearchableComboBox();
            this.lblPhase = new System.Windows.Forms.Label();
            this.lblEndDate = new System.Windows.Forms.Label();
            this.dteEndDate = new System.Windows.Forms.DateTimePicker();
            this.menuStrip1.SuspendLayout();
            this.grpBoxSelOptions.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtDataDir
            // 
            this.txtDataDir.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.txtDataDir.Location = new System.Drawing.Point(15, 47);
            this.txtDataDir.Name = "txtDataDir";
            this.txtDataDir.ReadOnly = true;
            this.txtDataDir.Size = new System.Drawing.Size(438, 20);
            this.txtDataDir.TabIndex = 2;
            this.txtDataDir.TextChanged += new System.EventHandler(this.txtDataDir_TextChanged);
            // 
            // btnUpdateJobCost
            // 
            this.btnUpdateJobCost.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnUpdateJobCost.Location = new System.Drawing.Point(449, 407);
            this.btnUpdateJobCost.Name = "btnUpdateJobCost";
            this.btnUpdateJobCost.Size = new System.Drawing.Size(98, 23);
            this.btnUpdateJobCost.TabIndex = 4;
            this.btnUpdateJobCost.Text = "&Update Job Cost";
            this.btnUpdateJobCost.UseVisualStyleBackColor = true;
            this.btnUpdateJobCost.Click += new System.EventHandler(this.btnUpdateJobCost_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(239, 84);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "End Period";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 87);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(76, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "Starting Period";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.optionsToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(9, 4);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(150, 24);
            this.menuStrip1.TabIndex = 12;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "&File";
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(92, 22);
            this.quitToolStripMenuItem.Text = "E&xit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // optionsToolStripMenuItem
            // 
            this.optionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.selectTMJobTypesToolStripMenuItem});
            this.optionsToolStripMenuItem.Name = "optionsToolStripMenuItem";
            this.optionsToolStripMenuItem.Size = new System.Drawing.Size(61, 20);
            this.optionsToolStripMenuItem.Text = "&Options";
            // 
            // selectTMJobTypesToolStripMenuItem
            // 
            this.selectTMJobTypesToolStripMenuItem.Name = "selectTMJobTypesToolStripMenuItem";
            this.selectTMJobTypesToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.selectTMJobTypesToolStripMenuItem.Text = "Select T&M Job Types";
            this.selectTMJobTypesToolStripMenuItem.Click += new System.EventHandler(this.selectTMJobTypesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.onlineHelpToolStripMenuItem,
            this.aboutToolStripMenuItem,
            this.activateToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.helpToolStripMenuItem.Text = "&Help";
            // 
            // onlineHelpToolStripMenuItem
            // 
            this.onlineHelpToolStripMenuItem.Name = "onlineHelpToolStripMenuItem";
            this.onlineHelpToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.onlineHelpToolStripMenuItem.Text = "Online Help";
            this.onlineHelpToolStripMenuItem.Click += new System.EventHandler(this.onlineHelpToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.aboutToolStripMenuItem.Text = "About";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // activateToolStripMenuItem
            // 
            this.activateToolStripMenuItem.Name = "activateToolStripMenuItem";
            this.activateToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
            this.activateToolStripMenuItem.Text = "Activate";
            this.activateToolStripMenuItem.Click += new System.EventHandler(this.activateToolStripMenuItem_Click);
            // 
            // demoLabel
            // 
            this.demoLabel.AutoSize = true;
            this.demoLabel.Location = new System.Drawing.Point(208, 15);
            this.demoLabel.Name = "demoLabel";
            this.demoLabel.Size = new System.Drawing.Size(81, 13);
            this.demoLabel.TabIndex = 14;
            this.demoLabel.Text = "This goes away";
            // 
            // radioShowTMJobs
            // 
            this.radioShowTMJobs.AutoSize = true;
            this.radioShowTMJobs.Checked = true;
            this.radioShowTMJobs.Location = new System.Drawing.Point(172, 188);
            this.radioShowTMJobs.Name = "radioShowTMJobs";
            this.radioShowTMJobs.Size = new System.Drawing.Size(126, 17);
            this.radioShowTMJobs.TabIndex = 15;
            this.radioShowTMJobs.TabStop = true;
            this.radioShowTMJobs.Text = "Show T&&M Jobs Only";
            this.radioShowTMJobs.UseVisualStyleBackColor = true;
            // 
            // radioShowAllJobs
            // 
            this.radioShowAllJobs.AutoSize = true;
            this.radioShowAllJobs.Location = new System.Drawing.Point(334, 188);
            this.radioShowAllJobs.Name = "radioShowAllJobs";
            this.radioShowAllJobs.Size = new System.Drawing.Size(91, 17);
            this.radioShowAllJobs.TabIndex = 16;
            this.radioShowAllJobs.Text = "Show All Jobs";
            this.radioShowAllJobs.UseVisualStyleBackColor = true;
            // 
            // dteStartDate
            // 
            this.dteStartDate.Location = new System.Drawing.Point(104, 121);
            this.dteStartDate.Name = "dteStartDate";
            this.dteStartDate.Size = new System.Drawing.Size(210, 20);
            this.dteStartDate.TabIndex = 17;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 127);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(55, 13);
            this.label1.TabIndex = 18;
            this.label1.Text = "Start Date";
            // 
            // chkUnbilled
            // 
            this.chkUnbilled.AutoSize = true;
            this.chkUnbilled.Location = new System.Drawing.Point(15, 188);
            this.chkUnbilled.Name = "chkUnbilled";
            this.chkUnbilled.Size = new System.Drawing.Size(131, 17);
            this.chkUnbilled.TabIndex = 19;
            this.chkUnbilled.Text = "Unbilled Records Only";
            this.chkUnbilled.UseVisualStyleBackColor = true;
            this.chkUnbilled.CheckedChanged += new System.EventHandler(this.chkUnbilled_CheckedChanged);
            // 
            // btnSMBDir
            // 
            this.btnSMBDir.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSMBDir.Location = new System.Drawing.Point(472, 45);
            this.btnSMBDir.Name = "btnSMBDir";
            this.btnSMBDir.Size = new System.Drawing.Size(75, 23);
            this.btnSMBDir.TabIndex = 20;
            this.btnSMBDir.Text = "&Browse";
            this.btnSMBDir.UseVisualStyleBackColor = true;
            this.btnSMBDir.Click += new System.EventHandler(this.btnSMBDir_Click);
            // 
            // cmbStartingPeriod
            // 
            this.cmbStartingPeriod.ConfigVarName = null;
            this.cmbStartingPeriod.DropDownWidth = 112;
            this.cmbStartingPeriod.FormattingEnabled = true;
            this.cmbStartingPeriod.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbStartingPeriod.Location = new System.Drawing.Point(104, 84);
            this.cmbStartingPeriod.Name = "cmbStartingPeriod";
            this.cmbStartingPeriod.Size = new System.Drawing.Size(113, 21);
            this.cmbStartingPeriod.TabIndex = 9;
            this.cmbStartingPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbStartingPeriod_SelectedIndexChanged);
            // 
            // cmbEndPeriod
            // 
            this.cmbEndPeriod.ConfigVarName = null;
            this.cmbEndPeriod.FormattingEnabled = true;
            this.cmbEndPeriod.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3",
            "4",
            "5",
            "6",
            "7",
            "8",
            "9",
            "10",
            "11",
            "12"});
            this.cmbEndPeriod.Location = new System.Drawing.Point(313, 84);
            this.cmbEndPeriod.Name = "cmbEndPeriod";
            this.cmbEndPeriod.Size = new System.Drawing.Size(112, 21);
            this.cmbEndPeriod.TabIndex = 7;
            this.cmbEndPeriod.SelectedIndexChanged += new System.EventHandler(this.cmbEndPeriod_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 217);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(76, 13);
            this.label2.TabIndex = 21;
            this.label2.Text = "Summarize By:";
            // 
            // chkSumCustomer
            // 
            this.chkSumCustomer.AutoSize = true;
            this.chkSumCustomer.ConfigSettingName = "chkSumCustomer";
            this.chkSumCustomer.Location = new System.Drawing.Point(34, 241);
            this.chkSumCustomer.Name = "chkSumCustomer";
            this.chkSumCustomer.Size = new System.Drawing.Size(70, 17);
            this.chkSumCustomer.TabIndex = 22;
            this.chkSumCustomer.Text = "Customer";
            this.chkSumCustomer.UseVisualStyleBackColor = true;
            // 
            // chkSumPeriod
            // 
            this.chkSumPeriod.AutoSize = true;
            this.chkSumPeriod.ConfigSettingName = "chkSumPeriod";
            this.chkSumPeriod.Location = new System.Drawing.Point(34, 287);
            this.chkSumPeriod.Name = "chkSumPeriod";
            this.chkSumPeriod.Size = new System.Drawing.Size(113, 17);
            this.chkSumPeriod.TabIndex = 23;
            this.chkSumPeriod.Text = "Accounting Period";
            this.chkSumPeriod.UseVisualStyleBackColor = true;
            // 
            // chkSumJob
            // 
            this.chkSumJob.AutoSize = true;
            this.chkSumJob.ConfigSettingName = "chkSumJob";
            this.chkSumJob.Location = new System.Drawing.Point(34, 264);
            this.chkSumJob.Name = "chkSumJob";
            this.chkSumJob.Size = new System.Drawing.Size(43, 17);
            this.chkSumJob.TabIndex = 24;
            this.chkSumJob.Text = "Job";
            this.chkSumJob.UseVisualStyleBackColor = true;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 322);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 25;
            this.label5.Text = "Tax part class";
            // 
            // cboTaxPartClass
            // 
            this.cboTaxPartClass.ConfigVarName = null;
            this.cboTaxPartClass.FormattingEnabled = true;
            this.cboTaxPartClass.Location = new System.Drawing.Point(104, 319);
            this.cboTaxPartClass.Name = "cboTaxPartClass";
            this.cboTaxPartClass.Size = new System.Drawing.Size(203, 21);
            this.cboTaxPartClass.TabIndex = 26;
            this.cboTaxPartClass.SelectedIndexChanged += new System.EventHandler(this.cboTaxPartClass_SelectedIndexChanged);
            // 
            // grpBoxSelOptions
            // 
            this.grpBoxSelOptions.Controls.Add(this.radCombineForBilling);
            this.grpBoxSelOptions.Controls.Add(this.radScanJobForTax);
            this.grpBoxSelOptions.Location = new System.Drawing.Point(17, 388);
            this.grpBoxSelOptions.Name = "grpBoxSelOptions";
            this.grpBoxSelOptions.Size = new System.Drawing.Size(408, 75);
            this.grpBoxSelOptions.TabIndex = 27;
            this.grpBoxSelOptions.TabStop = false;
            this.grpBoxSelOptions.Text = "Job cost selection option";
            // 
            // radCombineForBilling
            // 
            this.radCombineForBilling.AutoSize = true;
            this.radCombineForBilling.Location = new System.Drawing.Point(17, 41);
            this.radCombineForBilling.Name = "radCombineForBilling";
            this.radCombineForBilling.Size = new System.Drawing.Size(160, 17);
            this.radCombineForBilling.TabIndex = 1;
            this.radCombineForBilling.TabStop = true;
            this.radCombineForBilling.Text = "Combine Job Costs for Billing";
            this.radCombineForBilling.UseVisualStyleBackColor = true;
            // 
            // radScanJobForTax
            // 
            this.radScanJobForTax.AutoSize = true;
            this.radScanJobForTax.Location = new System.Drawing.Point(17, 19);
            this.radScanJobForTax.Name = "radScanJobForTax";
            this.radScanJobForTax.Size = new System.Drawing.Size(187, 17);
            this.radScanJobForTax.TabIndex = 0;
            this.radScanJobForTax.TabStop = true;
            this.radScanJobForTax.Text = "Scan open job costs for tax liability";
            this.radScanJobForTax.UseVisualStyleBackColor = true;
            // 
            // cboPhaseNum
            // 
            this.cboPhaseNum.ConfigVarName = null;
            this.cboPhaseNum.FormattingEnabled = true;
            this.cboPhaseNum.Location = new System.Drawing.Point(104, 353);
            this.cboPhaseNum.Name = "cboPhaseNum";
            this.cboPhaseNum.Size = new System.Drawing.Size(113, 21);
            this.cboPhaseNum.TabIndex = 29;
            this.cboPhaseNum.SelectedIndexChanged += new System.EventHandler(this.cboPhaseNum_SelectedIndexChanged);
            // 
            // lblPhase
            // 
            this.lblPhase.AutoSize = true;
            this.lblPhase.Location = new System.Drawing.Point(48, 356);
            this.lblPhase.Name = "lblPhase";
            this.lblPhase.Size = new System.Drawing.Size(37, 13);
            this.lblPhase.TabIndex = 28;
            this.lblPhase.Text = "Phase";
            // 
            // lblEndDate
            // 
            this.lblEndDate.AutoSize = true;
            this.lblEndDate.Location = new System.Drawing.Point(14, 158);
            this.lblEndDate.Name = "lblEndDate";
            this.lblEndDate.Size = new System.Drawing.Size(52, 13);
            this.lblEndDate.TabIndex = 31;
            this.lblEndDate.Text = "End Date";
            // 
            // dteEndDate
            // 
            this.dteEndDate.Location = new System.Drawing.Point(104, 152);
            this.dteEndDate.Name = "dteEndDate";
            this.dteEndDate.Size = new System.Drawing.Size(210, 20);
            this.dteEndDate.TabIndex = 30;
            // 
            // JobCostManagement
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(574, 472);
            this.Controls.Add(this.lblEndDate);
            this.Controls.Add(this.dteEndDate);
            this.Controls.Add(this.cboPhaseNum);
            this.Controls.Add(this.lblPhase);
            this.Controls.Add(this.grpBoxSelOptions);
            this.Controls.Add(this.cboTaxPartClass);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.chkSumJob);
            this.Controls.Add(this.chkSumPeriod);
            this.Controls.Add(this.chkSumCustomer);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnSMBDir);
            this.Controls.Add(this.chkUnbilled);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dteStartDate);
            this.Controls.Add(this.radioShowAllJobs);
            this.Controls.Add(this.radioShowTMJobs);
            this.Controls.Add(this.demoLabel);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.cmbStartingPeriod);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.cmbEndPeriod);
            this.Controls.Add(this.btnUpdateJobCost);
            this.Controls.Add(this.txtDataDir);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(590, 510);
            this.Name = "JobCostManagement";
            this.Text = "Syscon Job Cost Management Tool";
            this.Load += new System.EventHandler(this.JobCostManagement_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.grpBoxSelOptions.ResumeLayout(false);
            this.grpBoxSelOptions.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtDataDir;
        private System.Windows.Forms.Button btnUpdateJobCost;
        private SysconCommon.GUI.SearchableComboBox cmbEndPeriod;
        private System.Windows.Forms.Label label3;
        private SysconCommon.GUI.SearchableComboBox cmbStartingPeriod;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem onlineHelpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem activateToolStripMenuItem;
        private System.Windows.Forms.Label demoLabel;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem optionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem selectTMJobTypesToolStripMenuItem;
        private System.Windows.Forms.RadioButton radioShowTMJobs;
        private System.Windows.Forms.RadioButton radioShowAllJobs;
        private System.Windows.Forms.DateTimePicker dteStartDate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkUnbilled;
        private System.Windows.Forms.Button btnSMBDir;
        private System.Windows.Forms.Label label2;
        private SysconCommon.GUI.SysconCheckBox chkSumCustomer;
        private SysconCommon.GUI.SysconCheckBox chkSumPeriod;
        private SysconCommon.GUI.SysconCheckBox chkSumJob;
        private System.Windows.Forms.Label label5;
        private SysconCommon.GUI.SearchableComboBox cboTaxPartClass;
        private System.Windows.Forms.GroupBox grpBoxSelOptions;
        private System.Windows.Forms.RadioButton radCombineForBilling;
        private System.Windows.Forms.RadioButton radScanJobForTax;
        private SysconCommon.GUI.SearchableComboBox cboPhaseNum;
        private System.Windows.Forms.Label lblPhase;
        private System.Windows.Forms.Label lblEndDate;
        private System.Windows.Forms.DateTimePicker dteEndDate;


    }
}

