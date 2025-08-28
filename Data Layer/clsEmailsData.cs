using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsEmailsData
    {
      
       
        public static int? AddSendNewMessage( int SenderID, byte SenderType,  string Message,
            DateTime Time,int RecipientID, byte RecipientType)
        {
            int ? EmailID = -1;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_AddSendNewMessage
                                    @EmailID output ,
                                    @SenderID, @SenderType,
                                    @Message, 
                                    @Time, @RecipientID, 
                                    @RecipientType";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", EmailID);
                        command.Parameters.AddWithValue("@SenderID", SenderID);
                        command.Parameters.AddWithValue("@SenderType", SenderType);
                        command.Parameters.AddWithValue("@Message", Message);
                        command.Parameters.AddWithValue("@Time", Time);
                        command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        command.Parameters.AddWithValue("@RecipientType", RecipientType);
                         object result =  command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int EmID))
                        {
                            EmailID = EmID;
                        }

                    }

                }
            } 
            catch (Exception ex) 
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return EmailID;
        }

        public static DataTable GetEmailInfoFor(byte ForWho, byte Type, byte FromWho, int PageNumber, int RowPerPage,
             int? SenderID, int? RecipientID)
        {
            DataTable dtUserEmail = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string Query= @"exec SP_GetEmailInfoFor
                                      @ForWho
                                     ,@FromWho
                                     ,@Type
                                     ,@PageNumber
                                     ,@RowPerPage
                                     ,@SenderID
                                     ,@RecipientID";

                    connection.Open();
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@ForWho", ForWho);
                        command.Parameters.AddWithValue("@FromWho", FromWho);
                        command.Parameters.AddWithValue("@Type", Type);
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);
                        if (SenderID.HasValue)
                        {
                            command.Parameters.AddWithValue("@SenderID", SenderID);
                            command.Parameters.AddWithValue("@RecipientID", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@SenderID", DBNull.Value);
                            command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        }
                       
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dtUserEmail.Load(reader);
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error); }
            return dtUserEmail;

        }

        //public static DataTable GetEmailMessagesBy(int ForWho, int Type, int PageNumber,
        //    int RowPerPage, int? SenderUserID, int? RecipientPersonID, int? RecipientUserID)
        //{
        //    DataTable dtUserEmail = new DataTable();
        //    try
        //    {
        //        using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
        //        {
        //            connection.Open();
        //            string Query;
        //            Query = @"exec SP_GetEmailMessagesBy
        //                    @ForWho,
        //                    @Type,
        //                    @PageNumber,
        //                    @RowPerPage,
        //                    @SenderUserID,
        //                    @RecipientPersonID,
        //                    @RecipientUserID";

        //            using (SqlCommand command = new SqlCommand(Query, connection))
        //            {
        //                command.Parameters.AddWithValue("@ForWho", ForWho);
        //                command.Parameters.AddWithValue("@Type", Type);
        //                command.Parameters.AddWithValue("@PageNumber", PageNumber);
        //                command.Parameters.AddWithValue("@RowPerPage", RowPerPage);

        //                if(SenderUserID.HasValue)
        //                    command.Parameters.AddWithValue("@SenderUserID", SenderUserID);
        //                else
        //                    command.Parameters.AddWithValue("@SenderUserID", DBNull.Value);

        //                if (RecipientPersonID.HasValue)
        //                    command.Parameters.AddWithValue("@RecipientPersonID", RecipientPersonID);
        //                else
        //                    command.Parameters.AddWithValue("@RecipientPersonID", DBNull.Value);

        //                if (RecipientUserID.HasValue)
        //                    command.Parameters.AddWithValue("@RecipientUserID", RecipientUserID);
        //                else
        //                    command.Parameters.AddWithValue("@RecipientUserID", DBNull.Value);


        //                using (SqlDataReader reader = command.ExecuteReader())
        //                {
        //                    if (reader.HasRows)
        //                    {
        //                        dtUserEmail.Load(reader);
        //                    }
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex) { clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error); }
        //    return dtUserEmail;
        //}

        public static bool FindEmailByID(int EmailID, ref int SenderID, ref byte SenderType, ref string Message,
          ref DateTime Time, ref int RecipientID, ref byte RecipientType)
        {
            bool IsFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindEmailByID 
                                    @EmailID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", EmailID);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                SenderID = (int)reader["SenderID"];
                                SenderType = (byte)reader["SenderType"];
                                Message = (string)reader["Message"];
                                Time = (DateTime)reader["Time"];
                                RecipientID = (int)reader["RecipientID"];
                                RecipientType = (byte)reader["RecipientType"];


                                IsFound = true;
                            }
                        }
                    }

                }
            }
            catch (Exception ex) { clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error); }
            return IsFound;

        }
   
        public static bool DeleteMessage(int MessageID)
        {
            int RecordEffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_DeleteEmailByID
                                    @EmailID";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmailID", MessageID);

                        RecordEffected = command.ExecuteNonQuery();

                    }
                    

                }
            }
            catch(Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return RecordEffected > 0;
        }


        public static bool UpdateMessage(int EmailID, int SenderID, byte SenderType, string Message,
            DateTime Time, int RecipientID, byte RecipientType)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_UpdateMessageByID
                                    @EmailID
                                   ,@SenderID
                                   ,@SenderType
                                   ,@Message
                                   ,@Time
                                   ,@RecipientID
                                   ,@RecipientType";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Message", Message);
                        command.Parameters.AddWithValue("@Time", Time);
                        command.Parameters.AddWithValue("@SenderID", SenderID);
                        command.Parameters.AddWithValue("@SenderType", SenderType);
                        command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        command.Parameters.AddWithValue("@RecipientType", RecipientType);
                        command.Parameters.AddWithValue("@EmailID", EmailID);

                        RecordEffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch(Exception ex)
            {
                clsEventLog.WriteEventLog(Message, EventLogEntryType.Error);
            }

            return RecordEffected > 0;

        }


    }
}
