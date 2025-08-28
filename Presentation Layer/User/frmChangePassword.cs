using DVLD.Classes;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Employee
{
    public partial class frmChangePassword : Form
    {
        private int _UserID;
        private clsUser _User;
       
        public frmChangePassword(int UserID )
        {
            InitializeComponent();

            _UserID=UserID;
        }

        private void _ResetDefualtValues()
        {
            txtCurrentPassword.Text = "";
            txtNewPassword.Text = "";
            txtConfirmPassword.Text = "";
            txtCurrentPassword.Focus(); 
        }

        private void frmChangePassword_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _ResetDefualtValues();

              _User = clsUser.FindByUserID(_UserID);

            if (_User == null)
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Could not Find Employee with id = " + _UserID,
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                 this.Close();

                return;

            }
            ctrlUserCard1.LoadUserInfo(_UserID, DVLD.Controls.ctrlUserCard.enLoadUserInfo.ByUserID);

        }

        private void txtCurrentPassword_Validating(object sender, CancelEventArgs e)
        {

            if (string.IsNullOrEmpty(txtCurrentPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Password cannot be blank");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };

            // if (_User.Password != txtCurrentPassword.Text.Trim())
            //Edit

            if (!clsHash.IsPasswordCorrect(txtCurrentPassword.Text.Trim(), Convert.FromBase64String(_User.Password), Convert.FromBase64String(_User.Salt)))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtCurrentPassword, "Current password is wrong!");
                return;
            }
            else
            {
                errorProvider1.SetError(txtCurrentPassword, null);
            };
        }

        private void txtNewPassword_Validating(object sender, CancelEventArgs e)
        {
            if (string.IsNullOrEmpty(txtNewPassword.Text.Trim()))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtNewPassword, "New Password cannot be blank");
            }
            else
            {
                errorProvider1.SetError(txtNewPassword, null);
            };
        }

        private void txtConfirmPassword_Validating(object sender, CancelEventArgs e)
        {
            if (txtConfirmPassword.Text.Trim() != txtNewPassword.Text.Trim())
            {
                e.Cancel = true;
                errorProvider1.SetError(txtConfirmPassword, "Password Confirmation does not match New Password!");
            }
            else
            {
                errorProvider1.SetError(txtConfirmPassword, null);
            };
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
            byte[] Salt = clsHash.GenerateSalt();
            byte[] PasswordHasingWIthSalt = clsHash.ComputeHashWithSalt(txtNewPassword.Text.Trim(), Salt);

            _User.Salt = Convert.ToBase64String(Salt);
            _User.Password = Convert.ToBase64String(PasswordHasingWIthSalt);

            //_User.Password = txtNewPassword.Text;

            if (_User.Save())
            {
                MessageBox.Show("Password Changed Successfully.",
                   "Saved.", MessageBoxButtons.OK, MessageBoxIcon.Information );
                _ResetDefualtValues();
            }
            else
            {
                MessageBox.Show("An Erro Occured, Password did not change.",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();

        }

        private void txt_MouseEnter(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.PasswordChar = default(char);
        }

        private void txt_MouseLeave(object sender, EventArgs e)
        {
            TextBox txt = (TextBox)sender;
            txt.PasswordChar = '*';
        }


        private void txtCurrentPassword_MouseEnter(object sender, EventArgs e)
        {

        }

        private void txtCurrentPassword_MouseLeave(object sender, EventArgs e)
        {

        }
    }
}
