using DVLD.Applications.Controls;
using DVLD.Classes;
using DVLD.Payments.Controls;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.User
{
    public partial class frmPaymentOf : Form
    {
        public event Action<bool> OnPayment;
        int _UserID;
        clsCreditCard.enTypeOfCreditCard _TypeOfCreditCard;
        int _ID;
        float _Fees;
        ctrlPaymentFor.enPaymentFor _TypeOfPayment;

        public frmPaymentOf(int ID, float fees, ctrlPaymentFor.enPaymentFor typeOfPayment,clsCreditCard.enTypeOfCreditCard typeOfCreditCard)
        {
            InitializeComponent();

            _ID = ID;
            _Fees = fees;
            _TypeOfPayment = typeOfPayment;
            _TypeOfCreditCard = typeOfCreditCard;
            ctrlPaymentFor1.TypeOfPayment = _TypeOfPayment;

            if (typeOfPayment == ctrlPaymentFor.enPaymentFor.Application)
                _UserID = clsUser.FindByPersonID(clsApplication.FindBaseApplication(ID).ApplicantPersonID).UserID.Value;
            else
                _UserID = clsUser.FindByPersonID(clsTestAppointment.Find(ID).LocalDrivingLicenseApplicationInfo.ApplicationInfo.ApplicantPersonID).UserID.Value;
        }

        private void frmPaymentOfApplication_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
           ctrlPaymentFor1.ctrlPaymentFor_Load(_TypeOfPayment, _ID, _Fees, _TypeOfCreditCard);
            ctrlCreditCard1.ctrlCreditCard_Load(_UserID, _TypeOfCreditCard);
            ctrlCreditCard1.ShowBalance = true;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlPaymentFor1_OnPayment_1(object sender, ctrlPaymentFor.clsPaymentInfo e)
        {
            ctrlCreditCard1.ctrlCreditCard_Load(_UserID, _TypeOfCreditCard);
            if (e.Result)
            {
                if (e.paymentFor == ctrlPaymentFor.enPaymentFor.Application)
                {

                    if (e.PaymentInfo.AppInfo != null)
                    {
                        if (clsUtil.ChangeOrderOfApplicationsToPaidFor(_UserID,
                                e.PaymentInfo.AppInfo.ApplicationTypeID,
                                clsUtil.enOperationOrder.update))
                        {
                            MessageBox.Show("Record in file is updated.");
                        }
                    }
                }
                else
                {
                    if (e.PaymentInfo.AppointmentInfo != null) 
                    {
                       if( clsUtil.ChangeOrderOfAppointmentsToPaidFor(_UserID,
                            e.PaymentInfo.AppointmentInfo.TestTypeID, clsUtil.enOperationOrder.update))
                        {
                            MessageBox.Show("Record in file is updated.");
                        }
                    }
                    
                }
            }
                OnPayment?.Invoke(e.Result);
        }
    }
}
