using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Applications.Detain_License
{
    public partial class frmDetainLicenseApplication : Form
    {

        private int _DetainID = -1;
        private int _SelectedLicenseID = -1;
        public frmDetainLicenseApplication()
        {
            InitializeComponent();
        }

        private void btnDetain_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("You Must enter the fine fees firs!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFineFees.Focus();
                return;
            }


            if (MessageBox.Show("Are you sure you want to detain this license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int LicenseID;

            if(ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                LicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID.Value;
                clsUtil.DeleteLicensePDF(LicenseID, clsDetainedLicense.enLicenseType.Local);
                _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text),
                    clsGlobal.CurrentEmployee.EmployeeID ?? -1);
            }
            else
            {
                LicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.InternationalLicenseID.Value;
                clsUtil.DeleteLicensePDF(LicenseID, clsDetainedLicense.enLicenseType.International);
                _DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.Detain(Convert.ToSingle(txtFineFees.Text),
                    clsGlobal.CurrentEmployee.EmployeeID ?? -1);
            }

            
            if (_DetainID == -1)
            {
                MessageBox.Show("Faild to Detain License", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }
            
            lblDetainID.Text = _DetainID.ToString();
            MessageBox.Show("License Detained Successfully with ID=" + _DetainID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnDetain.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            txtFineFees.Enabled= false;
            llShowLicenseInfo.Enabled = true;
        }

        private void frmDetainLicenseApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            lblDetainDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;

        }
        public Action OnClosed;

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
             _SelectedLicenseID = obj;

            lblLicenseID.Text = _SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1)

            {
                return;
            }
            int ActiveLicenseID;
            bool IsDetained, IsActive;

            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                IsDetained = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsDetained;
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsActive;
                ActiveLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.GetActiveLicenseID();
            }
            else
            {
                IsDetained = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsDetained;
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsActive;
                ActiveLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.GetActiveInternationalLicenseID();
            }

                //ToDo: make sure the license is not detained already.
        if (IsDetained)
            {
                MessageBox.Show("Selected License already detained, choose another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnDetain.Enabled = false;
                return;
            }

          if(!IsActive)
            {
                MessageBox.Show($"This License {_SelectedLicenseID} isn`t Active,\n\nPlease Enter this license ID {ActiveLicenseID} to continue.", "About Active License!", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                btnDetain.Enabled = false;
                return;


            }

           txtFineFees.Focus();
           btnDetain.Enabled = true;
        }

        private void frmDetainLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {

            int PersonID;
            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverInfo.PersonID;
            else
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DriverInfo.PersonID;
            frmShowPersonLicenseHistory frm =
             new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm;
            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
                frm = new frmShowLicenseInfo(_SelectedLicenseID);
            else
                frm = new frmShowInternationalLicenseInfo(_SelectedLicenseID);
            frm.ShowDialog();
        }

        private void txtFineFees_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtFineFees.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Fees cannot be empty!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);

            };


            if (!clsValidatoin.IsNumber(txtFineFees.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFineFees, "Invalid Number.");
            }
            else
            {
                errorProvider1.SetError(txtFineFees, null);
            };
        }

        private void txtFineFees_KeyPress(object sender, KeyPressEventArgs e)
        {

        }
    }
}
