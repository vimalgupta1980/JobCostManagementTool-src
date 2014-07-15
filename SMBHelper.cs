using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using SysconCommon;
using SysconCommon.Common;
using SysconCommon.Algebras.DataTables;

namespace Syscon.JobCostManagementTool
{
    public class SMBHelper
    {
        /// <summary>
        /// Returns the dataset version of currently connected database.
        /// The database name is with which the connection is made to.
        /// </summary>
        /// <returns></returns>
        public static decimal GetDataSetVersion()
        {
            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                decimal retVal = -1;

                string strSql = "SELECT VAL(vernum) FROM cmpany WHERE RECNO() = 1";
                try
                {
                    retVal = con.GetScalar<decimal>(strSql);
                }
                catch
                {
                    retVal = -1;
                    return retVal;
                }

                return retVal;
            }
        }

        /// <summary>
        /// *	Returns the requested GL Set up info the passed data set - 
        ///   Valid for version 19.x and greater
        ///*	Error Codes
        ///*	-1		S100C Data Directory reference invalid
        ///PARAMETERS mbdir, pName
        ///* Valid pNames
        ///*	CURRENTFISCALYEAR	- Returns the current fiscal year
        ///*	CURRENTPERIOD		- Returns the current accounting period that is open
        ///*	FISCALYEAREND		- Returns the fiscal year end
        ///*	ARCONTROL			- Returns the AR Control Account
        ///*	ARTAX				- Returns the AR Tax Liability Account
        ///*	SRCONTROL			- Returns the SR Control Account
        ///*	APCONTROL			- Returns the AP Control Account
        ///*	FIELDNAME			- Returns the result of evaluationg lgrset.fieldname */
        /// </summary>
        /// <returns></returns>
        public static int GetDataSetGLInfo(string pName)
        {
            using (var con = SysconCommon.Common.Environment.Connections.GetOLEDBConnection())
            {
                int retVal = 0;
                string sqlStr = string.Empty;
                pName = pName.Trim();

                try
                {
                    switch (pName)
                    {
                        case "CURRENTFISCALYEAR":
                            sqlStr = "SELECT YEAR(fscyrd) FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "CURRENTPERIOD":
                            sqlStr = "SELECT curprd FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "FISCALYEAREND":
                            sqlStr = "SELECT fscyrd FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "ARCONTROL":
                            sqlStr = "SELECT ded_ar FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "ARTAX":
                            sqlStr = "SELECT ar_tax FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "SRCONTROL":
                            sqlStr = "SELECT ded_sr FROM lgrset WHERE RECNO() = 1";
                            break;
                        case "APCONTROL":
                            sqlStr = "SELECT ded_ap FROM lgrset WHERE RECNO() = 1";
                            break;
                        default: 				//&& Try to evaluate the field name directly
                            sqlStr = string.Format("SELECT {0} FROM lgrset WHERE RECNO() = 1", pName);
                            break;
                    }
                    //IF ISNULL(laTemp(1,1))
                    //    RtnVal = -2
                    //ELSE
                    //    RtnVal = laTemp(1,1)
                    //ENDIF 
                    retVal = con.GetScalar<int>(sqlStr);
                }
                catch (Exception ex)
                {
                    retVal = -1;
                    return retVal;
                }

                return retVal;
            }
        }

        /// <summary>
        /// TODO:- Implement this when required
        /// </summary>
        public static void GLSetUpValue()
        {
            throw new NotImplementedException("This method is currently not implemented.");
        }

    }
}
