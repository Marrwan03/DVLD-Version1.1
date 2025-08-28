using DVLD_Buisness;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Dashboards.User.Controls
{
    public partial class ctrlPaymentsSection : UserControl
    {
        public ctrlPaymentsSection()
        {
            InitializeComponent();
        }

        void _SetSettingsForCircleProgress(Guna2CircleProgressBar PaymentCircle,
            Guna2CircleProgressBar NotPaymentCircle,
            int TotalRecord, int RecordOfPayment)
        {
            PaymentCircle.Maximum = TotalRecord;
            NotPaymentCircle.Maximum = PaymentCircle.Maximum;

            PaymentCircle.Value = RecordOfPayment;
            NotPaymentCircle.Value = NotPaymentCircle.Maximum - RecordOfPayment;
        }

        public void ctrlPaymentsSection_Load(int PersonID)
        {
           // set all Application record number then - PaymentNumber
           clsPerson person = clsPerson.Find(PersonID);
            if(person == null)
            {
                MessageBox.Show($"this personID[{PersonID}] is not found.",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _SetSettingsForCircleProgress(pbPaymentApplication, pbNotPaymentApplication,
                person.GetNumberOfAllTypesApplicationBy(),
                 clsPayment.NumberOfPaymentsBy(clsPayment.enPaymentType.Application, PersonID));

            _SetSettingsForCircleProgress(pbPaymentAppointment, pbNotPaymentAppointment,
                person.GetNumberOfRowsForAppTestAppointmentsBy(),
                clsPayment.NumberOfPaymentsBy(clsPayment.enPaymentType.Appointment, PersonID));
        }


    }
}
