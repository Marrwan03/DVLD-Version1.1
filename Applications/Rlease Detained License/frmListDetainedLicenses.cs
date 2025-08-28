using DVLD.Applications.Detain_License;
using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.Remoting.Lifetime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Rlease_Detained_License
{
    public partial class frmListDetainedLicenses : Form
    {

        private DataTable _dtDetainedLicenses;

        public frmListDetainedLicenses()
        {
            InitializeComponent();
        }
        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        void _FilldgvDetainedLicenses(int NumberOfPage)
        {
            _dtDetainedLicenses = clsDetainedLicense.GetDetainedLicensesBy(NumberOfPage,6);
            cbFilterBy.SelectedIndex = 0;

            dgvDetainedLicenses.DataSource = _dtDetainedLicenses;
            lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
        }

        private void frmListDetainedLicenses_Load(object sender, EventArgs e)
        {
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsDetainedLicense.GetNumberOfRows(), 6);
            ctrlSwitchSearch1.NumberOfPage = 1;
            _FilldgvDetainedLicenses(ctrlSwitchSearch1.NumberOfPage);

            if (dgvDetainedLicenses.Rows.Count > 0)
            {
                dgvDetainedLicenses.Columns[0].HeaderText = "D.ID";
                 dgvDetainedLicenses.Columns[0].Width = 60;

                dgvDetainedLicenses.Columns[1].HeaderText = "Act.License.ID";
                dgvDetainedLicenses.Columns[1].Width = 100;

                dgvDetainedLicenses.Columns[2].HeaderText = "License Type";
                dgvDetainedLicenses.Columns[2].Width = 90;

                dgvDetainedLicenses.Columns[3].HeaderText = "D.Date";
                 dgvDetainedLicenses.Columns[3].Width = 160;
                
                dgvDetainedLicenses.Columns[4].HeaderText = "Is Released";
                 dgvDetainedLicenses.Columns[4].Width = 90;

                dgvDetainedLicenses.Columns[5].HeaderText = "Fine Fees";
                 dgvDetainedLicenses.Columns[5].Width = 90;
                
                dgvDetainedLicenses.Columns[6].HeaderText = "R.Date";
                dgvDetainedLicenses.Columns[6].Width = 160;

                dgvDetainedLicenses.Columns[7].HeaderText = "N.No.";
                 dgvDetainedLicenses.Columns[7].Width = 90;

                dgvDetainedLicenses.Columns[8].HeaderText = "Full Name";
                 dgvDetainedLicenses.Columns[8].Width = 330;

                dgvDetainedLicenses.Columns[9].HeaderText = "Rlease App.ID";
                dgvDetainedLicenses.Columns[9].Width = 150;

            }
            dgvDetainedLicenses.EnableHeadersVisualStyles = false;
            dgvDetainedLicenses.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;

            this.Region = clsGlobal.CornerForm(Width, Height);

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Detain ID":
                    FilterColumn = "DetainID";
                    break;
                case "Is Released":
                    {
                        FilterColumn = "IsReleased";
                        break;
                    };

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;


                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                case "Release Application ID":
                    FilterColumn = "ReleaseApplicationID";
                    break;

                default:
                    FilterColumn = "None";
                    break;
            }


            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                ctrlSwitchSearch1.Visible = true;
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                return;
            }

            ctrlSwitchSearch1.Visible = false;
            if (FilterColumn == "DetainID" || FilterColumn == "ReleaseApplicationID")
                //in this case we deal with numbers not string.
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblTotalRecords.Text = _dtDetainedLicenses.Rows.Count.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbFilterBy.Text == "Is Released")
            {
                ctrlSwitchSearch1.Visible = false;
                txtFilterValue.Visible = false;
                cbIsReleased.Visible = true;
                cbIsReleased.Focus();
                cbIsReleased.SelectedIndex = 0;
            }

            else

            {
                txtFilterValue.Visible = (cbFilterBy.Text != "None");
                cbIsReleased.Visible = (cbFilterBy.Text == "None");
                cbIsReleased.Visible = false;

                if (cbFilterBy.Text == "None")
                {
                    txtFilterValue.Enabled = false;                  
                }
                else
                    txtFilterValue.Enabled = true;


                if(_dtDetainedLicenses != null)
                {
                    _dtDetainedLicenses.DefaultView.RowFilter = "";
                    lblTotalRecords.Text = dgvDetainedLicenses.Rows.Count.ToString();
                }
               

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void cbIsReleased_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterColumn = "IsReleased";
            string FilterValue = cbIsReleased.Text;

            switch (FilterValue)
            {
                case "All":
                    break;
                case "Yes":
                    FilterValue = "1";
                    break;
                case "No":
                    FilterValue = "0";
                    break;
            }


            if (FilterValue == "All")
            {
                _dtDetainedLicenses.DefaultView.RowFilter = "";
                ctrlSwitchSearch1.Visible = true;
            }
            else
            {
                //in this case we deal with numbers not string.
                _dtDetainedLicenses.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, FilterValue);
                ctrlSwitchSearch1.Visible = false;
            }

            lblTotalRecords.Text = _dtDetainedLicenses.Rows.Count.ToString();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Detain ID" || cbFilterBy.Text == "Release Application ID" )
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void showPersonLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonLicenseHistory))
            {
                clsGlobal.PermisionMessage("ShowPersonLicenseHistory");
                return;
            }
            int ActiveLicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            string TypeOfLicense = dgvDetainedLicenses.CurrentRow.Cells[2].Value.ToString();
            int PersonID;

            if (TypeOfLicense == "LicenseID")
                PersonID = clsLicense.Find(ActiveLicenseID).DriverInfo.PersonID;
            else
                PersonID = clsInternationalLicense.Find(ActiveLicenseID).DriverInfo.PersonID;

            frmShowPersonLicenseHistory frm = new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void PesonDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonDetails))
            {
                clsGlobal.PermisionMessage("ShowPersonDetails");
                return;
            }
            int ActiveLicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            string TypeOfLicense = dgvDetainedLicenses.CurrentRow.Cells[2].Value.ToString();
            int PersonID;

            if (TypeOfLicense == "LicenseID")
                PersonID = clsLicense.Find(ActiveLicenseID).DriverInfo.PersonID;
            else
                PersonID = clsInternationalLicense.Find(ActiveLicenseID).DriverInfo.PersonID;

            frmShowPersonInfo frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseLDL))
            {
                clsGlobal.PermisionMessage("ShowLicenseLDL");
                return;
            }
            int ActiveLicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            string TypeOfLicense = dgvDetainedLicenses.CurrentRow.Cells[2].Value.ToString();
            Form frm;
            if(TypeOfLicense == "LicenseID")
            {
                frm = new frmShowLicenseInfo(ActiveLicenseID);
            }
            else
            {
                frm = new frmShowInternationalLicenseInfo(ActiveLicenseID);
            }
           
            
            frm.ShowDialog();

        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DetainLicense))
            {
                clsGlobal.PermisionMessage("DetainLicense");
                return;
            }
            frmDetainLicenseApplication frm= new frmDetainLicenseApplication(); 
            frm.ShowDialog();
            //refresh
            frmListDetainedLicenses_Load(null,null);

        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReleaseDetainedLicense))
            {
                clsGlobal.PermisionMessage("ReleaseDetainedLicense");
                return;
            }
            frmReleaseDetainedLicenseApplication frm= new frmReleaseDetainedLicenseApplication();   
            frm.ShowDialog();
            //refresh
            frmListDetainedLicenses_Load(null, null);

        }

        private void releaseDetainedLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee .enPermision.ReleaseDetainedLicense))
            {
                clsGlobal.PermisionMessage("ReleaseDetainedLicense");
                return;
            }
            int ActiveLicenseID = (int)dgvDetainedLicenses.CurrentRow.Cells[1].Value;
            string TypeOfLicense = dgvDetainedLicenses.CurrentRow.Cells[2].Value.ToString();
            Form frm;
            if (TypeOfLicense == "LicenseID")
            {
                frm = new frmReleaseDetainedLicenseApplication(ActiveLicenseID, Licenses.Controls.ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense);
            }
            else
            {
                frm = new frmReleaseDetainedLicenseApplication(ActiveLicenseID,  Licenses.Controls.ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.InternationalDrivingLicense);
            }


            frm.ShowDialog();
            //refresh
            frmListDetainedLicenses_Load(null, null);



        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {
            releaseDetainedLicenseToolStripMenuItem.Enabled = !(bool)dgvDetainedLicenses.CurrentRow.Cells[4].Value;
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvDetainedLicenses(e.CurrentNumberOfPage);
        }

        private void ctrlSwitchSearch1_ChangePageToRight(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvDetainedLicenses(e.CurrentNumberOfPage);
        }
    }
}

       
    
