using DVLD.Applications;
using DVLD.Classes;
using DVLD.Licenses.International_Licenses;
using DVLD.Payments;
using DVLD.Tests;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using iText.Layout.Element;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD.User.Controls
{
    public partial class ctrlYourDrivingLicenseApplications : UserControl
    {
        public ctrlYourDrivingLicenseApplications()
        {
            InitializeComponent();
        }
        int _PersonID;
        DataTable _dtAllYourDrivingLicenseApplications;
        void _FilldgvYourLocalDrivingLicenseApplications(int PageNumber)
        {
            ctrlSwitchSearch1.NumberOfPage = PageNumber;
            _dtAllYourDrivingLicenseApplications = clsPerson.GetApplicationReportForAllTypesLicenseBy(_PersonID, ctrlSwitchSearch1.NumberOfPage, 8);
            lblNote.Visible = _dtAllYourDrivingLicenseApplications.Rows.Count == 0; 
            lblNumberOfRecord.Text = _dtAllYourDrivingLicenseApplications.Rows.Count.ToString();
            dgvYourAllDrivingLicenseApplications.DataSource = _dtAllYourDrivingLicenseApplications;
        }
        void _RefreshData()
        {
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsPerson.GetNumberOfAllTypesApplicationBy(_PersonID), 8);
            _FilldgvYourLocalDrivingLicenseApplications(1);
            if (dgvYourAllDrivingLicenseApplications.Rows.Count > 0)
            {
                dgvYourAllDrivingLicenseApplications.Columns[0].HeaderText = "App ID";
                dgvYourAllDrivingLicenseApplications.Columns[0].Width = 50;
                dgvYourAllDrivingLicenseApplications.Columns[1].HeaderText = "App Date";
                dgvYourAllDrivingLicenseApplications.Columns[2].HeaderText = "App Type";
                dgvYourAllDrivingLicenseApplications.Columns[3].HeaderText = "Last Status Date";
                dgvYourAllDrivingLicenseApplications.Columns[4].HeaderText = "Paid Fees";
                dgvYourAllDrivingLicenseApplications.Columns[4].Width = 75;
                dgvYourAllDrivingLicenseApplications.Columns[5].HeaderText = "Status";
                dgvYourAllDrivingLicenseApplications.Columns[5].Width = 85;
                dgvYourAllDrivingLicenseApplications.Columns[6].HeaderText = "Payment ID";
                dgvYourAllDrivingLicenseApplications.Columns[5].Width = 85;
                dgvYourAllDrivingLicenseApplications.Columns[7].HeaderText = "Create By";
            }
        }
        public void ctrlYourDrivingLicenseApplications_Load(int PersonID)
        {
            _PersonID = PersonID;
            _RefreshData();
            if (dgvYourAllDrivingLicenseApplications.Rows.Count == 0)
            {
                pFilter.Enabled = false;
                pSort.Enabled = false;
            }

            cbUserCards1.Fill_cbUserCardsBy(clsUser.FindByPersonID(PersonID).UserID.Value);
            if(cbUserCards1.Items.Count > 0)
            {
                cbUserCards1.SelectedIndex = 0;
            }
            else
            {
                payItToolStripMenuItem.Enabled = false;
                //picAddNewApplication.Enabled = false;
                cbUserCards1.Text = "Empty";
            }
            
        }

        private void ctrlYourDrivingLicenseApplications_Load(object sender, EventArgs e)
        {
            rbFilter.Checked = true;
            cbFilterBy.SelectedIndex = 0;
            dgvYourAllDrivingLicenseApplications.EnableHeadersVisualStyles = false;
            dgvYourAllDrivingLicenseApplications.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
        }


        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvYourLocalDrivingLicenseApplications(e.CurrentNumberOfPage);
        }

        private void showApplicationDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int AppID = (int)dgvYourAllDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            frmApplicationBasicInfo applicationBasicInfo = new frmApplicationBasicInfo(AppID);
            applicationBasicInfo.ShowDialog();
        }

        private void cmsYourLocalDrivingLicenseApplications_Opening(object sender, CancelEventArgs e)
        {
            payItToolStripMenuItem.Enabled = dgvYourAllDrivingLicenseApplications.CurrentRow.Cells[6].Value == DBNull.Value;
            showBillInfoToolStripMenuItem.Enabled = !payItToolStripMenuItem.Enabled;

        }

        void _VisibleControl(bool boolcbFilterBy, bool booltxtFilter, bool boolcbStatus)
        {
            cbFilterBy.Visible = boolcbFilterBy;
            txtFilter.Visible = booltxtFilter;
            cbStatus.Visible = boolcbStatus;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "None":
                    {
                        _VisibleControl(true, false, false);
                        txtFilter.Text = null;
                        break;
                    }
                case "App ID":
                    {
                        _VisibleControl(true, true, false);
                        break;
                    }
                default:
                    {
                        cbStatus.SelectedIndex = 0;
                        _VisibleControl(true, false, true);
                        break;
                    }
            };
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(cbFilterBy.Text == "App ID")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            if(cbFilterBy.Text == "None" || string.IsNullOrEmpty(txtFilter.Text))
            {
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = "";
                ctrlSwitchSearch1.Visible = true;
            }
            else
            {
                ctrlSwitchSearch1.Visible = false;
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = {1}", "ApplicationID", txtFilter.Text);
            }
            lblNumberOfRecord.Text = dgvYourAllDrivingLicenseApplications.Rows.Count.ToString();
            lblNote.Text = _dtAllYourDrivingLicenseApplications.Rows.Count.ToString();
        }

        private void cbSortBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Visiable = cbSortBy.Text == "All";
            ctrlSwitchSearch1.Visible = Visiable;
            if (Visiable)
            {
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = "";
                _FilldgvYourLocalDrivingLicenseApplications(1);
            }
            else
            {
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "ApplicationTypeTitle", cbSortBy.Text);
                lblNumberOfRecord.Text = _dtAllYourDrivingLicenseApplications.DefaultView.Count.ToString();
                lblNote.Visible = _dtAllYourDrivingLicenseApplications.Rows.Count == 0;
            }
            
        }

        private void rbFilter_CheckedChanged(object sender, EventArgs e)
        {
            
                pFilter.Visible = rbFilter.Checked;
                pSort.Visible = rbSort.Checked;
                if (pSort.Visible)
                    cbSortBy.SelectedIndex = 0;
        }

        private void cbStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Visiable = cbStatus.Text == "All";
            ctrlSwitchSearch1.Visible = Visiable;

            if(Visiable)
            {
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = "";
                _FilldgvYourLocalDrivingLicenseApplications(1);
            }
            else
            {
                _dtAllYourDrivingLicenseApplications.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "Status", cbStatus.Text);
                lblNumberOfRecord.Text = _dtAllYourDrivingLicenseApplications.DefaultView.Count.ToString();
                lblNote.Visible = _dtAllYourDrivingLicenseApplications.Rows.Count == 0;
            }

        }

        private void picAddNewApplication_Click(object sender, EventArgs e)
        {
            if (cbUserCards1.Items.Count == 0)
            {
                MessageBox.Show("You don`t have any creditcard to continue,\nyou should create new Account in Bank system", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (MessageBox.Show("Are you sure do you want Add New Application?", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            frmAddNewRequestForApp addNewOrderForApp = new frmAddNewRequestForApp(clsUser.FindByPersonID(_PersonID).UserID.Value);
           
            addNewOrderForApp.ShowDialog();
            _RefreshData();

        }

      

        private void payItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbUserCards1.Items.Count == -1)
            {
                MessageBox.Show("You don`t have any creditcard to continue,\nyou should create new Account in Bank system","Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int AppID = (int)dgvYourAllDrivingLicenseApplications.CurrentRow.Cells[0].Value;
            decimal fees = (decimal)dgvYourAllDrivingLicenseApplications.CurrentRow.Cells[4].Value;

            frmPaymentOf paymentOfApplication = new frmPaymentOf
                (AppID,
                (float)fees,
                Payments.Controls.ctrlPaymentFor.enPaymentFor.Application,
                clsCreditCard.GetTypeOfCreditCard(cbUserCards1.Text));
            paymentOfApplication.ShowDialog();
            _RefreshData();
        }

        private void cbUserCards1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void showBillInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PaymentID = (int)dgvYourAllDrivingLicenseApplications.CurrentRow.Cells[6].Value;
            frmPaymentInfo BillInfo = new frmPaymentInfo(PaymentID);
            BillInfo.ShowDialog();
        }
    }
}
