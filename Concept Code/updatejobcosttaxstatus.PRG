* UpdateJobCostTaxStatus
* 8/5/14 (law) Concept code to update the job cost tax status based on the tax
* district of a job. 
* This should be added as a final step each time the "Combine Job Costs for Billing" 
* scan is completed.

CLOSE TABLES
CLEAR


datadir = "f:\mb7\sample company v19\"

USE (datadir + "actrec") SHARED IN 0
USE (datadir + "taxdst") SHARED IN 0
USE (datadir + "jobcst") SHARED IN 0


targetjob = 225			
startdate = DATE(2011,1,1)
enddate = DATE(2014,12,31)

* Check the tax status of all unbilled job cost records that are marked to be
* manually overridden for the billing total

* Get the appropriate tax district information
SELECT taxdst.* FROM taxdst JOIN actrec ;
	ON taxdst.recnum = actrec.slstax ;
	WHERE actrec.recnum = targetjob ;
	INTO CURSOR taxinfo READWRITE NOFILTER 

* If there is no matching tax district, do not continue
IF RECCOUNT('taxinfo') <> 1
	? 'No matching tax district set up in job.'
	RETURN 
ENDIF 

* Set up an array of the tax status for this tax district.
* The array position correlates to the cost type of the job
* cost record
SELECT taxinfo
GO TOP 
LOCAL ARRAY taxstatus(9)
taxstatus(1) = IIF(taxinfo.mattax = "Y",1,0)
taxstatus(2) = IIF(taxinfo.labtax = "Y",1,0)
taxstatus(3) = IIF(taxinfo.eqptax = "Y",1,0)
taxstatus(4) = IIF(taxinfo.subtax = "Y",1,0)
taxstatus(5) = IIF(taxinfo.othtax = "Y",1,0)
taxstatus(6) = IIF(taxinfo.usrcs6 = "Y",1,0)
taxstatus(7) = IIF(taxinfo.usrcs7 = "Y",1,0)
taxstatus(8) = IIF(taxinfo.usrcs8 = "Y",1,0)
taxstatus(9) = IIF(taxinfo.usrcs9 = "Y",1,0)


* Update the taxable status of unbilled job cost records for this particular job
UPDATE jobcst SET taxabl = taxstatus(jobcst.csttyp) ;
	WHERE jobcst.jobnum = targetjob ;
		AND jobcst.ovrrde = 1 ;
		AND jobcst.bllsts = 1 ;
		AND BETWEEN(jobcst.trndte,startdate,enddate)

?'Processing complete'
