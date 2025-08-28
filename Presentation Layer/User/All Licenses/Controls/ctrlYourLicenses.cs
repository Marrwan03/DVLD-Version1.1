using DVLD.Classes;
using DVLD.DriverLicense;
using DVLD.Licenses.International_Licenses;
using DVLD.Properties;
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

namespace DVLD.All_Licenses.Controls
{
    public partial class ctrlYourLicenses : UserControl
    {
        public ctrlYourLicenses()
        {
            InitializeComponent();
            //LicenseType = enLicenseType.International;
        }
        public enum enLicenseType { Local, International}
        enLicenseType _LicenseType;
        public enLicenseType LicenseType { get { return _LicenseType; } }
        DataRow _CurrentLicenseInfo;


        DataTable _AllLocalLicenses;
        DataTable _AllInternationalLicenses;

        public void ctrlYourLicenses_Load(int UserID)
        {
            clsUser user = clsUser.FindByUserID(UserID);
            if (user != null)
            {
                _AllLocalLicenses = user.GetAllLocalLicense();
                _AllInternationalLicenses = user.GetAllInternationalLicense();
                cbLicenseType.SelectedIndex = 0;
            }
        }

        void _FillLicenseCard(DataRow Row)
        {
            
            lblName.Text = (string)Row["FullName"];

            int LicenseID;
            if (_LicenseType == enLicenseType.Local)
            {
                LicenseID = (int)Row["LicenseID"];
                lblLicenseType.Text = "Local";
            }
            else
            {
                LicenseID = (int)Row["InternationalLicenseID"];
                lblLicenseType.Text = "International";
            }

            lblLicenseID.Text = LicenseID.ToString();
            lblDateOfBirth.Text = clsFormat.DateToShort((DateTime)Row["DateOfBirth"]);
            lblDateOfIssue.Text = clsFormat.DateToShort((DateTime)Row["IssueDate"]);
            lblDateOfExpiration.Text = clsFormat.DateToShort((DateTime)Row["ExpirationDate"]);
            lblCountry.Text = (string)Row["CountryName"];
            lblBloodType.Text = (string)Row["BloodType"];
            if (Row["ImagePath"] == DBNull.Value)
            {
                if ((string)Row["Gender"] == "Male")
                    picYourFace.Image = Resources.Male_512;
                else
                    picYourFace.Image = Resources.Female_512;
            }
            else
            {
                picYourFace.ImageLocation = (string)Row["ImagePath"];
            }

        }

        void _FillLicenseCardBy(int NumberOfRow)
        {
            DataRow Row = null;
            if (cbLicenseType.Text == "Local License")
            {
                Row = _AllLocalLicenses.Rows[NumberOfRow - 1];
            }
            else
            {
                Row = _AllInternationalLicenses.Rows[NumberOfRow - 1];
            }
            _CurrentLicenseInfo = Row;
            _FillLicenseCard(Row);
        }

        void _VisibleAllInfo(bool Visible)
        {
            if(Visible)
            {
                pictureBox1.Image = Resources.License;             
            }
            else
            { pictureBox1.Image = Resources.Question_32;}

            lblName.Visible = Visible;
            lblLicenseID.Visible = Visible;
            lblDateOfBirth.Visible = Visible;
            lblDateOfIssue.Visible = Visible;
            lblCountry.Visible = Visible;
            lblBloodType.Visible = Visible;
            lblDateOfExpiration.Visible = Visible;
            lblLicenseType.Visible = Visible;
            picYourFace.Visible = Visible;
            ctrlSwitchSearch1.Visible = Visible;
        }

       public void ResetAllInfo()
        {
            string Data = "[ ??? ]";
            lblName.Text = Data;
            lblLicenseID.Text = Data;
            lblDateOfBirth.Text = Data;
            lblDateOfIssue.Text = Data;
            lblCountry.Text = Data;
            lblBloodType.Text = Data;
            lblDateOfExpiration.Text = Data;
            lblLicenseType.Text = Data;
        }

        private void cbLicenseType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(cbLicenseType.Text == "Local License")
            {
                _LicenseType = enLicenseType.Local;
                if (_AllLocalLicenses.Rows.Count == 0)
                {                   
                    ResetAllInfo();
                    ctrlSwitchSearch1.Enabled = false;
                    _VisibleAllInfo(false);
                    return;

                }
                ctrlSwitchSearch1.MaxNumberOfPage = _AllLocalLicenses.Rows.Count;
                
            }
            else
            {
                _LicenseType = enLicenseType.International;
                if (_AllInternationalLicenses.Rows.Count == 0)
                {
                    ResetAllInfo();
                    ctrlSwitchSearch1.Enabled = false;
                    _VisibleAllInfo(false);
                    return;
                }
                ctrlSwitchSearch1.MaxNumberOfPage = _AllInternationalLicenses.Rows.Count;
            }

            _VisibleAllInfo(true);
            ctrlSwitchSearch1.NumberOfPage = 1;
            _FillLicenseCardBy(ctrlSwitchSearch1.NumberOfPage);
        }

        private void ctrlYourLicenses_Load(object sender, EventArgs e)
        {

        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillLicenseCardBy(e.CurrentNumberOfPage);
        }

        private void cmsLicense_Opening(object sender, CancelEventArgs e)
        {
           // showLicenseInfoToolStripMenuItem.Enabled = ctrlSwitchSearch1.Enabled;
        }

        private void showLicenseInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int LicenseID = 0;
           if (LicenseType == enLicenseType.Local)
            {
                LicenseID = (int)_CurrentLicenseInfo["LicenseID"];
                frmShowLicenseInfo LicenseInfo = new frmShowLicenseInfo(LicenseID);
                LicenseInfo.ShowDialog();
            }
           else
            {
                LicenseID = (int)_CurrentLicenseInfo["InternationalLicenseID"];
                frmShowInternationalLicenseInfo internationalLicenseInfo = new frmShowInternationalLicenseInfo(LicenseID);
                internationalLicenseInfo.ShowDialog();
            }

        }
    }
}
