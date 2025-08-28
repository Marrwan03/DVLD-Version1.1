using System;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsDetainedLicense
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enLicenseType : byte { Local=1,International}
        public enLicenseType LicenseType {  get; set; }
        public int DetainID { set; get; }
        public int LicenseID { set; get; }
        public DateTime DetainDate { set; get; }

        public float FineFees { set; get; }
        public int CreatedByEmployeeID { set; get; }
        public clsEmployee CreatedByEmployeeInfo { set; get; }
        public bool IsReleased { set; get; }
        public DateTime ReleaseDate { set; get; }
        public int ReleasedByEmployeeID { set; get; }
        public clsEmployee ReleasedByEmployeeInfo { set; get; }
        public int ReleaseApplicationID { set; get; }
       
        public clsDetainedLicense()

        {
            this.DetainID = -1;
            this.LicenseID = 0;
            this.DetainDate = DateTime.Now;
            this.FineFees = 0;
            this.CreatedByEmployeeID = -1;
            this.IsReleased = false;
            this.ReleaseDate = DateTime.MaxValue;
            this.ReleasedByEmployeeID = 0;
            this.ReleaseApplicationID = -1;

            Mode = enMode.AddNew;

        }

        public clsDetainedLicense(int DetainID,
            int LicenseID,enLicenseType licenseType,  DateTime DetainDate,
            float FineFees,  int CreatedByEmployeeID,
            bool IsReleased,  DateTime ReleaseDate,
            int ReleasedByEmployeeID,  int ReleaseApplicationID)

        {
            this.DetainID = DetainID;
            this.LicenseID = LicenseID;
            this.LicenseType = licenseType;
            this.DetainDate = DetainDate;
            this.FineFees = FineFees;
            this.CreatedByEmployeeID = CreatedByEmployeeID;
            this.CreatedByEmployeeInfo = clsEmployee.FindByEmployeeID(this.CreatedByEmployeeID);
            this.IsReleased = IsReleased;
            this.ReleaseDate = ReleaseDate;
            this.ReleasedByEmployeeID = ReleasedByEmployeeID;
            this.ReleasedByEmployeeInfo = clsEmployee.FindByEmployeeID(this.ReleasedByEmployeeID);
            this.ReleaseApplicationID = ReleaseApplicationID;
            Mode = enMode.Update;
        }

        private bool _AddNewDetainedLicense()
        {
            //call DataAccess Layer 

            this.DetainID = clsDetainedLicenseData.AddNewDetainedLicense( this.LicenseID,
                (byte)this.LicenseType,this.DetainDate,this.FineFees,this.CreatedByEmployeeID);
            
            return (this.DetainID != -1);
        }

        private bool _UpdateDetainedLicense()
        {
            //call DataAccess Layer 

            return clsDetainedLicenseData.UpdateDetainedLicense(
                this.DetainID, this.LicenseID, (byte)this.LicenseType, this.DetainDate,this.FineFees,this.CreatedByEmployeeID);
        }

        public static clsDetainedLicense Find(int DetainID)
        {
            int LicenseID=0;
            byte LicenseType=0;
            DateTime DetainDate = DateTime.Now;
            float FineFees= 0; int CreatedByEmployeeID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByEmployeeID = -1; int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByID(DetainID,ref LicenseID,
            ref LicenseType, ref DetainDate,
            ref FineFees, ref CreatedByEmployeeID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByEmployeeID, ref ReleaseApplicationID))

                return new clsDetainedLicense(DetainID, LicenseID,
                     (enLicenseType)LicenseType,  DetainDate,
                     FineFees,  CreatedByEmployeeID,
                     IsReleased,  ReleaseDate,
                     ReleasedByEmployeeID,  ReleaseApplicationID);
            else
                return null;

        }

        public static DataTable GetAllDetainedLicenses()
        {
            return clsDetainedLicenseData.GetAllDetainedLicenses();

        }
        public static DataTable GetDetainedLicensesBy(int PageNumber, int RowPerPage)
        {
            return clsDetainedLicenseData.GetDetainedLicensesBy(PageNumber, RowPerPage);
        }
        public static int GetNumberOfRows()
        {
            return clsDetainedLicenseData.GetNumberOfRows();
        }

        public static clsDetainedLicense FindByLicenseID(int LicenseID, enLicenseType LicenseType)
        {
            int DetainID = -1; DateTime DetainDate = DateTime.Now;
            float FineFees = 0; int CreatedByUserID = -1;
            bool IsReleased = false; DateTime ReleaseDate = DateTime.MaxValue;
            int ReleasedByUserID = -1; int ReleaseApplicationID = -1;

            if (clsDetainedLicenseData.GetDetainedLicenseInfoByLicenseID(LicenseID, (byte)LicenseType,
            ref DetainID, ref DetainDate,
            ref FineFees, ref CreatedByUserID,
            ref IsReleased, ref ReleaseDate,
            ref ReleasedByUserID, ref ReleaseApplicationID))

                return new clsDetainedLicense(DetainID, LicenseID,
                     LicenseType, DetainDate,
                     FineFees, CreatedByUserID,
                     IsReleased, ReleaseDate,
                     ReleasedByUserID, ReleaseApplicationID);
            else
                return null;

        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDetainedLicense())
                    {
                        
                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDetainedLicense();

            }

            return false;
        }

        public static bool IsLicenseDetained(int LicenseID, enLicenseType licenseType)
        {
            return clsDetainedLicenseData.IsLicenseDetained(LicenseID, (byte)licenseType);
        }

        public bool ReleaseDetainedLicense(int ReleasedByEmployeeID, int ReleaseApplicationID)
        {
            return clsDetainedLicenseData.ReleaseDetainedLicense(this.DetainID, ReleasedByEmployeeID, ReleaseApplicationID);
        }

    
    }
}
