using DVLD.Classes;
using DVLD.Properties;
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
using static DVLD_Buisness.clsTestType;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Tests
{

  
    public partial class ctrlScheduleTest : UserControl
    {
        public class ScheduleInfo : EventArgs
        {
            public bool Result { get; }
            public int? TestAppointmentID { get; }

            public ScheduleInfo(bool result, int? testAppointmentID)
            {
                Result = result;
                TestAppointmentID = testAppointmentID;
            }
        }

        public event EventHandler<ScheduleInfo> OnSaveSchedule;
       
        public void FocusOnDate()
        {
            dtpTestDate.Focus();
        }
        public enum enMode { AddNew = 0, Update = 1 };
        private enMode _Mode=enMode.AddNew;
        public enum enCreationMode { FirstTimeSchedule=0, RetakeTestSchedule= 1 };
        private enCreationMode _CreationMode=enCreationMode.FirstTimeSchedule;
        private int? _ActiveRetakeAppID = null;
        private clsApplication _AppInfo;

        private clsTestType.enTestType _TestTypeID =clsTestType.enTestType.VisionTest; 
        private clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        private int _LocalDrivingLicenseApplicationID = -1;
        private clsTestAppointment _TestAppointment;
        private int _TestAppointmentID = -1;

        public clsTestType.enTestType TestTypeID
        {
            get
            {
                return _TestTypeID;
            }
            set
            {
                _TestTypeID = value; 

                switch (_TestTypeID)
                {

                    case clsTestType.enTestType.VisionTest:
                        {
                            gbTestType.Text = "Vision Test";
                            pbTestTypeImage.Image = Resources.Vision_512;
                            break;
                        }

                    case clsTestType.enTestType.WrittenTest:
                        {
                            gbTestType.Text = "Written Test";
                            pbTestTypeImage.Image = Resources.Written_Test_512;
                            break;
                        }
                    case clsTestType.enTestType.StreetTest:
                        {
                            gbTestType.Text = "Street Test";
                            pbTestTypeImage.Image = Resources.driving_test_512;
                            break;
                        

                        }
                }
            }
        }

        public void LoadInfo(int LocalDrivingLicenseApplicationID,int AppointmentID= -1 )
        {
            //if no appointment id this means AddNew mode otherwise it's update mode.
            if (AppointmentID == -1)
                _Mode = enMode.AddNew;
            else
                _Mode = enMode.Update;

            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            _TestAppointmentID = AppointmentID;     
          
            _LocalDrivingLicenseApplication =  clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

           if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("Error: No Local Driving License Application with ID = " + _LocalDrivingLicenseApplicationID.ToString(),
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return;
            }

           //decide if the createion mode is retake test or not based if the person attended this test before
            if (_LocalDrivingLicenseApplication.DoesAttendTestType(_TestTypeID))
            
                _CreationMode = enCreationMode.RetakeTestSchedule;
            else
                _CreationMode = enCreationMode.FirstTimeSchedule;


            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                lblRetakeAppFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                //create GetActiveRetakeAppID and check from testappointmentTable [testtype]
                _ActiveRetakeAppID = _LocalDrivingLicenseApplication.ApplicationInfo.GetActiveApplicationID(clsApplication.enApplicationType.RetakeTest);
                if(_ActiveRetakeAppID.HasValue)
                {
                 _AppInfo = clsApplication.FindBaseApplication(_ActiveRetakeAppID.Value);
                    if(_AppInfo == null)
                    {
                        MessageBox.Show($"We cannot Find This Application By : [{_ActiveRetakeAppID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    dtpTestDate.Enabled = _AppInfo.IsPaid();
                    lblRetakeTestAppID.Text = _AppInfo.ApplicationID.ToString(); 
                }    
                else
                {
                lblRetakeTestAppID.Text = "0";
                }

            }
            else
            {
                gbRetakeTestInfo.Enabled = false;
                lblTitle.Text = "Schedule Test";
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }

            lblLocalDrivingLicenseAppID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblDrivingClass.Text = _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName;
            lblFullName.Text = _LocalDrivingLicenseApplication.PersonFullName;

            //this will show the trials for this test before  
            lblTrial.Text = _LocalDrivingLicenseApplication.TotalTrialsPerTest(_TestTypeID).ToString();
            lblFees.Text = clsTestType.Find(_TestTypeID).Fees.ToString();

            if (_Mode==enMode.AddNew)
            {
               
                dtpTestDate.MinDate = clsTestAppointment.GetLastAppointmentDateBy(_LocalDrivingLicenseApplicationID);
              //  lblRetakeTestAppID.Text = "N/A";
                _TestAppointment = new clsTestAppointment();
            }

           else
            {

                if (!_LoadTestAppointmentData())
                    return;
            }

         
            lblTotalFees.Text= (Convert.ToSingle(lblFees.Text) + Convert.ToSingle(lblRetakeAppFees.Text)).ToString();


           if (!_HandleActiveTestAppointmentConstraint())
                return;

            if (!_HandleAppointmentLockedConstraint())
                return;

            if (!_HandlePrviousTestConstraint())
                return;
            


        }
        private bool _HandleActiveTestAppointmentConstraint()
        {
            if (_Mode == enMode.AddNew && clsLocalDrivingLicenseApplication.IsThereAnActiveScheduledTest(_LocalDrivingLicenseApplicationID, _TestTypeID))
            {
                lblUserMessage.Text = "Person Already have an active appointment for this test";
                btnSave.Enabled = false;
                dtpTestDate.Enabled = false;
                return false;
            }

            return true;
        }
        private bool _LoadTestAppointmentData()
        {
            _TestAppointment = clsTestAppointment.Find(_TestAppointmentID);

            if (_TestAppointment == null)
            {
                MessageBox.Show("Error: No Appointment with ID = " + _TestAppointmentID.ToString(),
                "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                btnSave.Enabled = false;
                return false;
            }

          //  lblFees.Text = _TestAppointment.PaidFees.ToString();

            //we compare the current date with the appointment date to set the min date.
            if (DateTime.Compare(DateTime.Now, _TestAppointment.AppointmentDate) < 0)
                dtpTestDate.MinDate = DateTime.Now;
            else
                dtpTestDate.MinDate = _TestAppointment.AppointmentDate;

            dtpTestDate.Value = _TestAppointment.AppointmentDate;

            if (!_TestAppointment.RetakeTestApplicationID.HasValue)
            {
                lblRetakeAppFees.Text = "0";
                lblRetakeTestAppID.Text = "N/A";
            }
            else
            {
                lblRetakeAppFees.Text = _TestAppointment.RetakeTestAppInfo.PaidFees.ToString();
                gbRetakeTestInfo.Enabled = true;
                lblTitle.Text = "Schedule Retake Test";
                lblRetakeTestAppID.Text = _TestAppointment.RetakeTestApplicationID.ToString();

            }
            return true;
        }
        private bool _HandleAppointmentLockedConstraint()
        {
            //if appointment is locked that means the person already sat for this test
            //we cannot update locked appointment

            
            if (_TestAppointment.Status == clsTestAppointment.enStatus.Locked)
            {
                lblUserMessage.Visible = true;
                lblUserMessage.Text = "Person already sat for the test, appointment loacked.";
                dtpTestDate.Enabled = false;
                btnSave.Enabled = false;
                return false;

            }
            else
                lblUserMessage.Visible = false;

            return true;
        }
        private bool _HandlePrviousTestConstraint()
        {
            //we need to make sure that this person passed the prvious required test before apply to the new test.
            //person cannno apply for written test unless s/he passes the vision test.
            //person cannot apply for street test unless s/he passes the written test.

            switch (TestTypeID)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        //in this case no required prvious test to pass.
                        lblUserMessage.Visible = false;

                        return true;
                    }
                case clsTestType.enTestType.WrittenTest:
                    //Written Test, you cannot sechdule it before person passes the vision test.
                    //we check if pass visiontest 1.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.VisionTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Vision Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

                case clsTestType.enTestType.StreetTest:

                    //Street Test, you cannot sechdule it before person passes the written test.
                    //we check if pass Written 2.
                    if (!_LocalDrivingLicenseApplication.DoesPassTestType(clsTestType.enTestType.WrittenTest))
                    {
                        lblUserMessage.Text = "Cannot Sechule, Written Test should be passed first";
                        lblUserMessage.Visible = true;
                        btnSave.Enabled = false;
                        dtpTestDate.Enabled = false;
                        return false;
                    }
                    else
                    {
                        lblUserMessage.Visible = false;
                        btnSave.Enabled = true;
                        dtpTestDate.Enabled = true;
                    }


                    return true;

            }
            return true;

        }
        private bool _HandleRetakeApplication()
        {
            //this will decide to create a seperate application for retake test or not.
            // and will create it if needed , then it will linkit to the appoinment.
            if (_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                //Refresh
                _ActiveRetakeAppID = _LocalDrivingLicenseApplication.ApplicationInfo.GetActiveApplicationID(clsApplication.enApplicationType.RetakeTest);

                if (_ActiveRetakeAppID.HasValue)
                {
                    _AppInfo = clsApplication.FindBaseApplication(_ActiveRetakeAppID.Value);
                    if (_AppInfo == null)
                        return false;
                    else if (!_AppInfo.IsPaid())
                    {
                        MessageBox.Show("This User cannot continue with his appointment,\nBecause he doesn`t pay his Retake application Fees.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }

                }

                else
                {
                    //incase the mode is add new and creation mode is retake test we should create a seperate application for it.
                    //then we linke it with the appointment.

                    //First Create Applicaiton 
                    clsApplication Application = new clsApplication();
                    Application.ApplicantPersonID = _LocalDrivingLicenseApplication.ApplicationInfo.ApplicantPersonID;
                    Application.ApplicationDate = DateTime.Now;
                    Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RetakeTest;
                    Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                    Application.LastStatusDate = DateTime.Now;
                    Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RetakeTest).Fees;
                    Application.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID ?? -1;

                    if (!Application.Save())
                    {
                        _TestAppointment.RetakeTestApplicationID = null;
                        MessageBox.Show("Faild to Create application", "Faild",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);                       
                    }
                    else
                    {
                        MessageBox.Show("Add new Retake Application, User has to pay it to continue with his Appointment", "Pay Retake App",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblRetakeTestAppID.Text = Application.ApplicationID.ToString();
                    }
                    return false;
                }
                
            }
            return true;
        }

        public ctrlScheduleTest()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_CreationMode == enCreationMode.RetakeTestSchedule)
            {
                if (!_HandleRetakeApplication())
                    return;
            }

            _TestAppointment.TestTypeID = _TestTypeID;
            _TestAppointment.LocalDrivingLicenseApplicationID = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID;
            _TestAppointment.AppointmentDate = dtpTestDate.Value;
            _TestAppointment.PaidFees = Convert.ToSingle(lblFees.Text);
            _TestAppointment.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID ?? -1;
            if(_ActiveRetakeAppID.HasValue)
            {
            _TestAppointment.RetakeTestApplicationID = _ActiveRetakeAppID;
                _TestAppointment.RetakeTestAppInfo = _AppInfo;

            }

            bool Saved = _TestAppointment.Save();
            int? TestAppointmentID = null;
            if (Saved)
            {
                _Mode = enMode.Update;
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                TestAppointmentID = _TestAppointment.TestAppointmentID;
                clsUtil.ChangeOrderOfAppointmentsToPaidFor(
                    clsUser.FindByPersonID(_TestAppointment.LocalDrivingLicenseApplicationInfo.ApplicationInfo.ApplicantPersonID).UserID.Value,
                    _TestTypeID, clsUtil.enOperationOrder.delete);
            }
            else
            {
                MessageBox.Show($"Error: Data Is not Saved Successfully,\nCheck about the date you should set upper than LastAppointmentDate[{clsFormat.DateToShort(clsTestAppointment.GetLastAppointmentDateBy(_LocalDrivingLicenseApplicationID))}].", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            OnSaveSchedule?.Invoke(this, new ScheduleInfo(Saved, TestAppointmentID));
    }

        private void gbTestType_Enter(object sender, EventArgs e)
        {

        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {

        }

        private void label7_Click(object sender, EventArgs e)
        {

        }

        private void dtpTestDate_Validating(object sender, CancelEventArgs e)
        {
            if (clsTestAppointment.IsDatetimeIntertwined(_LocalDrivingLicenseApplication.GetLastDateTime(), dtpTestDate.Value))
            {
                e.Cancel = true;
                errorProvider1.SetError(dtpTestDate, "This Date already use it, Choice another date");
            }
            else
            {
                e.Cancel = false;
                errorProvider1.SetError(dtpTestDate, null);
            }
        }
    }
}
