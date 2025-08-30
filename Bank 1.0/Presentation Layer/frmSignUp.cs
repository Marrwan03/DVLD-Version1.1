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
using DVLD_Buisness;

namespace Bank
{
    public partial class frmSignUp : Form
    {
        public frmSignUp()
        {
            InitializeComponent();
        }

        private void txtPassword_MouseEnter(object sender, EventArgs e)
        {
           
            txtPassword.PasswordChar = default(char);
        }

        private void txtPassword_MouseLeave(object sender, EventArgs e)
        {
            txtPassword.PasswordChar = '*';
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = clsUser.FindByUsernameAndPassword(txtUsername.Text, txtPassword.Text);
            if(clsGlobal.CurrentUser == null)
            {
                if(MessageBox.Show("Username / Password is not correct", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    txtUsername.Text = null;
                    txtPassword.Text = null;
                    txtUsername.Focus();
                    return;
                }
            }
           
            frmDashBoard dashBoard = new frmDashBoard(this);
            this.Hide();
            dashBoard.ShowDialog();
        }

        private void frmSignUp_Load(object sender, EventArgs e)
        {
            // تعيين زوايا دائرية
            

            this.Region = clsGlobal.CornerForm(Width, Height);
            txtUsername.Focus();
        }

        private void txtPassword_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnLogin.PerformClick();
        }

        private void txtUsername_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnLogin.PerformClick();
        }
    }
}
