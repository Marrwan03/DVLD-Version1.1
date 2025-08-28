using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Configuration;

namespace DVLD_DataAccess
{
    public class clsUserData
    {
        public static bool GetUserInfoByUserID(int? UserID, ref int PersonID, ref string UserName,
            ref string Password,ref string Salt,  ref byte Status,  ref byte BloodType, ref int CreatedByEmployeeID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetUserInfoByUserID
                                    @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                UserName = (string)reader["UserName"];
                                Password = (string)reader["Password"];

                                if (reader["Salt"] == DBNull.Value)
                                {
                                    Salt = null;
                                }
                                else
                                {
                                    Salt = (string)reader["Salt"];
                                }

                                Status = (byte)reader["Status"];
                                BloodType = (byte)reader["BloodType"];
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

        public static bool GetUserInfoByPersonID(int PersonID, ref int UserID, ref string UserName,
          ref string Password,ref string Salt, ref byte Status,  ref byte BloodType, ref int CreatedByEmployeeID)
        {
            bool isFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetUserInfoByPersonID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                UserID = (int)reader["UserID"];
                                UserName = (string)reader["UserName"];
                                Password = (string)reader["Password"];
                                Salt = (string)reader["Salt"];
                                Status = (byte)reader["Status"];
                                BloodType = (byte)reader["BloodType"];
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

        public static bool GetUserInfoByUsername(string UserName,ref string Password,ref  string Salt,
            ref int UserID, ref int PersonID, ref byte Status,  ref byte BloodType, ref int CreatedByEmployeeID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetUserInfoByUsername
                                     @UserName";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", UserName);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                UserID = (int)reader["UserID"];
                                PersonID = (int)reader["PersonID"];
                                Password = (string)reader["Password"];
                                if (reader["Salt"] == DBNull.Value)
                                {
                                    Salt = null;
                                }
                                else
                                {
                                    Salt = (string)reader["Salt"];
                                }
                                Status = (byte)reader["Status"];
                                BloodType = (byte)reader["BloodType"];
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

        public static int? AddNewUser(int PersonID,  string UserName,
             string Password, string Salt,  byte Status, byte BloodType, int CreatedByEmployeeID)
        {
        
            int UserID = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"EXECUTE SP_AddNewUser
                                     @UserID OUTPUT ,@PersonID ,@UserName
                                    ,@Password, @Salt, @BloodType
                                    ,@CreatedByEmployeeID,@Status";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Salt", Salt);
                        command.Parameters.AddWithValue("@BloodType", BloodType);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);
                        command.Parameters.AddWithValue("@Status", Status);
                    connection.Open();

                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(), out int uID))
                        {
                            UserID = uID;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (UserID == 0)
                return null;
            else
                return UserID;
        }

        public static bool UpdateUser(int? UserID, int PersonID, string UserName,
             string Password,string Salt, byte Status, int CreatedByEmployeeID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query = @"exec SP_UpdateUser
                                    @UserID ,
                                    @PersonID, @UserName,
                                    @Password, @Salt,
                                    @Status, @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@UserName", UserName);
                        command.Parameters.AddWithValue("@Password", Password);
                        command.Parameters.AddWithValue("@Salt", Salt);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

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

        public static DataTable GetAllUsers()
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetAllUsers";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
        public static DataTable GetUsersBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetUsersBy
                                    @PageNumber, @RowPerPage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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
        public static DataTable GetArchiveOfAllUsersBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_ArchiveOfAllUsersBy
                                    @PageNumber, @RowPerPage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
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

        public static bool DeleteUser(int UserID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_DeleteUser
                                    @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        rowsAffected = command.ExecuteNonQuery();
                    }

                }
            }

            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                // Console.WriteLine("Error: " + ex.Message);
            }


            return (rowsAffected > 0);

        }

        public static bool UpdateStatus(int UserID, byte NewStatus)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_UpdateStatusOfUsers
                                    @UserID,
                                    @NewStatus";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        rowsAffected = command.ExecuteNonQuery();
                    }

                }
            }

            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                // Console.WriteLine("Error: " + ex.Message);
            }


            return (rowsAffected > 0);

        }


        public static bool IsUserExist(int UserID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_IsUserExistByUserID
                                    @UserID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool IsExists))
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

        public static bool IsUserExist(string UserName)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsUserExistByUsername
                                     @UserName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserName", UserName);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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

        public static bool IsUserExistByPersonID(int PersonID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsUserExistByPersonID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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

        public static bool ChangePassword(int UserID, string NewPassword, string Salt)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_ChangePassword
                                    @UserID,
                                    @Password, @Salt";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Password", NewPassword);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@Salt", Salt);

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
        public static string GetPassword(string Username)
        {
            string Password=null;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetPasswordByUsername
                                    @Username";
                    using(SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);

                        object result = command.ExecuteScalar();
                        if(result != null)
                        {
                            Password = result.ToString();
                        }

                    }


                }
            }
            catch(Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Password;
        }

        public static string GetSaltOfPassword(string Username)
        {
            string Salt = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetSaltOfPasswordByUsername
                                    @Username";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Username", Username);

                        object result = command.ExecuteScalar();
                        if (result != null)
                        {
                            Salt = result.ToString();
                        }

                    }


                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Salt;
        }
        public static int GetNumberOfRowsForUsers()
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = "exec SP_GetNumberOfRowsForUsers";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {

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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Rows;
        }
        public static int GetNumberOfRowsForUsersArchive()
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = "exec SP_GetNumberOfRowsForUsersArchive";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Rows;
        }
        public static int? GetUserIDByPersonID(int PersonID)
        {
            int? ID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetUserIDByPersonID
                                     @PersonID";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            ID = id;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return ID;
        }

    }
}
