using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Applications.Rlease_Detained_License
{
    public partial class frmReleaseDetainedLicenseApplication : Form
    {
        clsApplication _CurrentApplication;
        private int _SelectedLicenseID = -1;
        float _FineFees;

        public frmReleaseDetainedLicenseApplication()
        {
            InitializeComponent();
        }

        public frmReleaseDetainedLicenseApplication(int ActiveLicenseID, ctrlDriverLicenseInfoWithFilter.enTypeOfLicense TypeOfLicense)
        {
            InitializeComponent();
            _SelectedLicenseID = ActiveLicenseID;

            ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = TypeOfLicense;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            // ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(ActiveLicenseID);
            if (_CurrentApplication != null)
            {
                lblApplicationID.Text = _CurrentApplication.ApplicationID.ToString();
                llblShowApplicationInfo.Enabled = true;
            }
        }
        public frmReleaseDetainedLicenseApplication(int ActiveLicenseID, bool LocalLicense)
        {
            InitializeComponent();
            _SelectedLicenseID = ActiveLicenseID;
            if (LocalLicense)
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense;
            else
                ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.InternationalDrivingLicense;
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(_SelectedLicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;

            if(_CurrentApplication != null)
            {
                lblApplicationID.Text = _CurrentApplication.ApplicationID.ToString();
                llblShowApplicationInfo.Enabled = true;
            }

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

            bool IsDetained;
            int DetainID, LicenseID;
            DateTime DetainDate;            
            int PersonID;

            if (ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                lblLicenseIDTitle.Text = "Local License ID:";
                IsDetained = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.IsDetained;
                DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DetainedInfo.DetainID;
                LicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID.Value;
                DetainDate = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DetainedInfo.DetainDate;
                _FineFees = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DetainedInfo.FineFees;
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverInfo.PersonID;
            }
            else
            {
                lblLicenseIDTitle.Text = "International License ID:";
                IsDetained = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.IsDetained;
                DetainID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DetainedInfo.DetainID;
                LicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.InternationalLicenseID.Value;
                DetainDate = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DetainedInfo.DetainDate;
                _FineFees = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DetainedInfo.FineFees;
                PersonID = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.DriverInfo.PersonID;
            }


            //ToDo: make sure the license is not detained already.
            if (!IsDetained)
            {
                MessageBox.Show($"Selected License {ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID} is not detained.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int? ActiveAppID;
            ActiveAppID = clsApplication.GetActiveApplicationID(
                 PersonID,
                 clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense);

            if (ActiveAppID.HasValue)
                _CurrentApplication = clsApplication.FindBaseApplication(ActiveAppID.Value);
            else
                llblShowApplicationInfo.Enabled = false;


                lblApplicationFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees.ToString();
            lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;

            lblDetainID.Text = DetainID.ToString();
            lblLicenseID.Text = LicenseID.ToString();
           
           // lblCreatedByEmployee.Text = CreatedByEmployeeInfo.UserInfo.PersonInfo.FirstName;
            lblDetainDate.Text = clsFormat.DateToShort(DetainDate);
            lblFineFees.Text = _FineFees.ToString();
            lblTotalFees.Text = (Convert.ToSingle( lblApplicationFees.Text) + Convert.ToSingle(lblFineFees.Text)).ToString();

            btnRelease.Enabled = true;
        }

        private void frmReleaseDetainedLicenseApplication_Activated(object sender, EventArgs e)
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
                frm = new frmShowLicenseInfo(_SelectedLicenseID);
            }
            else
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseInt))
                {
                    clsGlobal.PermisionMessage("ShowLicenseInt");
                    return;
                }
                frm = new frmShowInternationalLicenseInfo(_SelectedLicenseID);
            }
                
            frm.ShowDialog();
        }

        private void btnRelease_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to release this detained  license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            
            (bool? IsRelease, int? ApplicationID) Released;

            if(ctrlDriverLicenseInfoWithFilter1.TypeOfLicense == ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense)
            {
                Released = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.
                    ReleaseDetainedLicense(clsGlobal.CurrentEmployee.EmployeeID.Value, _FineFees);
            }
            else
            {
                Released = ctrlDriverLicenseInfoWithFilter1.SelectedInternationalLicenseInfo.
                    ReleaseDetainedLicense(clsGlobal.CurrentEmployee.EmployeeID.Value, _FineFees);
            }
            bool IsCompleted = Released.IsRelease != null? Released.IsRelease.Value : false
                && Released.ApplicationID.HasValue;

            lblApplicationID.Text = Released.ApplicationID.HasValue ? Released.ApplicationID.Value.ToString() : "[???]";
            llblShowApplicationInfo.Enabled = Released.ApplicationID.HasValue;

            if (! IsCompleted )
            {
                MessageBox.Show("Faild to to release the Detain License,\nCheck the application info, User has to pay it",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int UserID = clsUser.FindByPersonID
                (clsApplication.FindBaseApplication(Released.ApplicationID.Value)
                .ApplicantPersonID).UserID.Value;

            clsUtil.ChangeOrderOfApplicationsToPaidFor(UserID,
                (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense,
                 clsUtil.enOperationOrder.delete);

            MessageBox.Show("Detained License released Successfully ",
                "Detained License Released", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnRelease.Enabled = false;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;
           
        }

        private void frmReleaseDetainedLicenseApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void llblShowApplicationInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowApplicationDetails))
            {
                clsGlobal.PermisionMessage("ShowApplicationDetails");
                return;
            }

            if(lblApplicationID.Text == "[???]" || string.IsNullOrEmpty(lblApplicationID.Text))
            {
                MessageBox.Show("Application ID is Null, so you cannot see Application Details",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int AppID = Convert.ToInt32(lblApplicationID.Text);
            frmApplicationBasicInfo applicationBasicInfo = new frmApplicationBasicInfo(AppID);
            applicationBasicInfo.ShowDialog();
        }
    }
}
