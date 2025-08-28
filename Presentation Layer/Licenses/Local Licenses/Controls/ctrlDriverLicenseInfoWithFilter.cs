using DVLD.Controls;
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

namespace DVLD.Licenses.Controls
{
    public partial class ctrlDriverLicenseInfoWithFilter : UserControl
    {

        // Define a custom event handler delegate with parameters
        public event Action<int> OnLicenseSelected;
        // Create a protected method to raise the event with a parameter
        protected virtual void LicenseSelected(int LicenseID)
        {
            Action<int> handler = OnLicenseSelected;
            if (handler != null)
            {
                handler(LicenseID); // Raise the event with the parameter
            }
        }
        public enum enTypeOfLicense { LocalDrivingLicense, InternationalDrivingLicense}
        enTypeOfLicense _TypeOfLicense;
        public enTypeOfLicense TypeOfLicense { get { return _TypeOfLicense; }
        
        set 
            {
                _TypeOfLicense = value;

                switch (_TypeOfLicense)
                {
                    case enTypeOfLicense.LocalDrivingLicense:
                        {
                            cbTypeOfLicense.Text = "Local Driving License";
                            ctrlDriverLicenseInfo1.Visible = true;
                            ctrlDriverInternationalLicenseInfo1.Visible = false;
                            break;
                        }
                    default:
                        {
                            cbTypeOfLicense.Text = "International Driving License";
                            ctrlDriverLicenseInfo1.Visible = false;
                            ctrlDriverInternationalLicenseInfo1.Visible = true;
                            break;
                        }
                }


            }
        }
        public bool EnabledTypeOfLicense { get { return cbTypeOfLicense.Enabled; } set {  cbTypeOfLicense.Enabled = value; } }
        public ctrlDriverLicenseInfoWithFilter()
        {
            InitializeComponent();
        }

        private bool _FilterEnabled = true;
      
        public bool FilterEnabled
        {
            get
            {
                return _FilterEnabled;
            }
            set
            {
                _FilterEnabled = value;
                gbFilters.Enabled = _FilterEnabled;
            }
        }

        private int _LicenseID = -1;

        public int LocalLicenseID
        {
            get { return ctrlDriverLicenseInfo1.LicenseID; }
        }
        public int InternationalLicenseID
        { get { return ctrlDriverInternationalLicenseInfo1.InternationalLicenseID; } }

        public clsLicense SelectedLocalLicenseInfo
        { get { return ctrlDriverLicenseInfo1.SelectedLicenseInfo; } }
        public clsInternationalLicense SelectedInternationalLicenseInfo
        { get { return ctrlDriverInternationalLicenseInfo1.SelectedLicenseInfo; } }

        public void LoadLicenseInfo(int LicenseID)
        {
            txtLicenseID.Text = LicenseID.ToString();
            if (_TypeOfLicense == enTypeOfLicense.LocalDrivingLicense)
            {
                ctrlDriverLicenseInfo1.LoadInfo(LicenseID);
                _LicenseID = ctrlDriverLicenseInfo1.LicenseID;
            }
            else
            {
                ctrlDriverInternationalLicenseInfo1.LoadInfo(LicenseID);
                _LicenseID = ctrlDriverInternationalLicenseInfo1.InternationalLicenseID;
            }
                
            if (OnLicenseSelected != null && FilterEnabled)
                // Raise the event with a parameter
                OnLicenseSelected?.Invoke(_LicenseID);


        }

        private void txtLicenseID_KeyPress(object sender, KeyPressEventArgs e)
        {
         
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

          
            // Check if the pressed key is Enter (character code 13)
            if (e.KeyChar == (char)13)
            {
              
                btnFind.PerformClick();
            }

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtLicenseID.Focus();
                return;

            }
            _LicenseID= int.Parse(txtLicenseID.Text);
            LoadLicenseInfo(_LicenseID);
        }

        public void txtLicenseIDFocus()
        {
            txtLicenseID.Focus();
        }

        private void txtLicenseID_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtLicenseID.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtLicenseID, "This field is required!");
            }
            else
            {
                //e.Cancel = false;
                errorProvider1.SetError(txtLicenseID, null);
            }
        }

        private void ctrlDriverLicenseInfoWithFilter_Load(object sender, EventArgs e)
        {

        }

        private void cbTypeOfLicense_Click(object sender, EventArgs e)
        {
            TypeOfLicense = enTypeOfLicense.LocalDrivingLicense;
        }

        private void cbTypeOfLicense_SelectedIndexChanged(object sender, EventArgs e)
        {
            TypeOfLicense = (enTypeOfLicense)cbTypeOfLicense.SelectedIndex;
        }
    }
}
