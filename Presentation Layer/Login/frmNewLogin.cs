using DVLD.Classes;
using DVLD.Dashboards;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace DVLD
{
    public partial class frmNewLogin : Form
    {
        public frmNewLogin()
        {
            InitializeComponent();
            this.Load += frmNewLogin_Load;
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsUser user = clsUser.FindByUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());

            if (user != null)
            {

                if (switchRememberMe.Checked)
                {
                    //store username and password
                    //clsGlobal.RememberUsernameAndPassword(txtUserName.Text.Trim(), txtPassword.Text.Trim());
                    clsUtil.RememberUsernameAndPasswordRegistry(txtUserName.Text.Trim(), txtPassword.Text.Trim());

                }
                else
                {
                    //store empty username and password
                    // clsGlobal.RememberUsernameAndPassword("", "");
                    clsUtil.RememberUsernameAndPasswordRegistry("", "");

                }
                clsEmployee employee = clsEmployee.FindEmployeeByUserID(user.UserID.Value);
                if (employee != null)
                {

                    if (employee.Status== clsUser.enStatus.NotActive)
                    {
                        txtUserName.Focus();
                        MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if(employee.Status == clsUser.enStatus.Deleted)
                    {
                        txtUserName.Focus();
                        MessageBox.Show("Your accound is deleted, Contact Admin.", "In Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    clsGlobal.CurrentEmployee = employee;
                }
                else
                {

                    //incase the user is not active
                    if (user.Status == clsUser.enStatus.NotActive)
                    {
                        txtUserName.Focus();
                        MessageBox.Show("Your accound is not Active, Contact Admin.", "In Active Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    else if (user.Status == clsUser.enStatus.Deleted)
                    {
                        txtUserName.Focus();
                        MessageBox.Show("Your accound is deleted, Contact Admin.", "In Delete Account", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    clsGlobal.CurrentUser = user;
                    frmDashboardOfUser DashboardOfUser = new frmDashboardOfUser(this);
                    DashboardOfUser.ShowDialog();
                    return;
                }
                this.Hide();
                frmDashboardOfEmployee DashboardOfEmployee = new frmDashboardOfEmployee(this);
                DashboardOfEmployee.ShowDialog();


            }
            else
            {
                txtUserName.Focus();
                MessageBox.Show("Invalid Username/Password.", "Wrong Credintials", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void frmNewLogin_Load(object sender, EventArgs e)
        {
            string UserName = "", Password = "";
            //clsGlobal.GetStoredCredential(ref UserName, ref Password)
            if (clsUtil.GetStoredCredentialRegistry(ref UserName, ref Password))
            {
                txtUserName.Text = UserName;
                txtPassword.Text = Password;
                switchRememberMe.Checked = true;
            }
            else
                switchRememberMe.Checked = false;


            this.Region = clsGlobal.CornerForm(Width, Height);
        }

        private void txtPassword_MouseEnter(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = default(char);
        }

        private void txtPassword_MouseLeave(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
           
        }



        private void guna2Chip1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void chkRememberMe_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void txtUserName_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnLogin.PerformClick();
        }

        private void chkRememberMe_CheckedChanged_1(object sender, EventArgs e)
        {

        }
    }
}
