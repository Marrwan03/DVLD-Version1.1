using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsTestAppointmentData
    {

        public static bool GetTestAppointmentInfoByID(int TestAppointmentID, 
            ref int TestTypeID, ref int LocalDrivingLicenseApplicationID,
            ref DateTime  AppointmentDate, ref float PaidFees,ref int? PaymentID,
            ref int CreatedByEmployeeID, ref int? RetakeTestApplicationID, ref byte Status)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetTestAppointmentInfoByID
                                    @TestAppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;
                                TestTypeID = (int)reader["TestTypeID"];
                                LocalDrivingLicenseApplicationID = (int)reader["LocalDrivingLicenseApplicationID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                if (reader["PaymentID"] == DBNull.Value)
                                    PaymentID = null;
                                else
                                    PaymentID = (int)reader["PaymentID"];

                                if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                    RetakeTestApplicationID = null;
                                else
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];
                                Status = (byte)reader["Status"];
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

        public static bool GetLastTestAppointment( 
             int LocalDrivingLicenseApplicationID,  int TestTypeID, 
            ref int TestAppointmentID,ref DateTime AppointmentDate,
            ref float PaidFees, ref int? PaymentID, ref int CreatedByEmployeeID,
            ref int? RetakeTestApplicationID, ref byte Status)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastTestAppointment
                                     @TestTypeID,
                                     @LocalDrivingLicenseApplicationID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {

                                // The record was found
                                isFound = true;

                                TestAppointmentID = (int)reader["TestAppointmentID"];
                                AppointmentDate = (DateTime)reader["AppointmentDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                if (reader["PaymentID"] == DBNull.Value)
                                    PaymentID = null;
                                else
                                    PaymentID = (int)reader["PaymentID"];
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];

                                if (reader["RetakeTestApplicationID"] == DBNull.Value)
                                    RetakeTestApplicationID = null;
                                else
                                    RetakeTestApplicationID = (int)reader["RetakeTestApplicationID"];

                                Status = (byte)reader["Status"];
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

        public static DataTable GetAllTestAppointments()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllTestAppointments";


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
        public static DataTable GetApplicationTestAppointmentsPerTestType(int LocalDrivingLicenseApplicationID,int TestTypeID)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_GetApplicationTestAppointmentsPerTestType
                                    @LocalDrivingLicenseApplicationID,
                                    @TestTypeID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);


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
        public static DataTable GetApplicationTestAppointmentsPerTestTypeBy(int LocalDrivingLicenseApplicationID, int TestTypeID, int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @" exec SP_GetAppTestAppointmentsPerTestTypeByPageNumber
                             @TestTypeID,
                             @LocalDrivingLicenseApplicationID,
                             @PageNumber,
                             @RowPerPage";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
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
        public static DataTable GetApplicationTestAppointmentsBy(int PersonID, int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetAllTestAppointmentInfoByPersonIDAndPageNumber
                                    @PersonID,
                                    @PageNumber,
                                    @RowPerPage";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
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
        public static DataTable GetArchiveOfAllAppointmentsBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_ArchiveOfAllAppointmentsBy
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

            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return dt;

        }
        public static int AddNewTestAppointment(
             int TestTypeID,  int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate,  float PaidFees, int ? PaymentID,  int CreatedByEmployeeID, int? RetakeTestApplicationID)
        {
            int TestAppointmentID = -1;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_AddNewTestAppointment
                                    @TestAppointmentID output,
                                    @LocalDrivingLicenseApplicationID, 
                                    @TestTypeID, @AppointmentDate, 
                                    @PaidFees, @PaymentID, @CreatedByEmployeeID, 
                                    @RetakeTestApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        if (PaymentID.HasValue)
                            command.Parameters.AddWithValue("@PaymentID", PaymentID);
                        else
                            command.Parameters.AddWithValue("@PaymentID", DBNull.Value);

                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        if (!RetakeTestApplicationID.HasValue)

                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            TestAppointmentID = ID;
                        }



                        

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }


            return TestAppointmentID;

        }

        public static bool UpdateTestAppointment(int TestAppointmentID,  int TestTypeID,  int LocalDrivingLicenseApplicationID,
             DateTime AppointmentDate,  float PaidFees, int? PaymentID,
             int CreatedByEmployeeID,int? RetakeTestApplicationID, byte Status)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateTestAppointment
                                    @TestAppointmentID,
                                    @LocalDrivingLicenseApplicationID, 
                                    @TestTypeID, @AppointmentDate, 
                                    @PaidFees,@PaymentID, @CreatedByEmployeeID, 
                                    @IsLocked,@RetakeTestApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@AppointmentDate", AppointmentDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        if (PaymentID.HasValue)
                            command.Parameters.AddWithValue("@PaymentID", PaymentID);
                        else
                            command.Parameters.AddWithValue("@PaymentID", DBNull.Value);
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                        if (!RetakeTestApplicationID.HasValue)

                            command.Parameters.AddWithValue("@RetakeTestApplicationID", DBNull.Value);
                        else
                            command.Parameters.AddWithValue("@RetakeTestApplicationID", RetakeTestApplicationID);

                        command.Parameters.AddWithValue("@Status", Status);



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

        public static int? GetTestID(int TestAppointmentID)
        {
            int TestID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetTestID
                                    @TestAppointmentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {


                        command.Parameters.AddWithValue("@TestAppointmentID", TestAppointmentID);


                        connection.Open();

                        object result = command.ExecuteScalar();

                        if (result != null && int.TryParse(result.ToString(), out int insertedID))
                        {
                            TestID = insertedID;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (TestID == -1)
                return null;
            return TestID;

        }

        public static int GetNumberOfRowsForAppTestAppointmentsPerTestTypeBy(int LocalDrivingLicenseApplicationID, int TestTypeID)
        {

            int Rows=0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfRowsForAppTestAppointmentsPerTestTypeBy
                                    @LocalDrivingLicenseApplicationID,
                                    @TestTypeID";


                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);

                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(), out int Row))
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
        public static int GetNumberOfRowsForAppTestAppointmentsBy(int PersonID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfRowsForTestAppointmentBy
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);

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
        public static bool SetNewPaymentIDFor(int AppointmentID, int? NewPaymentID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_SetNewPaymentIDForAppointment
                                    @AppointmentID,
                                    @NewPaymentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        if (NewPaymentID.HasValue)
                            command.Parameters.AddWithValue("@NewPaymentID", NewPaymentID.Value);
                        else
                            command.Parameters.AddWithValue("@NewPaymentID", DBNull.Value);


                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }
            return (rowsAffected > 0);
        }

        public static int? GetLastRetakeAppIDBy(int TestTypeID, int LocalDrivingLicenseApplications)
        {
            int? RetakeAppID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastRetakeAppID
                                    @TestTypeID,
                                    @LocalDrivingLicenseApplications";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        command.Parameters.AddWithValue("@TestTypeID", TestTypeID);
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplications", LocalDrivingLicenseApplications);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            RetakeAppID = ID;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return RetakeAppID;
        }
        public static int GetNumberOfCreatedAppointmentBy(int EmployeeID)
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_NumberOfCreatedAppointmentBy
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
        public static int GetNumberOfRowsForAppointmentsArchive()
        {
            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfRowsForAppointmentsArchive";

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
        public static int GetTotalOfPassTestsBy(int LocalDrivingLicenseApplicationID)
        {
            int TotalOfTests = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_TotalOfTests
                                    @LocalDrivingLicenseApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Tests))
                        {
                            TotalOfTests = Tests;
                        }
                    }

                }

            }

            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return TotalOfTests;

        }
        public static bool UpdateStatus(int AppointmentID, byte NewStatus)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateStatusOfTestAppointmentsBy
                                    @AppointmentID,
                                    @NewStatus";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@AppointmentID", AppointmentID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        

                        connection.Open();
                        rowsAffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }
            return (rowsAffected > 0);
        }

        public static DateTime GetLastAppointmentDateBy(int LocalDrivingLicenseApplicationID)
        {
            DateTime LastDate= DateTime.Now;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetLastAppointmentDateBy
                                        @LocalDrivingLicenseApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@LocalDrivingLicenseApplicationID", LocalDrivingLicenseApplicationID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if(result != null && DateTime.TryParse(result.ToString(), out DateTime dt))
                            LastDate = dt;
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return LastDate;
        }

    }
}
