using DVLD_DataAccess;
using DVLD_DataAccess.MyFunSQL;
using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Xml.Linq;
using static DVLD_Buisness.clsLicense;

namespace DVLD_Buisness
{
    public class clsInternationalLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public clsDriver DriverInfo;
        public int? InternationalLicenseID { set; get; }  
        public int ApplicationID { set; get; }
         clsApplication _ApplicationInfo;
        public clsApplication ApplicationInfo { get { return _ApplicationInfo; } }
        public int DriverID { set; get; }
        public int IssuedUsingLocalLicenseID { set; get; }   
        public clsLicense LocalLicense { get { return clsLicense.Find(IssuedUsingLocalLicenseID); } }
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }    
        public bool IsActive { set; get; }
       
        public bool IsDetained { get { return clsDetainedLicense.IsLicenseDetained(this.InternationalLicenseID.Value, clsDetainedLicense.enLicenseType.International); } }
        clsDetainedLicense _DetainedInfo;
        public clsDetainedLicense DetainedInfo { get { return _DetainedInfo; } }
        public int CreatedByEmployeeID { set; get; }
        clsEmployee _EmployeeInfo;
        public clsEmployee EmployeeInfo { get { return _EmployeeInfo; } }

        public clsInternationalLicense()
        {
            this.InternationalLicenseID = -1;
            this.DriverID = -1;
            this.IssuedUsingLocalLicenseID = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;  
            this.IsActive = true;
            Mode = enMode.AddNew;

        }

        public clsInternationalLicense(int InternationalLicenseID, int ApplicationID,
            int DriverID, int IssuedUsingLocalLicenseID,
            DateTime IssueDate, DateTime ExpirationDate,bool IsActive,int CreatedByEmployeeID)

        {

            this.InternationalLicenseID = InternationalLicenseID;
            this.ApplicationID=ApplicationID;
            this.DriverID = DriverID;
            this.IssuedUsingLocalLicenseID = IssuedUsingLocalLicenseID;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.IsActive = IsActive;
            this.CreatedByEmployeeID = CreatedByEmployeeID;

            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this._DetainedInfo = clsDetainedLicense.FindByLicenseID(InternationalLicenseID, clsDetainedLicense.enLicenseType.International);
            this._ApplicationInfo = clsApplication.FindBaseApplication(ApplicationID);
            this._EmployeeInfo = clsEmployee.FindByEmployeeID(CreatedByEmployeeID);
            Mode = enMode.Update;
        }

        private bool _AddNewInternationalLicense()
        {
            //call DataAccess Layer 

            this.InternationalLicenseID = 
                clsInternationalLicenseData.AddNewInternationalLicense(this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
               this.IssueDate, this.ExpirationDate, 
               this.IsActive, this.CreatedByEmployeeID);


            return this.InternationalLicenseID.HasValue;
        }

        private bool _UpdateInternationalLicense()
        {
            //call DataAccess Layer 

            return clsInternationalLicenseData.UpdateInternationalLicense(
                this.InternationalLicenseID.Value,this.ApplicationID, this.DriverID, this.IssuedUsingLocalLicenseID,
               this.IssueDate, this.ExpirationDate, 
               this.IsActive, this.CreatedByEmployeeID);
        }

        public static clsInternationalLicense Find(int InternationalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; int IssuedUsingLocalLicenseID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
             bool IsActive = true;
            int CreatedByEmployeeID = 1;

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByID(InternationalLicenseID,ref ApplicationID, ref DriverID, 
                ref IssuedUsingLocalLicenseID,
            ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByEmployeeID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                                         IssueDate, ExpirationDate, IsActive,CreatedByEmployeeID);
            }
             
            else
                return null;
        }

        public static clsInternationalLicense FindByLocalLicenseID(int IssuedUsingLocalLicenseID)
        {
            int ApplicationID = -1;
            int DriverID = -1; int InternationalLicenseID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            bool IsActive = true;
            int CreatedByEmployeeID = 1;

            if (clsInternationalLicenseData.GetInternationalLicenseInfoByLocalLicenseID(ref InternationalLicenseID, ref ApplicationID, ref DriverID,
                 IssuedUsingLocalLicenseID,
            ref IssueDate, ref ExpirationDate, ref IsActive, ref CreatedByEmployeeID))
            {
                return new clsInternationalLicense(InternationalLicenseID, ApplicationID, DriverID, IssuedUsingLocalLicenseID,
                                         IssueDate, ExpirationDate, IsActive, CreatedByEmployeeID);
            }

            else
                return null;
        }

        public static DataTable GetAllInternationalLicenses()
        {
            return clsInternationalLicenseData.GetAllInternationalLicenses();

        }
        public static DataTable GetAllInternationalLicensesBy(int PageNumber, int RowPerPage)
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesBy(PageNumber, RowPerPage);
        }
        public static DataTable GetDriverIntLicensesBy(int DriverID, int PageNumber, int RowPerPage)
            => clsInternationalLicenseData.GetDriverIntLicensesBy(DriverID, PageNumber, RowPerPage);
        public Boolean IsLicenseExpired()
        {

            return (this.ExpirationDate < DateTime.Now);

        }
        public bool DeactivateCurrentLicense()
        {
            return clsInternationalLicenseData.DeactivateInternationalLicense(this.InternationalLicenseID.Value);
        }

        public int Detain(float FineFees, int CreatedByEmployeeID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();

            detainedLicense.LicenseID = this.InternationalLicenseID.Value;
            detainedLicense.LicenseType = clsDetainedLicense.enLicenseType.International;
            detainedLicense.DetainDate = DateTime.Now;
            detainedLicense.FineFees = Convert.ToSingle(FineFees);
            detainedLicense.CreatedByEmployeeID = CreatedByEmployeeID;

            if (!detainedLicense.Save())
            {
                return -1;
            }
            DeactivateCurrentLicense();
            return detainedLicense.DetainID;
        }
        public (clsInternationalLicense IntLicenseInfo,int? ApplicationID) RenewLicense(int CreatedByEmployeeID)
        {
            clsApplication Application 
                = clsApplication.FindBaseApplication(this.ApplicationInfo.ApplicantPersonID,
                clsApplication.enApplicationType.RenewDrivingLicense);

            bool IsPaid = false;

            if(Application != null)
            {
                IsPaid = Application.IsPaid();
            }
            else
            {
                Application= new clsApplication();
                Application.ApplicantPersonID = this.DriverInfo.PersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees =
                    clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees
                    + this.LocalLicense.LicenseClassInfo.ClassFees;
                Application.CreatedByEmployeeID = CreatedByEmployeeID;
            }

            if (Application.Save())
            {
                if (!IsPaid)
                    return (null, Application.ApplicationID);
            }
            else
                return (null, null);

            clsInternationalLicense NewInternationalLicense
                = new clsInternationalLicense();

            NewInternationalLicense.ApplicationID = Application.ApplicationID;
            NewInternationalLicense.DriverID = this.DriverID;
            NewInternationalLicense.IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID;
            NewInternationalLicense.IssueDate = DateTime.Now;
            NewInternationalLicense.ExpirationDate = DateTime.Now.AddYears(1);
            NewInternationalLicense.IsActive = true;
            NewInternationalLicense.CreatedByEmployeeID = CreatedByEmployeeID;

            if(!NewInternationalLicense.Save()) { return (null,null); }

            DeactivateCurrentLicense();

            return (NewInternationalLicense, NewInternationalLicense.ApplicationID);
        }

        public (clsInternationalLicense IntLicenseInfo, int? ApplicationID) Replace(clsLicense.enIssueReason IssueReason, int CreatedByEmployeeID)
        {
            clsApplication Application =
                clsApplication.FindBaseApplication(
                    this.ApplicationInfo.ApplicantPersonID,
                (IssueReason == enIssueReason.DamagedReplacement) ?
                clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                clsApplication.enApplicationType.ReplaceLostDrivingLicense);

            bool  IsPaid = false;
            if (Application != null)
            {
                IsPaid = Application.IsPaid();
            }
            else
            {
                Application = new clsApplication();
                Application.ApplicantPersonID = this.DriverInfo.PersonID;
                Application.ApplicationDate = DateTime.Now;

                Application.ApplicationTypeID = (IssueReason == clsLicense.enIssueReason.DamagedReplacement) ?
                    (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                    (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
                Application.CreatedByEmployeeID = CreatedByEmployeeID;
            }

            if (Application.Save())
            {
                if (!IsPaid)
                    return (null, Application.ApplicationID);
            }
            else
                return (null, null);

            clsInternationalLicense NewInternationalLicense = new clsInternationalLicense();
            NewInternationalLicense.ApplicationID = Application.ApplicationID;
            NewInternationalLicense.DriverID = this.DriverID;
            NewInternationalLicense.IssuedUsingLocalLicenseID = this.IssuedUsingLocalLicenseID;
            NewInternationalLicense.IssueDate = this.IssueDate;
            NewInternationalLicense.ExpirationDate = this.ExpirationDate;
            NewInternationalLicense.IsActive = true;
            NewInternationalLicense.CreatedByEmployeeID = CreatedByEmployeeID;

            if (!NewInternationalLicense.Save()) { return (null,null); }

            DeactivateCurrentLicense();
            return (NewInternationalLicense,Application.ApplicationID);

        }

        public (bool? IsRelease, int? ApplicationID) ReleaseDetainedLicense(int ReleasedByEmployeeID, float FineFees)
        {
            clsApplication Application
                            = clsApplication.FindBaseApplication(this.ApplicationInfo.ApplicantPersonID,
                            clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense);

            bool IsPaid = false;
            if (Application != null)
            {
                IsPaid = Application.IsPaid();
            }
            else
            {
                Application = new clsApplication();
                Application.ApplicantPersonID = this.DriverInfo.PersonID;
                Application.ApplicationDate = DateTime.Now;
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense).Fees + FineFees;
                Application.CreatedByEmployeeID = ReleasedByEmployeeID;
            }

            if (Application.Save())
            {
                if (!IsPaid)
                {
                    return (null, Application.ApplicationID);
                }
            }
            else
            {
                return (null, null);
            }
            this.IsActive = true;

            if(!this.Save())
            {
                return (null, null);
            }
            return (this.DetainedInfo.ReleaseDetainedLicense(ReleasedByEmployeeID, Application.ApplicationID), Application.ApplicationID);
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewInternationalLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateInternationalLicense();

            }

            return false;
        }

        public static int GetActiveInternationalLicenseIDBy(int DriverID, int IssuedUsingLocalLicenseID)
        {

            return clsInternationalLicenseData.GetActiveInternationalLicenseIDBy(DriverID, IssuedUsingLocalLicenseID);

        }

        public int GetActiveInternationalLicenseID()
        {

            return clsInternationalLicenseData.GetActiveInternationalLicenseIDBy(this.DriverID, this.IssuedUsingLocalLicenseID);

        }
       
        public static bool IsInternationalLicenseExistsByLocalLicenseID(int LocalLicenseID)
        {
            return clsInternationalLicenseData.IsInternationalLicenseExistsByLocalLicenseID(LocalLicenseID);
        }
        public static bool IsAppIDExists(int AppID) => clsInternationalLicenseData.IsAppIDExists(AppID);
        public static int GetNumberOfRows()
        {
            return clsInternationalLicenseData.GetNumberOfRows();
        }
        public static int GetMaximamPage()
        {
            return clsGets.GetMaximamPage(GetNumberOfRows(), 3);
        }
        public static int GetLastIntLicenseID(int PersonID, int LicenseClassID)
            => clsInternationalLicenseData.GetLastIntLicenseID(PersonID, LicenseClassID);
        public static int GetNumberOfRowsForIntLicenses(int DriverID)
            => clsInternationalLicenseData.GetNumberOfRowsForIntLicenses(DriverID);

    }
}
