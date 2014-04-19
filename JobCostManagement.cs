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
    /// The main UI for the job cost management tool.
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
            string costCode     = Env.GetConfigVar("CostCode", "0", true);

            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                //Get clsnme values from prtcls and fill the part class combo box
                DataTable partClassDt = con.GetDataTable("prtcls", "select recnum, clsnme from prtcls order by recnum");
                cboTaxPartClass.DataSource = (from s in partClassDt.Rows.ToIEnumerable()
                                              select (s[0].ToString().Trim() + "-" + s[1].ToString().Trim())).ToArray();

                //Get cost codes from cstcde
                DataTable costCodeDt = con.GetDataTable("CostCode", "Select recnum, cdenme from cstcde order by recnum");
                cboCostCode.DataSource = new string[] { "None" }.Concat((from s in costCodeDt.Rows.ToIEnumerable()
                                                                         select (Convert.ToInt32(s[0]).ToString().Trim() + "-" + s[1].ToString().Trim()))).ToArray();
            }            

            cboTaxPartClass.SelectedItem    = partClass;
            cboCostCode.SelectedItem        = costCode;

            radioShowAllJobs.Checked = Env.GetConfigVar("ShowAllJobs", false, false);
            radioShowTMJobs.Checked = Env.GetConfigVar("ShowTnMJobs", false, false);

            DateTime startDate;
            DateTime endDate;

            DateTime.TryParse(Env.GetConfigVar("StartDate"), out startDate);
            DateTime.TryParse(Env.GetConfigVar("EndDate"), out endDate);

            dteStartDate.Value = (startDate == DateTime.MinValue) ? DateTime.Now : startDate;
            dteEndDate.Value = (endDate == DateTime.MinValue) ? DateTime.Now : endDate; ;
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
            Env.SetConfigVar("product_id", 322504);
            
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
                var license = SysconCommon.Protection.ProtectionInfo.GetLicense(product_id, product_version);

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
                        int costCode = 0;
                        FillTaxPartInfo(out taxPartClassId);
                        FillCostCodeInfo(out costCode);

                        try
                        {
                            //Based on the other parameters selected, run the Option 1 or Option 2.
                            foreach (long jobNum in jobnums)
                            {
                                // OPTION 1
                                if (this.radScanJobForTax.Checked)
                                {
                                    // This routine scans job costs for tax liabilities
                                    jobCostDB.ScanForTaxLiability(jobNum, phaseNum, taxPartClassId);

                                    MessageBox.Show("Finished scanning jobs for tax liabilities");
                                }

                                // OPTION 2
                                if (this.radCombineForBilling.Checked)
                                {
                                    //Save the start and end date to config
                                    Env.SetConfigVar("StartDate", dteStartDate.Value);
                                    Env.SetConfigVar("EndDate", dteEndDate.Value);

                                    // The next two procedures are run together to create billable cost records that 
                                    // are combined from distinct job cost records by cost type.
                                    jobCostDB.ConsolidateJobCost(dteStartDate.Value, dteEndDate.Value, jobNum, phaseNum, costCode);
                                    jobCostDB.UpdateTMTJobCost(dteStartDate.Value, dteEndDate.Value, jobNum, phaseNum);

                                    MessageBox.Show("Finished consolidating job costs");
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                    }
                    else
                    {
                        MessageBox.Show("No jobs selected.", "Error", MessageBoxButtons.OK);
                    }
                }                
            }
        }

        private void FillTaxPartInfo(out int taxPartClassId)
        {
            int taxPartId = 0;

            try
            {
                //string clsName = cboTaxPartClass.SelectedItem.ToString().Split('-')[1];
                taxPartId = Convert.ToInt32(cboTaxPartClass.SelectedItem.ToString().Split('-')[0]);//con.GetScalar<int>("SELECT recnum from prtcls where clsnme = \"{0}\"", cboTaxPartClass.SelectedItem);
            }
            catch
            {
                taxPartClassId = taxPartId;
            }


            taxPartClassId = taxPartId;
        }

        private void FillCostCodeInfo(out int costCode)
        {
            try
            {
                costCode = Convert.ToInt32(cboCostCode.SelectedItem.ToString().Split('-')[0]);
            }
            catch
            {
                costCode = 0;
                Env.Log("Error in parsing cost code from cost code combo box selected value.");
            }
        }

        private void cboTaxPartClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("taxPartClass", cboTaxPartClass.SelectedItem);
        }

        private void cboCostCode_SelectedIndexChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("CostCode", cboCostCode.SelectedItem);
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

        private void radioShowAllJobs_CheckedChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("ShowAllJobs", radioShowAllJobs.Checked);
        }

        private void radioShowTMJobs_CheckedChanged(object sender, EventArgs e)
        {
            Env.SetConfigVar("ShowTnMJobs", radioShowTMJobs.Checked);
        }

        #region Menu Handlers
        
        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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
            System.Diagnostics.Process.Start("http://syscon-inc.com/product-support/CustomApplication/support.asp");
        }

        private void activateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var product_id = Env.GetConfigVar("product_id", 0, false);
            var product_version = Env.GetConfigVar("product_version", "1.0.0.0", false);

            var frm = new SysconCommon.Protection.ProtectionPlusOnlineActivationForm(product_id, product_version);
            frm.ShowDialog();
            this.OnLoad(null);
        }

        #endregion  //Menu Handlers                

        #endregion

        

    }
}
