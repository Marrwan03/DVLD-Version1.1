using DVLD.Controls;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD;
using DVLD.Licenses.International_Licenses;
using DVLD.DriverLicense;
using DVLD.Classes;
using DVLD_Buisness.Global_Classes;
using DVLD.User;
using DVLD.Payments;

namespace DVLD.Licenses.Local_Licenses.Controls
{
    public partial class ctrlDriverLicenses : UserControl
    {
        private int _DriverID;
        private clsDriver _Driver ;
        private DataTable _dtDriverLocalLicensesHistory;
        private DataTable _dtDriverInternationalLicensesHistory;

        public ctrlDriverLicenses()
        {
            InitializeComponent();
        }

        void _FilldgvLocalLicensesHistory(int NumberOfPage)
        {
            ctrlSwitchSearchForLocalLicense.NumberOfPage = NumberOfPage;
            _dtDriverLocalLicensesHistory = clsDriver.GetLicenses(_DriverID, ctrlSwitchSearchForLocalLicense.NumberOfPage, 4);
            dgvLocalLicensesHistory.DataSource = _dtDriverLocalLicensesHistory;
            lblLocalLicensesRecords.Text = dgvLocalLicensesHistory.Rows.Count.ToString();
        }

        private void _LoadLocalLicenseInfo()
        {
            ctrlSwitchSearchForLocalLicense.MaxNumberOfPage = clsGet.GetMaximamPage(clsLicense.GetNumberOfRowsForLocalLicenses(_DriverID), 4);
            _FilldgvLocalLicensesHistory(1);
            lblMessageWhenLLicenseIsEmpty.Visible = dgvLocalLicensesHistory.Rows.Count == 0;
            ctrlSwitchSearchForLocalLicense.Visible = !lblMessageWhenLLicenseIsEmpty.Visible;

            if (dgvLocalLicensesHistory.Rows.Count > 0)
            {
                dgvLocalLicensesHistory.Columns[0].HeaderText = "Lic.ID";
                dgvLocalLicensesHistory.Columns[1].HeaderText = "App.ID";
                dgvLocalLicensesHistory.Columns[2].HeaderText = "Class Name";
                dgvLocalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvLocalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvLocalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvLocalLicensesHistory.Columns[6].HeaderText = "Paid Fees";
            }
            dgvLocalLicensesHistory.EnableHeadersVisualStyles = false;
            dgvLocalLicensesHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        void _FilldgvInternationalLicensesHistory(int NumberOfPage)
        {
            ctrlSwitchSearchForIntLicense.NumberOfPage = NumberOfPage;
            _dtDriverInternationalLicensesHistory = clsDriver.GetInternationalLicenses(_DriverID, ctrlSwitchSearchForIntLicense.NumberOfPage, 4);
            dgvInternationalLicensesHistory.DataSource = _dtDriverInternationalLicensesHistory;
            lblInternationalLicensesRecords.Text = dgvInternationalLicensesHistory.Rows.Count.ToString();
        }

        private void _LoadInternationalLicenseInfo()
        {
            ctrlSwitchSearchForIntLicense.MaxNumberOfPage = clsGet.GetMaximamPage(clsInternationalLicense.GetNumberOfRowsForIntLicenses(_DriverID), 4);
            _FilldgvInternationalLicensesHistory(1);
            lblMessageWhenIntLicenseIsEmpty.Visible = dgvInternationalLicensesHistory.Rows.Count == 0;
            ctrlSwitchSearchForIntLicense.Visible = !lblMessageWhenIntLicenseIsEmpty.Visible;
            if (dgvInternationalLicensesHistory.Rows.Count > 0)
            {
                dgvInternationalLicensesHistory.Columns[0].HeaderText = "Int.License ID";
                dgvInternationalLicensesHistory.Columns[1].HeaderText = "Application ID";
                dgvInternationalLicensesHistory.Columns[2].HeaderText = "L.License ID";
                dgvInternationalLicensesHistory.Columns[3].HeaderText = "Issue Date";
                dgvInternationalLicensesHistory.Columns[4].HeaderText = "Expiration Date";
                dgvInternationalLicensesHistory.Columns[5].HeaderText = "Is Active";
                dgvInternationalLicensesHistory.Columns[6].HeaderText = "Paid Fees";
            }

            dgvInternationalLicensesHistory.EnableHeadersVisualStyles = false;
            dgvInternationalLicensesHistory.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        public void LoadInfo(int DriverID)
        {
            _DriverID = DriverID;
            _Driver = clsDriver.FindByDriverID(_DriverID);

            if(_Driver == null )
            {
                MessageBox.Show($"There isn`t driver with Driver ID = {_DriverID}.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _DriverID = -1;
                return;
            }


            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();

        }

        public void LoadInfoByPersonID(int PersonID)
        {
            
            _Driver = clsDriver.FindByPersonID(PersonID);
            if (_Driver == null)
            {
                MessageBox.Show($"There isn`t driver with Person ID = {PersonID}.", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _DriverID = _Driver.DriverID.Value;
            _LoadLocalLicenseInfo();
            _LoadInternationalLicenseInfo();
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsGlobal.CurrentEmployee != null && !clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseLDL))
            {
                clsGlobal.PermisionMessage("ShowLicenseLDL");
                return;
            }
            int LicenseID = (int)dgvLocalLicensesHistory.CurrentRow.Cells[0].Value;
            DriverLicense.frmShowLicenseInfo frm = new DriverLicense.frmShowLicenseInfo(LicenseID);
            //frmShowLicenseInfo frm = new frmShowLicenseInfo(LocalLicenseID);
            frm.ShowDialog();
            
        }

        public void Clear()
        {
            _dtDriverLocalLicensesHistory.Clear();
            _dtDriverInternationalLicensesHistory.Clear();
        }

        private void InternationalLicenseHistorytoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (clsGlobal.CurrentEmployee != null&&!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseInt))
            {
                clsGlobal.PermisionMessage("ShowLicenseInt");
                return;
            }
            int InternationalLicenseID = (int)dgvInternationalLicensesHistory.CurrentRow.Cells[0].Value;
            frmShowInternationalLicenseInfo frm = new frmShowInternationalLicenseInfo(InternationalLicenseID);
            frm.ShowDialog();
        }

        private void ctrlDriverLicenses_Load(object sender, EventArgs e)
        {

        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void ctrlSwitchSearchForLocalLicense_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvLocalLicensesHistory(e.CurrentNumberOfPage);
        }

        private void ctrlSwitchSearchForIntLicense_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvInternationalLicensesHistory(e.CurrentNumberOfPage);
        }

      

        private void cmsLocalLicenseHistory_Opening(object sender, CancelEventArgs e)
        {
            
        }

        private void cmsLocalLicenseHistory_Opening_1(object sender, CancelEventArgs e)
        {
            localLicenseInfoToolStripMenuItem.Enabled = dgvLocalLicensesHistory.Rows.Count > 0;
        }

        private void cmsInterenationalLicenseHistory_Opening(object sender, CancelEventArgs e)
        {
            InternationalLicenseHistorytoolStripMenuItem.Enabled = dgvInternationalLicensesHistory.Rows.Count > 0;
        }
    }
}
