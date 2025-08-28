
using System;
using System.Configuration;
namespace DVLD_DataAccess
{
   public static class clsDataAccessSettings
    {
      // public static string ConnectionString = "Server=.;Database=DVLD;User Id=sa;Password=sa123456;";
      
        // public static string ConnectionString = ConfigurationManager.ConnectionStrings["MyDbConnection"].ConnectionString;

        public static string ConnectionString = ConfigurationManager.AppSettings["ConnectionString"];
    }
}
