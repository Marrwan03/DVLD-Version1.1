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

namespace DVLD.Tests
{
    public partial class frmScheduleTest: Form
    {


        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestType.enTestType _TestTypeID = clsTestType.enTestType.VisionTest;
        private int _AppointmentID=-1;

        public frmScheduleTest(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID, int AppointmentID=-1)
        {
            
            InitializeComponent();
            
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _TestTypeID = TestTypeID;
            _AppointmentID = AppointmentID;
           

        }

        private void frmScheduleTest_Load(object sender, EventArgs e)
        {
            
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlScheduleTest1.TestTypeID= _TestTypeID;
            ctrlScheduleTest1.LoadInfo(_LocalDrivingLicenseApplicationID, _AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlScheduleTest1_OnSaveSchedule(object sender, ctrlScheduleTest.ScheduleInfo e)
        {
            if(e.Result)
            {
                lblTitle.Text = $"Update A Schedule Test ID[{e.TestAppointmentID}]";
            }
        }
    }
}
