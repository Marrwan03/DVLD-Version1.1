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
using System.Xml.Linq;
using System.IO;
using DVLD.People;
using DVLD.Classes;

namespace DVLD.Controls
{
    public partial class ctrlPersonCard : UserControl
    {
        Color _HeaderColor;
        public Color HeaderColor 
        {
            get { return _HeaderColor; }
            set 
            {
                _HeaderColor = value; 
                gbPersonInfo.BorderColor = _HeaderColor;
                gbPersonInfo.CustomBorderColor = _HeaderColor;
            }
        }

        Color _BackGroundColor;
        public Color BackGroundColor 
        {
            get { return _BackGroundColor; }
            set
            {
                _BackGroundColor = value;
                this.BackColor = _BackGroundColor;
                this.gbPersonInfo.FillColor= _BackGroundColor;

            }
        }
        Color _ForeColorTitle;
        public Color ForeColorTitle
        { get { return _ForeColorTitle; }
            set 
            {
                _ForeColorTitle = value;
                gbPersonInfo.ForeColor = _ForeColorTitle;
                lblPersonIDTitle.ForeColor= _ForeColorTitle;
                lblNameTitle.ForeColor = _ForeColorTitle;
                lblNationalNoTitle.ForeColor = _ForeColorTitle;
                lblGendorTitle.ForeColor = _ForeColorTitle;
                lblEmailTitle.ForeColor = _ForeColorTitle;
                lblAddressTitle.ForeColor = _ForeColorTitle;
                lblDateOfBirthTitle.ForeColor = _ForeColorTitle;
                lblPhoneTitle.ForeColor = _ForeColorTitle;
                lblCountryTitle.ForeColor = _ForeColorTitle;
                gbPersonInfo.ForeColor = _ForeColorTitle;
                llEditPersonInfo.ForeColor = _ForeColorTitle;
            }
        }

        Color _ForeColorAnsweres;
        public Color ForeColorAnsweres {
            get { return _ForeColorAnsweres; } 
            set {
                _ForeColorAnsweres = value;
                lblPersonID.ForeColor = _ForeColorAnsweres;
                lblFullName.ForeColor = _ForeColorAnsweres;
                lblNationalNo.ForeColor = _ForeColorAnsweres;
                lblGendor.ForeColor = _ForeColorAnsweres;
                lblEmail.ForeColor = _ForeColorAnsweres;
                lblAddress.ForeColor = _ForeColorAnsweres;
                lblDateOfBirth.ForeColor = _ForeColorAnsweres;
                lblPhone.ForeColor = _ForeColorAnsweres;
                lblCountry.ForeColor = _ForeColorAnsweres;
                llEditPersonInfo.ForeColor = _ForeColorAnsweres;
            } }




        private clsPerson _Person;

        private int?  _PersonID=null;

        public int? PersonID   
        {
            get { return _PersonID.Value; }   
        }

        public clsPerson SelectedPersonInfo
        {
            get { return _Person; }
        }

        public ctrlPersonCard()
        {
            InitializeComponent();
        }

        public void LoadPersonInfo(int PersonID)
        {
            _Person=clsPerson.Find(PersonID);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with PersonID = " + PersonID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
           
                _FillPersonInfo();
        }

        public void LoadPersonInfo(string NationalNo)
        {
            _Person = clsPerson.Find(NationalNo);
            if (_Person == null)
            {
                ResetPersonInfo();
                MessageBox.Show("No Person with National No. = " + NationalNo.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            
             _FillPersonInfo();
        }

        private void _LoadPersonImage()
        {
            if (_Person.Gendor == 0)
                pbPersonImage.Image = Resources.Male_512;
            else
                pbPersonImage.Image = Resources.Female_512;

            string ImagePath = _Person.ImagePath;
            if (ImagePath != "")
                if (File.Exists(ImagePath))
                    pbPersonImage.ImageLocation= ImagePath;
                else
                    MessageBox.Show("Could not find this image: = " + ImagePath, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void _FillPersonInfo()
        {
            llEditPersonInfo.Enabled = true;
            _PersonID = _Person.PersonID ?? -1;
            lblPersonID.Text=_Person.PersonID.ToString();
            lblNationalNo.Text = _Person.NationalNo;
            lblFullName.Text = _Person.FullName;
            lblGendor.Text = _Person.Gendor == 0 ? "Male" : "Female";
            if(string.IsNullOrEmpty(_Person.Email))
            {
                lblEmail.Text = "Doesn`t Have";
            }
            else
            {
                lblEmail.Text = _Person.Email;
            }
            
            lblPhone.Text = _Person.Phone;
            lblDateOfBirth.Text = _Person.DateOfBirth.ToShortDateString();
            lblCountry.Text= clsCountry.Find( _Person.NationalityCountryID).CountryName ;
            lblAddress.Text= _Person.Address;
            _LoadPersonImage();

           


        }

        public void ResetPersonInfo()
        {
            _PersonID = -1;
            lblPersonID.Text = "[????]";
            lblNationalNo.Text = "[????]";
            lblFullName.Text = "[????]";
            pbGendor.Image = Resources.Man_32;
            lblGendor.Text = "[????]";
            lblEmail.Text = "[????]";
            lblPhone.Text = "[????]";
            lblDateOfBirth.Text = "[????]";
            lblCountry.Text = "[????]";
            lblAddress.Text = "[????]";
            pbPersonImage.Image = Resources.Male_512;
        
        }

        void _RefreshPhoto(string PicPath)
        {
            pbPersonImage.Load(PicPath);
            OnChangedPicture?.Invoke(PicPath);
        }
            
        public event Action<string> OnChangedPicture;
        private void llEditPersonInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if (clsGlobal.CurrentEmployee != null)
            {
                if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.EditPersonPeople))
                {
                    clsGlobal.PermisionMessage("EditPersonPeople");
                    return;
                }
            }
            else
            {
                if(clsGlobal.CurrentUser!=null)
                {
                    if(clsGlobal.CurrentUser.PersonID != _PersonID.Value)
                    {
                        MessageBox.Show("You don`t have an Access to edit this person", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return;
                    }
                }
            }
            
            frmAddUpdatePerson frm = new frmAddUpdatePerson(_PersonID.Value);
            frm.OnChangedPicture += _RefreshPhoto;
            frm.ShowDialog();

            //refresh
            LoadPersonInfo(_PersonID.Value);
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }

        private void pbPersonImage_Click(object sender, EventArgs e)
        {

        }
    }
}
