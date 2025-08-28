using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_Buisness;
using System;
using System.Windows.Forms;
using static DVLD_Buisness.clsLicense;

namespace DVLD.Applications.ReplaceLostOrDamagedLicense
{
    public partial class frmReplaceLostOrDamagedLicenseApplication : Form
    {
        private int? _NewLicenseID = null;
        clsApplication _CurrentApplication;


        public frmReplaceLostOrDamagedLicenseApplication()
        {
            InitializeComponent();
        }

        private int _GetApplicationTypeID()
        {
            //this will decide which application type to use accirding 
            // to user selection.

            if (rbDamagedLicens.Checked)
                return (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense;
            else
                return (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;
        }
        private enIssueReason _GetIssueReason()
        {
            //this will decide which reason to issue a replacement for
            
            if (rbDamagedLicens.Checked)

                return enIssueReason.DamagedReplacement;
            else
                return enIssueReason.LostReplacement; 
        }

        public void frmReplaceLostOrDamagedLicenseApplication_Load(int LicenseID, bool LocalLicense, bool ForLost)
        {
            if (LocalLicense)
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense;
            else
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.InternationalDrivingLicense;

            if (ForLost)
                rbLostLicense.Checked = true;
            else
                rbDamagedLicens.Checked = true;

            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            gbReplacementFor.Enabled = false;

            if (_CurrentApplication != null)
            {
                lblApplicationID.Text = _CurrentApplication.ApplicationID.ToString();
                llblShowApplicationInfo.Enabled = true;
            }

            //ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(LicenseID);
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;
          
           // rbDamagedLicens.Checked = true;

        }

        private void rbDamagedLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Damaged License";
            this.Text=lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void rbLostLicense_CheckedChanged(object sender, EventArgs e)
        {
            lblTitle.Text = "Replacement for Lost License";
            this.Text = lblTitle.Text;
            lblApplicationFees.Text = clsApplicationType.Find(_GetApplicationTypeID()).Fees.ToString();
        }

        private void frmReplaceLostOrDamagedLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }
        int _SelectedLicenseID;
        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
            _SelectedLicenseID = obj;
            lblOldLicenseID.Text = _SelectedLicenseID.ToString();
            llShowLicenseHistory.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1)
            {
                return;
            }

            bool IsActive;
            int? ActiveAppID;

            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense
                == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsActive;
                ActiveAppID = clsApplication.GetActiveApplicationID(clsLicense.Find(_SelectedLicenseID).ApplicationInfo.ApplicantPersonID,
                    (clsApplication.enApplicationType)_GetApplicationTypeID());
            }
            else
            {
                IsActive = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsActive;
                ActiveAppID = clsApplication.GetActiveApplicationID(clsInternationalLicense.Find(_SelectedLicenseID).ApplicationInfo.ApplicantPersonID,
                    (clsApplication.enApplicationType)_GetApplicationTypeID());
            }

            //dont allow a replacement if is not Active .
            if (!IsActive)
            {
                MessageBox.Show("Selected License is not Not Active, choose an active license."
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueReplacement.Enabled = false;
                return;
            }
            if (ActiveAppID.HasValue)
                _CurrentApplication = clsApplication.FindBaseApplication(ActiveAppID.Value);
            btnIssueReplacement.Enabled = true;
        }

        private void btnIssueReplacement_Click(object sender, EventArgs e)
        {
            if(!rbDamagedLicens.Checked && !rbLostLicense.Checked)
            {
                MessageBox.Show("You have to choice the reason to continue", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error );
                return;
            }
            if (MessageBox.Show("Are you sure you want to Issue a Replacement for the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            int? ApplicationID;
            bool IsCompleted;
            clsDetainedLicense.enLicenseType licenseType;
            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                licenseType = clsDetainedLicense.enLicenseType.Local;
                var NewLocalLicense =
                   ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.Replace(_GetIssueReason(),
                   clsGlobal.CurrentEmployee.EmployeeID ?? -1);

                IsCompleted = (NewLocalLicense.LicenseInfo != null && NewLocalLicense.ApplicationID.HasValue);
                ApplicationID = NewLocalLicense.ApplicationID;
                if(NewLocalLicense.LicenseInfo!=null)
                    _NewLicenseID = NewLocalLicense.LicenseInfo.LicenseID.Value;

            }
            else
            {
                licenseType = clsDetainedLicense.enLicenseType.International;
                var NewintLicense = 
                    ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.Replace(_GetIssueReason(),
                    clsGlobal.CurrentEmployee.EmployeeID ?? -1);

                IsCompleted = (NewintLicense.IntLicenseInfo != null && NewintLicense.ApplicationID.HasValue);
                ApplicationID = NewintLicense.ApplicationID;
                if (NewintLicense.IntLicenseInfo != null)
                    _NewLicenseID = NewintLicense.IntLicenseInfo.InternationalLicenseID.Value;
            }

            lblApplicationID.Text = ApplicationID.HasValue ? ApplicationID.Value.ToString() : "[???]";
            llblShowApplicationInfo.Enabled = ApplicationID.HasValue;

            if (!IsCompleted)
            {
                MessageBox.Show("Faild to Issue a replacemnet for this  License,\nCheck the User`s application info, He has to pay it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            int UserID = clsUser.FindByPersonID
                (clsApplication.FindBaseApplication(ApplicationID.Value).ApplicantPersonID)
                .UserID.Value;

            clsUtil.DeleteLicensePDF(_SelectedLicenseID, licenseType);

            clsUtil.ChangeOrderOfApplicationsToPaidFor(UserID, _GetApplicationTypeID(),
                  clsUtil.enOperationOrder.delete);
           
            lblRreplacedLicenseID.Text = _NewLicenseID.HasValue ? _NewLicenseID.Value.ToString() : "[???]";

            if (MessageBox.Show("Licensed Replaced Successfully with ID=" + _NewLicenseID.ToString(),
                 "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
                {
                    clsInternationalLicense internationalLicense =
                    clsInternationalLicense.FindByLocalLicenseID(_SelectedLicenseID);

                    if (internationalLicense != null)
                    {
                        internationalLicense.IssuedUsingLocalLicenseID = _NewLicenseID.Value;
                        if (internationalLicense.Save())
                        {
                            MessageBox.Show($"You have an international license and we refresh the data,\nto set this new license ID[{_NewLicenseID}] instead of {_SelectedLicenseID}", "Note!"
                                 , MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }


            btnIssueReplacement.Enabled = false;
            gbReplacementFor.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;
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
                frm = new frmShowLicenseInfo(_NewLicenseID.Value);
            else
                frm = new frmShowInternationalLicenseInfo(_NewLicenseID.Value);

            frm.ShowDialog();
        }

        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void llblApplicationInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowApplicationDetails))
                clsGlobal.PermisionMessage("ShowApplicationDetails");

            int? AppID = null;
            if (lblApplicationID.Text != "[???]")
            {
                AppID = Convert.ToInt32(lblApplicationID.Text);
            }
            
            if(AppID.HasValue)
            {
                frmApplicationBasicInfo applicationBasicInfo
                = new frmApplicationBasicInfo(AppID.Value);
                applicationBasicInfo.ShowDialog();
            }
            
        }
    }
}
