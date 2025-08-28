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

namespace DVLD.Tests.Controls
{
    public partial class ctrlAppointmentInfo : UserControl
    {
        int _TestAppointmentID;
        clsTestAppointment _TestAppointmentInfo;
        public int TestAppointmentID { get { return _TestAppointmentID; } }
        public clsTestAppointment TestAppointmentInfo { get { return _TestAppointmentInfo; } }

        void _RefreshData()
        {
            string Data = "[???]";
            lblAppointmentID.Text = Data;
            lblTestType.Text = Data;
            lblFees.Text = Data;
            lblStatus.Text = Data;
            lblAppontmentDate.Text = Data;
            lblClassType.Text = Data;
            lblIsPass.Text = Data;
            lblCreatedBy.Text = Data;
            lblIsPaid.Text = Data;
            lblPaidID.Text = Data;
        }

        void _FillData()
        {
            lblAppointmentID.Text = _TestAppointmentInfo.TestAppointmentID.ToString();
            lblTestType.Text = _TestAppointmentInfo.TestTypeInfo.Title;
            lblFees.Text = _TestAppointmentInfo.PaidFees.ToString();
            lblStatus.Text =
                _TestAppointmentInfo.Status == clsTestAppointment.enStatus.Locked ? "Locked" :
                (_TestAppointmentInfo.Status == clsTestAppointment.enStatus.NotLocked? "NotLocked":
                "Deleted");
            lblAppontmentDate.Text = clsFormat.DateToShort(_TestAppointmentInfo.AppointmentDate);
            lblClassType.Text = _TestAppointmentInfo.LocalDrivingLicenseApplicationInfo.LicenseClassInfo.ClassName;
            lblIsPass.Text = _TestAppointmentInfo.TestInfo != null ? (_TestAppointmentInfo.TestInfo.TestResult? "Yes" : "No") : "in progress";
            lblCreatedBy.Text = _TestAppointmentInfo.CreatedByEmployeeInfo.UserName;
            lblIsPaid.Text = _TestAppointmentInfo.IsPaid() ? "Yes" : "No";
            lblPaidID.Text = _TestAppointmentInfo.PaymentID.HasValue ? _TestAppointmentInfo.PaymentID.Value.ToString() : "[???]";
        }

        public void ctrlAppointmentInfo_Load(int TestAppointmentID)
        {
            _TestAppointmentID = TestAppointmentID;
            _TestAppointmentInfo = clsTestAppointment.Find(_TestAppointmentID);
            if(_TestAppointmentInfo == null )
            {
                MessageBox.Show($"This Appointment With ID[{_TestAppointmentID}] is not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _RefreshData();
                return;
            }
            _FillData();
        }

        public ctrlAppointmentInfo()
        {
            InitializeComponent();
        }
    }
}
