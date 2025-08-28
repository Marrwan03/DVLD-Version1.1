using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.DriverLicense
{
    //check about license paied fees
    public partial class frmIssueDriverLicenseFirstTime : Form
    {
        private int _LocalDrivingLicenseApplicationID;
        private  clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;

        public frmIssueDriverLicenseFirstTime(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();

        }

        private void frmIssueDriverLicenseFirstTime_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            txtNotes.Focus();            
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication ==null)
            {

                MessageBox.Show("No Applicaiton with ID=" + _LocalDrivingLicenseApplicationID.ToString(), "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            if(!_LocalDrivingLicenseApplication.ApplicationInfo.IsPaid())
            {
                MessageBox.Show("Person Should Pay this application First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnIssueLicense.Enabled = false;
                picPrint.Enabled = false;
                return;
            }

            if (!_LocalDrivingLicenseApplication.PassedAllTests())
            {

                MessageBox.Show("Person Should Pass All Tests First.", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;
            }

            int LicenseID = _LocalDrivingLicenseApplication.GetActiveLicenseID();
            if (LicenseID !=-1)
            {
                 
                MessageBox.Show("Person already has License before with License ID=" + LicenseID.ToString() , "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
                return;

            }

            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
          


        }

        private void btnIssueLicense_Click(object sender, EventArgs e)
        {
            
            int? LicenseID = _LocalDrivingLicenseApplication.IssueLicenseForTheFirtTime(txtNotes.Text.Trim(),clsGlobal.CurrentEmployee.EmployeeID.Value);

            if (LicenseID.HasValue)
            {
                MessageBox.Show("License Issued Successfully with License ID = " + LicenseID.ToString(),
                    "Succeeded",MessageBoxButtons.OK, MessageBoxIcon.Information);

                clsUtil.DeleteIssueLicenseOrderBy(
                    clsUser.FindByPersonID(
                        _LocalDrivingLicenseApplication.ApplicationInfo.ApplicantPersonID).UserID.Value);

                ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
                _LicenseID = LicenseID.Value;
                btnIssueLicense.Enabled = false;
                picPrint.Enabled = true;
            }
              else
            {
                MessageBox.Show("License Was not Issued ! " ,
                 "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        int _LicenseID;
        private void picPrint_Click(object sender, EventArgs e)
        {
            if(btnIssueLicense.Enabled)
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

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            clsUtil.printDocument_PrintPage(sender, e, _LicenseID, clsDetainedLicense.enLicenseType.Local);
        }

        private void printPreviewDialog1_Load(object sender, EventArgs e)
        {

        }
    }
}
