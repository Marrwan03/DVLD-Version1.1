using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsDriverData
    {

        public static bool GetDriverInfoByDriverID(int DriverID, 
            ref int PersonID,ref int CreatedByEmployeeID, ref DateTime CreatedDate )
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDriverInfoByDriverID
                                    @DriverID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DriverID", DriverID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];


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

        public static bool GetDriverInfoByPersonID(int PersonID,ref int DriverID,
            ref int CreatedByEmployeeID, ref DateTime CreatedDate)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDriverInfoByPersonID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DriverID = (int)reader["DriverID"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];
                                CreatedDate = (DateTime)reader["CreatedDate"];

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

        public static DataTable GetAllDrivers()
            {

                DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllDrivers";

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
        public static DataTable GetDriversBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try { 
            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

                connection.Open();

                string query = @"exec SP_GetDriversBy
                           @PageNumber, 
                           @RowPerPage";

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

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return dt;

        }

        public static int GetNumberOfRowsForDrivers()
        {

            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = "exec SP_GetNumberOfRowsForDrivers";

                    using (SqlCommand command = new SqlCommand(query, connection))
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
                    // Console.WriteLine("Error: " + ex.Message);
                    clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                }
                

            return Rows;

        }

        public static int? AddNewDriver( int PersonID, int CreatedByEmployeeID)
        {
            int DriverID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewDriver
                                    @PersonID, @CreatedByEmployeeID, 
                                    @CreatedDate";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);
                        command.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            DriverID = ID;
                        }

                       
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (DriverID == -1)
                return null;

            return DriverID;

        }

        public static bool UpdateDriver(int DriverID, int PersonID, int CreatedByEmployeeID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    //we dont update the createddate for the driver.
                    string query = @"exec SP_UpdateDriver
                                    @DriverID,
                                    @PersonID, 
                                    @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DriverID", DriverID);
                        command.Parameters.AddWithValue("@PersonID", PersonID);
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

        public static bool IsExistsBy(int UserID)
        {
            bool IsExists=false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsUserExistsInDriver
                                    @UserID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@UserID", UserID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if(result != null && Boolean.TryParse((result.ToString()=="1"? "true" : "false"),out bool exists))
                        {
                            IsExists = exists;
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
            return IsExists;
        }

    }
}
