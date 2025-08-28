using DVLD.Applications;
using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_Buisness;
using System;
using System.Windows.Forms;
using static DVLD_Buisness.clsApplication;

namespace DVLD.Licenses
{
    public partial class frmRenewLocalDrivingLicenseApplication: Form
    {
        private int? _NewLicenseID=null;
        clsApplication _CurrentApplication;
        public frmRenewLocalDrivingLicenseApplication()
        {
            InitializeComponent();
            
        }

        public void frmRenewLocalDrivingLicenseApplication_Load(int? LicenseID, bool LocalLicense)
        {
            if (!LicenseID.HasValue)
            {
                MessageBox.Show("You cannot continue because LicenseID is null!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (LocalLicense)
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense;
            else
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.InternationalDrivingLicense;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID.Value);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            if(_CurrentApplication != null)
            {
                lblApplicationID.Text = _CurrentApplication.ApplicationID.ToString();
                llblShowApplicationInfo.Enabled = true;
            }
        }

        private void frmRenewLocalDrivingLicenseApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;

            lblExpirationDate.Text = "???";
            lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees.ToString();
            lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;

        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            int? SelectedLicenseID = obj;

            lblOldLicenseID.Text = SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (SelectedLicenseID.HasValue);

            if (!SelectedLicenseID.HasValue)

            {
                return;
            }

            int DefaultValidityLength;
            float ClassFees;
            string Notes;
            bool IsLicenseExpired, IsActive;
            DateTime ExpirationDate;
            int? ActiveRenamedAppID = null;

            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                DefaultValidityLength = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseClassInfo.DefaultValidityLength;
                ClassFees = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseClassInfo.ClassFees;
                Notes = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.Notes;
                ExpirationDate = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.ExpirationDate;
                IsLicenseExpired = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsLicenseExpired();
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsActive;
                ActiveRenamedAppID = clsApplication.GetActiveApplicationID
                (clsLicense.Find(SelectedLicenseID.Value).ApplicationInfo.ApplicantPersonID,
                 enApplicationType.RenewDrivingLicense);
            }
            else
            {
                DefaultValidityLength = 1;
                ClassFees = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.LocalLicense.LicenseClassInfo.ClassFees;
                Notes = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.LocalLicense.Notes;
                ExpirationDate = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.ExpirationDate;
                IsLicenseExpired = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsLicenseExpired();
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsActive;
                clsApplication.GetActiveApplicationID
                (clsInternationalLicense.Find(SelectedLicenseID.Value).ApplicationInfo.ApplicantPersonID,
                 enApplicationType.RenewDrivingLicense);
            }
  
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(DefaultValidityLength));
            lblLicenseFees.Text = ClassFees.ToString();
            lblTotalFees.Text = 
                (clsApplicationType.Find(
                    (int)clsApplication.enApplicationType.RenewDrivingLicense).Fees
                + Convert.ToSingle(lblLicenseFees.Text)).ToString();
            txtNotes.Text =Notes;
            _CurrentApplication = clsApplication.FindBaseApplication(ActiveRenamedAppID ?? -1);

            //check the license is not Expired.
            if (!IsLicenseExpired)
            {
                MessageBox.Show("Selected License is not yet expiared, it will expire on: " + clsFormat.DateToShort(ExpirationDate)    
                    ,"Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }

            //check the license is not Active.
            if (!IsActive)
            {
                MessageBox.Show("Selected License is  Not Active, choose an active license." 
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnRenewLicense.Enabled = false;
                return;
            }
            btnRenewLicense.Enabled = true;
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void btnRenewLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to Renew the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }
            bool IsCompleted;
            int OldLicenseID;
            int? ApplicationID;
            
            clsDetainedLicense.enLicenseType licenseType;

           if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                OldLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID.Value;
               
                var NewLocalLicense =
                ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.RenewLicense(txtNotes.Text.Trim(),
                clsGlobal.CurrentEmployee.EmployeeID ?? -1);
                IsCompleted = (NewLocalLicense.ApplicationID != null && NewLocalLicense.LicenseInfo != null);
                licenseType = clsDetainedLicense.enLicenseType.Local;
                ApplicationID = NewLocalLicense.ApplicationID;
                if(NewLocalLicense.LicenseInfo != null)
                    _NewLicenseID = NewLocalLicense.LicenseInfo.LicenseID;

            }
           else
            {
                OldLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.InternationalLicenseID.Value;
                licenseType = clsDetainedLicense.enLicenseType.International;

                var NewIntLicense =
                    ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.RenewLicense(clsGlobal.CurrentEmployee.EmployeeID ?? -1);

                IsCompleted = (NewIntLicense.IntLicenseInfo != null && NewIntLicense.ApplicationID != null);
                ApplicationID = NewIntLicense.ApplicationID;
                if(NewIntLicense.IntLicenseInfo != null)
                    _NewLicenseID = NewIntLicense.IntLicenseInfo.InternationalLicenseID;
            }

            lblApplicationID.Text = ApplicationID.HasValue ? ApplicationID.Value.ToString() : "[???]";
            llShowLicenseInfo.Enabled = ApplicationID.HasValue;

            if (!IsCompleted)
            {
                MessageBox.Show("Faild to Renew the License,\nCheck the application info, User has to pay it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int UserID = clsUser.FindByPersonID
                (clsApplication.FindBaseApplication(ApplicationID.Value).ApplicantPersonID)
                .UserID.Value;
            
            clsUtil.DeleteLicensePDF(OldLicenseID, licenseType);

            clsUtil.ChangeOrderOfApplicationsToPaidFor(UserID,
                (int)clsApplication.enApplicationType.RenewDrivingLicense,
                  clsUtil.enOperationOrder.delete);
            
            lblRenewedLicenseID.Text = _NewLicenseID.HasValue? _NewLicenseID.Value.ToString() : "[???]";
            MessageBox.Show("Licensed Renewed Successfully with ID=" + _NewLicenseID.ToString(), "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);
            llblShowApplicationInfo.Enabled = true;
            btnRenewLicense.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
        }

        private void frmRenewLocalDrivingLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();

        }

        private void llShowLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            int PersonID;
            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseLDL))
                {
                    clsGlobal.PermisionMessage("ShowLicenseLDL");
                    return;
                }
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverInfo.PersonID;
            }
            else
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseInt))
                {
                    clsGlobal.PermisionMessage("ShowLicenseInt");
                    return;
                }
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DriverInfo.PersonID;
            }

            frmShowPersonLicenseHistory frm =
               new frmShowPersonLicenseHistory(PersonID);
            frm.ShowDialog();
        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Form frm;
            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseLDL))
                {
                    clsGlobal.PermisionMessage("ShowLicenseLDL");
                    return;
                }
                frm = new frmShowLicenseInfo(_NewLicenseID.HasValue? _NewLicenseID.Value : -1);
            }
            else
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseInt))
                {
                    clsGlobal.PermisionMessage("ShowLicenseInt");
                    return;
                }
                frm = new frmShowInternationalLicenseInfo(_NewLicenseID.HasValue ? _NewLicenseID.Value : -1);
            }
            frm.ShowDialog();
        }
        public Action OnClosed;

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void llblShowApplicationInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowApplicationDetails))
            {
                clsGlobal.PermisionMessage("ShowApplicationDetails");
                return;
            }
           
            frmApplicationBasicInfo applicationBasicInfo = new frmApplicationBasicInfo(_CurrentApplication.ApplicationID);
            applicationBasicInfo.ShowDialog();
        }
    }
}
