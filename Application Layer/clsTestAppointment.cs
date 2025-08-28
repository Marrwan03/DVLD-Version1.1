using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using DVLD_Buisness.Interfaces;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsTestAppointment : IReturn
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enStatus { NotLocked, Locked, Deleted };
        public enStatus Status = enStatus.NotLocked;
        public int TestAppointmentID { set; get; }
        public clsTestType.enTestType TestTypeID { set; get; }
        clsTestType _TestTypeInfo;
        public clsTestType TestTypeInfo { get { return _TestTypeInfo; } }
        int _LocalDrivingLicenseApplicationID;
        public int LocalDrivingLicenseApplicationID 
        {
            set 
            {
                _LocalDrivingLicenseApplicationID = value;
                LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(value);
            }
            get {  return _LocalDrivingLicenseApplicationID;}
        }
        public clsLocalDrivingLicenseApplication LocalDrivingLicenseApplicationInfo { set; get; }
        public DateTime AppointmentDate { set; get; }
        public float PaidFees { set; get; }
        public int CreatedByEmployeeID { set; get; }
        public clsEmployee CreatedByEmployeeInfo { set; get; }
        public int? RetakeTestApplicationID { set; get; }
        public clsApplication RetakeTestAppInfo { set; get; }
        public int?PaymentID { set; get; }
        public clsPayment AppointmentPaymentInfo { set; get; }
        public bool IsPaid() => PaymentID.HasValue;

        public int?  TestID   
        {
            get { return _GetTestID(); }   
          
        }
        public clsTest TestInfo {
            get
            {
                if(TestID.HasValue)
                    return clsTest.Find(TestID.Value);
                else
                    return null;
            } 
        }

        public clsTestAppointment()

        {
            this.TestAppointmentID = -1;
            this.TestTypeID = clsTestType.enTestType.VisionTest;
            this.AppointmentDate = DateTime.Now;
            this.PaidFees = 0;
            this.CreatedByEmployeeID = -1;
            this.RetakeTestApplicationID = null;
            LocalDrivingLicenseApplicationInfo = null;
            AppointmentPaymentInfo = null;
            PaymentID = null;
            
            Mode = enMode.AddNew;
            Status= enStatus.NotLocked;
        }

        public clsTestAppointment(int TestAppointmentID, clsTestType.enTestType TestTypeID,
           int LocalDrivingLicenseApplicationID, DateTime AppointmentDate, float PaidFees, 
           int?PaymentID,int CreatedByEmployeeID, int? RetakeTestApplicationID, enStatus status)

        {
            this.TestAppointmentID = TestAppointmentID;
            this.TestTypeID = TestTypeID;
            this._TestTypeInfo = clsTestType.Find(TestTypeID);
            this.LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;
            this.LocalDrivingLicenseApplicationInfo = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(LocalDrivingLicenseApplicationID);
            this.AppointmentDate = AppointmentDate;
            this.PaidFees = PaidFees;
            this.CreatedByEmployeeID = CreatedByEmployeeID;
            this.CreatedByEmployeeInfo = clsEmployee.FindByEmployeeID(CreatedByEmployeeID);
            this.RetakeTestApplicationID= RetakeTestApplicationID;
            this.Status = status;
            this.RetakeTestAppInfo = clsApplication.FindBaseApplication(RetakeTestApplicationID??-1);
            this.PaymentID = PaymentID;
            if(this.PaymentID.HasValue)
                this.AppointmentPaymentInfo = clsPayment.Find(this.PaymentID.Value);

            Mode = enMode.Update;
        }

        private bool _AddNewTestAppointment()
        {
            //call DataAccess Layer 

            this.TestAppointmentID = clsTestAppointmentData.AddNewTestAppointment((int) this.TestTypeID,this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate,this.PaidFees,this.PaymentID,this.CreatedByEmployeeID,this.RetakeTestApplicationID);

            return (this.TestAppointmentID != -1);
        }

        private bool _UpdateTestAppointment()
        {
            //call DataAccess Layer 

            return clsTestAppointmentData.UpdateTestAppointment(this.TestAppointmentID, (int) this.TestTypeID, this.LocalDrivingLicenseApplicationID,
                this.AppointmentDate, this.PaidFees, this.PaymentID, this.CreatedByEmployeeID,this.RetakeTestApplicationID, (byte)this.Status);
        }

        public static clsTestAppointment Find(int TestAppointmentID)
        {
            int TestTypeID = 1;  int LocalDrivingLicenseApplicationID = -1;
            byte status = 0;
            DateTime AppointmentDate = DateTime.Now;  float PaidFees = 0;
            int? PaymentID = -1;
            int CreatedByUserID = -1; bool IsLocked=false;int? RetakeTestApplicationID = -1;

            if (clsTestAppointmentData.GetTestAppointmentInfoByID(TestAppointmentID, ref  TestTypeID, ref  LocalDrivingLicenseApplicationID,
            ref   AppointmentDate, ref  PaidFees, ref PaymentID, ref  CreatedByUserID, ref RetakeTestApplicationID, ref status))

                return new clsTestAppointment(TestAppointmentID,  (clsTestType.enTestType) TestTypeID,  LocalDrivingLicenseApplicationID,
             AppointmentDate,  PaidFees, PaymentID,  CreatedByUserID, RetakeTestApplicationID, (enStatus)status);
            else
                return null;
        }

        public static clsTestAppointment GetLastTestAppointment(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID )
        {
             int TestAppointmentID=-1;
            byte status = 0;
            DateTime AppointmentDate = DateTime.Now; float PaidFees = 0;
            int? PaymentID = -1;
            int CreatedByUserID = -1;bool IsLocked=false;int? RetakeTestApplicationID=-1;

            if (clsTestAppointmentData.GetLastTestAppointment( LocalDrivingLicenseApplicationID, (int) TestTypeID,
                ref TestAppointmentID,ref AppointmentDate, ref PaidFees, ref PaymentID, ref CreatedByUserID,ref RetakeTestApplicationID, ref status))

                return new clsTestAppointment(TestAppointmentID, TestTypeID, LocalDrivingLicenseApplicationID,
             AppointmentDate, PaidFees, PaymentID,CreatedByUserID, RetakeTestApplicationID, (enStatus) status);
            else
                return null;

        }

        public static DataTable GetAllTestAppointments()
        {
            return clsTestAppointmentData.GetAllTestAppointments();

        }

        public  DataTable GetApplicationTestAppointmentsPerTestType(clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(this.LocalDrivingLicenseApplicationID,(int) TestTypeID);

        }

        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID,clsTestType.enTestType TestTypeID)
        {
            return clsTestAppointmentData.GetApplicationTestAppointmentsPerTestType(LocalDrivingLicenseApplicationID, (int) TestTypeID);

        }
        public static DataTable GetApplicationTestAppointmentsPerTestTypeBy
            (int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID, int PageNumber, int RowPerPage)
            =>
            clsTestAppointmentData.GetApplicationTestAppointmentsPerTestTypeBy
            (LocalDrivingLicenseApplicationID, (int)TestTypeID, PageNumber, RowPerPage);

        public static DataTable GetApplicationTestAppointmentsBy(int PersonID, int PageNumber, int RowPerPage)
            => clsTestAppointmentData.GetApplicationTestAppointmentsBy(PersonID, PageNumber, RowPerPage);

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTestAppointment())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTestAppointment();

            }

            return false;
        }

        private int?  _GetTestID()
        {
            return clsTestAppointmentData.GetTestID(TestAppointmentID);
        }

        public static bool IsDatetimeIntertwined(DateTime LastDate, DateTime dtpDate)
        {

            if (LastDate.Day == dtpDate.Day && LastDate.Month == dtpDate.Month && LastDate.Year == dtpDate.Year)
            {
                return true;
            }

            //if Date Upper than dtpTestDate.Value
            return DateTime.Compare(LastDate, dtpDate) == 1;
        }
        public static int GetNumberOfRowsForAppTestAppointmentsPerTestTypeBy
            (int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestTypeID)
            =>
            clsTestAppointmentData.GetNumberOfRowsForAppTestAppointmentsPerTestTypeBy(LocalDrivingLicenseApplicationID, (int)TestTypeID);

        public static int GetNumberOfRowsForAppTestAppointmentsBy(int PersonID)
            => clsTestAppointmentData.GetNumberOfRowsForAppTestAppointmentsBy(PersonID);

        public static bool SetNewPaymentIDFor(int AppointmentID, int? NewPaymentID)
        {
            if (!NewPaymentID.HasValue)
                return false;

            return clsTestAppointmentData.SetNewPaymentIDFor(AppointmentID, NewPaymentID);
        }

        public bool SetNewPaymentIDFor(int? NewPaymentID)
        {
           this.PaymentID = NewPaymentID;
           return SetNewPaymentIDFor(this.TestAppointmentID, this.PaymentID);
        }

        public static int? GetLastRetakeAppIDBy(clsTestType.enTestType TestTypeID, int LocalDrivingLicenseApplications)
            => clsTestAppointmentData.GetLastRetakeAppIDBy((int)TestTypeID, LocalDrivingLicenseApplications);

        public static DataTable GetArchiveOfAllAppointmentsBy(int PageNumber, int RowPerPage)
            => clsTestAppointmentData.GetArchiveOfAllAppointmentsBy(PageNumber, RowPerPage);

        public static int GetNumberOfRowsForAppointmentsArchive()
            =>clsTestAppointmentData.GetNumberOfRowsForAppointmentsArchive();

        public static bool UpdateStatus(int AppointmentID, enStatus NewStatus)
            => clsTestAppointmentData.UpdateStatus(AppointmentID, (byte)NewStatus);

        public bool Return()
            => UpdateStatus(this.TestAppointmentID, enStatus.NotLocked);

        public static int GetTotalOfPassTestsBy(int LocalDrivingLicenseApplicationID)
           => clsTestAppointmentData.GetTotalOfPassTestsBy(LocalDrivingLicenseApplicationID);

        public int GetTotalOfPassTestsBy()
            => GetTotalOfPassTestsBy(this.LocalDrivingLicenseApplicationID);

        public static DateTime GetLastAppointmentDateBy(int LocalDrivingLicenseApplicationID)
            => clsTestAppointmentData.GetLastAppointmentDateBy(LocalDrivingLicenseApplicationID);
    }
}
