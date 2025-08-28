using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsLicenseData
    {

        public static bool GetLocalLicenseInfoByID(int LicenseID,ref int ApplicationID, ref int DriverID, ref int LicenseClass,
            ref DateTime IssueDate, ref DateTime ExpirationDate,ref string Notes,
            ref float PaidFees,ref bool IsActive, ref byte IssueReason, ref int CreatedByEmployeeID)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLocalLicenseInfoByID
                                    @LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];

                                if (reader["Notes"] == DBNull.Value)
                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

                                PaidFees = Convert.ToSingle(reader["PaidFees"]);

                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];


                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }

                }
                }
                catch (Exception ex)
                {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                isFound = false;
                }

                return isFound;
            }

        public static bool Find(ref int LicenseID, ref int ApplicationID, int DriverID, int LicenseClass,
            ref DateTime IssueDate, ref DateTime ExpirationDate, ref string Notes,
            ref float PaidFees, ref bool IsActive, ref byte IssueReason, ref int CreatedByEmployeeID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_FindLicenseBy
                                    @DriverID,
                                    @LicenseClass";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                LicenseID = (int)reader["LicenseID"];
                                ApplicationID = (int)reader["ApplicationID"];
                                LicenseClass = (int)reader["LicenseClass"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];

                                if (reader["Notes"] == DBNull.Value)
                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

                                PaidFees = Convert.ToSingle(reader["PaidFees"]);

                                IsActive = (bool)reader["IsActive"];
                                IssueReason = (byte)reader["IssueReason"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];
                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }

                        }
                    }

                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                isFound = false;
            }

            return isFound;
        }

        public static DataTable GetAllLocalLicenses()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllLocalLicenses";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }

                        }
                    }
                }
                }

                catch (Exception ex)
                {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

                return dt;

            }
        public static DataTable GetAllLocalLicensesBy(int DriverID)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllLocalLicenseBy
                                    @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }

                        }
                    }
                }
            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return dt;

        }

        public static int? AddNewLocalLicense(  int ApplicationID, int DriverID,  int LicenseClass,
             DateTime IssueDate,  DateTime ExpirationDate,  string Notes,
             float PaidFees,bool IsActive,byte IssueReason,  int CreatedByEmployeeID)
        {
            int LicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"
                             exec SP_AddNewLocalLicense
                                  @ApplicationID,@DriverID,@LicenseClass,
                                  @IssueDate,@ExpirationDate,@Notes,
                                  @PaidFees,@IsActive,
                                  @IssueReason,
                                  @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);

                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (Notes == "")
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);

                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);

                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            LicenseID = ID;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (LicenseID == -1)
                return null;

            return LicenseID;

        }

        public static bool UpdateLocalLicense(int? LicenseID ,int ApplicationID, int DriverID, int LicenseClass,
             DateTime IssueDate, DateTime ExpirationDate, string Notes,
             float PaidFees,bool IsActive,byte IssueReason, int CreatedByEmployeeID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateLocalLicense
                                    @ApplicationID, @DriverID,  
                                    @LicenseClass, @IssueDate,
                                    @ExpirationDate,@Notes,
                                    @PaidFees ,@IsActive,
                                    @IssueReason,@CreatedByEmployeeID,@LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@LicenseClass", LicenseClass);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);

                        if (Notes == "")
                            command.Parameters.AddWithValue("@Notes", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@Notes", Notes);

                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@IssueReason", IssueReason);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }

            return (rowsAffected > 0);
        }
        public static int GetActiveLicenseIDByPersonID(int PersonID,int LicenseClassID)
        {
            int LicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetActiveLicenseIDByPersonID
                                        @PersonID,
                                        @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return LicenseID;
        }
        public static int GetLastLicenseID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastLicenseID
                                        @PersonID,
                                        @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return LicenseID;
        }
        public static int? GetDeActiveLicenseIDByPersonID(int PersonID, int LicenseClassID)
        {
            int LicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDeActiveLicenseIDBy
                                        @PersonID,
                                        @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (LicenseID == -1)
                return null;

            return LicenseID;
        }

        public static bool DeactivateLicense(int? LicenseID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_DeactivateLocalLicenseBy
                                    @LicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);


                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }

            return (rowsAffected > 0);
        }
        public static DataTable GetDriverLocalLicensesBy(int DriverID, int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_GetDriverLocalLicensesBy
                                    @DriverID,@PageNumber, @RowPerPage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)

                            {
                                dt.Load(reader);
                            }
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return dt;

        }
        public static int GetNumberOfRowsForLocalLicenses(int DriverID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_GetNumberOfRowsForLocalLicensesBy
                                     @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DriverID", DriverID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Row))
                        {
                            Rows = Row;
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Rows;
        }
        public static bool IsAppIDExists(int AppID)
        {
            bool Exists = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsAppIDExistsAtLicenseBy
                                    @AppID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppID", AppID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if(result != null && bool.TryParse((result.ToString() =="1"? "true":"false"), out bool ex))
                        {
                            Exists = ex;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }

            return Exists;
        }
        public static int GetNumberOfCreatedLocalLicenseBy(int EmployeeID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_NumberOfCreatedLocalLicenseBy
                                     @EmpID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmpID", EmployeeID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Row))
                        {
                            Rows = Row;
                        }
                    }
                }


            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Rows;
        }

    }
}
