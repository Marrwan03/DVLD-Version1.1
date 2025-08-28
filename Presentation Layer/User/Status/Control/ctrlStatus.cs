using DVLD.DriverLicense;
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

namespace DVLD.User.Status.Control
{
    public partial class ctrlStatus : UserControl
    {
        int _PersonID;
        public void ctrlStatus_Load(int PersonID)
        {
            _PersonID = PersonID;
            pbSucessTests.Maximum =clsTestType.GetCountOfTestTypes();
            lblTotalOfTest.Text = $"0 / {pbSucessTests.Maximum}";
            cbAllLDLAppID1.FillcbAllLDLAppIDBy(PersonID);

            if (cbAllLDLAppID1.Items.Count>0)
                cbAllLDLAppID1.SelectedIndex = 0;
            else
            {
                gbTestsSection.Enabled = false;
                lblNote.Visible = true;
                cbAllLDLAppID1.Enabled = false;
            }
                
        }
        public ctrlStatus()
        {
            
            InitializeComponent();
        }

        int _CurrentLDLAppID;
        clsLicense _ActiveLicense;
        private void cbAllLDLAppID1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _CurrentLDLAppID = Convert.ToInt32(cbAllLDLAppID1.Text);
            pbSucessTests.Value = clsTestAppointment.GetTotalOfPassTestsBy(_CurrentLDLAppID);
            lblTotalOfTest.Text = $"{pbSucessTests.Value} / {pbSucessTests.Maximum}";
            ctrlAllTestsInfo1.ctrlAllTestsInfo_Load(_CurrentLDLAppID);
            _ActiveLicense = _GetActiveLicense();
        }

        clsLicense _GetActiveLicense()
        {
            clsLicense ActiveLicense = null;
            clsLocalDrivingLicenseApplication localDrivingLicenseApplication
                = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_CurrentLDLAppID);

            if (localDrivingLicenseApplication != null)
            {
                clsDriver driver = clsDriver.FindByPersonID(_PersonID);
                if (driver != null)
                {
                    ActiveLicense = clsLicense.Find(driver.DriverID.Value,
                        (clsLicense.enLicenseClass)localDrivingLicenseApplication.LicenseClassInfo.LicenseClassID);
                }
            }

            return ActiveLicense;
        }

        private void cmsLicense_Opening(object sender, CancelEventArgs e)
        {
            cmsLicense.Enabled = _ActiveLicense != null ;
           
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowLicenseInfo showLicenseInfo
                = new frmShowLicenseInfo(_ActiveLicense.LicenseID.Value);
            showLicenseInfo.ShowDialog();
        }
    }
}
