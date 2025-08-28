using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsDetainedLicenseData
    {

        public static bool GetDetainedLicenseInfoByID(int DetainID,
            ref int LicenseID,ref byte LicenseType, ref DateTime DetainDate,
            ref float FineFees,ref int CreatedByEmployeeID, 
            ref bool IsReleased, ref DateTime ReleaseDate, 
            ref int ReleasedByEmployeeID,ref int ReleaseApplicationID)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDetainedLicenseInfoByID
                                    @DetainID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DetainID", DetainID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                LicenseID = (int)reader["LicenseID"];
                                LicenseType = (byte)reader["LicenseType"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];
                                IsReleased = (bool)reader["IsReleased"];
                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByEmployeeID"] == DBNull.Value)

                                    ReleasedByEmployeeID = -1;
                                else
                                    ReleasedByEmployeeID = (int)reader["ReleasedByUserID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];


                                // The record was found
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

        
        public static bool GetDetainedLicenseInfoByLicenseID(int LicenseID, byte LicenseType,
         ref int DetainID, ref DateTime DetainDate,
         ref float FineFees, ref int CreatedByEmployeeID,
         ref bool IsReleased, ref DateTime ReleaseDate,
         ref int ReleasedByEmployeeID, ref int ReleaseApplicationID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDetainedLicenseInfoByLicenseID
                                    @LicenseID,
                                    @LicenseType";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@LicenseType", LicenseType);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                DetainID = (int)reader["DetainID"];
                                DetainDate = (DateTime)reader["DetainDate"];
                                FineFees = Convert.ToSingle(reader["FineFees"]);
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];

                                IsReleased = (bool)reader["IsReleased"];

                                if (reader["ReleaseDate"] == DBNull.Value)

                                    ReleaseDate = DateTime.MaxValue;
                                else
                                    ReleaseDate = (DateTime)reader["ReleaseDate"];


                                if (reader["ReleasedByEmployeeID"] == DBNull.Value)

                                    ReleasedByEmployeeID = -1;
                                else
                                    ReleasedByEmployeeID = (int)reader["ReleasedByEmployeeID"];

                                if (reader["ReleaseApplicationID"] == DBNull.Value)

                                    ReleaseApplicationID = -1;
                                else
                                    ReleaseApplicationID = (int)reader["ReleaseApplicationID"];

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

        public static DataTable GetAllDetainedLicenses()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllDetainedLicenses";

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

        public static DataTable GetDetainedLicensesBy(int PageNumber, int RowPerPage)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetDetainedLicenseBy
                           @PageNumber,@RowPerPage";

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

        public static int GetNumberOfRows()
        {
            int NumberOfRows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetNumberOfRowsForDetainedLicenses";

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


        public static int AddNewDetainedLicense( 
            int LicenseID,
            byte LicenseType,  DateTime DetainDate,
            float FineFees,  int CreatedByEmployeeID)
        {
            int DetainID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewDetainedLicense
                                    @DetainID output,
                                    @LicenseID, @LicenseType,
                                    @DetainDate, @FineFees, @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@LicenseType", LicenseType);
                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            DetainID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return DetainID;

        }

        public static bool UpdateDetainedLicense(int DetainID, 
            int LicenseID,byte LicenseType, DateTime DetainDate,
            float FineFees, int CreatedByEmployeeID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateDetainedLicense
                                    @DetainedLicenseID ,
                                    @LicenseID, @LicenseType,
                                    @DetainDate, @FineFees, @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DetainedLicenseID", DetainID);
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@LicenseType", LicenseType);
                        command.Parameters.AddWithValue("@DetainDate", DetainDate);
                        command.Parameters.AddWithValue("@FineFees", FineFees);
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


        public static bool ReleaseDetainedLicense(int DetainID,
                 int ReleasedByEmployeeID, int ReleaseApplicationID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_ReleaseDetainedLicense
                                     @DetainID ,
                                     @ReleaseDate, @ReleasedByEmployeeID,
                                     @ReleaseApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@DetainID", DetainID);
                        command.Parameters.AddWithValue("@ReleasedByEmployeeID", ReleasedByEmployeeID);
                        command.Parameters.AddWithValue("@ReleaseApplicationID", ReleaseApplicationID);
                        command.Parameters.AddWithValue("@ReleaseDate", DateTime.Now);
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

        public static bool IsLicenseDetained(int LicenseID, byte LicenseType)
        {
            bool IsDetained = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsLicenseDetained
                                    @LicenseID,
                                    @LicenseType";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LicenseID", LicenseID);
                        command.Parameters.AddWithValue("@LicenseType", LicenseType);
                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null)
                        {
                            IsDetained = Convert.ToBoolean(result);
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return IsDetained;
            ;

        }

      

    }
}
