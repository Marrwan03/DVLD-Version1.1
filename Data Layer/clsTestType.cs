using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsTestTypeData
    {

        public static bool GetTestTypeInfoByID(int TestTypeID, 
            ref string TestTypeTitle, ref string TestDescription ,ref float TestFees)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetTestTypeInfoByID 
                                    @TestTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestTypeTitle = (string)reader["TestTypeTitle"];
                                TestDescription = (string)reader["TestTypeDescription"];
                                TestFees = Convert.ToSingle(reader["TestTypeFees"]);

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

        public static bool GetTestTypeInfoByTitle(ref int TestTypeID,
           string TestTypeTitle, ref string TestDescription, ref float TestFees)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_GetTestTypeInfoByTitle
                                    @Title";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Title", TestTypeTitle);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestTypeID = (int)reader["TestTypeID"];
                                TestDescription = (string)reader["TestTypeDescription"];
                                TestFees = Convert.ToSingle(reader["TestTypeFees"]);

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


        public static DataTable GetAllTestTypes()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllTestTypes";

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

        public static int AddNewTestType( string Title,string Description, float Fees)
        {
            int TestTypeID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewTestType
                                    @TestTypeID output,
                                    @TestTypeTitle, @TestTypeDescription,
                                    @TestTypeFees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            TestTypeID = ID;
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }



            return TestTypeID;

        }

        public static bool UpdateTestType(int TestTypeID,string Title,string Description, float Fees)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateTestType
                                    @TestTypeID,
                                    @TestTypeTitle, @TestTypeDescription,
                                    @TestTypeFees";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@TestTypeTitle", Title);
                        command.Parameters.AddWithValue("@TestTypeDescription", Description);
                        command.Parameters.AddWithValue("@TestTypeFees", Fees);

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

        public static int GetCountOfTestTypes()
        {
            int Count = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetCountOfTestTypes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int C))
                        {
                            Count = C;
                        }


                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return Count;

        }

    }
}
