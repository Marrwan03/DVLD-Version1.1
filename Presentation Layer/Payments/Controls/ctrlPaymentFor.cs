using DVLD.Employee;
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
    public partial class ctrlPaymentFor : UserControl
    {

       public enum enPaymentFor { Application, Appointment};
        enPaymentFor _TypeOfPayment= enPaymentFor.Appointment;

        void _VisibleControls(bool AppInfo, bool AppIntInfo)
        {
            ctrlApplicationBasicInfo1.Visible = AppInfo;
            ctrlAppointmentInfo1.Visible = AppIntInfo; 
        }

        void _CalculateTheControlsBy(enPaymentFor Type)
        {
            switch (Type)
            {
                case enPaymentFor.Application:
                    {
                        lblTitle.Text = "Application";
                        _VisibleControls(true, false);                        
                        break;
                    }

                    case enPaymentFor.Appointment:
                    {
                        lblTitle.Text = "Appointment";
                        _VisibleControls(false, true);
                        break;
                    }
            }
        }

        public enPaymentFor TypeOfPayment
        {
            get
            {
                return _TypeOfPayment;
            }
            set
            {
                _TypeOfPayment = value;
                _CalculateTheControlsBy(_TypeOfPayment);
            }
        }
        public class clsPaymentInfo : EventArgs
        {
            public enPaymentFor paymentFor { get; }
            public int PaymentID { get; }
            public bool Result { get; }
          public (clsApplication AppInfo, clsTestAppointment AppointmentInfo) PaymentInfo { get; }

            public clsPaymentInfo(enPaymentFor paymentFor, int paymentID, bool result, (clsApplication AppInfo, clsTestAppointment AppointmentInfo) paymentInfo)
            {
                this.paymentFor = paymentFor;
                PaymentID = paymentID;
                Result = result;
                PaymentInfo = paymentInfo;
            }
        }
        public event EventHandler<clsPaymentInfo> OnPayment;
        public ctrlPaymentFor()
        {
            InitializeComponent();
        }

        (clsApplication AppInfo, clsTestAppointment AppointmentInfo) _Info =(null, null);
        int _ID;
        int? _UserID;
        float _Fees;
        clsCreditCard.enTypeOfCreditCard _typeOfCreditCard;
        public void ctrlPaymentFor_Load(enPaymentFor typeOfPayment,int ID, float Fess, clsCreditCard.enTypeOfCreditCard typeOfCreditCard)
        {
            _ID = ID;
            _Fees = Fess;
            _typeOfCreditCard = typeOfCreditCard;
            TypeOfPayment = typeOfPayment;
           switch(TypeOfPayment)
            {
                case enPaymentFor.Application:
                    {
                        _Info.AppInfo = clsApplication.FindBaseApplication(_ID);
                        if(_Info.AppInfo!=null)
                        {
                            _UserID = clsUser.FindByPersonID(_Info.AppInfo.ApplicantPersonID).UserID.Value;
                            ctrlApplicationBasicInfo1.LoadApplicationInfo(ID); 
                        }
                        break;
                    }
                case enPaymentFor.Appointment:
                    {
                        _Info.AppointmentInfo = clsTestAppointment.Find(_ID);
                        if(_Info.AppointmentInfo!=null)
                        {
                            _UserID = clsUser.FindByPersonID(_Info.AppointmentInfo.LocalDrivingLicenseApplicationInfo.ApplicationInfo.ApplicantPersonID).UserID.Value;
                            ctrlAppointmentInfo1.ctrlAppointmentInfo_Load(ID);
                        }
                        break;
                    }
            }
        }

     

       
        private void ctrlPaymentFor_Load(object sender, EventArgs e)
        {

        }

        clsPayment _payment;
        private void btnPayit_Click(object sender, EventArgs e)
        {
            if(!_UserID.HasValue)
            {
                MessageBox.Show("There is a problem you cannot pay,\nCheck [ID & typeOfPayment]", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            clsCreditCard creditCard = clsCreditCard.Find(_UserID.Value, _typeOfCreditCard);
            if (creditCard != null)
            {
                if (creditCard.Payment(_Fees))
                {
                    _payment = new clsPayment();
                    _payment.CardId = creditCard.CardID;
                    _payment.Amount = (decimal)_Fees;
                    _payment.DateTime = DateTime.Now;

                    if (_payment.Save())
                    {
                        bool Success = false;
                        switch (_TypeOfPayment)
                        {
                            case enPaymentFor.Application:
                                {
                                    Success = _Info.AppInfo.SetNewPaymentIDFor(_payment.PaymentID);
                                    break;
                                }
                            case enPaymentFor.Appointment:
                                {
                                    Success = _Info.AppointmentInfo.SetNewPaymentIDFor(_payment.PaymentID);
                                    break;
                                }
                        }

                        if (!Success)
                        {
                            MessageBox.Show($"Set New Payment is failed", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        if (MessageBox.Show($"Successfull Payment, PaymentID[{_payment.PaymentID.Value}]", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                        {
                            ctrlPaymentFor_Load(_TypeOfPayment, _ID, _Fees, _typeOfCreditCard);
                            _payment.CreditCardInfo = clsCreditCard.Find(_payment.CardId.Value);
                        }

                        OnPayment?.Invoke(this, new clsPaymentInfo(_TypeOfPayment, _payment.PaymentID.Value, Success, _Info));
                        btnPayit.Enabled = false;
                    }
                    else
                    {
                        MessageBox.Show($"Failed Payment", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else
                {
                    MessageBox.Show($"You don`t have money to paid this fees[{_Fees} $],\nYou have [{creditCard.Balance} $] in your card",
                   "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show($"We cannot find any creditcard by [UserID: [{_UserID}], Type: [{clsCreditCard.GetStringTypeOfCreditCard((byte)_typeOfCreditCard)}] ].",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
    }
}
