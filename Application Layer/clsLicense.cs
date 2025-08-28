using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.IO;
using System.Runtime.ExceptionServices;
using System.Security.Permissions;
using System.Xml.Linq;
using DVLD_DataAccess;
using static System.Net.Mime.MediaTypeNames;
using static DVLD_Buisness.clsLicense;

namespace DVLD_Buisness
{
    public class clsLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enIssueReason { FirstTime = 1, Renew = 2, DamagedReplacement = 3, LostReplacement = 4 };

        public enum enLicenseClass
        {
            Small_Motorcycle = 1, Heavy_Motorcycle_License, Ordinary_driving_license,
            Commercial, Agricultural, Small_and_medium_bus, Truck_and_heavy_vehicle
        }

        public clsDriver DriverInfo;
        public int? LicenseID { set; get; }
        public int ApplicationID { set; get; }
        clsApplication _ApplicationInfo;
        public clsApplication ApplicationInfo { get { return _ApplicationInfo; } }

        public int DriverID { set; get; }
        public int LicenseClass { set; get; }
        public clsLicenseClass LicenseClassInfo;
        public DateTime IssueDate { set; get; }
        public DateTime ExpirationDate { set; get; }
        public string Notes { set; get; }
       
        public float PaidFees { set; get; }
        public bool IsActive { set; get; }
        public enIssueReason IssueReason { set; get; }
        public string IssueReasonText
        { get 
            { 
                return GetIssueReasonText(this.IssueReason); 
            } 
        }
        public clsDetainedLicense DetainedInfo { set; get; }
        public int CreatedByEmployeeID { set; get; }
        clsEmployee _CreatedByEmployeeInfo;
        public clsEmployee CreatedByEmployeeInfo { get { return _CreatedByEmployeeInfo; } }
        public bool IsDetained
        {
            get { return clsDetainedLicense.IsLicenseDetained(this.LicenseID.Value, clsDetainedLicense.enLicenseType.Local); }
        }

        public clsLicense()

        {
            this.LicenseID = null;
            this.ApplicationID= -1;
            this.DriverID = -1;
            this.LicenseClass = -1;
            this.IssueDate = DateTime.Now;
            this.ExpirationDate = DateTime.Now;
            this.Notes = "";
            this.PaidFees = 0;
            this.IsActive = true;
            this.IssueReason = enIssueReason.FirstTime;
            this.CreatedByEmployeeID = -1;

            Mode = enMode.AddNew;

        }

        public clsLicense(int LicenseID,int ApplicationID, int DriverID, int LicenseClass,
            DateTime IssueDate, DateTime ExpirationDate, string Notes,
            float PaidFees, bool IsActive,enIssueReason IssueReason, int CreatedByEmployeeID)

        {
            this.LicenseID = LicenseID;
            this.ApplicationID = ApplicationID;
            this.DriverID = DriverID;
            this.LicenseClass = LicenseClass;
            this.IssueDate = IssueDate;
            this.ExpirationDate = ExpirationDate;
            this.Notes = Notes;
            this.PaidFees = PaidFees;
            this.IsActive = IsActive;
            this.IssueReason = IssueReason;
            this.CreatedByEmployeeID = CreatedByEmployeeID;

            this._ApplicationInfo = clsApplication.FindBaseApplication(ApplicationID);
            this.DriverInfo = clsDriver.FindByDriverID(this.DriverID);
            this.LicenseClassInfo = clsLicenseClass.Find(this.LicenseClass);
            this.DetainedInfo=clsDetainedLicense.FindByLicenseID(this.LicenseID.Value, clsDetainedLicense.enLicenseType.Local);
            this._CreatedByEmployeeInfo = clsEmployee.FindByEmployeeID(CreatedByEmployeeID);

            Mode = enMode.Update;
        }

        private bool _AddNewLicense()
        {
            //call DataAccess Layer 

            this.LicenseID = clsLicenseData.AddNewLocalLicense(this.ApplicationID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive,(byte) this.IssueReason, this.CreatedByEmployeeID);


            return this.LicenseID.HasValue;
        }

        private bool _UpdateLicense()
        {
            //call DataAccess Layer 

            return clsLicenseData.UpdateLocalLicense(this.LicenseID, this.ApplicationID, this.DriverID, this.LicenseClass,
               this.IssueDate, this.ExpirationDate, this.Notes, this.PaidFees,
               this.IsActive,(byte) this.IssueReason, this.CreatedByEmployeeID);
        }

        public static clsLicense Find(int LicenseID)
        {
            int ApplicationID = -1; int DriverID = -1; int LicenseClass = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; bool IsActive = true; int CreatedByEnployeeID = 1;
            byte IssueReason = 1;
            if (clsLicenseData.GetLocalLicenseInfoByID(LicenseID,ref ApplicationID, ref DriverID, ref LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive,ref IssueReason, ref CreatedByEnployeeID))

                return new clsLicense(LicenseID,ApplicationID, DriverID, LicenseClass,
                                     IssueDate, ExpirationDate, Notes,
                                     PaidFees, IsActive,(enIssueReason) IssueReason, CreatedByEnployeeID);
            else
                return null;

        }

        public static clsLicense Find(int DriverID, enLicenseClass LicenseClass)
        {
            int LicenseID=-1,ApplicationID = -1;
            DateTime IssueDate = DateTime.Now; DateTime ExpirationDate = DateTime.Now;
            string Notes = "";
            float PaidFees = 0; bool IsActive = true; int CreatedByEnployeeID = 1;
            byte IssueReason = 1;
            if (clsLicenseData.Find(ref LicenseID, ref ApplicationID, DriverID,(int) LicenseClass,
            ref IssueDate, ref ExpirationDate, ref Notes,
            ref PaidFees, ref IsActive, ref IssueReason, ref CreatedByEnployeeID))

                return new clsLicense(LicenseID, ApplicationID, DriverID,(int) LicenseClass,
                                     IssueDate, ExpirationDate, Notes,
                                     PaidFees, IsActive, (enIssueReason)IssueReason, CreatedByEnployeeID);
            else
                return null;

        }

        public static DataTable GetAllLicenses()
        {
            return clsLicenseData.GetAllLocalLicenses();

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewLicense())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateLicense();

            }

            return false;
        }

        public static bool IsLicenseExistByPersonID(int PersonID, int LicenseClassID)
        {
            return (GetActiveLicenseIDByPersonID(PersonID, LicenseClassID) != -1);
        }

        public static int GetLastLicenseID(int PersonID, int LicenseClassID)
            => clsLicenseData.GetLastLicenseID(PersonID, LicenseClassID);

        public static int GetActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {

            return clsLicenseData.GetActiveLicenseIDByPersonID(PersonID, LicenseClassID);

        }

        public int GetActiveLicenseID()
        {

            return clsLicenseData.GetActiveLicenseIDByPersonID(this.DriverInfo.PersonID, this.LicenseClass);

        }

        public static DataTable GetDriverLocalLicenses(int DriverID, int PageNumber, int RowPerPage)
            => clsLicenseData.GetDriverLocalLicensesBy(DriverID, PageNumber, RowPerPage);
        

        public Boolean IsLicenseExpired()
        {

            return ( this.ExpirationDate < DateTime.Now );

        }

        public bool DeactivateCurrentLicense()
        {
            //Delete Docomuntation
            

            return clsLicenseData.DeactivateLicense(this.LicenseID);
        }

        public static string GetIssueReasonText(enIssueReason IssueReason)
        {

            switch (IssueReason)
            {
                case enIssueReason.FirstTime:
                    return "First Time";
                case enIssueReason.Renew:
                    return "Renew";
                case enIssueReason.DamagedReplacement:
                    return "Replacement for Damaged";
                case enIssueReason.LostReplacement:
                    return "Replacement for Lost";
                default:
                    return "First Time";
            }
        }

        public int Detain(float FineFees,int CreatedByEmployeeID)
        {
            clsDetainedLicense detainedLicense = new clsDetainedLicense();

            detainedLicense.LicenseID = this.LicenseID.Value;
            detainedLicense.LicenseType = clsDetainedLicense.enLicenseType.Local;
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

            ApplicationID = Application.ApplicationID;
            this.IsActive = true;
            if(!this.Save())
            {
                return (null, null);
            }
            return (this.DetainedInfo.ReleaseDetainedLicense(ReleasedByEmployeeID, Application.ApplicationID), Application.ApplicationID);
        }

        public (clsLicense LicenseInfo,int? ApplicationID) RenewLicense(string Notes, int CreatedByEmployeeID)
        { 
            clsApplication Application 
                = clsApplication.FindBaseApplication(this.ApplicationInfo.ApplicantPersonID, clsApplication.enApplicationType.RenewDrivingLicense);

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
                Application.ApplicationTypeID = (int)clsApplication.enApplicationType.RenewDrivingLicense;
                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find((int)clsApplication.enApplicationType.RenewDrivingLicense).Fees + this.LicenseClassInfo.ClassFees;
                Application.CreatedByEmployeeID = CreatedByEmployeeID;
            }
           

            if (Application.Save())
            {
                if(!IsPaid)
                {
                return (null, Application.ApplicationID);
                }
            }
            else
            {
                return (null, null);
            }

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            int DefaultValidityLength = this.LicenseClassInfo.DefaultValidityLength;
            NewLicense.ExpirationDate = DateTime.Now.AddYears(DefaultValidityLength);
            NewLicense.Notes = Notes;
            NewLicense.PaidFees = Application.PaidFees;
            NewLicense.IsActive = true;
            NewLicense.IssueReason = clsLicense.enIssueReason.Renew;
            NewLicense.CreatedByEmployeeID = CreatedByEmployeeID;


            if (!NewLicense.Save())
            {
                return (null, null);
            }

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return (NewLicense, NewLicense.ApplicationID);
        }

        public (clsLicense LicenseInfo, int? ApplicationID) Replace(enIssueReason IssueReason, int CreatedByEmployeeID)
        {

            clsApplication Application =
                clsApplication.FindBaseApplication(
                    this.ApplicationInfo.ApplicantPersonID,
                (IssueReason == enIssueReason.DamagedReplacement) ?
                clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                clsApplication.enApplicationType.ReplaceLostDrivingLicense);

            bool IsPaid=false;

            if (Application != null)
            {
                IsPaid = Application.IsPaid();
            }
            else
            {
                Application = new clsApplication();
                Application.ApplicantPersonID = this.DriverInfo.PersonID;
                Application.ApplicationDate = DateTime.Now;

                Application.ApplicationTypeID = (IssueReason == enIssueReason.DamagedReplacement) ?
                    (int)clsApplication.enApplicationType.ReplaceDamagedDrivingLicense :
                    (int)clsApplication.enApplicationType.ReplaceLostDrivingLicense;

                Application.ApplicationStatus = clsApplication.enApplicationStatus.Completed;
                Application.LastStatusDate = DateTime.Now;
                Application.PaidFees = clsApplicationType.Find(Application.ApplicationTypeID).Fees;
                Application.CreatedByEmployeeID = CreatedByEmployeeID;
            }

            if (Application.Save())
            {
                if(!IsPaid)
                    return (null, Application.ApplicationID);
            }
            else
                return (null, null);

            clsLicense NewLicense = new clsLicense();
            NewLicense.ApplicationID = Application.ApplicationID;
            NewLicense.DriverID = this.DriverID;
            NewLicense.LicenseClass = this.LicenseClass;
            NewLicense.IssueDate = DateTime.Now;
            NewLicense.ExpirationDate = this.ExpirationDate;
            NewLicense.Notes = this.Notes;
            NewLicense.PaidFees = 0;// no fees for the license because it's a replacement.
            NewLicense.IsActive = true;
            NewLicense.IssueReason = IssueReason;
            NewLicense.CreatedByEmployeeID = CreatedByEmployeeID;

            if (!NewLicense.Save())
                return (null, null);

            //we need to deactivate the old License.
            DeactivateCurrentLicense();

            return (NewLicense, Application.ApplicationID); ;
        }

        public static bool IsLicenseCorrectForHim(byte Age, string LicenseClassName)
        {
            return Age >= clsLicenseClass.Find(LicenseClassName).MinimumAllowedAge;
        }
        public static int GetNumberOfRowsForLocalLicenses(int DriverID)
            => clsLicenseData.GetNumberOfRowsForLocalLicenses(DriverID);
        public static bool IsAppIDExists(int AppID) => clsLicenseData.IsAppIDExists(AppID);



    }

    }

