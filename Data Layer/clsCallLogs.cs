using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsCallLogs
    {

        public static int? AddNewCallLog(int CallerID,byte CallerType, string PhoneNumber, DateTime CallTime, int Duration,
            byte CallType,  byte Status,
            int RecipientID, byte RecipientType)
        {
            int? CallLogID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_AddNewCallLog
                                    @CallID output,
                                    @CallerID, @CallerType,
                                    @PhoneNumber,@CallTime,
                                    @Duration, @CallType,
                                    @Status, @RecipientID,
                                    @RecipientType";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CallID", CallLogID);
                        command.Parameters.AddWithValue("@CallerID", CallerID);
                        command.Parameters.AddWithValue("@CallerType", CallerType);
                        command.Parameters.AddWithValue("@PhoneNumber", PhoneNumber);
                        command.Parameters.AddWithValue("@CallTime", CallTime);
                        command.Parameters.AddWithValue("@Duration", Duration);
                        command.Parameters.AddWithValue("@CallType", CallType);
                        command.Parameters.AddWithValue("@Status", Status);
                        command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        command.Parameters.AddWithValue("@RecipientType", RecipientType);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            CallLogID = ID;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return CallLogID;

    }


        public static bool FindCallLogBy(int CallID,ref int CallerID,ref byte CallerType, ref string PhoneNumber,
            ref DateTime CallTime,ref int Duration,
            ref byte CallType,ref byte Status,
           ref int RecipientID,ref byte RecipientType)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindCallLogBy
                                    @CallID";
                    using(SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CallID", CallID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                CallerID = (int)reader["CallerID"];
                                CallerType = (byte)reader["CallerType"];
                                PhoneNumber = (string)reader["PhoneNumber"];
                                CallTime = (DateTime)reader["CallTime"];
                                Duration = (int)reader["Duration"];
                                CallType = (byte)reader["CallType"];
                                Status = (byte)reader["Status"];
                                RecipientID = (int)reader["RecipientID"];
                                RecipientType = (byte)reader["RecipientType"];
                                IsFound = true;
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                
            }
            return IsFound;
        }

       


        public static DataTable GetCallLogBy(byte TypeOfOrder,int PageNumber,int RowPerPage, int CallerID,
            byte CallerType, int RecipientID, byte RecipientType)
        {
            DataTable dt = new DataTable();

            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec [dbo].[SP_GetCallLogBy] 
                                       @Type
                                      ,@PageNumber
                                      ,@RowPerPage
                                      ,@CallerID
                                      ,@CallerType
                                      ,@RecipientID
                                      ,@RecipientType";

                    using(SqlCommand command = new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@Type", TypeOfOrder);
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                        command.Parameters.AddWithValue("@CallerID", CallerID);
                        command.Parameters.AddWithValue("@CallerType", CallerType);
                        command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        command.Parameters.AddWithValue("@RecipientType", RecipientType);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
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


        public  static bool DeleteCallLog(int CallID)
        {
            int RecordEffected = 0;
            try
            {
                using(SqlConnection  connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_DeleteCallLogBy
                                    @CallID";
                    using(SqlCommand command = new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@CallID", CallID);
                        RecordEffected =  command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return RecordEffected > 0;
        }

    }
}
