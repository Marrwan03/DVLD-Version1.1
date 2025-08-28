using DVLD.Classes;
using DVLD.MyCustomeControls;
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

namespace DVLD.User
{
    public partial class frmAddUpdateCreditCard : Form
    {
        enum enMode { Add, Update}
        enMode _Mode;
        public Action<int> OnAddNewCreditCard;


        int _CardID;
        clsCreditCard _CreditCardInfo = new clsCreditCard();
        public frmAddUpdateCreditCard(int CardID)
        {
            InitializeComponent();
            _CardID = CardID;
            _Mode = enMode.Update;
        }
        public frmAddUpdateCreditCard()
        {
            InitializeComponent();
            _Mode = enMode.Add;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        void _RefreshData()
        {
            lblTitle.Text = "Add New Credit Card";
            mtxtCardNumber.Text = null;
            mtxtCVV.Text = null;
            cbTypeOfCreditCard1.SelectedIndex = 0;
          
        }

        void _LoadData()
        {
            lblTitle.Text = "Update Credit Card";
            _CreditCardInfo = clsCreditCard.Find(_CardID);
            if(_CreditCardInfo == null)
            {
                MessageBox.Show($"This CardID[{_CardID}] is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error); return;
            }
            ctrlCreditCard1.ctrlCreditCard_Load(_CreditCardInfo.CardID.Value);
            mtxtCardNumber.Text = _CreditCardInfo.CardNumber;
            mtxtCVV.Text = _CreditCardInfo.CVV;
            mtxtCardNumber.Enabled = false;
            mtxtCVV.Enabled = false;
            cbTypeOfCreditCard1.SelectedIndex = cbTypeOfCreditCard1.FindString(_CreditCardInfo.GetStringTypeOfCreditCard());
            cbTypeOfCreditCard1.Enabled = false;
            chbIsActive.Checked = _CreditCardInfo.IsActive;

        }

        private void frmAddUpdateCreditCard_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            chbIsActive.Checked = true;
            cbTypeOfCreditCard1.FillTypeOfCreditCard();

            if(_Mode == enMode.Update)
                _LoadData();
            else
                _RefreshData();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(!this.ValidateChildren())
            {
                MessageBox.Show("You have to fill all requred data", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int UserID = 0;
            if (_CreditCardInfo.UserID.HasValue)
                UserID = _CreditCardInfo.UserID.Value;
            else
                UserID = clsGlobal.CurrentUser.UserID.Value;

            if (clsCreditCard.IsUserHasCreditCardWithSameTypeBy(UserID, (byte)clsCreditCard.GetTypeOfCreditCard(cbTypeOfCreditCard1.Text)))
            {
                MessageBox.Show("You have already this card type and it`s Active,\nChoice another type", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (clsCreditCard.IsCardNumberExists(mtxtCardNumber.Text))
                {
                    MessageBox.Show("This Card number is already exists", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    mtxtCardNumber.Focus();
                    return;
                }

            _CreditCardInfo.CardNumber = mtxtCardNumber.Text;            
            _CreditCardInfo.UserID = UserID;
            _CreditCardInfo.Balance = 0;
            _CreditCardInfo.CVV = mtxtCVV.Text;
            _CreditCardInfo.IsActive = chbIsActive.Checked;
            _CreditCardInfo.CardType = clsCreditCard.GetTypeOfCreditCard(cbTypeOfCreditCard1.Text);

            if(_CreditCardInfo.Save())
            {
                MessageBox.Show("Saved is successfully :-)", "Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);
                ctrlCreditCard1.ctrlCreditCard_Load(_CreditCardInfo.CardID.Value);
                if (_Mode == enMode.Add)
                {
                    OnAddNewCreditCard?.Invoke(UserID);
                    _CardID = _CreditCardInfo.CardID.Value;
                }

                _Mode = enMode.Update;
                frmAddUpdateCreditCard_Load(null, null);
            }
            else
            {
                _RefreshData();
                MessageBox.Show("Saved is failed :-(", "failed!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void cbTypeOfCreditCard1_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlCreditCard1.typeOfCreditCard = clsCreditCard.GetTypeOfCreditCard(cbTypeOfCreditCard1.Text);
        }
    }
}
