using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using SysconCommon.Accounting;
using SysconCommon.Common;
using SysconCommon.Algebras.DataTables;
using SysconCommon.Common.Environment;

namespace Syscon.JobCostManagementTool
{
    public partial class TMTypes : Form
    {
        public TMTypes()
        {
            InitializeComponent();
        }

        private DataTable Data = null;

        public IEnumerable<int> TimeAndMaterialTypes
        {
            get
            {
                try
                {
                    List<int> rv = new List<int>();

                    if (Data != null)
                    {
                        foreach (var r in Data.Rows.ToIEnumerable())
                        {
                            if ((bool)r[2])
                                rv.Add((int)r[1]);
                        }

                        return rv;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
        }

        private void TMTypes_Load(object sender, EventArgs e)
        {
            try
            {
                var types = Accounting.GetJobTypes();

                var _selected = Env.GetConfigVar("tmtypes", "0", true).Split(',').Where(c => c != "");
                var selected = _selected.Select(c => decimal.Parse(c));
                Data = (from type in types
                        select new
                        {
                            TimeAndMaterials = selected.Contains(type.Recnum),
                            JobType = type.Recnum,
                            Description = type.Name,
                        }).ToDataTable("types");

                this.dataGridView1.DataSource = Data;
                var ctemplate = new DataGridViewCheckBoxCell();
                this.dataGridView1.Columns["TimeAndMaterials"].CellTemplate = ctemplate;
                this.dataGridView1.Columns["Description"].AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                this.dataGridView1.AutoResizeColumns();

                this.Deactivate += new EventHandler(TMTypeSelector_Deactivate);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void TMTypeSelector_Deactivate(object sender, EventArgs e)
        {
            this.dataGridView1.CurrentCell = null;
        }
    }
}

