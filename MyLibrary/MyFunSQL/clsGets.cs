using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess.MyFunSQL
{
    public class clsGets
    {
        public static int GetMaximamPage(int NumberOfRows, int DivRowsBy) 
        {
            int MaxPage = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = "select dbo.GetMaximamPage(@NumberOfRows, @DivRowsBy)";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@NumberOfRows", NumberOfRows);
            command.Parameters.AddWithValue("@DivRowsBy", DivRowsBy);

            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int insertedID))
                {
                    MaxPage = insertedID;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
            }
            return MaxPage;
        }

        public static int GetMaxNumberOfRowsForEmail(byte Type,
        int SenderID,byte SenderType, int RecipientID, byte RecipientType)
        {
            int MaxRows = 0;

            SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString);

            string query = @"EXECUTE [dbo].[SP_GetMaxOfRowsForEmail] 
                                           @Type
                                           ,@SenderID
                                           ,@SenderType
                                           ,@RecipientID
                                           ,@RecipientType";

            SqlCommand command = new SqlCommand(query, connection);

            command.Parameters.AddWithValue("@Type", Type);
            command.Parameters.AddWithValue("@SenderID", SenderID);
            command.Parameters.AddWithValue("@SenderType", SenderType);
            command.Parameters.AddWithValue("@RecipientID", RecipientID);
            command.Parameters.AddWithValue("@RecipientType", RecipientType);


            try
            {
                connection.Open();

                object result = command.ExecuteScalar();

                if (result != null && int.TryParse(result.ToString(), out int Rows))
                {
                    MaxRows = Rows;
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            finally
            {
                connection.Close();
            }
            return MaxRows;
        }

        public static int GetMaxNumberOfRowsForCallLog(byte Type,
        int CallerID, byte CallerType, int RecipientID, byte RecipientType)
        {
            int MaxRows=0;
            try
            {
               
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"EXECUTE [dbo].[SP_GetMaxOfRowsForCallLog] 
                                                     @Type
                                                    ,@CallerID
                                                    ,@CallerType
                                                    ,@RecipientID
                                                    ,@RecipientType";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Type", Type);
                        command.Parameters.AddWithValue("@CallerID", CallerID);
                        command.Parameters.AddWithValue("@CallerType", CallerType);
                        command.Parameters.AddWithValue("@RecipientID", RecipientID);
                        command.Parameters.AddWithValue("@RecipientType", RecipientType);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Row))
                        {
                            MaxRows = Row;
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return MaxRows;

        }



    }
}
