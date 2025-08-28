using System;
using System.Data;
using DVLD_DataAccess;

namespace DVLD_Buisness
{
    public class clsCountry
    {

        public int ID { set; get; }
        public string CountryName { set; get; }

        public string CountryCode { set; get; }
   
        public clsCountry()

        {
            this.ID = -1;
            this.CountryName = "";
            this.CountryCode = "";
        }

        private clsCountry(int ID, string CountryName, string CountryCode)

        {
            this.ID = ID;
            this.CountryName = CountryName;
            this.CountryCode = CountryCode;
        }

        public static clsCountry Find(int ID)
        {
            string CountryName = "", CountryCode="";

            if (clsCountryData.GetCountryInfoByID(ID, ref CountryName, ref CountryCode))

                return new clsCountry(ID, CountryName, CountryCode);
            else
                return null;

        }

        public static clsCountry Find(string CountryName)
        {

            int ID = -1;
            string CountryCode = "";
            if (clsCountryData.GetCountryInfoByName(CountryName, ref ID, ref CountryCode))

                return new clsCountry(ID, CountryName, CountryCode);
            else
                return null;

        }

        public static DataTable GetAllCountries()
        {
            return clsCountryData.GetAllCountries();

        }

    }
}
