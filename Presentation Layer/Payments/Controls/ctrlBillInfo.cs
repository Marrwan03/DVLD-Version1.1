using DVLD.Classes;
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

namespace DVLD.Payments.Controls
{
    public partial class ctrlBillInfo : UserControl
    {
        public ctrlBillInfo()
        {
            InitializeComponent();
        }
        clsPayment _PaymentInfo;
        int _PaymentID;

        public clsPayment PaymentInfo { get { return _PaymentInfo; } }
        public int PaymentID { get { return _PaymentID;} }

        public void ctrlBillInfo_Load(int PaymentID)
        {
            _PaymentID = PaymentID;
            _PaymentInfo = clsPayment.Find(_PaymentID);

            if(_PaymentInfo == null )
            {
                MessageBox.Show($"This PaymentID [{_PaymentID}] is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _RefreshData();
                return;
            }
            _LoadData();
        }

        void _RefreshData()
        {
            string Data = "[ ??? ]";
            lblPaymentID.Text = Data;
            lblCardType.Text = Data;
            lblAmount.Text = Data;
            lblTime.Text = Data;
            lblPaymentBy.Text = Data;
        }

        void _LoadData()
        {
            lblPaymentID.Text = _PaymentID.ToString();
            lblCardType.Text = _PaymentInfo.CreditCardInfo.GetStringTypeOfCreditCard();
            lblAmount.Text = _PaymentInfo.Amount.ToString();
            lblTime.Text = _PaymentInfo.DateTime.ToString("g");
            lblPaymentBy.Text = _PaymentInfo.CreditCardInfo.UserInfo.PersonInfo.FullName;
        }

    }
}
