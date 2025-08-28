using DVLD.Licenses.International_License;
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

namespace DVLD.Dashboards.User.Controls
{
    public partial class ctrlLicensesSection : UserControl
    {
        public ctrlLicensesSection()
        {
            InitializeComponent();
        }

        int _PersonID;
        int _NumberOfLocalLicense;
        public int NumberOfLocalLicense { get { return _NumberOfLocalLicense; }}

        int _NumberOfInternationalLicense;
        public int NumberOfInternationalLicense {  get { return _NumberOfInternationalLicense; }}
        public void ctrlLicensesSection_Load(int PersonID)
        {
            _PersonID = PersonID;
            clsDriver driver = clsDriver.FindByPersonID(_PersonID);
            if (driver != null)
            {
                lblNumberOfLocalLicense.Text = clsLicense.GetNumberOfRowsForLocalLicenses(driver.DriverID.Value).ToString();
                lblNumberOfInternationalLicenses.Text = clsInternationalLicense.GetNumberOfRowsForIntLicenses(driver.DriverID.Value).ToString();
            }
            else
            {
                lblNumberOfLocalLicense.Text = "0";
                lblNumberOfInternationalLicenses.Text = "0";
            }
            _NumberOfLocalLicense=Convert.ToInt32(lblNumberOfLocalLicense.Text);
            _NumberOfInternationalLicense = Convert.ToInt32(lblNumberOfInternationalLicenses.Text);
        }

        private void showLicenseHistoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmShowPersonLicenseHistory frmShowPersonLicenseHistory = new frmShowPersonLicenseHistory(_PersonID);
            frmShowPersonLicenseHistory.ShowDialog();
        }

        private void cmsLicenses_Opening(object sender, CancelEventArgs e)
        {
            showLicenseHistoryToolStripMenuItem.Enabled =
                lblNumberOfInternationalLicenses.Text != "0"
                && lblNumberOfInternationalLicenses.Text != "0";
        }
    }
}
