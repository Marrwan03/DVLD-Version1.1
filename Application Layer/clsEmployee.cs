using DVLD_Buisness.Interfaces;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace DVLD_Buisness
{
    public class clsEmployee : clsUser,IReturn
    {
        public enum enMode { Add, Update}
        public enMode Mode;
        int? _EmployeeID;
        public enum enPermision : long
        {
            All = -1, RenewDrivingLicense = 1, ReplacementforLostorDamagedLicense = 2, ManageLocalLicenses = 4, AddNewLocalLicense = 8,

            EditApplication = 16, DeleteAppointment = 32, IssueLicenseFirstTime = 64, ShowPersonLicenseHistory = 128, ShowApplicationDetails = 256,

            DeleteApplication = 512, SechduleTests = 1024, ShowLicenseLDL = 2048, AddNewInternationalLicense = 4096, ManageInternationalLicenses = 8192,

            ShowLicenseDetails = 16384,ManageDetainLicense = 32768, ReleaseDetainedLicense = 65536,

            ManageApplicationTypes = 131072, EditApplicationType = 262144, ManageTestTypes = 524288,

            EditTestType = 1048576, ShowPersonDetails= 2_097_152, DeletePersonPeople = 4_194_304, AddNewPerson = 8_388_608,

            EditPersonPeople = 16_777_216, SendEmailPeople = 33_554_432, PhoneCallPeople = 67_108_864, ShowUserDetails = 134_217_728,

            EditUser = 268_435_456, ChangePasswordUser = 536_870_912, AddNewUser = 1_073_741_824, DeleteUser = 2_147_483_648, SendEmailUsers = 4_294_967_296,

            PhoneCallUsers = 8_589_934_592, DetainLicense = 17_179_869_184, SetPermision = 34_359_738_368,
            
            ShowPersonEmail = 68_719_476_736, ShowUserEmail = 137_438_953_472, ShowPersonCallLog = 274_877_906_944,
            
            ShowUserCallLog = 549_755_813_888, ShowLicenseInt = 1_099_511_627_776, ShowEmployeesDetails= 2_199_023_255_552, EditEmployee= 4_398_046_511_104,

            AddNewEmployee = 8_796_093_022_208, ShowPermisions = 17_592_186_044_416, DeleteEmployee = 35_184_372_088_832,

            SendEmailEmployees = 70_296_744_177_664, PhoneCallEmployees = 140_593_488_355_328, ShowEmployeesEmail= 281_186_976_710_656, ShowEmployeesCallLog= 562_373_953_421_312,

            ReturnTheArchives = 1_124_747_906_842_624, EditAppointment = 2_249_495_813_685_248


        }
        public long Permisions { get; set; }
        public int? EmployeeID { get { return _EmployeeID; } }
        public DateTime HireDate { get; set; }
        public DateTime? ExitDate { get; set; }
        public decimal Monthltysalary { get; set; }
        public double BounsPerc {  get; set; }
        public override enStatus Status { get; set; }

        public clsEmployee(int employeeID, int userID, DateTime hireDate, DateTime? exitDate, long permisions,
            decimal monthltysalary, double bounsPerc, int PersonID, string Username,
            string Password, string Salt, enStatus Status, enBloodType BloodType, int CreatedUserByEmployeeID, bool Duplication)
        {
            base.UserID = UserID;
            base.PersonID = PersonID;
            base.PersonInfo = clsPerson.Find(PersonID);
            base.UserName = Username;
            base.Password = Password;
            base.Salt = Salt;
            base.BloodType = BloodType;
            base.CreatedByEmployeeID = CreatedUserByEmployeeID;
            if (!Duplication)
                base._CreatedByEmployeeInfo = FindByEmployeeID(CreatedByEmployeeID, true);

            _EmployeeID = employeeID;
            UserID = userID;
            HireDate = hireDate;
            ExitDate = exitDate;
            Monthltysalary = monthltysalary;
            BounsPerc = bounsPerc;
            this.Status = Status;
            Permisions = permisions;

            Mode = enMode.Update;
        }

        public clsEmployee()
        {
            _EmployeeID = null;
            UserID = 0;
            HireDate = DateTime.MinValue;
            ExitDate = DateTime.MinValue;
            Monthltysalary = 0;
            BounsPerc = 0;
            this.Status =  enStatus.Active;
            Permisions = 0;
            Mode = enMode.Add;

            base.UserName = "";
            base.Password = "";
            base.Salt = null;
            base.BloodType = enBloodType.None;
            base.Mode = (clsUser.enMode)Mode;
        }

        public static DataTable GetEmployeeBy(int PageNumber, int RowPerPage)
        {
            return clsEmployeesData.GetEmployeeBy(PageNumber, RowPerPage);
        }

        public static int GetNumberOfRows()
        {
            return clsEmployeesData.GetNumberOfRowsForEmployee();
        }

        public static clsEmployee FindByEmployeeID(int EmployeeID, bool Duplication = false)
        {
            int userId=0;
            DateTime? exitDate = null;
            DateTime hireDate = DateTime.MinValue;
            decimal monthltysalary = 0;
            double bounsPerc = 0;
            byte status = 0;
            long permisions = 0;
            int CreatedUserByEmployeeID = 0;
            int PersonID = -1;
            string UserName = "", Password = "", Salt = "";
            byte BloodType = 0;

            if (clsEmployeesData.FindByEmployeeID(EmployeeID, ref userId, ref hireDate, ref exitDate,ref permisions, ref monthltysalary, ref bounsPerc, ref status,
               ref PersonID, ref UserName, ref Password, ref Salt, ref BloodType, ref CreatedUserByEmployeeID))
            {
                
                return new clsEmployee(EmployeeID, userId, hireDate, exitDate, permisions, monthltysalary, bounsPerc, PersonID
                    , UserName, Password, Salt, (enStatus)status, (enBloodType)BloodType,CreatedUserByEmployeeID, Duplication);
            }
            else
                return null;

        }
        public static clsEmployee FindEmployeeByUserID(int UserID, bool Duplication = false)
        {
            int employeeID = 0;
            DateTime? exitDate = null;
            DateTime hireDate = DateTime.MinValue;
            decimal monthltysalary = 0;
            double bounsPerc = 0;
            byte status = 0;
            long permisions = 0;
            int CreatedUserByEmployeeID = 0;
            int PersonID = -1;
            string UserName = "", Password = "", Salt = "";
            byte BloodType = 0;

            if (clsEmployeesData.FindByUserID(ref employeeID,  UserID, ref hireDate, ref exitDate, ref permisions, ref monthltysalary, ref bounsPerc,ref status,
               ref PersonID, ref UserName, ref Password, ref Salt, ref BloodType, ref CreatedUserByEmployeeID))
            {
                return new clsEmployee(employeeID, UserID, hireDate, exitDate, permisions, monthltysalary, bounsPerc, PersonID
                    , UserName, Password, Salt, (enStatus)status, (enBloodType)BloodType, CreatedUserByEmployeeID, Duplication);
            }
            else
                return null;
        }

        bool _AddNewEmployee()
        {
            _EmployeeID = clsEmployeesData.AddNewEmployee(UserID.Value, HireDate, null,Permisions, Monthltysalary, BounsPerc, (byte)Status);
            if (_EmployeeID.HasValue)
                return true;
            else
                return false;
        }

        bool _UpdateEmployee()
        {
            return clsEmployeesData.UpdateEmployeeByID(EmployeeID.Value, UserID.Value, HireDate, ExitDate, Permisions,Monthltysalary, BounsPerc, (byte)Status);
        }

        public bool Save()
        {
            base.Mode = (clsUser.enMode)Mode;
            if(!base.Save()) return false;

            switch (Mode)
            {
                case enMode.Add:
                    {
                        if(_AddNewEmployee())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        return false;
                    }
                case enMode.Update:
                    {
                        return _UpdateEmployee();
                    }
            }
            return false;
        }

        public static bool IsUserHasEmployeeAcc(int UserID)
        {
            return clsEmployeesData.IsUserHasEmployeeAcc(UserID);
        }

        public static long GetValueOfPermision(bool Checked, enPermision permision)
        {
            return Checked ? (long)permision : 0;
        }

        public static bool CheckAccessPermision(enPermision permision, long Permisions)
        {
            long permisionlong = (long)permision;
            long i = permisionlong & Permisions;
            enPermision permisionUser = (enPermision)i;
            if (permisionUser == permision)
            {
                return true;
            }
            return false;
        }

        public bool CheckAccessPermision(enPermision permision)
        {
           return CheckAccessPermision(permision, this.Permisions);
        }

        public static bool ChangePermision(int EmployeeID, long NewPermisions)
        {
            return clsEmployeesData.ChangePermision(EmployeeID, NewPermisions);
        }

        public bool ChangePermision(long NewPermisions)
        {
            return ChangePermision(this.EmployeeID.Value, NewPermisions);
        }

        public static decimal GetMaximumOfMonthltysalary()
        {
          return  clsEmployeesData.GetMaximumOfMonthltysalary();
        }

        public static bool DeleteEmployeeBy(int EmployeeID)
        {
            return clsEmployeesData.DeleteEmployeeBy(EmployeeID);
        }

        public static bool IsEmplooyeeExists(int EmployeeID)
        {
            return clsEmployeesData.IsEmployeeExists(EmployeeID);
        }

        public DataTable GetYourEmail(int PageNumber, int RowPerPage)
           => GetYourEmailBy(this.EmployeeID.Value, PageNumber, RowPerPage);
        public static DataTable GetYourEmailBy(int EmployeeID, int PageNumber, int RowPerPage)
            => clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForEmployee, (int)clsEmail.enType.YourEmail, (int)clsEmail.enFrom.ByEmployee
            , PageNumber, RowPerPage, null, EmployeeID);


        public DataTable GetYourMessages(int PageNumber, int RowPerPage) =>
          GetYourMessagesBy(EmployeeID.Value, RowPerPage, PageNumber);
        public static DataTable GetYourMessagesBy(int EmployeeID, int PageNumber, int RowPerPage)
            => clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForEmployee, (int)clsEmail.enType.YourMessages, (int)clsEmail.enFrom.ByEmployee
            , PageNumber, RowPerPage, EmployeeID, null);


        public static DataTable GetCallLogsBy(clsCallLog.enOrderType Type, int EmployeeID, int PageNumber, int RowPerPage)
            => clsCallLogs.GetCallLogBy((byte)Type, PageNumber, RowPerPage, EmployeeID,
                (byte)clsCallLog.enFrom.ByEmployee, EmployeeID, (byte)clsCallLog.enFor.ForEmployee);

        public DataTable GetCallLogsBy(clsCallLog.enOrderType Type, int PageNumber, int RowPerPage)
            => GetCallLogsBy(Type, this.EmployeeID.Value, PageNumber, RowPerPage);

        public int GetNumberOfCreatedAppointment()
            =>clsTestAppointmentData.GetNumberOfCreatedAppointmentBy(this.EmployeeID.Value);

        public int GetNumberOfCreatedApp()
            => clsApplicationData.GetNumberOfCreatedAppBy(this.EmployeeID.Value);

        public int GetNumberOfCreatedLocalLicense()
            => clsLicenseData.GetNumberOfCreatedLocalLicenseBy(this.EmployeeID.Value);

        public int GetNumberOfCreatedInternationalLicense()
            => clsInternationalLicenseData.GetNumberOfCreatedInternationalLicenseBy(this.EmployeeID.Value);

        public static DataTable GetArchiveOfAllEmployeesBy(int PageNumber, int RowPerPage)
            => clsEmployeesData.GetArchiveOfAllEmployeesBy(PageNumber, RowPerPage);

        public static int GetNumberOfRowsForEmployeesArchive()
            => clsEmployeesData.GetNumberOfRowsForEmployeesArchive();

        public static bool UpdateStatus(int EmployeeID, enStatus NewStatus)
            => clsEmployeesData.UpdateStatus(EmployeeID, (byte)NewStatus);

        public bool Return()
            => UpdateStatus(this.EmployeeID.Value, enStatus.Active);

        public static int? GetEmployeeIDByUserID(int UserID)
            => clsEmployeesData.GetEmployeeIDByUserID(UserID);

    }
}
