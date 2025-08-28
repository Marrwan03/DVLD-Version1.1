using System;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsDriver
    {

        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        clsPerson _PersonInfo;
        public clsPerson PersonInfo { get { return _PersonInfo; } }
        clsUser _UserInfo;
        public clsUser UserInfo { get { return _UserInfo; } }

        public int? DriverID { set; get; }
        public int PersonID { set; get; }
        public int CreatedByEmployeeID { set; get; }
        clsEmployee _CreatedByEmployeeInfo;
        public clsEmployee CreatedByEmployeeInfo { get {  return _CreatedByEmployeeInfo; } }
        public DateTime CreatedDate {  get; }

        public clsDriver()

        {
            this.DriverID = null;
            this.PersonID = -1;
            this.CreatedByEmployeeID = -1;
            this.CreatedDate=DateTime.Now;
            Mode = enMode.AddNew;

        }

        public clsDriver(int DriverID, int PersonID,int CreatedByEmployeeID, DateTime CreatedDate)

        {
            this.DriverID = DriverID;
            this.PersonID = PersonID;
            this.CreatedByEmployeeID = CreatedByEmployeeID;
            this.CreatedDate = CreatedDate;
            this._PersonInfo = clsPerson.Find(PersonID);
            this._UserInfo = clsUser.FindByPersonID(PersonID);
            this._CreatedByEmployeeInfo = clsEmployee.FindByEmployeeID(CreatedByEmployeeID);
            Mode = enMode.Update;
        }

        private bool _AddNewDriver()
        {
            //call DataAccess Layer 

            this.DriverID = clsDriverData.AddNewDriver( PersonID,  CreatedByEmployeeID);
              

            return this.DriverID.HasValue;
        }

        private bool _UpdateDriver()
        {
            //call DataAccess Layer 

            return clsDriverData.UpdateDriver(this.DriverID.Value,this.PersonID,this.CreatedByEmployeeID);
        }

        public static clsDriver FindByDriverID(int DriverID)
        {
            
            int PersonID = -1; int CreatedByEmployeeID = -1;DateTime CreatedDate= DateTime.Now; 

            if (clsDriverData.GetDriverInfoByDriverID(DriverID, ref PersonID,ref CreatedByEmployeeID, ref CreatedDate))

                return new clsDriver(DriverID,  PersonID,  CreatedByEmployeeID,  CreatedDate);
            else
                return null;

        }

        public static clsDriver FindByPersonID(int PersonID)
        {

            int DriverID = -1; int CreatedByEmployeeID = -1; DateTime CreatedDate = DateTime.Now;

            if (clsDriverData.GetDriverInfoByPersonID( PersonID, ref DriverID,  ref CreatedByEmployeeID, ref CreatedDate))

                return new clsDriver(DriverID, PersonID, CreatedByEmployeeID, CreatedDate);
            else
                return null;

        }

        public static DataTable GetAllDrivers()
        {
            return clsDriverData.GetAllDrivers();

        }
        public static DataTable GetDriverBy(int PageNumber, int RowPerPage)
        {
            return clsDriverData.GetDriversBy(PageNumber, RowPerPage);
        }

        public static int GetNumberOfRows()
        {
            return clsDriverData.GetNumberOfRowsForDrivers();
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewDriver())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateDriver();

            }

            return false;
        }

        public static DataTable GetLicenses(int DriverID, int PageNumber, int RowPerPage)
        => clsLicense.GetDriverLocalLicenses(DriverID, PageNumber, RowPerPage);

        public static DataTable GetInternationalLicenses(int DriverID, int PageNumber, int RowPerPage)
        => clsInternationalLicense.GetDriverIntLicensesBy(DriverID, PageNumber, RowPerPage);

        public static bool IsExistsBy(int UserID) => clsDriverData.IsExistsBy(UserID);

    }
}
