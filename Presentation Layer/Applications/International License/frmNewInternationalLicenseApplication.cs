using DVLD.Classes;
using DVLD.Licenses.Controls;
using DVLD.Licenses.International_License;
using DVLD.Licenses.International_Licenses;
using DVLD_Buisness;
using System;
using System.Windows.Forms;
using static DVLD_Buisness.clsApplication;

namespace DVLD.Applications.International_License
{
    public partial class frmNewInternationalLicenseApplication : Form
    {
        int _SelectedLicenseID;
        private int _InternationalLicenseID = -1;
        int? _CurrentAppID;
        clsApplication _CurrentAppInfo;
      
        public frmNewInternationalLicenseApplication()
        {
            InitializeComponent();
        }

        public frmNewInternationalLicenseApplication(int LicenseID)
        {
            InitializeComponent();
            ctrlDriverLicenseInfoWithFilter1.LoadLicenseInfo(LicenseID);
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            ctrlDriverLicenseInfoWithFilter1.TypeOfLicense =
                Licenses.Controls.ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense;
        }

        private void ctrlDriverLicenseInfoWithFilter1_OnLicenseSelected(int obj)
        {
             _SelectedLicenseID = obj;

            lblLocalLicenseID.Text = _SelectedLicenseID.ToString();

            llShowLicenseHistory.Enabled = (_SelectedLicenseID != -1);

            if (_SelectedLicenseID == -1) 
            
            {
                return;
            }
            //check the license class, person could not issue international license without having
            //normal license of class 3.

            if (ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseClass != 3)
            {
                MessageBox.Show("Selected License should be Class 3, select another one.", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            //check if person already has an active international license
            int ActiveInternaionalLicenseID = clsInternationalLicense.GetActiveInternationalLicenseIDBy
                (ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverID,
                ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID.Value);

            if (ActiveInternaionalLicenseID != -1)
            {
                MessageBox.Show("Person already has an active international license with ID = " + ActiveInternaionalLicenseID.ToString(), "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                llShowLicenseInfo.Enabled = true;
                _InternationalLicenseID = ActiveInternaionalLicenseID;
                btnIssueLicense.Enabled = false;
                return;
            }

            _CurrentAppID = clsApplication.GetActiveApplicationID
                (clsLicense.Find(_SelectedLicenseID).ApplicationInfo.ApplicantPersonID,
                 enApplicationType.NewInternationalLicense);

            if(_CurrentAppID.HasValue)
            {
                _CurrentAppInfo = clsApplication.FindBaseApplication(_CurrentAppID.Value);
                if(_CurrentAppInfo==null)
                {
                    MessageBox.Show($"This active Application ID{_CurrentAppID.Value} is not found",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {
                    lblIsPaid.Text = _CurrentAppInfo.IsPaid() ? "Yes" : "No";
                    lblApplicationID.Text = _CurrentAppInfo.ApplicationID.ToString();
                }
                
            }


            btnIssueLicense.Enabled = true;
        }

        private void frmNewInternationalLicenseApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            ctrlDriverLicenseInfoWithFilter1.TypeOfLicense = ctrlDriverLicenseInfoWithFilter.enTypeOfLicense.LocalDrivingLicense;
            ctrlDriverLicenseInfoWithFilter1.EnabledTypeOfLicense = false;

            lblApplicationDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblIssueDate.Text = lblApplicationDate.Text;
            lblExpirationDate.Text = clsFormat.DateToShort(DateTime.Now.AddYears(1));//add one year.
            lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees.ToString();
            lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;
        }
        public Action OnClosed;

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to issue the license?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }

            if (_CurrentAppInfo != null)
            {
                if(!_CurrentAppInfo.IsPaid())
                {
                    MessageBox.Show($"Occure Error: This user has to pay the application fess[{_CurrentAppInfo.PaidFees} $].", "Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            else
            {
                _CurrentAppInfo = new clsApplication();
                _CurrentAppInfo.ApplicationTypeID = (int)enApplicationType.NewInternationalLicense;
                _CurrentAppInfo.ApplicantPersonID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverInfo.PersonID;
                _CurrentAppInfo.ApplicationDate = DateTime.Now;
                _CurrentAppInfo.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                _CurrentAppInfo.LastStatusDate = DateTime.Now;
                _CurrentAppInfo.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.NewInternationalLicense).Fees;
                _CurrentAppInfo.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID.Value;

                if(!_CurrentAppInfo.Save())
                {
                    MessageBox.Show("There is an error when you added an application, please try again!","Error",
                        MessageBoxButtons.OK, MessageBoxIcon.Error );
                    _CurrentAppInfo = null;
                    return;
                }
                else
                {
                    MessageBox.Show($"This user has to pay the application fess[{_CurrentAppInfo.PaidFees} $] to continue", "Warning!",
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);

                    lblApplicationID.Text = _CurrentAppInfo.ApplicationID.ToString();

                    //Refresh
                    _CurrentAppID = clsApplication.GetActiveApplicationID
                (clsLicense.Find(_SelectedLicenseID).ApplicationInfo.ApplicantPersonID,
                 enApplicationType.NewInternationalLicense);
                    _CurrentAppInfo = clsApplication.FindBaseApplication(_CurrentAppID.Value);

                    return;
                }
            }


           clsInternationalLicense InternationalLicense= new clsInternationalLicense();
            InternationalLicense.ApplicationID = _CurrentAppID.Value;
            InternationalLicense.DriverID= ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverID;
            InternationalLicense.IssuedUsingLocalLicenseID = ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.LicenseID ?? -1;
            InternationalLicense.IssueDate= DateTime.Now;
            InternationalLicense.ExpirationDate= DateTime.Now.AddYears(1);
           InternationalLicense.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID.Value;

            if (!InternationalLicense.Save())
            {
                MessageBox.Show("Faild to Issue International License" , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                return;
            }

            lblApplicationID.Text = InternationalLicense.ApplicationID.ToString();
            _InternationalLicenseID = InternationalLicense.InternationalLicenseID.Value;
            lblInternationalLicenseID.Text = InternationalLicense.InternationalLicenseID.ToString();

            clsUtil.ChangeOrderOfApplicationsToPaidFor(
                clsDriver.FindByDriverID(InternationalLicense.DriverID).UserInfo.UserID.Value,
                (int)clsApplication.enApplicationType.NewInternationalLicense,
                 clsUtil.enOperationOrder.delete);

            MessageBox.Show("International License Issued Successfully with ID=" + InternationalLicense.InternationalLicenseID.ToString() , "License Issued", MessageBoxButtons.OK, MessageBoxIcon.Information);

            btnIssueLicense.Enabled = false;
            picPrint.Enabled = true;
            ctrlDriverLicenseInfoWithFilter1.FilterEnabled = false;
            llShowLicenseInfo.Enabled = true;


        }

        private void llShowDriverLicenseHistory_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
           

            frmShowPersonLicenseHistory  frm = 
                new frmShowPersonLicenseHistory(ctrlDriverLicenseInfoWithFilter1.SelectedLocalLicenseInfo.DriverInfo.PersonID);
            frm.ShowDialog();

        }

        private void llShowLicenseInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmShowInternationalLicenseInfo frm =
              new frmShowInternationalLicenseInfo(_InternationalLicenseID);
            frm.ShowDialog();
        }

        private void frmNewInternationalLicenseApplication_Activated(object sender, EventArgs e)
        {
            ctrlDriverLicenseInfoWithFilter1.txtLicenseIDFocus();
        }

        private void ctrlDriverLicenseInfoWithFilter1_Load(object sender, EventArgs e)
        {

        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            clsUtil.printDocument_PrintPage(sender, e, _InternationalLicenseID, clsDetainedLicense.enLicenseType.International);
        }

        private void picPrint_Click(object sender, EventArgs e)
        {
            if (btnIssueLicense.Enabled)
            {
                MessageBox.Show("You cannot print license card until you issue it",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            MessageBox.Show("Filename should be like License ID.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }
    }
}
