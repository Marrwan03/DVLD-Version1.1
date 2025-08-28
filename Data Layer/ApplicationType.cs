using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsApplicationTypeData
    {

        public static bool GetApplicationTypeInfoByID(int ApplicationTypeID, 
            ref string ApplicationTypeTitle, ref float ApplicationFees)
            {
                bool isFound = false;
                try
                {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetApplicationTypeInfoByID
                                          @ApplicationTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        connection.Open();

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                ApplicationTypeTitle = (string)reader["ApplicationTypeTitle"];
                                ApplicationFees = Convert.ToSingle(reader["ApplicationFees"]);
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
                    clsEventLog.WriteEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error);
                    isFound = false;
                }

                return isFound;
            }

        public static DataTable GetAllApplicationTypes()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllApplicationTypes";

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
                    clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                }
                return dt;

            }
        public static DataTable GetApplicationTypesBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetApplicationTypesBy
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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return dt;

        }

        public static int AddNewApplicationType( string Title, float Fees)
        {
            int ApplicationTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                        connection.Open();

                    string query = @"exec SP_AddNewApplicationType
                                    @ApplicationTypeTitle,
                                    @ApplicationFees,
                                    @ApplicationTypeID output";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            ApplicationTypeID = ID;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);

            }

            return ApplicationTypeID;

        }

        public static bool UpdateApplicationType(int ApplicationTypeID,string Title, float Fees)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateApplicationType
                                    @ApplicationTypeTitle,
                                    @ApplicationFees,
                                    @ApplicationTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationTypeTitle", Title);
                        command.Parameters.AddWithValue("@ApplicationFees", Fees);

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }

            return (rowsAffected > 0);
        }

        public static int GetNumberOfRowsForAppTypes()
        {
            int Rows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfRowsForAppTypes";

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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return Rows;
        }
    }
}
