using System;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DVLD_DataAccess
{
    public class clsCreditCardData
    {
        public static bool FindByCardID(int cardID,ref int userID,ref string cardNumber,ref string cVV,
           ref DateTime issueDate, ref byte cardType, ref decimal balance, ref bool isActive)
        {
            bool IsFound = false;
            try
            {

                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindCreditCardBy
                                     @CardID";

                    using(SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CardID", cardID);
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                IsFound = true;
                                userID = (int)reader["UserID"];
                                cardNumber = (string)reader["CardNumber"];
                                cVV = (string)reader["CVV"];
                                issueDate = (DateTime)reader["IssueDate"];
                                cardType = (byte)reader["CardType"];
                                balance = (decimal)reader["Balance"];
                                isActive = (bool)reader["IsActive"];
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return IsFound;
        }

        public static bool FindByUserIDAndCardType(ref int cardID,  int userID, ref string cardNumber, ref string cVV,
          ref DateTime issueDate, byte cardType, ref decimal balance, ref bool isActive)
        {
            bool IsFound = false;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindCreditCardByUserIDAndCardType
                                     @UserID,
                                     @CardType";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@CardType", cardType);
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            
                            if (reader.Read())
                            {
                                IsFound = true;
                                cardID = (int)reader["CardID"];
                                cardNumber = (string)reader["CardNumber"];
                                cVV = (string)reader["CVV"];
                                issueDate = (DateTime)reader["IssueDate"];
                                balance = (decimal)reader["Balance"];
                                isActive = (bool)reader["IsActive"];
                            }
                        }
                    }

                }

            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return IsFound;
        }
        public static DataTable FindByUserID(int userID)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindCreditCardByUserID
                                    @UserID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", userID);
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
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, System.Diagnostics.EventLogEntryType.Error);
            }
            return dt;
        }

        public static bool IsUserHasCreditCardWithSameTypeBy(int UserID, byte CardType)
        {
            bool IsExists = false ;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_IsUserHasCreditCardWithSameTypeBy
                                    @UserID,
                                    @CardType";
                    using(SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@CardType", CardType);

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool IsFound))
                        {
                            IsExists = IsFound;
                        }

                    }
                }
            }
            catch(Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return IsExists;
        }

        public static int? AddNewCreaditCard(int userID, string cardNumber, string cVV,
           DateTime issueDate, byte cardType, decimal balance, bool isActive)
        {
            int? CreaditCardID = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"EXECUTE SP_AddNewCreaditCard
                                     @CardID OUTPUT
                                    ,@UserID
                                    ,@CardNumber
                                    ,@CVV
                                    ,@IssueDate
                                    ,@CardType
                                    ,@Balance
                                    ,@IsActive";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CardID", CreaditCardID);
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@CardNumber", cardNumber);
                        command.Parameters.AddWithValue("@CVV", cVV);
                        command.Parameters.AddWithValue("@IssueDate", issueDate);
                        command.Parameters.AddWithValue("@CardType", cardType);
                        command.Parameters.AddWithValue("@Balance", balance);
                        command.Parameters.AddWithValue("@IsActive",isActive);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            CreaditCardID=ID;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return CreaditCardID;
        }


        public static bool UpdateCreaditCardBy(int CardID, int userID, string cardNumber, string cVV,
           DateTime issueDate, byte cardType, decimal balance, bool isActive)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"EXECUTE SP_UpdateCreaditCard
   @CardID
  ,@UserID
  ,@CardNumber
  ,@CVV
  ,@IssueDate
  ,@CardType
  ,@Balance
  ,@IsActive";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CardID", CardID);
                        command.Parameters.AddWithValue("@UserID", userID);
                        command.Parameters.AddWithValue("@CardNumber", cardNumber);
                        command.Parameters.AddWithValue("@CVV", cVV);
                        command.Parameters.AddWithValue("@IssueDate", issueDate);
                        command.Parameters.AddWithValue("@CardType", cardType);
                        command.Parameters.AddWithValue("@Balance", balance);
                        command.Parameters.AddWithValue("@IsActive", isActive);

                       RecordEffected = command.ExecuteNonQuery();

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return RecordEffected > 0;
        }

        public static bool IsCardNumberExists(string CardNumber)
        {
            bool Exists = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_IsCardNumberExists
                                    @CardNumber";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@CardNumber", CardNumber);

                        object result = command.ExecuteScalar();
                        if(result != null && bool.TryParse(result.ToString(), out bool Found))
                        {
                            Exists = Found;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return Exists;
        }

        public static bool IsUserHasCreditCardBy(int UserID)
        {
            bool IsExists = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_IsUserHasCreditCard
                                    @UserID";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool Found))
                        {
                            IsExists = Found;
                        }

                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return IsExists;
        }

        public static bool Payment(float Fees, int UserID, byte CardType)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_Payment
                                        @Fees ,
                                        @UserID ,
                                        @CardType";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@Fees", Fees);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@CardType", CardType);

                        RecordEffected = command.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return RecordEffected > 0;
        }
       

    }
}
