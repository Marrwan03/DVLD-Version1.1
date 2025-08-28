using DVLD.Classes;
using DVLD.DriverLicense;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Licenses.International_Licenses
{
    public partial class frmShowInternationalLicenseInfo : Form
    {
        private int _InternationalLicenseID;
        public frmShowInternationalLicenseInfo(int InternationalLicenseID)
        {
            InitializeComponent();
            _InternationalLicenseID = InternationalLicenseID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmShowInternationalLicenseInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlDriverInternationalLicenseInfo1.LoadInfo(_InternationalLicenseID);
        }

        private void picPrint_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do you want print this license card?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if (!ctrlDriverInternationalLicenseInfo1.SelectedLicenseInfo.IsActive)
            {
                MessageBox.Show("This license is not active, so i cannot print it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsUtil.IsLicensePDFExists(_InternationalLicenseID, DVLD_Buisness.clsDetainedLicense.enLicenseType.International))
            {
                MessageBox.Show("This license is already exists, so i cannot print it again", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }



            MessageBox.Show("Filename should be like License ID,\nAnd you must save in correct file", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (printPreviewDialog1.ShowDialog() == DialogResult.OK)
            {
                printDocument1.Print();
            }
        }

        private void printDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            clsUtil.printDocument_PrintPage(sender, e,  _InternationalLicenseID, DVLD_Buisness.clsDetainedLicense.enLicenseType.International);
        }
    }
}
