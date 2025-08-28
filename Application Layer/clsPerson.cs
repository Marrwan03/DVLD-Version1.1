using System;
using System.Data;
using System.Xml.Linq;
using DVLD_DataAccess;
using System.Text;
using System.Threading.Tasks;
using DVLD_Buisness.Interfaces;

namespace DVLD_Buisness
{
    public  class clsPerson:IReturn
    {
        public enum enMode { AddNew = 0, Update = 1 };
        public enMode Mode = enMode.AddNew;

        public int? PersonID { set; get; }
        public string FirstName { set; get; }
        public string SecondName { set; get; }
        public string ThirdName { set; get; }
        public string LastName { set; get; }
        public string FullName
        {
            //get { return FirstName + " " + SecondName + " " + ThirdName + " " + LastName; }
            get
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(FirstName + " ");
                sb.Append(SecondName + " ");
                if(string.IsNullOrEmpty(ThirdName))
                    sb.Append(ThirdName + " ");
                sb.Append(LastName);
                return sb.ToString();
            }

        }
        public string NationalNo { set; get; }
        public DateTime DateOfBirth { set; get; }
        public short Gendor { set; get; }
        public string Address { set; get; }
        public string Phone { set; get; }
        public string Email { set; get; }
        public int NationalityCountryID { set; get; }

        public clsCountry CountryInfo;

        private string _ImagePath;
      
        public string ImagePath   
        {
            get { return _ImagePath; }   
            set { _ImagePath = value; }  
        }

        private byte GetPersonAge()
        {
            TimeSpan sp = DateTime.Now.Subtract(this.DateOfBirth);
            DateTime Age = new DateTime(sp.Ticks);
            return Convert.ToByte(Age.Year);
        }
        public byte Age {  set; get; }
        public bool IsDeleted { set; get; }
        public clsPerson()

        {
            this.PersonID = null;
            this.FirstName = "";
            this.SecondName = "";
            this.ThirdName = "";
            this.LastName = "";
            this.DateOfBirth = DateTime.Now;
            this.Address = "";
            this.Phone = "";
            this.Email = "";
            this.NationalityCountryID = -1;
            this.Age = 0;
            this.ImagePath = "";
            this.IsDeleted = false;
            Mode = enMode.AddNew;
        }

        private clsPerson(int PersonID, string FirstName,string SecondName, string ThirdName,
            string LastName,string NationalNo, DateTime DateOfBirth,short Gendor,
             string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath, bool IsDeleted)

        {
            this.PersonID = PersonID;
            this.FirstName = FirstName;
            this.SecondName= SecondName;
            this.ThirdName = ThirdName;
            this.LastName = LastName;
            this.NationalNo = NationalNo;   
            this.DateOfBirth = DateOfBirth;
            this.Gendor= Gendor;
            this.Address = Address;
            this.Phone = Phone;
            this.Email = Email;
            this.NationalityCountryID = NationalityCountryID;
            this.ImagePath = ImagePath;
            this.IsDeleted = IsDeleted;
            this.Age = GetPersonAge();
            this.CountryInfo = clsCountry.Find(NationalityCountryID);
            Mode = enMode.Update;
        }

        private bool _AddNewPerson()
        {
            //call DataAccess Layer 

            this.PersonID = clsPersonData.AddNewPerson(
                this.FirstName,this.SecondName ,this.ThirdName,
                this.LastName,this.NationalNo,
                this.DateOfBirth, this.Gendor, this.Address, this.Phone, this.Email,
                this.NationalityCountryID, this.ImagePath, this.IsDeleted);

            return (this.PersonID != null);
        }

        private bool _UpdatePerson()
        {
            //call DataAccess Layer 

            return clsPersonData.UpdatePerson(
                this.PersonID, this.FirstName,this.SecondName,this.ThirdName,
                this.LastName, this.NationalNo, this.DateOfBirth, this.Gendor,
                this.Address, this.Phone, this.Email, 
                  this.NationalityCountryID, this.ImagePath, this.IsDeleted);
        }

        public static clsPerson Find(int PersonID)
        {

            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",NationalNo="", Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int NationalityCountryID = -1;
            short Gendor = 0;
            bool IsDeleted = false;


            bool IsFound = clsPersonData.GetPersonInfoByID 
                                (
                                    PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref NationalNo, ref DateOfBirth,
                                    ref Gendor, ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath, ref IsDeleted
                                );

            if (IsFound)
                //we return new object of that person with the right data
                return new clsPerson(PersonID, FirstName,SecondName ,ThirdName, LastName,
                          NationalNo, DateOfBirth,Gendor, Address, Phone, Email,NationalityCountryID, ImagePath, IsDeleted);
            else
                return null;
        }

        public static clsPerson Find(string NationalNo)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "",  Email = "", Phone = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID=-1,NationalityCountryID = -1;
            short Gendor = 0;
            bool IsDeleted = false;
            bool IsFound = clsPersonData.GetPersonInfoByNationalNo
                                (
                                    NationalNo, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gendor,ref Address, ref Phone, ref Email,
                                    ref NationalityCountryID, ref ImagePath, ref IsDeleted
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth,Gendor, Address, Phone, Email, NationalityCountryID, ImagePath, IsDeleted);
            else
                return null;
        }

        public static clsPerson FindBy(string Phone)
        {
            string FirstName = "", SecondName = "", ThirdName = "", LastName = "", Email = "", NationalNo = "", Address = "", ImagePath = "";
            DateTime DateOfBirth = DateTime.Now;
            int PersonID = -1, NationalityCountryID = -1;
            short Gendor = 0;
            bool IsDeleted = false;
            bool IsFound = clsPersonData.GetPersonInfoByPhone
                                (
                                    Phone, ref PersonID, ref FirstName, ref SecondName,
                                    ref ThirdName, ref LastName, ref DateOfBirth,
                                    ref Gendor, ref Address, ref NationalNo, ref Email,
                                    ref NationalityCountryID, ref ImagePath, ref IsDeleted
                                );

            if (IsFound)

                return new clsPerson(PersonID, FirstName, SecondName, ThirdName, LastName,
                          NationalNo, DateOfBirth, Gendor, Address, Phone, Email, NationalityCountryID, ImagePath, IsDeleted);
            else
                return null;
        }

        public bool Save()
        {
            switch (Mode)
            {
                case enMode.AddNew:
                    if (_AddNewPerson())
                    {

                        Mode = enMode.Update;
                        return true;
                    }
                    else
                    {
                        return false;
                    }

                case enMode.Update:

                    return _UpdatePerson();

            }

            return false;
        }

        public async static Task< DataTable> GetAllPeople() => await clsPersonData.GetAllPeople();

        public static DataTable GetPeopleBy(int PageNumber, int RowPerPage) => clsPersonData.GetPeopleBy(PageNumber, RowPerPage);
        

        public static bool DeletePerson(int ID)
        {
            return clsPersonData.DeletePerson(ID); 
        }

        public static bool isPersonExist(int ID)
        {
           return clsPersonData.IsPersonExist(ID);
        }

        public static bool isPersonExist(string NationlNo)
        {
            return clsPersonData.IsPersonExist(NationlNo);
        }

        public static bool isPhoneExist(string Phone)
        {
            return clsPersonData.IsPhoneExists(Phone);
        }

        public static bool isEmailExist(string Email)
        {
            return clsPersonData.IsEmailExists(Email);
        }


        public DataTable GetYourEmail(int PageNumber, int RowPerPage)
            => GetYourEmailBy(this.PersonID.Value,PageNumber, RowPerPage);

        public static DataTable GetYourEmailBy(int PersonID,int PageNumber, int RowPerPage) 
            => clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForPerson, (int)clsEmail.enType.YourEmail, (int)clsEmail.enFrom.ByPerson
            , PageNumber, RowPerPage,null, PersonID);

        public DataTable GetYourMessages(int PageNumber, int RowPerPage) =>
          GetYourMessagesBy(PersonID.Value, RowPerPage, PageNumber);
        public static DataTable GetYourMessagesBy(int PersonID, int PageNumber, int RowPerPage)
            => clsEmailsData.GetEmailInfoFor((int)clsEmail.enFor.ForUser, (int)clsEmail.enType.YourMessages, (int)clsEmail.enFrom.None
            , PageNumber, RowPerPage, PersonID, null);

        public static DataTable GetCallLogsBy(clsCallLog.enOrderType Type,int PersonID, int PageNumber, int RowPerPage)
            => clsCallLogs.GetCallLogBy((byte)Type,PageNumber, RowPerPage, PersonID,
                (byte)clsCallLog.enFrom.ByPerson, PersonID, (byte)clsCallLog.enFor.ForPerson);

        public  DataTable GetCallLogsBy(clsCallLog.enOrderType Type, int PageNumber, int RowPerPage)
           => GetCallLogsBy(Type, this.PersonID.Value, PageNumber, RowPerPage);

        public bool IsPersonHasEmail()
        {
            return clsPersonData.IsPersonHasEmail(this.PersonID.Value);
        }

        public static bool IsPersonHasEmail(int PersonID)
        {
            return clsPersonData.IsPersonHasEmail(PersonID);
        }

        public static int GetNumberOfRows()
        {
            return clsPersonData.GetNumberOfRowsForPeople();
        }

        public static DataTable GetApplicationReportForLocalLicenseBy(int PersonID, int PageNumber, int RowPerPage)
        {
            return clsApplicationData.GetApplicationReportForLocalLicenseBy(PersonID, PageNumber, RowPerPage);
        }
        public DataTable GetApplicationReportForLocalLicense(int PageNumber, int RowPerPage)
        {
            return GetApplicationReportForLocalLicenseBy(this.PersonID.Value, PageNumber, RowPerPage);
        }
        public static int GetNumberOfLocalApplicationBy(int PersonID)
        {
            return clsApplicationData.GetNumberOfLocalApplicationBy(PersonID);
        }
        public int GetNumberOfLocalApplicationBy()
        {
            return GetNumberOfLocalApplicationBy(this.PersonID.Value);
        }

        public static DataTable GetApplicationReportForAllTypesLicenseBy(int PersonID, int PageNumber, int RowPerPage)
        {
            return clsApplicationData.GetApplicationReportForAllTypesLicenseBy(PersonID, PageNumber, RowPerPage);
        }
        public DataTable GetApplicationReportForAllTypesLicenseBy(int PageNumber, int RowPerPage)
        {
            return GetApplicationReportForAllTypesLicenseBy(this.PersonID.Value, RowPerPage, PageNumber);
        }
        public static int GetNumberOfAllTypesApplicationBy(int PersonID)
        {
            return clsApplicationData.GetNumberOfAllTypesApplicationBy(PersonID);
        }
        public int GetNumberOfAllTypesApplicationBy()
        {
            return GetNumberOfAllTypesApplicationBy(this.PersonID.Value);
        }
        public int GetNumberOfRowsForAppTestAppointmentsBy()
            => clsTestAppointmentData.GetNumberOfRowsForAppTestAppointmentsBy(this.PersonID.Value);

        public static DataTable GetArchiveOfAllPeopleBy(int PageNumber, int RowPerPage)
            => clsPersonData.GetArchiveOfAllPeopleBy(PageNumber, RowPerPage);

        public static int GetNumberOfRowsForPeopleArchive()
            => clsPersonData.GetNumberOfRowsForPeopleArchive();

        public bool Return()
        => clsPersonData.ReturnDeletedPerson(this.PersonID.Value);
    }
}
