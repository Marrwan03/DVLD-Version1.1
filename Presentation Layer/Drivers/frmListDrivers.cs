using DVLD.Classes;
using DVLD.Licenses.International_License;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Drivers
{
    public partial class frmListDrivers : Form
    {
        private DataTable _dtAllDrivers;

        public frmListDrivers()
        {
            InitializeComponent();
        }
        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();

        }

        void _FilldgvDrivers(int NumberOfRows)
        {
            ctrlSwitchSearch1.NumberOfPage = NumberOfRows;
            _dtAllDrivers = clsDriver.GetDriverBy(ctrlSwitchSearch1.NumberOfPage, 3);
            dgvDrivers.DataSource = _dtAllDrivers;
            lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();

        }

        private void frmListDrivers_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            cbFilterBy.SelectedIndex = 0;
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsDriver.GetNumberOfRows());
            _FilldgvDrivers(1);
            lblWhenDriverListIsEmpty.Visible = dgvDrivers.Rows.Count == 0;
            ctrlSwitchSearch1.Visible = !lblWhenDriverListIsEmpty.Visible;
            if (dgvDrivers.Rows.Count>0)
            {
                dgvDrivers.Columns[0].HeaderText = "Driver ID";
                dgvDrivers.Columns[1].HeaderText = "Person ID";
                dgvDrivers.Columns[2].HeaderText = "National No.";
                dgvDrivers.Columns[3].HeaderText = "Full Name";
                dgvDrivers.Columns[4].HeaderText = "Date";
                dgvDrivers.Columns[5].HeaderText = "Active Licenses";
            }
            dgvDrivers.EnableHeadersVisualStyles = false;
            dgvDrivers.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;


        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Visible = (cbFilterBy.Text != "None");

            if (cbFilterBy.Text == "None")
            {
                txtFilterValue.Enabled = false;
            }
            else
                txtFilterValue.Enabled = true;

            txtFilterValue.Text = "";
            txtFilterValue.Focus();
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Driver ID":
                    FilterColumn = "DriverID";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                ctrlSwitchSearch1.Visible = true;
                ctrlSwitchSearch1.NumberOfPage = 1;
                _dtAllDrivers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvDrivers.Rows.Count.ToString();
                return;
            }
            ctrlSwitchSearch1.Visible = false;
            if (FilterColumn != "FullName" && FilterColumn != "NationalNo")
                //in this case we deal with numbers not string.
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllDrivers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = _dtAllDrivers.DefaultView.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Driver ID" || cbFilterBy.Text == "Person ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonDetails))
            {
                clsGlobal.PermisionMessage("ShowPersonDetails");
                return;
            }

            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;
            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
            //refresh
            frmListDrivers_Load(null, null);

        }


        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonLicenseHistory))
            {
                clsGlobal.PermisionMessage("ShowPersonLicenseHistory");
                return;
            }
            int PersonID = (int)dgvDrivers.CurrentRow.Cells[1].Value;

          
            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvDrivers(e.CurrentNumberOfPage);
        }
    }
}
