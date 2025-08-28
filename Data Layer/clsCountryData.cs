using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsCountryData
    {

        public static bool GetCountryInfoByID(int ID, ref string CountryName, ref string CountryCode)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_GetCountryInfoByID
                                    @CountryID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CountryID", ID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                CountryName = (string)reader["CountryName"];
                                CountryCode = (string)reader["CountryCode"];

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

        public static bool GetCountryInfoByName(string CountryName, ref int ID, ref string CountryCode)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetCountryInfoByName
                            @CountryName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@CountryName", CountryName);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                ID = (int)reader["CountryID"];
                                CountryCode = (string)reader["CountryCode"];
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

        public static DataTable GetAllCountries()
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllCountries";

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

    }
}
