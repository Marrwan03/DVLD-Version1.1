
using DVLD_Buisness;
using System;

using System.ComponentModel;

using System.Windows.Forms;

using DVLD.Classes;

using DVLD_Buisness.Global_Classes;

namespace DVLD.Employee
{
    public partial class frmAddUpdateUser: Form
    {
        public delegate void MyDelegate(int UserID);
        public event MyDelegate DataBack;
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode;
        private int? _UserID;
        clsUser _User;
       
        public frmAddUpdateUser()
        {
            InitializeComponent();

            _Mode = enMode.AddNew;
        }

        public frmAddUpdateUser(int UserID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _UserID = UserID;
        }

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values

            if (_Mode == enMode.AddNew)
            {
                lblTitle.Text = "Add New User";
                this.Text = "Add New User";
                _User = new clsUser();
             
                tbLoginInfo.Enabled = false;
                
                ctrlPersonCardWithFilter1.FilterFocus();
            }
            else
            {
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                tbLoginInfo.Enabled = true;
                btnSave.Enabled=true;
             

            }
            
            txtUserName.Text = "";
            txtPassword.Text = "";
            txtConfirmPassword.Text = "";
            chkIsActive.Checked = true; 


        }

        private void _LoadData()
        {

            _User = clsUser.FindByUserID(_UserID.Value);
            ctrlPersonCardWithFilter1.FilterEnabled = false;

            if (_User == null)
            {
                MessageBox.Show("No User with ID = " + _UserID, "User Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            //the following code will not be executed if the person was not found
            lblUserID.Text = _User.UserID.ToString();
            txtUserName.Text = _User.UserName;
            txtPassword.Text = _User.Password;
            txtConfirmPassword.Text = _User.Password;
            chkIsActive.Checked = _User.Status== clsUser.enStatus.Active;
            ctrlPersonCardWithFilter1.LoadPersonInfo(_User.PersonID);
            ctrlBloodTest1.ctrlBloodTest_Load(_User.BloodType);
            
        }

        private void frmAddUpdateUser_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
                _LoadData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", 
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(ctrlBloodTest1.NumberOfBloodInNeedle < 100)
            {
                MessageBox.Show("You must do Test Blood for this user", "UnKnown BloodType", MessageBoxButtons.YesNo, MessageBoxIcon.Error);
                ctrlBloodTest1.Focus();
                return;
            }

            _User.PersonID = ctrlPersonCardWithFilter1.PersonID.Value;
            _User.UserName = txtUserName.Text.Trim();

            byte[] Salt = clsHash.GenerateSalt();
            byte[] PasswordHasingWIthSalt = clsHash.ComputeHashWithSalt(txtPassword.Text.Trim(), Salt);

            _User.Salt = Convert.ToBase64String(Salt);
            _User.Password = Convert.ToBase64String(PasswordHasingWIthSalt);
            _User.Status = chkIsActive.Checked? clsUser.enStatus.Active : clsUser.enStatus.NotActive;
            _User.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID.Value;

            if (_User.Save())
            {
                _UserID = _User.UserID;
                DataBack?.Invoke(_UserID.Value);
                lblUserID.Text = _User.UserID.ToString();
                //change form mode to update.
                _Mode = enMode.Update;
                lblTitle.Text = "Update User";
                this.Text = "Update User";

                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlPersonCardWithFilter1.FilterEnabled = false;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtPassword.Text.Trim())
            {
                    e.Cancel = true;
                    errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };

        }

        private void txtPassword_Validating(object sender, CancelEventArgs e)
        {
            if ( string.IsNullOrEmpty ( txtPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtPassword, "Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtPassword, null);
            };

        }

        private void txtUserName_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtUserName.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtUserName, "Username cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtUserName, null);
            };


            if (_Mode == enMode.AddNew)
            {

                if (clsUser.isUserExist(txtUserName.Text.Trim()))
                {
                    e.Cancel = true;
                    errorProvider1.SetError(txtUserName, "username is used by another user");
                }
                else
                {
                    errorProvider1.SetError(txtUserName, null);
                };
            } 
            else
            {
                //incase update make sure not to use anothers user name
                if (_User.UserName !=txtUserName.Text.Trim())
                {
                        if (clsUser.isUserExist(txtUserName.Text.Trim()))
                        {
                            e.Cancel = true;
                            errorProvider1.SetError(txtUserName, "username is used by another user");
                            return;
                        }
                        else
                        {
                            errorProvider1.SetError(txtUserName, null);
                        };
                }
            }
        }

     
        private void btnPersonInfoNext_Click(object sender, EventArgs e)
        {
            
            if (_Mode==enMode.Update)
            {
                btnSave.Enabled = true;
                tbLoginInfo.Enabled = true;//tbLoginInfo
                tcUserInfo.SelectedTab = tcUserInfo.TabPages["tbLoginInfo"];
                return;
            }

           // incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {

                if (clsUser.isUserExistForPersonID(ctrlPersonCardWithFilter1.PersonID.Value))
                {

                    MessageBox.Show("Selected Person already has a user, choose another one.", "Select another Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    ctrlPersonCardWithFilter1.FilterFocus();
                    tbLoginInfo.Enabled = false;
                }

                else
                {
                    btnSave.Enabled = true;
                    tbLoginInfo.Enabled = true;
                    tcUserInfo.SelectedTab = tcUserInfo.TabPages["tbLoginInfo"];
                }
            }

            else

            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
                tbLoginInfo.Enabled = false;
            }

        }


        private void frmAddUpdateUser_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void txtUserName_TextChanged(object sender, EventArgs e)
        {

        }

        private void chkIsActive_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {

        }

        private void txtPassword_MouseEnter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.PasswordChar = default(char);
        }

        private void txtPassword_MouseLeave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.PasswordChar = '*';
        }

        private void guna2Chip1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {

        }

       

        

        private void chkIsActive_CheckedChanged_1(object sender, EventArgs e)
        {

        }

        private void btnBloodTest_Click(object sender, EventArgs e)
        {
            if (ctrlBloodTest1.Visible)
                ctrlBloodTest1.Visible = false;
            else
                ctrlBloodTest1.Visible = true;
        }

        private void ctrlBloodTest1_CompletedBloodTest(object sender, ctrlBloodTest.clsBloodTest e)
        {
            _User.BloodType = e.BloodType;
            if(MessageBox.Show($"Your Blood Type is : {e.BloodTypeString}.", "Tested Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ctrlBloodTest1.BloodType = e.BloodType;
            }
            
        }

        private void cbTypeOfCreditCard1_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected_1(int obj)
        {

        }
    }
}
