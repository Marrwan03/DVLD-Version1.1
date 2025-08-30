using Bank.Custome;
using DataLogic2;
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

namespace Bank
{
    public partial class frmShowBalance : Form
    {
        int _UserID;
        clsDeposite _LastDepositeLog;
        clsDeposite _LastWithdrawLog;
        DataTable _dtAllYourCards;
        public frmShowBalance(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;

        }

        private void frmShowBalance_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _dtAllYourCards = clsCreditCard.FindByUserID(_UserID);
            customcbYourCreaditCards2.FillComboBoxBy(_dtAllYourCards);
            if(_dtAllYourCards.Rows.Count > 0)
                customcbYourCreaditCards2.SelectedIndex = 0;


        }

        private void customcbYourCreaditCards2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int CardID = (int)_dtAllYourCards.Rows[customcbYourCreaditCards2.SelectedIndex]["CardID"];
            lblNumberOfDeposite.Text = clsDeposite.GetNumberOfDepositeBy(CardID).ToString();
            btnLastDeposite.Tag = "Show";
            ctrlUserBankInfoForDeposite1.Visible = false;

            lblNumberOfWithdraw.Text = clsDeposite.GetNumberOfWithdrawBy(CardID).ToString();
            btnLastWithdraw.Tag = "Show";
            ctrlUserBankInfoForWithdraw1.Visible = false;

            lblBalance.Text = clsCreditCard.Find(CardID).Balance.ToString() + " $";
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _FillDepositeLog(int DepositeID)
        {
            ctrlUserBankInfoForDeposite1.Visible = true;
            ctrlUserBankInfoForDeposite1.ctrlUserBankInfo_Load(clsDeposite.Find(DepositeID).ToCardID.Value,
                Bank.Controls.ctrlUserBankInfo.enStatus.Recipient);
        }

        private void btnLastDeposite_Click(object sender, EventArgs e)
        {
            if(btnLastDeposite.Tag.ToString() == "Show")
            {
                btnLastDeposite.Tag = "Hide";
                ctrlUserBankInfoForDeposite1.Visible = true;
                _LastDepositeLog = clsDeposite.FindLastDepositeBy((int)_dtAllYourCards.Rows[customcbYourCreaditCards2.SelectedIndex]["CardID"]);
            }
            else
            {
                btnLastDeposite.Tag = "Show";
                ctrlUserBankInfoForDeposite1.Visible = false;
                return;
            }
           
            if(_LastDepositeLog == null)
            {
                if(MessageBox.Show("You don`t have any Deposite :-(", "Non Deposite", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    ctrlUserBankInfoForDeposite1.RefreshData();
                    if(MessageBox.Show("Do you want add New Deposite?", "Add new Deposite", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        frmAddDeposite AddNewDeposite = new frmAddDeposite(_UserID);
                        AddNewDeposite.AddNewDeposite += _FillDepositeLog;
                        AddNewDeposite.ShowDialog();
                       
                    }
                }
                

            }
            else
            {
                ctrlUserBankInfoForDeposite1.ctrlUserBankInfo_Load(_LastDepositeLog.ToCardID.Value,
                    Bank.Controls.ctrlUserBankInfo.enStatus.Recipient);
            }

        }

        private void btnLastWithdraw_Click(object sender, EventArgs e)
        {
            if(btnLastWithdraw.Tag.ToString() == "Show")
            {
                btnLastWithdraw.Tag = "Hide";
                ctrlUserBankInfoForWithdraw1.Visible = true;
                //if(customcbYourCreaditCards2)
                _LastWithdrawLog = clsDeposite.FindLastWithdrawBy((int)_dtAllYourCards.Rows[customcbYourCreaditCards2.SelectedIndex]["CardID"]);
            }
            else
            {
                btnLastWithdraw.Tag = "Show";
                ctrlUserBankInfoForWithdraw1.Visible = false;
                return;
            }

            if(_LastWithdrawLog == null)
            {
                MessageBox.Show("You don`t have any Withdraw :-(", "Empty Withdraw", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlUserBankInfoForWithdraw1.RefreshData();
            }
            else
            {
                ctrlUserBankInfoForWithdraw1.ctrlUserBankInfo_Load(_LastWithdrawLog.FromCardID.Value,
                    Bank.Controls.ctrlUserBankInfo.enStatus.Sender);
            }

        }
    }
}
