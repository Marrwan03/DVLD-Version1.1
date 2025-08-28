using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Eventing.Reader;

namespace DVLD_Buisness
{
    public class clsTest
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int TestID { set; get; }
        public int TestAppointmentID { set; get; }
        public clsTestAppointment TestAppointmentInfo { set; get; }
        public bool TestResult { set; get; }
        public string Notes { set; get; }

        public clsTest()

        {
            this.TestID = -1;
            this.TestAppointmentID = -1;
            this.TestResult = false;
            this.Notes ="";

            Mode = enMode.AddNew;

        }

        public clsTest(int TestID,int TestAppointmentID,
            bool TestResult, string Notes)

        {
            this.TestID = TestID;
            this.TestAppointmentID = TestAppointmentID;
            this.TestAppointmentInfo = clsTestAppointment.Find(TestAppointmentID);
            this.TestResult = TestResult;
            this.Notes = Notes;

            Mode = enMode.Update;
        }

        private bool _AddNewTest()
        {
            //call DataAccess Layer 

            this.TestID = clsTestData.AddNewTest(this.TestAppointmentID,
                this.TestResult,this.Notes);
              

            return (this.TestID != -1);
        }

        private bool _UpdateTest()
        {
            //call DataAccess Layer 

            return clsTestData.UpdateTest(this.TestID, this.TestAppointmentID,
                this.TestResult, this.Notes);
        }

        public static clsTest Find(int TestID)
        {
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = "";

            if (clsTestData.GetTestInfoByID( TestID,
            ref  TestAppointmentID, ref  TestResult,
            ref  Notes))

                return new clsTest(TestID,
                        TestAppointmentID,  TestResult,
                        Notes);
            else
                return null;

        }

        public static clsTest FindLastTestPerPersonAndLicenseClass
            (int PersonID, int LicenseClassID, clsTestType.enTestType TestTypeID)
        {
            int TestID = -1;
            int TestAppointmentID = -1;
            bool TestResult = false; string Notes = ""; 

            if (clsTestData.GetLastTestByPersonAndTestTypeAndLicenseClass
                (PersonID,LicenseClassID,(int) TestTypeID, ref TestID,
            ref TestAppointmentID, ref TestResult,
            ref Notes))

                return new clsTest(TestID,
                        TestAppointmentID, TestResult,
                        Notes);
            else
                return null;

        }

        public static DataTable GetAllTests()
        {
            return clsTestData.GetAllTests();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewTest())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateTest();

            }

            return false;
        }

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            return clsTestData.GetPassedTestCount(LocalDrivingLicenseApplicationID);
        }

        public static bool  PassedAllTests(int LocalDrivingLicenseApplicationID)
        {
            //if total passed test less than 3 it will return false otherwise will return true
            return GetPassedTestCount(LocalDrivingLicenseApplicationID) == 3;
        }

        public static List<int> GetAllTestsIDBy(int LocalDrivingLicenseApplicationID)
        => clsTestData.GetAllTestsIDBy(LocalDrivingLicenseApplicationID);

    }
}
