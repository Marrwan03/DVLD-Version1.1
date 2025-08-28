using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsInternationalLicenseData
    {
        public static bool GetInternationalLicenseInfoByID(int InternationalLicenseID, 
            ref int ApplicationID, 
            ref int DriverID, ref int IssuedUsingLocalLicenseID, 
            ref DateTime IssueDate, ref DateTime ExpirationDate,ref bool IsActive, ref int CreatedByEmployeeID)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetInternationalLicenseInfoByID
                                     @InternationalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];
                                IssuedUsingLocalLicenseID = (int)reader["IssuedUsingLocalLicenseID"];
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];


                                IsActive = (bool)reader["IsActive"];
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
        public static bool GetInternationalLicenseInfoByLocalLicenseID(ref int InternationalLicenseID,
           ref int ApplicationID,
           ref int DriverID,  int IssuedUsingLocalLicenseID,
           ref DateTime IssueDate, ref DateTime ExpirationDate, ref bool IsActive, ref int CreatedByEmployeeID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetInternationalLicenseInfoByLocalID
                                     @LocalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalLicenseID", IssuedUsingLocalLicenseID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                InternationalLicenseID = (int)reader["InternationalLicenseID"];
                                ApplicationID = (int)reader["ApplicationID"];
                                DriverID = (int)reader["DriverID"];                             
                                IssueDate = (DateTime)reader["IssueDate"];
                                ExpirationDate = (DateTime)reader["ExpirationDate"];
                                IsActive = (bool)reader["IsActive"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];

                                isFound = true;
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
        public static DataTable GetAllInternationalLicenses()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllInternationalLicenses";

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
        public static DataTable GetAllInternationalLicensesBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"
            exec SP_GetInternationalLicensesBy
            @PageNumber , @RowPerPage ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
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
        public static DataTable GetAllInternationalLicensesBy(int DriverID)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllInternationalLicenseBy
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
        public static DataTable GetDriverInternationalLicenses(int DriverID)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDriverInternationalLicensesBy
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
        public static int? AddNewInternationalLicense( int ApplicationID,
             int DriverID,  int IssuedUsingLocalLicenseID,
             DateTime IssueDate,  DateTime ExpirationDate, bool IsActive,  int CreatedByEmployeeID)
        {
            int InternationalLicenseID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_AddNewInternationalLicense
                                    @InternationalLicenseID output, 
                                    @ApplicationID, @DriverID,
                                    @IssuedUsingLocalLicenseID, 
                                    @IssueDate, @ExpirationDate, 
                                    @IsActive, @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (InternationalLicenseID == -1)
                return null;

            return InternationalLicenseID;

        }

        public static bool UpdateInternationalLicense(
              int InternationalLicenseID , int ApplicationID,
             int DriverID, int IssuedUsingLocalLicenseID,
             DateTime IssueDate, DateTime ExpirationDate, bool IsActive, int CreatedByEmployeeID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateInternationalLicenseBy
                                    @InternationalLicenseID, 
                                    @ApplicationID, @DriverID,
                                    @IssuedUsingLocalLicenseID, 
                                    @IssueDate, @ExpirationDate, 
                                    @IsActive, @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        command.Parameters.AddWithValue("@IssueDate", IssueDate);
                        command.Parameters.AddWithValue("@ExpirationDate", ExpirationDate);
                        command.Parameters.AddWithValue("@IsActive", IsActive);
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

        public static int GetActiveInternationalLicenseIDBy(int DriverID, int IssuedUsingLocalLicenseID)
        {
            int InternationalLicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetActiveInternationalLicenseIDBy
                                    @DriverID,
                                    @IssuedUsingLocalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@IssuedUsingLocalLicenseID", IssuedUsingLocalLicenseID);
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            InternationalLicenseID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return InternationalLicenseID;
        }

        public static int GetLastIntLicenseID(int PersonID, int LicenseClassID)
        {
            int LastInternationalLicenseID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastIntLicenseID
                                    @PersonID,
                                    @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int LastLicenseID))
                        {
                            LastInternationalLicenseID = LastLicenseID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return LastInternationalLicenseID;
        }

        public static bool IsInternationalLicenseExistsByLocalLicenseID(int LocalLicenseID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_IsInternationalLicenseExistsByLocalLicenseID
                                     @LocalLicenseID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalLicenseID", LocalLicenseID);

                        object result = command.ExecuteScalar();    
                        if(result != null && bool.TryParse((result.ToString() == "1"? "True" : "False"),out bool IsExists))
                        {
                            isFound = IsExists; 
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool IsAppIDExists(int AppID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_IsAppIDExistsinInternationalLicense
                                      @AppID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@AppID", AppID);

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse((result.ToString() == "1" ? "True" : "False"), out bool IsExists))
                        {
                            isFound = IsExists;
                        }

                    }

                }
            }

            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                //Console.WriteLine("Error: " + ex.Message);
                isFound = false;
            }

            return isFound;
        }

        public static bool DeactivateInternationalLicense(int InternationalLicenseID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_DeactivateInternationalLicenseBy
                                    @InternationalLicenseID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@InternationalLicenseID", InternationalLicenseID);


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

       public static int GetNumberOfRows()
        {
            int NumberOfRows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetNumberOfRowsForInternationalLicense";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        connection.Open();
                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int Rows))
                        {
                            NumberOfRows = Rows;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return 0;
            }

            return NumberOfRows;
        }
        public static DataTable GetDriverIntLicensesBy(int DriverID, int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_GetDriverIntLicensesBy
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
        public static int GetNumberOfRowsForIntLicenses(int DriverID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_GetNumberOfRowsForDriverIntLicensesBy
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
        public static int GetNumberOfCreatedInternationalLicenseBy(int EmployeeID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_NumberOfCreatedInternationalLicenseBy
                                    @EmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

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
