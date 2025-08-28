using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsLicenseClassData
    {

        public static bool GetLicenseClassInfoByID(int LicenseClassID, 
            ref string ClassName, ref string ClassDescription, ref byte MinimumAllowedAge, 
            ref byte DefaultValidityLength, ref float ClassFees)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLicenseClassInfoByID
                                    @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ClassName = (string)reader["ClassName"];
                                ClassDescription = (string)reader["ClassDescription"];
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                                ClassFees = Convert.ToSingle(reader["ClassFees"]);

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

        public static bool GetLicenseClassInfoByClassName( string ClassName, ref int LicenseClassID,
            ref string ClassDescription, ref byte MinimumAllowedAge,
           ref byte DefaultValidityLength, ref float ClassFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLicenseClassInfoByClassName
                                     @ClassName";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ClassName", ClassName);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;
                                LicenseClassID = (int)reader["LicenseClassID"];
                                ClassDescription = (string)reader["ClassDescription"];
                                MinimumAllowedAge = (byte)reader["MinimumAllowedAge"];
                                DefaultValidityLength = (byte)reader["DefaultValidityLength"];
                                ClassFees = Convert.ToSingle(reader["ClassFees"]);

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

        public static DataTable GetAllLicenseClasses()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllLicenseClasses";

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

        public static int AddNewLicenseClass(string ClassName, string ClassDescription,
            byte MinimumAllowedAge,byte DefaultValidityLength, float ClassFees)
        {
            int LicenseClassID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewLicenseClass
                                    @LicenseClassID output,
                                    @ClassName,
                                    @ClassDescription,@MinimumAllowedAge,
                                    @DefaultValidityLength,@ClassFees";



                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ClassName", ClassName);
                        command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);



                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            LicenseClassID = insertedID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return LicenseClassID;

        }

        public static bool UpdateLicenseClass(int LicenseClassID, string ClassName, 
            string ClassDescription,
            byte MinimumAllowedAge, byte DefaultValidityLength, float ClassFees)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_UpdateLicenseClass
                                    @LicenseClassID,@ClassName,
                                    @ClassDescription,@MinimumAllowedAge,
                                    @DefaultValidityLength,@ClassFees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@ClassName", ClassName);
                        command.Parameters.AddWithValue("@ClassDescription", ClassDescription);
                        command.Parameters.AddWithValue("@MinimumAllowedAge", MinimumAllowedAge);
                        command.Parameters.AddWithValue("@DefaultValidityLength", DefaultValidityLength);
                        command.Parameters.AddWithValue("@ClassFees", ClassFees);


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

        
    }
}
