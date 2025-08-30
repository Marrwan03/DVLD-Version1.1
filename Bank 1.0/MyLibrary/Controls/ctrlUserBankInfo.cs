using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank.Controls
{
    public partial class ctrlUserBankInfo : UserControl
    {
        int _CardID;
        int _UserID;
        public ctrlUserBankInfo()
        {
            InitializeComponent();
            
        }
        public enum enStatus { Sender, Recipient}
        enStatus _Status;

        clsCreditCard _CreditCard;

       public void RefreshData()
        {
            string Data = "[ ??? ]";
            lblFullName.Text = Data;
            lblCardType.Text = Data;
            lblUsername.Text = Data;
            lblStatus.Text = Data;
        }

        void _LoadData()
        {
            lblFullName.Text = _CreditCard.UserInfo.PersonInfo.FullName;
            lblCardType.Text = _CreditCard.GetStringTypeOfCreditCard();
            lblUsername.Text = _CreditCard.UserInfo.UserName;
            switch(_Status)
            {
                case enStatus.Sender:
                    {
                        lblStatus.Text = "Sender";
                        break;
                    }
                    case enStatus.Recipient:
                    {
                        lblStatus.Text = "Recipient";
                        break;
                    }
            }
            
        }

        public void ctrlUserBankInfo_Load(int CardID, enStatus Status)
        {
            _CardID = CardID;
            _CreditCard = clsCreditCard.Find(CardID);

            if (_CreditCard != null)
            {
                _Status = Status;
                _LoadData();
            }
            else
            {
                if(MessageBox.Show($"this cardID {CardID} is not found", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    RefreshData();
                }
            }


        }
        public void ctrlUserBankInfo_Load(int UserID,clsCreditCard.enTypeOfCreditCard typeOfCreditCard, enStatus Status)
        {
            _UserID = UserID;
            _CreditCard = clsCreditCard.Find(_UserID, typeOfCreditCard);

            if (_CreditCard != null)
            {
                _Status = Status;
                _LoadData();
            }
            else
            {
                if (MessageBox.Show($"this userID {_UserID} is not found", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    RefreshData();
                }
            }


        }

    }
}
