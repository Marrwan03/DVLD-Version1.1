using System;
using System.Data;
using System.Runtime.InteropServices;
using DVLD_DataAccess;
using System.Text;
using DVLD_Buisness.Global_Classes;
using DVLD_DataAccess.MyFunSQL;
using DVLD_Buisness.Interfaces;

namespace DVLD_Buisness
{
    public  class clsUser : IReturn
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;
        public enum enBloodType { None,A_plus, O_plus, B_plus, AB_plus,
            A_mines, O_mines, B_mines,AB_mines }
       
        public enBloodType BloodType { get; set; }
        public int? UserID { set; get; }
        public int PersonID { set; get; }
        public clsPerson PersonInfo;
        public string UserName { set; get; }
        public string Password { set; get; }
        public string Salt { set; get; }
       public enum enStatus:byte { NotActive=0,Active, Deleted }
        public virtual enStatus Status { get; set; }
        public int CreatedByEmployeeID { set; get; }
      protected  clsEmployee _CreatedByEmployeeInfo;
        public clsEmployee CreatedByEmployeeInfo { get { return _CreatedByEmployeeInfo; } }
       
        public clsUser()

        {     
            this.UserID = null;
            this.UserName = "";
            this.Password = "";
            this.Salt = null;
            this.Status =  enStatus.Active;
            BloodType = enBloodType.None;
            Mode = enMode.AddNew;
        }

        private clsUser(int? UserID, int PersonID, string Username,string Password,string Salt,
            enStatus Status, enBloodType BloodType, int CreatedByEmployeeID)

        {
            this.UserID = UserID; 
            this.PersonID = PersonID;
            this.PersonInfo = clsPerson.Find(PersonID);
            this.UserName = Username;
            this.Password = Password;
            this.Salt=Salt;
            this.Status = Status;
            this.BloodType = BloodType;
            this.CreatedByEmployeeID = CreatedByEmployeeID;
            this._CreatedByEmployeeInfo = clsEmployee.FindByEmployeeID(CreatedByEmployeeID);
            Mode = enMode.Update;
        }

        private bool _AddNewUser()
        {
            //call DataAccess Layer 

            this.UserID = clsUserData.AddNewUser(this.PersonID,this.UserName,
                this.Password,this.Salt ,(byte)this.Status,  (byte)this.BloodType, this.CreatedByEmployeeID);

            return (this.UserID.HasValue);
        }
        private bool _UpdateUser()
        {
            //call DataAccess Layer 

            return clsUserData.UpdateUser(this.UserID,this.PersonID,this.UserName,
                this.Password,this.Salt,(byte)this.Status, this.CreatedByEmployeeID);
        }

        public static clsUser FindByUserID(int UserID)
        {
            int PersonID = -1;
            
            string UserName = "", Password = "", Salt="";
            byte BloodType = 0, Status=0;
            int CreatedByEmployeeID = 0;
            bool IsFound = clsUserData.GetUserInfoByUserID
                                ( UserID,ref PersonID, ref UserName,ref Password,ref Salt,ref Status, ref BloodType, ref CreatedByEmployeeID);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID,PersonID,UserName,Password,Salt, (enStatus)Status, (enBloodType) BloodType, CreatedByEmployeeID);
            else
                return null;
        }
        public static clsUser FindByPersonID(int PersonID)
        {
            int UserID = -1;
            
            string UserName = "", Password = "", Salt="";
            byte BloodType = 0, Status = 0;
            int CreatedByEmployeeID = 0;
            bool IsFound = clsUserData.GetUserInfoByPersonID
                                (PersonID, ref UserID, ref UserName, ref Password,ref Salt, ref Status, ref BloodType, ref CreatedByEmployeeID);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, UserID, UserName, Password,Salt, (enStatus)Status, (enBloodType)BloodType, CreatedByEmployeeID);
            else
                return null;
        }

        public static clsUser FindByUsername(string Username)
        {
            int UserID = -1, PersonID=-1;
            
            string UserName = "", Password = "", Salt = "";
            byte BloodType = 0, Status = 0;
            int CreatedByEmployeeID = 0;
            bool IsFound = clsUserData.GetUserInfoByUsername
                                (Username, ref Password, ref Salt, ref UserID,ref PersonID, ref Status,  ref BloodType, ref CreatedByEmployeeID);

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, UserID, UserName, Password, Salt, (enStatus)Status, (enBloodType)BloodType, CreatedByEmployeeID);
            else
                return null;
        }

        public static clsUser FindByUsernameAndPassword(string UserName,string PasswordInput)
        {
            int UserID = -1, PersonID = -1;
           
            bool IsActive = false;
            string Salt = "", Password="";
            byte BloodType = 0, Status = 0;
            int CreatedByEmployeeID = 0;
            bool IsFound = clsUserData.GetUserInfoByUsername(UserName, ref Password, ref Salt, ref UserID, ref PersonID, ref Status , ref BloodType, ref CreatedByEmployeeID) &&
                clsHash.IsPasswordCorrect(PasswordInput, Convert.FromBase64String(Password), Convert.FromBase64String(Salt));

            if (IsFound)
                //we return new object of that User with the right data
                return new clsUser(UserID, PersonID, UserName, Password,Salt, (enStatus)Status, (enBloodType)BloodType, CreatedByEmployeeID);
            else
                return null;
        }
        
        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewUser())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdateUser();

            }

            return false;
        }

        public static DataTable GetAllUsers()
        {
            return clsUserData.GetAllUsers();
        }
        public static DataTable GetUsersBy(int PageNumber, int RowPerPage)
            => clsUserData.GetUsersBy(PageNumber, RowPerPage);

        public static bool DeleteUser(int UserID)
        {
            return clsUserData.DeleteUser(UserID); 
        }

        public static bool isUserExist(int UserID)
        {
           return clsUserData.IsUserExist(UserID);
        }

        public static bool isUserExist(string UserName)
        {
            return clsUserData.IsUserExist(UserName);
        }

        public static bool isUserExistForPersonID(int PersonID)
        {
            return clsUserData.IsUserExistByPersonID(PersonID);
        }

        public  DataTable GetYourEmail( int PageNumber, int RowPerPage) =>
           GetYourEmailBy(UserID.Value,RowPerPage, PageNumber);
        public static DataTable GetYourEmailBy(int UserID,int PageNumber, int RowPerPage) =>
            clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForUser, (int)clsEmail.enType.YourEmail, (int)clsEmail.enFrom.ByUser
            , PageNumber, RowPerPage, null, UserID );

        public  DataTable GetYourMessages(int PageNumber, int RowPerPage) =>
          GetYourMessagesBy(UserID.Value, RowPerPage, PageNumber);
        public static DataTable GetYourMessagesBy(int UserID,int PageNumber, int RowPerPage)
            => clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForUser, (int)clsEmail.enType.YourMessages, (int)clsEmail.enFrom.ByUser
            , PageNumber, RowPerPage, UserID, null);


        public static DataTable GetCallLogsBy(clsCallLog.enOrderType Type, int UserID, int PageNumber, int RowPerPage)
           => clsCallLogs.GetCallLogBy((byte)Type, PageNumber, RowPerPage,
                UserID, (byte)clsCallLog.enFrom.ByUser, UserID, (byte)clsCallLog.enFor.ForUser);
        public DataTable GetCallLogsBy(clsCallLog.enOrderType Type, int PageNumber, int RowPerPage)
          => GetCallLogsBy(Type, this.UserID.Value, PageNumber, RowPerPage);

        public int? SendMessage(string Message, int RecipientID, clsEmail.enFor RecipientType)
        {
            return clsEmailsData.AddSendNewMessage(this.UserID.Value, (byte)clsEmail.enFrom.ByUser, Message, DateTime.Now, RecipientID, (byte)RecipientType);
        }
        public static int GetNumberOfRows() => clsUserData.GetNumberOfRowsForUsers();

        public static string GetBloodType(enBloodType bloodType)
        {
            switch(bloodType)
            {
                case enBloodType.A_plus:
                    return "A+";

                case enBloodType.O_plus:
                    return "O+";

                case enBloodType.B_plus:
                    return "B+";

                case enBloodType.AB_plus:
                    return "AB+";

                case enBloodType.A_mines:
                    return "A-";

                case enBloodType.O_mines:
                    return "O-";

                case enBloodType.B_mines:
                    return "B-";

                case enBloodType.AB_mines:
                    return "AB-";
                default:
                    return "Draw";
            }
        }

        public string GetBloodType()
        {
            return GetBloodType(this.BloodType);
        }

        public static enBloodType GetRandomeBloodType()
        {
            Random rand = new Random();
            return (enBloodType)rand.Next(1, 8);
        }

        public static DataTable GetAllLocalLicenseBy(int DriverID)
        {
            return clsLicenseData.GetAllLocalLicensesBy(DriverID);
        }
        public  DataTable GetAllLocalLicense()
        {
            return GetAllLocalLicenseBy(clsDriver.FindByPersonID(this.PersonID).DriverID.Value);
        }
        public static DataTable GetAllInternationalLicenseBy(int DriverID)
        {
            return clsInternationalLicenseData.GetAllInternationalLicensesBy(DriverID);
        }
        public DataTable GetAllInternationalLicense()
        {
            return GetAllInternationalLicenseBy(clsDriver.FindByPersonID(this.PersonID).DriverID.Value);
        }

        public static DataTable GetArchiveOfAllUsersBy(int PageNumber, int RowPerPage)
            => clsUserData.GetArchiveOfAllUsersBy(PageNumber, RowPerPage);

        public static int GetNumberOfRowsForUsersArchive()
            => clsUserData.GetNumberOfRowsForUsersArchive();
        public static bool UpdateStatus(int UserID, enStatus newstatus)
            => clsUserData.UpdateStatus(UserID, (byte)newstatus);

        public bool Return()
            => UpdateStatus(this.UserID.Value, enStatus.Active);
        public static int? GetUserIDByPersonID(int PersonID)
            => clsUserData.GetUserIDByPersonID(PersonID);
    }
}
