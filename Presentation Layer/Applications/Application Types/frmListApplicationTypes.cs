using DVLD.Classes;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using iText.Kernel.Colors;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications
{
    public partial class frmListApplicationTypes : Form
    {
        private  DataTable _dtAllApplicationTypes;

        public frmListApplicationTypes()
        {
            InitializeComponent();
        }
        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }
        void _FilldgvApplicationTypes(int NumberOfPage )
        {
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
            _dtAllApplicationTypes = clsApplicationType.GetApplicationTypesBy(ctrlSwitchSearch1.NumberOfPage, 3);
            dgvApplicationTypes.DataSource = _dtAllApplicationTypes;
            lblRecordsCount.Text = dgvApplicationTypes.Rows.Count.ToString();
        }
        private void frmListApplicationTypes_Load(object sender, EventArgs e)
        {
         ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsApplicationType.GetNumberOfRows());
            _FilldgvApplicationTypes(1);
            lblWhenAppTypesListAreEmpty.Visible = dgvApplicationTypes.Rows.Count == 0;
            ctrlSwitchSearch1.Visible = !lblWhenAppTypesListAreEmpty.Visible;

            if (dgvApplicationTypes.Rows.Count>0)
            {
                dgvApplicationTypes.Columns[0].HeaderText = "ID";
                dgvApplicationTypes.Columns[1].HeaderText = "Title";
                dgvApplicationTypes.Columns[2].HeaderText = "Fees";
            }

            dgvApplicationTypes.EnableHeadersVisualStyles = false;
            dgvApplicationTypes.ColumnHeadersDefaultCellStyle.BackColor = System.Drawing.Color.Black;

            this.Region = clsGlobal.CornerForm(Width, Height);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision( clsEmployee.enPermision.EditApplicationType))
            {
                clsGlobal.PermisionMessage("EditApplicationType");
                return;
            }
            frmEditApplicationType frm = new frmEditApplicationType((int)dgvApplicationTypes.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
            frmListApplicationTypes_Load(null, null);

        }

        private void cmsApplicationTypes_Opening(object sender, CancelEventArgs e)
        {
           
        }

        private void dgvApplicationTypes_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvApplicationTypes(e.CurrentNumberOfPage);
        }
    }
}
