using System;
using System.Data.SqlClient;
using System.Data;
using System.Diagnostics;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsApplicationData
    {
        public static bool GetApplicationInfoByID(int ApplicationID, 
            ref int ApplicantPersonID, ref DateTime ApplicationDate, ref int ApplicationTypeID, 
            ref byte ApplicationStatus,ref DateTime LastStatusDate,
            ref float PaidFees,ref int? PaymentID, ref int CreatedByEmployeeID)
            {
                bool isFound = false;

                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetApplicationInfoByID
                                @ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                ApplicantPersonID = (int)reader["ApplicantPersonID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationTypeID = (int)reader["ApplicationTypeID"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                if (reader["PaymentID"] != DBNull.Value)
                                {
                                    PaymentID = (int)reader["PaymentID"];
                                }
                                else
                                {
                                    PaymentID = null;
                                }
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
        public static bool GetApplicationInfoByPersonIDAndAppTypeID(ref int ApplicationID,
           int ApplicantPersonID, ref DateTime ApplicationDate, int ApplicationTypeID,
           ref byte ApplicationStatus, ref DateTime LastStatusDate,
           ref float PaidFees, ref int? PaymentID, ref int CreatedByEmployeeID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetApplicationInfoByPersonIdAndAppTypeId
                                    @AppPersonID,
                                    @AppTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@AppPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@AppTypeID", ApplicationTypeID);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                ApplicationID = (int)reader["ApplicationID"];
                                ApplicationDate = (DateTime)reader["ApplicationDate"];
                                ApplicationStatus = (byte)reader["ApplicationStatus"];
                                LastStatusDate = (DateTime)reader["LastStatusDate"];
                                PaidFees = Convert.ToSingle(reader["PaidFees"]);
                                if (reader["PaymentID"] != DBNull.Value)
                                {
                                    PaymentID = (int)reader["PaymentID"];
                                }
                                else
                                {
                                    PaymentID = null;
                                }
                                CreatedByEmployeeID = (int)reader["CreatedByEmployeeID"];

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
        public static DataTable GetAllApplications()
            {

                DataTable dt = new DataTable();
                try
                {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetAllApplications";

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
        public static DataTable GetArchiveOfAllApplicationsBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_ArchiveOfAllApplicationsBy
                                    @PageNumber,
                                    @RowPerPage";

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
        public static DataTable GetApplicationReportForLocalLicenseBy(int PersonID, int PageNumber, int RowPerPage)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_GetApplicationReportForLocalLicenseBy
                                    @personID,
                                    @PageNumber,
                                    @RowPerPage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        command.Parameters.AddWithValue("@personID", PersonID);
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
        public static DataTable GetApplicationReportForAllTypesLicenseBy(int PersonID, int PageNumber, int RowPerPage)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_GetApplicationReportForAllTypesLicenseBy
                                    @personID,
                                    @PageNumber,
                                    @RowPerPage";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();
                        command.Parameters.AddWithValue("@personID", PersonID);
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
        public static int AddNewApplication( int ApplicantPersonID,  DateTime ApplicationDate,  int ApplicationTypeID,
             byte ApplicationStatus,  DateTime LastStatusDate,
             float PaidFees,int? PaymentID,  int CreatedByEmployeeID)
        {

            //this function will return the new person id if succeeded and -1 if not.
            int ApplicationID = -1;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewApplication
                            @ApplicationID output,
                            @ApplicantPersonID, @ApplicationDate,
                            @ApplicationTypeID, @ApplicationStatus,
                            @LastStatusDate, @PaidFees, @PaymentID,
                            @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@ApplicantPersonID", ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", PaidFees);
                        if(PaymentID.HasValue)
                        {
                            command.Parameters.AddWithValue("@PaymentID", PaymentID);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaymentID", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@CreatedByEmployeeID", CreatedByEmployeeID);

                    connection.Open();

                    object result = command.ExecuteScalar();

                    if (result != null && int.TryParse(result.ToString(), out int insertedID))
                    {
                        ApplicationID = insertedID;
                    }

                    }
                }

             }
           


            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return ApplicationID;
        }

        public static bool UpdateApplication(int ApplicationID, int ApplicantPersonID, DateTime ApplicationDate, int ApplicationTypeID,
             byte ApplicationStatus, DateTime LastStatusDate,
             float PaidFees,int? PaymentID, int CreatedByEmployeeID)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateApplication
                            @ApplicationID,
                            @ApplicantPersonID, @ApplicationDate,
                            @ApplicationTypeID, @ApplicationStatus,
                            @LastStatusDate, @PaidFees,@PaymentID,
                            @CreatedByEmployeeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@ApplicantPersonID", @ApplicantPersonID);
                        command.Parameters.AddWithValue("@ApplicationDate", @ApplicationDate);
                        command.Parameters.AddWithValue("@ApplicationTypeID", @ApplicationTypeID);
                        command.Parameters.AddWithValue("@ApplicationStatus", @ApplicationStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", @LastStatusDate);
                        command.Parameters.AddWithValue("@PaidFees", @PaidFees);
                        if (PaymentID.HasValue)
                        {
                            command.Parameters.AddWithValue("@PaymentID", PaymentID);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@PaymentID", DBNull.Value);
                        }
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

        public static bool DeleteApplication(int ApplicationID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_DeleteApplication
                                    @ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        connection.Open();

                        rowsAffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return (rowsAffected > 0);

        }

        public static bool IsApplicationExist(int ApplicationID)
        {
            bool isFound = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsApplicationExist
                                    @ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool Found))
                        {
                            isFound = Found;
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

        public static bool DoesPersonHaveActiveApplication(int PersonID, int ApplicationTypeID)
        {
           
           //incase the ActiveApplication ID !=-1 return true.
            return (GetActiveApplicationID(PersonID, ApplicationTypeID) != -1);
        }

        public static int? GetActiveApplicationID(int PersonID, int ApplicationTypeID)
        {
            int ActiveApplicationID =-1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetActiveApplicationID
                                    @ApplicantPersonID, 
                                    @ApplicationTypeID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);

                        connection.Open();
                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return ActiveApplicationID;
            }
            if (ActiveApplicationID == -1)
                return null;
            return ActiveApplicationID;
        }

        public static int GetActiveApplicationIDForLicenseClass(int PersonID, int ApplicationTypeID,int LicenseClassID)
        {
            int ActiveApplicationID = -1;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetActiveApplicationIDForLicenseClass
                                    @ApplicantPersonID, 
                                    @ApplicationTypeID,
                                    @LicenseClassID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicantPersonID", PersonID);
                        command.Parameters.AddWithValue("@ApplicationTypeID", ApplicationTypeID);
                        command.Parameters.AddWithValue("@LicenseClassID", LicenseClassID);
                        connection.Open();
                        object result = command.ExecuteScalar();


                        if (result != null && int.TryParse(result.ToString(), out int AppID))
                        {
                            ActiveApplicationID = AppID;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return ActiveApplicationID;
            }
            return ActiveApplicationID;
        }
      
        public static bool UpdateStatus(int ApplicationID, short NewStatus)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdateStatusOfApplication
                                    @NewStatus,
                                    @LastStatusDate,
                                    @ApplicationID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);
                        command.Parameters.AddWithValue("@LastStatusDate", DateTime.Now);


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
        public static int GetNumberOfRowsForApplicationsArchive()
        {
            int NumberOfRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetNumberOfRowsForApplicationsArchive";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int rows))
                        {
                            NumberOfRows = rows;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }
        public static int GetNumberOfLocalApplicationBy(int PersonID)
        {
            int NumberOfRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetNumberOfLocalApplicationBy
                                     @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int rows))
                        {
                            NumberOfRows = rows;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }
        public static int GetNumberOfAllTypesApplicationBy(int PersonID)
        {
            int NumberOfRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetNumberOfAllTypesApplicationBy
                                     @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int rows))
                        {
                            NumberOfRows = rows;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }
       public static bool SetNewPaymentIDFor(int ApplicationID, int? NewPaymentID)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_SetNewPaymentIDForApplications
                                    @ApplicationID,
                                    @NewPaymentID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@ApplicationID", ApplicationID);
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
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return false;
            }
            return (rowsAffected > 0);
        }
        public static int GetNumberOfCreatedAppBy(int EmployeeID)
        {
            int NumberOfRows = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_NumberOfCreatedAppBy
                                      @EmpID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@EmpID", EmployeeID);

                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int rows))
                        {
                            NumberOfRows = rows;
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                // Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }


    }
}
