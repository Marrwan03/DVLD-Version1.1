using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsPaymentsData
    {
        public static int? AddNewPayment(int CardID, decimal Amount, DateTime DateTime)
        {
            int? PaymentID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_AddNewPayment
                                    @NumberOfPayments output,
                                    @CardID,
                                    @Amount,
                                    @Datetime";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@NumberOfPayments", PaymentID);
                        command.Parameters.AddWithValue("@CardID", CardID);
                        command.Parameters.AddWithValue("@Amount", Amount);
                        command.Parameters.AddWithValue("@Datetime", DateTime);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            PaymentID = ID;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return PaymentID;
        }
        public static bool Find(int PaymentID,ref int CardID,ref decimal Amount,ref DateTime DateTime)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetBillInfoByPaymentID
                                    @NumberOfPayments";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@NumberOfPayments", PaymentID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read()) 
                            {
                                CardID = (int)reader["CardID"];
                                Amount = (decimal)reader["Amount"];
                                DateTime = (DateTime)reader["Datetime"];
                                IsFound = true;
                            }
                           
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return IsFound;
        }
        public static DataTable FindAllPaymentsBy(int UserID)
        {
            DataTable dtPaymentsInfo = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetBillInfoByUserID
                                    @UserID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                                dtPaymentsInfo.Load(reader);
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return dtPaymentsInfo;
        }
        public static DataTable GetAllPaymentsBy(byte PaymentType, int PersonID, int PageNumber, int RowPerPage)
        {
            DataTable dtAllPayments = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = string.Empty;
                    if(PaymentType == 1)
                        Query = @"exec SP_GetAllPaymentsInApplicationBy
                                    @PersonID,@PageNumber,@RowPerPage";
                    else
                        Query = @"exec SP_GetAllPaymentsInAppointmentBy
                                    @PersonID,@PageNumber,@RowPerPage";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                        {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                        using (SqlDataReader reader = command.ExecuteReader())
                            {
                                if (reader.HasRows)
                                    dtAllPayments.Load(reader);
                            }
                        }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return dtAllPayments;
        }
        public static int NumberOfPaymentsBy(byte PaymentType, int PersonID)
        {
            int NumberOfPayments = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = string.Empty;
                    if(PaymentType == 1)
                        Query = @"exec SP_NumberOfPaymentsInApplicationBy
                                    @PersonID";
                    else
                        Query = @"exec SP_NumberOfPaymentsInAppointmentBy
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                        {
                            command.Parameters.AddWithValue("@PersonID", PersonID);
                            object result = command.ExecuteScalar();
                            if (result != null && int.TryParse(result.ToString(), out int Rows))
                            {
                                NumberOfPayments = Rows;
                            }
                        }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return NumberOfPayments;
        }

    }
}
