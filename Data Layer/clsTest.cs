using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace DVLD_DataAccess
{
    public class clsTestData
    {

        public static bool GetTestInfoByID(int TestID, 
            ref int TestAppointmentID,ref bool TestResult, 
            ref string Notes  )
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetTestInfoByID @TestID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestID", TestID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"] == DBNull.Value)

                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

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


        public static bool GetLastTestByPersonAndTestTypeAndLicenseClass
            (int PersonID,int LicenseClassID,int TestTypeID, ref int TestID,
              ref int TestAppointmentID, ref bool TestResult,
              ref string Notes)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastTestByPersonAndTestTypeAndLicenseClass
                            @PersonID,
                            @LicenseClassID,@TestTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                TestID = (int)reader["TestID"];
                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                TestResult = (bool)reader["TestResult"];
                                if (reader["Notes"] == DBNull.Value)

                                    Notes = "";
                                else
                                    Notes = (string)reader["Notes"];

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


        public static DataTable GetAllTests()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = "exec SP_GetAllTests";

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
        public static List<int> GetAllTestsIDBy(int LocalDrivingLicenseApplicationID)
        {
            List<int> ints = new List<int>();
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllTestsIDBy
                                    @LocalDrivingLicenseApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.HasRows)

                            {
                                dt.Load(reader) ;
                                foreach (DataRow item in dt.Rows)
                                {
                                    ints.Add((int)item["TestID"]);
                                }
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

            return ints; 

        }

        public static int AddNewTest( int TestAppointmentID,  bool TestResult,
             string Notes)
        {
            int TestID = -1;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_AddNewTest
                                    @TestID output,
                                    @TestAppointmentID,@TestResult,
                                    @Notes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@TestID", TestID);
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);

                        if (Notes != "" && Notes != null)
                            command.Parameters.AddWithValue("@Notes", Notes);
                        else
                            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            TestID = ID;
                        }

                       
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return TestID;

        }

        public static bool UpdateTest(int TestID, int TestAppointmentID, bool TestResult,
             string Notes)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateTest
                                    @TestID,
                                    @TestAppointmentID,@TestResult,
                                    @Notes";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestID", TestID);
                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestResult", TestResult);
                        if (Notes != "" && Notes != null)
                            command.Parameters.AddWithValue("@Notes", Notes);
                        else
                            command.Parameters.AddWithValue("@Notes", System.DBNull.Value);

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

        public static byte GetPassedTestCount(int LocalDrivingLicenseApplicationID)
        {
            byte PassedTestCount = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetPassedTestCountBy
                                    @LocalDrivingLicenseApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);


                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && byte.TryParse(result.ToString(), out byte ptCount))
                        {
                            PassedTestCount = ptCount;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return PassedTestCount;



        }



    }
}
