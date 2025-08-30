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

namespace Bank
{
    public partial class frmDashBoard : Form
    {
        frmSignUp _SignUp;
        public frmDashBoard(frmSignUp SignUp)
        {
            InitializeComponent();
            _SignUp = SignUp;
        }

        private void btnLogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            _SignUp.Show();
        }

        private void frmDashBoard_Load(object sender, EventArgs e)
        {
            ctrlUserInfo1.ctrlUserInfo_Load(clsGlobal.CurrentUser.UserName);
            DataTable dtAllCreadiCards = clsCreditCard.FindByUserID(clsGlobal.CurrentUser.UserID.Value);
            lblNoteWrong.Visible = dtAllCreadiCards.Rows.Count == 0;
            if(lblNoteWrong.Visible)
            {
                pFeatures.Enabled = false;
            }
           // DataTable dtAllCreadiCards = clsCreditCardData.FindByUserID(clsGlobal.CurrentUser.UserID.Value);

            
            this.Region = clsGlobal.CornerForm(Width, Height);

            //_TypeOfCreditCard = clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCards1.Text);
        }



        private void btnDeposite_Click(object sender, EventArgs e)
        {
            frmAddDeposite deposite = new frmAddDeposite(clsGlobal.CurrentUser.UserID.Value);
            deposite.ShowDialog();
        }

        private void btnWithdraw_Click(object sender, EventArgs e)
        {
            frmListWithdraw withdrawlog = new frmListWithdraw(clsGlobal.CurrentUser.UserID.Value);
            withdrawlog.ShowDialog();
        }

        private void btnShowBalance_Click(object sender, EventArgs e)
        {
            frmShowBalance showBalance = new frmShowBalance(clsGlobal.CurrentUser.UserID.Value);
            showBalance.ShowDialog();
        }

        private void btnTransactionLog_Click(object sender, EventArgs e)
        {
            frmTransactionLog transactionLog = new frmTransactionLog(clsGlobal.CurrentUser.UserID.Value);
            transactionLog.ShowDialog();
        }

        private void ctrlUserInfo1_Load(object sender, EventArgs e)
        {

        }
    }
}
