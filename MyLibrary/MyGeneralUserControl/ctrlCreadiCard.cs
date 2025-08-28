using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DVLD.Classes;
using  DVLD_Buisness;

namespace MyControlLibrary
{
    public partial class ctrlCreadiCard: UserControl
    {
        public ctrlCreadiCard()
        {
            InitializeComponent();
        }

        bool _ShowBalance;
        public bool ShowBalance
        {
            get { return _ShowBalance; }
            set
            {
                _ShowBalance = value;
                lblBalance.Visible = value;
            }
        }
        
        clsCreditCard.enTypeOfCreditCard _typeOfCreditCard;
        public clsCreditCard.enTypeOfCreditCard typeOfCreditCard
        {
            get
            {
                return _typeOfCreditCard;
            }
            set
            {
                _typeOfCreditCard = value;
                lblTypeOfCreditCard.Text = clsCreditCard.GetStringTypeOfCreditCard(_typeOfCreditCard);
            }
        }

        DateTime _IssueDate;
        public DateTime IssueDate
        {
            get
            {
                return _IssueDate;
            }
            set
            {
                _IssueDate = value;
                lblIssueDate.Text = _IssueDate.ToString("MMM/dd");
            }
        }

        clsCreditCard _CardInfo = null;
        public clsCreditCard CardInfo { get { return _CardInfo; } }
        void _RefreshData()
        {
            lblTypeOfCreditCard.Text = "Null";
            lblNumberOfCardNumber.Text = "000 000 000 000";
            lblIssueDate.Text = DateTime.Now.ToString("MMM/dd");
            lblCVV.Text = "0000";
            if (lblBalance.Visible)
            {
                lblBalance.Text = "0 $";
            }

        }
        void _LoadData()
        {
            lblTypeOfCreditCard.Text = _CardInfo.GetStringTypeOfCreditCard();
            lblNumberOfCardNumber.Text = clsFormat.NumberOfCard(_CardInfo.CardNumber);
            lblIssueDate.Text = _CardInfo.IssueDate.ToString("MMM/dd");
            lblCVV.Text = _CardInfo.CVV;
            if (lblBalance.Visible)
            {
                lblBalance.Text = _CardInfo.Balance + " $";
            }
        }


        public void ctrlCreditCard_Load(int CardID)
        {
            _CardInfo = clsCreditCard.Find(CardID);
            if (CardInfo == null)
            {
                _RefreshData();
                return;
            }
            _LoadData();
        }

        public void ctrlCreditCard_Load(int UserID, clsCreditCard.enTypeOfCreditCard typeOfCreditCard)
        {
            _CardInfo = clsCreditCard.Find(UserID, typeOfCreditCard);
            if (CardInfo == null)
            {
                _RefreshData();
                return;
            }
            _LoadData();
        }

        private void ctrlCreadiCard_Load(object sender, EventArgs e)
        {
            ShowBalance = false;
            IssueDate = DateTime.Now;
        }
    }
}
