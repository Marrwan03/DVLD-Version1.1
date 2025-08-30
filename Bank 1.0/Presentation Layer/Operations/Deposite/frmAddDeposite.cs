using DataLogic2;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    public partial class frmAddDeposite : Form
    {
        int _FromUserID;
        clsCreditCard _FromCardInfo;
        clsCreditCard _ToCardInfo;
        clsDeposite _Deposite;
        DataTable _dtAllCreaditCard;
        public Action<int> AddNewDeposite;
        public frmAddDeposite(int FromUserID)
        {
            InitializeComponent();
            _FromUserID = FromUserID;
        }
        

        private void frmBankOperations_Load(object sender, EventArgs e)
        {

            this.Region = clsGlobal.CornerForm(Width, Height);
            _dtAllCreaditCard = clsCreditCard.FindByUserID(_FromUserID);
            if(_dtAllCreaditCard.Rows.Count > 0)
            {
                customcbYourCreaditCardsForSender.FillComboBoxBy(_dtAllCreaditCard);
                customcbYourCreaditCardsForSender.SelectedIndex = 0;
            }


            _FromCardInfo = clsCreditCard.Find(_FromUserID, clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCardsForSender.Text));
            lblTitle.Text = "Add New Deposite";
            _Deposite = new clsDeposite();

            lblFrom.Text = _FromCardInfo.UserInfo.PersonInfo.FullName;
            customcbYourCreaditCardsForSender.SelectedIndex = customcbYourCreaditCardsForSender.FindString(_FromCardInfo.GetStringTypeOfCreditCard());

        }
         bool _ValidateFloat(string Number)
        {
            // var pattern = @"^[0-9]*(?:\.[0-9]*)? $";
            var pattern = @"^[0-9]*(\.[0-9]*)? $";
            var regex = new Regex(@"^[0-9]*(\.[0-9]*)?$");

            return regex.IsMatch(Number);
        }
        private void txtAmount_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
                btnSend.PerformClick();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlFindCreditCard1_OnSelected(object sender, Controls.ctrlFindCreditCard.clsCreditCardEventArg e)
        {
            if(e.UserID == _FromUserID)
            {
                MessageBox.Show("You cannot do any deposite operation to your account", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlFindCreditCard1.RefreshData();
                ctrlFindCreditCard1.Focus();
                
                return;
            }

            _ToCardInfo = clsCreditCard.Find(e.UserID, e.typeOfCreditCard);
        }

        private void btnSend_Click(object sender, EventArgs e)
        {

            if(_ToCardInfo == null)
            {
                MessageBox.Show("You have to set recived card to complete this operation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if(string.IsNullOrEmpty(txtAmount.Text))
            {
                MessageBox.Show("You have to set amount to continue",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmount.Focus();
                return;
            }

            if(Convert.ToDecimal(txtAmount.Text) > _FromCardInfo.Balance)
            {
                MessageBox.Show($"This amount[{txtAmount.Text}] is upper than your balance,\nYou have [{_FromCardInfo.Balance}]$ in [{_FromCardInfo.GetStringTypeOfCreditCard()}]", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtAmount.Focus();
                return;
            }

            _Deposite.FromCardID = _FromCardInfo.CardID.Value; 
            _Deposite.DateTime = DateTime.Now;
            _Deposite.Amount = Convert.ToDecimal(txtAmount.Text);
            _Deposite.ToCardID = _ToCardInfo.CardID.Value;
            _Deposite.Note = txtNote.Text;
            if(_Deposite.Save() && _Deposite.CalculateBalanceAfterDeposite())
            {
                MessageBox.Show("Send successfully :-)", "Successfully",
                                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                AddNewDeposite?.Invoke(_Deposite.DepositeID.Value);
                ctrlFindCreditCard1.gbFilterEnabled = false;
                ctrlFindCreditCard1.customcbYourCreaditCardsEnabled = false;
                lblTime.Text = _Deposite.DateTime.ToString("dd-MM  hh:MM:ss");
                btnSend.Enabled = false;
                gbDepositeOp.Enabled = false;
                btnClose.Focus();
            }
            else
            {
                MessageBox.Show("Send Faile :-(", "Failed", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void txtAmount_Validating(object sender, CancelEventArgs e)
        {
            if(!_ValidateFloat(txtAmount.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtAmount, "The format of amount is wrong");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(txtAmount,null);
            }
        }

        private void ctrlFindCreditCard1_Load(object sender, EventArgs e)
        {

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void customcbYourCreaditCards1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FromCardInfo = clsCreditCard.Find(_FromUserID, clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCardsForSender.Text));
        }
    }
}
