using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Data.OleDb;

using SysconCommon.Algebras.DataTables;
using SysconCommon.Common;
using SysconCommon.Common.Environment;
using SysconCommon.Foxpro;
using SysconCommon.GUI;

namespace Syscon.JobCostManagementTool
{
    /// <summary>
    /// This class contains methods for updating, modifying the job costs.
    /// </summary>
    internal class JobCostDbHelper
    {
        /// <summary>
        /// This routine scans all job costs that have not been billed, and match the 
        /// job number and phase as passed by the user.  If there has been no tax liability
        /// accrued on the invoice from which this job cost originated, we create a job 
        /// cost record.
        /// </summary>
        /// <param name="jobNumber"></param>
        /// <param name="jobPhase"></param>
        /// <param name="taxPartClassId"></param>
        public void ScanForTaxLiability(long jobNumber, long jobPhase, int taxPartClassId)
        {
            //This part classification indicates which parts are considered tax parts
            int taxPartClass = taxPartClassId;

            double[] taxRates = new double[9];

            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                using
                    (
                        Env.TempDBFPointer
                        ActiveTaxParts = con.GetTempDBF(),
                        ActiveJobCosts = con.GetTempDBF(),
                        ActiveJobCostsTmp = con.GetTempDBF(),
                        ActiveAPLines = con.GetTempDBF(),
                        TaxJobCosts = con.GetTempDBF(),
                        TaxTemp = con.GetTempDBF()
                    )
                {

                    //Get the tax rate details
                    int taxCode = con.GetScalar<int>("SELECT slstax from actrec where recnum = {0}", jobNumber);
                    string taxDetail = con.GetScalar<string>("Select ntetxt from taxdst where recnum = {0}", taxCode);

                    FillTaxRates(taxRates, taxDetail);
                    
                    //Get the list of active tax rate parts for reference
                    int fldCount = con.ExecuteNonQuery("SELECT	recnum, prtnme, prtunt, csttyp, prtcls, prtcst FROM tkfprt WHERE prtcls = {0} "
                                                        + "INTO Table {1} ORDER BY recnum", taxPartClass, ActiveTaxParts);

                    //Get list of active job cost records to be billed
                    //TODO -   if JobNumber = zero, scan all jobs and phases
                    //         if JobPhase = zero, scan all phases for the jobs
                    fldCount = con.ExecuteNonQuery("SELECT * FROM jobcst "
                                                    + "WHERE jobnum = {0} "
                                                    + "AND phsnum = {1} "
                                                    + "AND status = 1 AND bllsts = 1 INTO Table {2}", jobNumber, jobPhase, ActiveJobCosts);

                    //Identify for each active job cost record if a tax burden has been applied in the
                    //entry of the originating transactions.   For now, that is only AP entries
                    //Get the list of AP invoices associated with the job costs
                    fldCount = con.ExecuteNonQuery("SELECT ajc.*, NVL(a.recnum, 00000000) as aprecnum, NVL(a.invnum, SPACE(15)) as apinvnum, 000 as taxprtcnt, "
                                                    + "PADR(ALLTRIM(SUBSTR(trnnum,1,LEN(trnnum)-2)) + \"-T\",LEN(trnnum)) as TaxTrnnum, 000 as taxAccCnt "
                                                    + "FROM {0} ajc LEFT JOIN acpinv a ON ajc.lgrrec = a.lgrrec "
                                                    + "INTO Table {1}", ActiveJobCosts, ActiveJobCostsTmp);


                    //Get the list of AP lines used to generate the job costs
                    //Include a marking if the part number is from the tax part classification
                    //this indicates that it is a taxing part
                    fldCount = con.ExecuteNonQuery("SELECT DISTINCT a.recnum, a.linnum, a.prtnum, NVL(t.prtcls, 0) as prtcls, a.linqty, a.linprc, a.extttl, "
                                                + "a.actnum, a.subact FROM apivln a "
                                                + "JOIN {0} ajc ON a.recnum = ajc.aprecnum "
                                                + "LEFT JOIN tkfprt t ON a.prtnum = t.recnum "
                                                + "WHERE ajc.srcnum = 11 INTO TABLE {1}", ActiveJobCostsTmp, ActiveAPLines);

                    //Mark each active job cost record as to whether there was a tax accrual/payment made on that
                    //job cost record.  This is done by counting the tax parts that were used on the invoice
                    int taxPartCount = con.GetScalar<int>("select COUNT(*) from {0} _ActiveAPLines, {1} _ActiveJobCosts WHERE "
                                                            + "_ActiveAPLines.recnum = _ActiveJobCosts.aprecnum AND _ActiveAPLines.prtcls = {2}"
                                                            , ActiveAPLines, ActiveJobCostsTmp, taxPartClass);

                    fldCount = con.ExecuteNonQuery("UPDATE {0} SET taxprtcnt = {1}", ActiveJobCostsTmp, taxPartCount);

                    int tranCount = con.GetScalar<int>("select COUNT(*) from {0} WHERE trnnum == taxtrnnum", ActiveJobCostsTmp);
                    fldCount = con.ExecuteNonQuery("UPDATE {0} SET taxacccnt = {1} "
                                                            + "where usrnme <> \"TaxAcc\"", ActiveJobCostsTmp, tranCount);

                    //Check to see if each job cost has already had taxes accrued
                    //Create list of job cost records that must be accrued with taxes
                    fldCount = con.ExecuteNonQuery("SELECT	recnum, jobnum, phsnum, trnnum, dscrpt, trndte, {0} as entdte, actprd, 31 as srcnum, 1 as status, 1 as bllsts, "
                                        + "cstcde, csttyp, cstamt as origcstamt, 00000000.00 as cstamt, 00000000.00 as blgamt, 000 as ovrrde, \"TaxAcc\" as usrnme "
                                        + "FROM {1} WHERE taxprtcnt = 0 AND taxacccnt = 0 AND INLIST(srcnum,11) INTO Table {2}",
                                        DateTime.Today.ToFoxproDate(), ActiveJobCostsTmp, TaxJobCosts);

                    //Update the basic information to identify these as tax accrual records
                    DataTable taxJobCostDt = con.GetDataTable("TaxJobCosts", "select * from {0}", TaxJobCosts);

                    foreach (DataRow dr in taxJobCostDt.Rows)
                    {
                        decimal recNum = (decimal)dr["recnum"];
                        decimal cstType = (decimal)dr["csttyp"];
                        decimal origcStament = (decimal)dr["origcstamt"];

                        //TODO: The array index starts from 0 so the taxRate will be different. Need to do -1 in the index
                        fldCount = con.ExecuteNonQuery("UPDATE {0} SET trnnum = ALLTRIM(SUBSTR(trnnum,1,LEN(trnnum)-2)) + \"-T\", "
                                                            + "dscrpt = ALLTRIM(SUBSTR(dscrpt,1,LEN(dscrpt)-4)) + \" Tax\", "
                                                            + "cstamt = {1}, "
                                                            + "blgamt = {2} WHERE recnum = {3}", TaxJobCosts, recNum, origcStament * (decimal)taxRates[(int)cstType], origcStament * (decimal)taxRates[(int)cstType]);
                    }

                    //Set this so that FoxPro doesn't try to insert null values in empty columns
                    SetNullOff(con);

                    //Add the records
                    int taxJobCostsCount = con.GetScalar<int>("select count(*) from {0}", TaxJobCosts);                                        

                    if (taxJobCostsCount > 0)
                    {
                        //TODO: Make the below code similar in functionality to the original foxpro code.
                        fldCount = con.ExecuteNonQuery("INSERT INTO jobcst ( recnum, jobnum, phsnum, trnnum, dscrpt, trndte, entdte, actprd, "
                                                               + "srcnum, status, bllsts, cstcde, csttyp, cstamt, blgamt, ovrrde, usrnme ) "
                                                               + "SELECT (select MAX(recnum +1) FROM jobcst) as recnum, jobnum, phsnum, trnnum, dscrpt, trndte, entdte, "
                                                               + "actprd, srcnum, status, bllsts, cstcde, csttyp, cstamt, blgamt, ovrrde, usrnme"
                                                               + " FROM {0}", TaxJobCosts);

                        Env.Log("{0} fields inserted in table jobcst", fldCount);
                    }
                }
                //Set null on again
                SetNullOn(con);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="jobNumber"></param>
        /// <param name="jobPhase"></para
        /// <param name="costCode"></param>
        public void ConsolidateJobCost(DateTime startDate, DateTime endDate, long jobNumber, long jobPhase, int costCode)
        {
            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                using 
                    (
                        Env.TempDBFPointer
                        ActiveJobCosts = con.GetTempDBF(),
                        MatCosts = con.GetTempDBF(),
                        SubMatCosts = con.GetTempDBF(),
                        NewMatCosts = con.GetTempDBF(),
                        NewSubMatCosts = con.GetTempDBF()
                    )
                {
                    Env.Log("Job cost consolidation started for job number: {0}", jobNumber);

                    jobPhase = 0;

                    //Get list of active job cost records to be billed
                    con.ExecuteNonQuery("SELECT * FROM jobcst WHERE jobnum = {0} AND phsnum = {1} AND status = 1 AND bllsts = 1 INTO TABLE {2}",
                                            jobNumber, jobPhase, ActiveJobCosts);

                    //Get the list of Material Records to be combined by cost type
                    con.ExecuteNonQuery("SELECT * FROM {0} WHERE csttyp = 1 INTO TABLE {1}", ActiveJobCosts, MatCosts);

                    //Get the list of Subcontract Material Records to be combined by cost type
                    con.ExecuteNonQuery("SELECT * FROM {0} WHERE csttyp = 7 INTO TABLE {1}", ActiveJobCosts, SubMatCosts);

                    //
                    DataTable _matCosts = con.GetDataTable("MatCosts", "Select * from {0}", MatCosts);
                    string MatCostDetail = "The following job cost records have been consolidated into a single billing record:" + "|";

                    foreach (DataRow dr in _matCosts.Rows)
                    {
                        MatCostDetail = MatCostDetail + Convert.ToString(dr["recnum"]).PadLeft(7) + "|";
                    }

                    Env.Log("Material cost detail memo: {0}", MatCostDetail);

                    DataTable _subMatCosts = con.GetDataTable("MatCosts", "Select * from {0}", SubMatCosts);
                    string SubMatDetail = "The following job cost records have been consolidated into a single billing record:" + "|";

                    foreach (DataRow dr in _subMatCosts.Rows)
                    {
                        SubMatDetail = SubMatDetail + Convert.ToString(dr["recnum"]).PadLeft(7) + "|";
                    }
                    Env.Log("Sub-material cost detail memo: {0}", SubMatDetail);

                    //Combine the material costs into a single record for appending to the actual job costs
                    int matCostCount = con.GetScalar<int>("select count(*) from {0}", MatCosts);
                    if (matCostCount > 0)
                    {
                        string formattedED = string.Format("'{0} {1}'", endDate.ToString("MM/dd/yy"), "Mat");

                        con.ExecuteNonQuery("SELECT {0} as jobnum, {1} as phsnum, {2} as trnnum, \"Materials\" as dscrpt, {3} as trndte,"
                                            + "{4} as entdte, MAX(actprd) as actprd, 31 as srcnum, 1 as status, 1 as bllsts,"
                                            + "{5} as cstcde, 1 as csttyp, SUM(cstamt) as blgamt, 1 as ovrrde, \"Combine\" as usrnme "
                                            + "FROM {6} INTO TABLE {7}",
                                            jobNumber, jobPhase, formattedED, endDate.ToFoxproDate(), DateTime.Today.ToFoxproDate(), costCode, MatCosts, NewMatCosts);
                    }

                    int subMatCostCount = con.GetScalar<int>("select count(*) from {0}", SubMatCosts);
                    if (subMatCostCount > 0)
                    {
                        string formattedED = string.Format("'{0} {1}'", endDate.ToString("MM/dd/yy"), "SubMat");

                        con.ExecuteNonQuery("SELECT {0} as jobnum, {1} as phsnum, {2} as trnnum, \"Subcontract Materials\" as dscrpt, "
                                                    + "{3} as trndte, {4} as entdte, MAX(actprd) as actprd, 31 as srcnum, 1 as status, 1 as bllsts, "
			                                        + "{5} as cstcde, 7 as csttyp, SUM(cstamt) as blgamt, 1 as ovrrde, \"Combine\" as usrnme "
                                                    + "FROM {6} INTO TABLE {7}",
                                                    jobNumber, jobPhase, formattedED, endDate.ToFoxproDate(), DateTime.Today.ToFoxproDate(), costCode, SubMatCosts, NewSubMatCosts);
                    }

                    SetNullOff(con);
                    int modifiedFldCount = 0;

                    //Execute the updates
                    //Add the records
                    if (matCostCount > 0)
                    {
                        modifiedFldCount = con.ExecuteNonQuery("INSERT INTO jobcst ( recnum, wrkord, jobnum, phsnum, trnnum, dscrpt, trndte, entdte, actprd, srcnum, "
                                                                + "status, bllsts, cstcde, csttyp, blgamt, ovrrde, usrnme, ntetxt ) "
                                                                + "SELECT (select MAX(recnum + 1) recnum FROM jobcst), '   ' as wrkord, "
                                                                + "_NewMatCosts.*, \"{0}\" as ntetxt FROM {1} _NewMatCosts", MatCostDetail, NewMatCosts);
                        Env.Log("Inserted {0} material cost records in jobcst table.", modifiedFldCount);
                    }
                    
                    if (subMatCostCount > 0)
                    {
                        modifiedFldCount = con.ExecuteNonQuery("INSERT INTO jobcst ( recnum, wrkord, jobnum, phsnum, trnnum, dscrpt, trndte, entdte, actprd, srcnum, status, "
                                                                + "bllsts, cstcde, csttyp, blgamt, ovrrde, usrnme, ntetxt ) "
                                                                + "SELECT (select MAX(recnum + 1) recnum FROM jobcst), '   ' as wrkord, "
                                                                + "_NewSubMatCosts.*, \"{0}\" FROM {1} _NewSubMatCosts", SubMatDetail, NewSubMatCosts);
                        Env.Log("Inserted {0} sub-material cost records in jobcst table.", modifiedFldCount);
                    }

                    //Finally, set all the billing status to non-billable for materials and subcontract materiasl
                    modifiedFldCount = con.ExecuteNonQuery("UPDATE jobcst SET jobcst.bllsts = 2 from {0} _ActiveJobCosts WHERE jobcst.recnum = _ActiveJobCosts.recnum "
	                                    + "AND INLIST(_ActiveJobCosts.csttyp,1,7)", ActiveJobCosts);
                    Env.Log("Updated billing status of {0} records in jobcst table.", modifiedFldCount);
                }

                Env.Log("--------------------------------------------------------------------------------\n");
            }
        }

        /// <summary>
        /// This application consolidates all job cost records that have been invoiced through T&M.
        /// </summary>
        /// <param name="startDate"></param>
        /// <param name="endDate"></param>
        /// <param name="jobNumber"></param>
        /// <param name="jobPhase"></param>
        public void UpdateTMTJobCost(DateTime startDate, DateTime endDate, long jobNumber, long jobPhase)
        {
            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                using 
                    (
                        Env.TempDBFPointer
                        CombinedCosts = con.GetTempDBF(),
                        RecordList = con.GetTempDBF(),
                        ACRInvList = con.GetTempDBF()
                    )
                {

                    //Find all the consolidated job costs in the selected time period
                    //That have been invoiced through T&M
                    int fldCount = con.ExecuteNonQuery("SELECT * FROM jobcst WHERE status = 1 AND acrinv > 0 AND BETWEEN(trndte,{0},{1}) "
                                                    + "AND usrnme == \"Combine\" into table {2}", startDate.ToFoxproDate(), endDate.ToFoxproDate(), CombinedCosts);

                    //Build a list of the job cost records that must be updated with the AR T&M invoice number
                    con.ExecuteNonQuery("create table {0} (recnum		n(10,0), acrinv		n(10,0))", RecordList);                    

                    //Read each record from the memo fields of the Combined Costs
                    DataTable _combinedCosts = con.GetDataTable("CombinedCosts", "SELECT * FROM {0}", CombinedCosts);
                    foreach (DataRow dr in _combinedCosts.Rows)
                    {
                        string nteTxt = (dr != null) ? (string)dr["ntetxt"] : string.Empty;
                        char[] delimiterChars = { '\n', '|' };
                        string[] nteTxts = nteTxt.Split(delimiterChars);

                        if (nteTxts.Length >= 2)
                        {
                            for (int i = 1; i <= nteTxts.Length; i++)
                            {
                                double recNum = 0.0;
                                if (double.TryParse(nteTxts[i], out recNum))
                                {
                                    con.ExecuteNonQuery("INSERT INTO {0} VALUES (VAL({1}), {2}", RecordList, recNum, dr["acrinv"]);
                                }
                            }
                        }
                    }

                    fldCount = con.ExecuteNonQuery("SELECT distinct acrinv FROM {0} INTO table {1}", CombinedCosts, ACRInvList);

                    //Reset all the records to blank 
                    fldCount = con.ExecuteNonQuery("UPDATE jobcst SET pieces = 0 from {0} _ACRInvList WHERE jobcst.acrinv = _ACRInvList.acrinv", ACRInvList);

                    //Now update the job cost records from the standardn, non-Combined records - excluding combined records
                    fldCount = con.ExecuteNonQuery("UPDATE jobcst SET pieces = _ACRInvList.acrinv from {0} _ACRInvList WHERE jobcst.acrinv = _ACRInvList.acrinv "
                                                    + "AND jobcst.usrnme <> \"Combine\"", ACRInvList);

                    //Update the combined records
                    fldCount = con.ExecuteNonQuery("UPDATE jobcst SET pieces = _RecordList.acrinv from {0} _RecordList WHERE _RecordList.recnum = jobcst.recnum", RecordList);
                }
                //Set default null setting to on again
                SetNullOn(con);
            }
        }


        #region Private Methods

        private void SetNullOn(OleDbConnection connection)
        {
            //Set this so that FoxPro doesn't try to insert null values in empty columns
            System.Data.OleDb.OleDbCommand dbCmdNull = connection.CreateCommand();
            dbCmdNull.CommandText = "SET NULL ON";
            dbCmdNull.ExecuteNonQuery();
        }

        private void SetNullOff(OleDbConnection connection)
        {
            //Set this so that FoxPro doesn't try to insert null values in empty columns
            System.Data.OleDb.OleDbCommand dbCmdNull = connection.CreateCommand();
            dbCmdNull.CommandText = "SET NULL OFF";
            dbCmdNull.ExecuteNonQuery();
        }

        private void FillTaxRates(double[] taxRates, string taxDetail)
        {
            double baseRate = 0.0;

            try
            {
                if (string.IsNullOrEmpty(taxDetail))
                    return;

                //Parse the taxDetails to get the tax rates
                string[] taxDetails = taxDetail.Split('|');
                if (taxDetails.Length > 0 && taxDetails.Length < 13)
                {
                    //Format incorrect
                    throw new InvalidOperationException("The format of data in memo field taxdst.ntetxt is invalid .");
                }

                //Get base rate
                string[] baseRates = taxDetails[1].Split('=');
                if (baseRates.Length == 2)
                {
                    double.TryParse(baseRates[1].Trim(' ', ']'), out baseRate);
                }

                for (int i = 3, j = 0; i < 12; i++, j++)
                {
                    double val = 0.0;
                    string[] rates = taxDetails[i].Split('=');
                    if (rates.Length == 2 && (!string.IsNullOrEmpty(rates[1].Trim())))
                    {
                        double.TryParse(rates[1].Trim(), out val);
                        taxRates[j] = baseRate * (val / 100);
                    }
                }
            }
            catch (Exception ex)
            {
                Env.Log("Error parsing tax rate info from table taxdst. Exception : {0}", ex.Message);
            }
        }

        #endregion

    }
}
