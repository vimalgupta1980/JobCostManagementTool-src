using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Security.Principal;
using System.Data.OleDb;

using SysconCommon.Common.Environment;
using SysconCommon.Common;
using SysconCommon.Algebras.DataTables;
using SysconCommon.Accounting;
using SysconCommon.GUI;
using SysconCommon.Foxpro;


namespace Syscon.JobCostManagementTool
{

    /// <summary>
    /// 
    /// </summary>
    public partial class JobCostManagement : Form
    {
        private SysconCommon.COMMethods mbapi = new SysconCommon.COMMethods();
        bool loaded = false;

        /// <summary>
        /// Ctor
        /// </summary>
        public JobCostManagement()
        {
            InitializeComponent();

            Env.CopyOldConfigs();

#if false
            Env.ConfigInjector = (name) =>
            {
                if (new string[] { "mbdir", "product_id", "product_version", "run_count" }.Contains(name))
                {
                    Env.SetConfigFile(Env.ConfigDataPath + "/config.xml");
                    return name;
                }
                else if (new string[] { "tmtypes", "nonbillablecostcodes" }.Contains(name))
                {
                    // return "dataset" + Env.GetConfigVar("datadir", "c:\\mb7\\sample company\\", true).GetMD5Sum() + "/" + name;
                    Env.SetConfigFile(Env.GetConfigVar("mbdir") + "/syscon_tm_analysis_config.xml");
                    return name;
                }
                else
                {
                    Env.SetConfigFile(Env.GetEXEDirectory() + "/config.xml");
                    var username = WindowsIdentity.GetCurrent().Name;
                    return "dataset" + (Env.GetConfigVar("mbdir", "c:\\mb7\\sample company\\", true) + username).GetMD5Sum() + "/" + name;
                }
            };
#else   
            Env.ConfigInjector = (name) =>
            {
                var dataset_specific = new string[] { "tmtypes", "nonbillablecostcodes" };

                if (dataset_specific.Contains(name))
                {
                    Env.SetConfigFile(Env.GetConfigVar("mbdir") + "/syscon_tm_analysis_config.xml");
                    return name;
                }
                else
                {
                    Env.SetConfigFile(Env.ConfigDataPath + "/config.xml");
                    return name;
                }
            };
#endif
        }

        #region Private Methods

        private void LoadValues()
        {
            string partClass    = Env.GetConfigVar("taxPartClass", "0", true);
            string phaseNum     = Env.GetConfigVar("PhaseNum", "0", true);

            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                //Get clsnme values from prtcls and fill the part class combo box
                DataTable partClassDt = con.GetDataTable("prtcls", "select clsnme from prtcls");
                cboTaxPartClass.DataSource = (from s in partClassDt.Rows.ToIEnumerable()
                                              select s[0].ToString().Trim()).ToArray();

                //Get phase name from jobphs and fill the phase name combo box
                DataTable phaseNumDt = con.GetDataTable("jobphs", "select distinct phsnme from jobphs");
                cboPhaseNum.DataSource = (from s in phaseNumDt.Rows.ToIEnumerable()
                                              select s[0].ToString().Trim()).ToArray();
            }            

            cmbEndPeriod.SelectItem<string>(p => p == Env.GetConfigVar("endperiod", "12", true));
            cmbStartingPeriod.SelectItem<string>(p => p == Env.GetConfigVar("startperiod", "0", true));
            cboTaxPartClass.SelectedItem    = partClass;
            cboPhaseNum.SelectedItem        = phaseNum;

            chkUnbilled.Checked = Env.GetConfigVar("ExportUnbilledOnly", false, true);
        }

        #endregion

        #region Licencing
		 
        private void SetupTrial(int daysLeft)
        {
            var msg = string.Format("You have {0} days left to evaluate this software", daysLeft);
            this.demoLabel.Text = msg;
            btnUpdateJobCost.Enabled = true;
        }

        private void SetupInvalid()
        {
            btnUpdateJobCost.Enabled = false;
            this.demoLabel.Text = "Your License has expired or is invalid";
        }

        private void SetupFull()
        {
            btnUpdateJobCost.Enabled = true;
            this.demoLabel.Text = "";
            this.activateToolStripMenuItem.Visible = false;
        }

        #endregion

        #region Control Event Handlers

        private void JobCostManagement_Load(object sender, EventArgs e)
        {
            // resets it everytime it is run so that the user can't just change to a product they already have a license for
            Env.SetConfigVar("product_id", 178510);
            
            var product_id = Env.GetConfigVar("product_id", 0, false);
            var product_version = "1.0.0.0";
            bool require_login = false;

            if (!loaded)
            {
                require_login = true;
                loaded = true;
                this.Text += " (version " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ")";
            }

            try
            {
                var license = SysconCommon.Protection.ProtectionInfo.GetLicense(product_id, product_version, 15751);

                if (license.IsTrial)
                {
                    if (!license.IsValid())
                    {
                        SetupInvalid();
                    }
                    else
                    {
                        var l = license as SysconCommon.Protection.TrialLicense;
                        SetupTrial(l.DaysLeft);
                    }
                }
                else
                {
                    SetupFull();
                }
            }
            catch
            {
                SetupInvalid();
            }
            
            txtDataDir.TextChanged +=new EventHandler(txtDataDir_TextChanged);

            if (require_login)
            {
                mbapi.smartGetSMBDir();

                if (mbapi.RequireSMBLogin() == null)
                    this.Close();
            }

            txtDataDir.Text = mbapi.smartGetSMBDir();
        }
        
        private void txtDataDir_TextChanged(object sender, EventArgs e)
        {
            SysconCommon.Common.Environment.Connections.SetOLEDBFreeTableDirectory(txtDataDir.Text);
            LoadValues();
        }

        private void btnUpdateJobCost_Click(object sender, EventArgs e)
        {
            SysconCommon.Common.Environment.Connections.SetOLEDBFreeTableDirectory(txtDataDir.Text);

            var validjobtypes_delim = Env.GetConfigVar<string>("tmtypes", "", true);
            var validjobtypes_strs  = validjobtypes_delim.Split(',');
            var validjobtypes       = validjobtypes_delim.Trim() == "" ? new long[] { } : validjobtypes_strs.Select(i => Convert.ToInt64(i));

            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                long[] jobs = null;

                if (this.radioShowTMJobs.Checked)
                {
                    using (var jobtyps = con.GetTempDBF())
                    {
                        con.ExecuteNonQuery("create table {0} (jobtyp n(3, 0))", jobtyps);
                        foreach (var jt in validjobtypes)
                        {
                            con.ExecuteNonQuery("insert into {0} (jobtyp) values ({1})", jobtyps, jt);
                        }

                        jobs = (from x in con.GetDataTable("Jobnums", "select actrec.recnum from actrec join {0} jobtypes on actrec.jobtyp = jobtypes.jobtyp", jobtyps).Rows
                                select Convert.ToInt64(x["recnum"])).ToArray();
                    }
                }

                var job_selector = new MultiJobSelector(jobs);

                if (!(job_selector.ShowDialog() == System.Windows.Forms.DialogResult.Cancel))
                {
                    var jobnums = job_selector.SelectedJobNumbers.ToArray();
                    JobCostDbHelper jobCostDB = new JobCostDbHelper();

                    if (jobnums.Length > 0)
                    {
                        long phaseNum = 0;
                        int taxPartClassId = 0;

                        FillPhaseAndTaxPartInfo(out phaseNum, out taxPartClassId);

                        //Based on the other parameters selected, run the Option 1 or Option 2.
                        foreach (long jobNum in jobnums)
                        {
                            // OPTION 1
                            if (this.radScanJobForTax.Checked)
                            {                                
                                // This routine scans job costs for tax liabilities
                                jobCostDB.ScanForTaxLiability(jobNum, phaseNum, taxPartClassId);
                            }

                            // OPTION 2
                            if (this.radCombineForBilling.Checked)
                            {
                                // The next two procedures are run together to create billable cost records that 
                                // are combined from distinct job cost records by cost type.

                                jobCostDB.ConsolidateJobCost(dteStartDate.Value, dteEndDate.Value, jobNum, phaseNum);
                                jobCostDB.UpdateTMTJobCost(dteStartDate.Value, dteEndDate.Value, jobNum, phaseNum);
                            }
                        }
                    }
                    else
                    {
                        MessageBox.Show("No jobs selected.", "Error", MessageBoxButtons.OK);
                    }
                }                
            }
        }

        private void FillPhaseAndTaxPartInfo(out long phaseNum, out int taxPartClassId)
        {
            long phase = 0;
            int taxPartId = 0;

            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                try
                {
                    phase = con.GetScalar<long>("SELECT phsnum from jobphs where phsnme = \"{0}\"", cboPhaseNum.SelectedItem);
                    taxPartId = con.GetScalar<int>("SELECT recnum from prtcls where clsnme = \"{0}\"", cboTaxPartClass.SelectedItem);
                }
                catch
                {
                    phaseNum = phase;
                    taxPartClassId = taxPartId;
                }
            }

            phaseNum = phase;
            taxPartClassId = taxPartId;
        }        

        private void cmbEndPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("endperiod", cmbEndPeriod.SelectedItem);
        }

        private void cmbStartingPeriod_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("startperiod", cmbStartingPeriod.SelectedItem);
        }

        private void cboTaxPartClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("taxPartClass", cboTaxPartClass.SelectedItem);
        }

        private void cboPhaseNum_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("PhaseNum", cboPhaseNum.SelectedItem);
        }

        private void chkUnbilled_CheckedChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("ExportUnbilledOnly", chkUnbilled.Checked);
        }

        private void btnSMBDir_Click(object sender, EventArgs e)
        {
            mbapi.smartSelectSMBDirByGUI();
            var usr = mbapi.RequireSMBLogin();
            if (usr != null)
            {
                txtDataDir.Text = mbapi.smartGetSMBDir();
            }
        }

        #region Menu Handlers
        
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void selectNonBillableCostCodesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //var frm = new CostCodeSelector();
            //frm.ShowDialog();
            //var nonbillable = frm.NonBillableCostCodes.Select(i => i.ToString()).ToArray();
            //var nonbillablecostcodes = string.Join(",", nonbillable);
            //Env.SetConfigVar("nonbillablecostcodes", nonbillablecostcodes);
        }

        private void selectTMJobTypesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new TMTypes();
            frm.ShowDialog();
            var tmtypes = frm.TimeAndMaterialTypes;
            var tmtypes_delimited = string.Join(",", tmtypes);
            Env.SetConfigVar("tmtypes", tmtypes_delimited);

            // make sure the correct jobs show up now
            LoadValues();
        }

        private void selectMBDirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var dlg = new FolderBrowserDialog();
            dlg.SelectedPath = Env.GetConfigVar("mbdir");
            var rslt = dlg.ShowDialog();
            if (rslt == DialogResult.OK)
            {
                var dir = dlg.SelectedPath + "\\";
                if (!File.Exists(dir + "actrec.dbf"))
                {
                    MessageBox.Show("Please choose a valid MB7 Path");
                }
                else
                {
                    this.txtDataDir.Text = dir;
                }
            }
        }
        
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var frm = new About();
            frm.ShowDialog();
        }

        private void onlineHelpToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("http://syscon-inc.com/product-support/2165/support.asp");
        }

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var product_id = Env.GetConfigVar("product_id", 0, false);
            var product_version = Env.GetConfigVar("product_version", "0.0.0.0", false);

            var frm = new SysconCommon.Protection.ProtectionPlusOnlineActivationForm(product_id, product_version);
            frm.ShowDialog();
            this.OnLoad(null);
        }

        #endregion  //Menu Handlers

        #endregion

    }
}
