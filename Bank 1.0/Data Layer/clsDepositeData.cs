using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DVLD_DataAccess;
using System.Diagnostics;
using System.Data.SqlClient;
using System.Data;

namespace DataAccess
{
    public class clsDepositeData
    {
        public static int? AddNewDeposite(int FromCardID, DateTime datetime, decimal amount, int ToCardID, string Note)
        {
            int DepositeID = -1;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"EXECUTE SP_AddNewDeposite
                                     @DepositeID OUTPUT
                                    ,@FromCardID
                                    ,@Datetime
                                    ,@Amount
                                    ,@ToCardID
                                    ,@Note";
                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DepositeID", DepositeID);
                        command.Parameters.AddWithValue("@FromCardID", FromCardID);
                        command.Parameters.AddWithValue("@Datetime", datetime);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@ToCardID", ToCardID);
                        if(string.IsNullOrEmpty(Note) || string.IsNullOrWhiteSpace(Note))
                        {
                            command.Parameters.AddWithValue("@Note", DBNull.Value);
                        }
                        else
                            command.Parameters.AddWithValue("@Note", Note);


                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            DepositeID = ID;
                        }

                    }

                }
            }
            catch(Exception ex) 
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            if (DepositeID == -1)
                return null;
            else
                return DepositeID;
        }
        public static bool UpdateDeposite(int DepositeID, int FromCardID, DateTime datetime, decimal amount, int ToCardID, string Note)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"EXECUTE SP_UpdateDeposite
                                     @DepositeID
                                    ,@FromCardID
                                    ,@Datetime
                                    ,@Amount
                                    ,@ToCardID
                                    ,@Note";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DepositeID", DepositeID);
                        command.Parameters.AddWithValue("@FromCardID", FromCardID);
                        command.Parameters.AddWithValue("@Datetime", datetime);
                        command.Parameters.AddWithValue("@Amount", amount);
                        command.Parameters.AddWithValue("@ToCardID", ToCardID);
                        if (string.IsNullOrEmpty(Note) || string.IsNullOrWhiteSpace(Note))
                        {
                            command.Parameters.AddWithValue("@Note", DBNull.Value);
                        }
                        else
                            command.Parameters.AddWithValue("@Note", Note);


                        RecordEffected = command.ExecuteNonQuery();

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return RecordEffected > 0;
        }

        public static bool FindLastDepositeBy(ref int DepositeID,  int FromCardID,ref DateTime datetime,ref decimal amount,ref int ToCardID,ref string Note)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_FindLastDepositeLogBy
                                    @FromCardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FromCardID", FromCardID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                                DepositeID = (int)reader["DepositeID"];
                                datetime = (DateTime)reader["Datetime"];
                                amount = (decimal)reader["Amount"];
                                ToCardID = (int)reader["ToCardID"];
                                if (reader["Note"] == DBNull.Value)
                                {
                                    Note = null;
                                }
                                else
                                {
                                    Note = (string)reader["Note"];
                                }
                                IsFound = true;

                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return IsFound;
        }
        public static bool FindLastWithdrawBy(ref int DepositeID,ref int FromCardID, ref DateTime datetime, ref decimal amount, int ToCardID, ref string Note)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_FindLastWithdrawLogBy
                                    @ToCardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ToCardID", ToCardID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FromCardID = (int)reader["FromCardID"];
                                DepositeID = (int)reader["DepositeID"];
                                datetime = (DateTime)reader["Datetime"];
                                amount = (decimal)reader["Amount"];
                                if (reader["Note"] == DBNull.Value)
                                {
                                    Note = null;
                                }
                                else
                                {
                                    Note = (string)reader["Note"];
                                }
                                IsFound = true;

                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return IsFound;
        }
        public static bool Find( int DepositeID,ref int FromCardID,ref DateTime datetime, ref decimal amount, ref int ToCardID, ref string Note)
        {
            bool IsFound = false;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_FindDepositeLogByID
                                    @DepositeID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@DepositeID", DepositeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                FromCardID = (int)reader["FromCardID"];
                                datetime = (DateTime)reader["Datetime"];
                                amount = (decimal)reader["Amount"];
                                ToCardID = (int)reader["ToCardID"];
                                if (reader["Note"] == DBNull.Value)
                                {
                                    Note = null;
                                }
                                else
                                {
                                    Note = (string)reader["Note"];
                                }
                                IsFound = true;
                            }
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return IsFound;
        }

        public static DataTable GetWithdrawLogBy(int CardID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetWithdrawLogBy
                                        @CardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CardID", CardID);

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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return dt;
        }
        public static DataTable GetDepositeLogBy(int CardID)
        {
            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetDepositeLogBy
                                        @CardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@CardID", CardID);

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
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return dt;
        }

        public static int GetNumberOfDepositeBy(int CardID)
        {
            int NumberOfDeposite = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfDepositeBy
                                        @FromCardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@FromCardID", CardID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Count))
                        {
                            NumberOfDeposite = Count;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return NumberOfDeposite;
        }
        public static int GetNumberOfWithdrawBy(int CardID)
        {
            int NumberOfWithdraw = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"exec SP_GetNumberOfWithdrawBy
                                    @ToCardID";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@ToCardID", CardID);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Count))
                        {
                            NumberOfWithdraw = Count;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }

            return NumberOfWithdraw;
        }

        public static bool CalculateBalanceAfterDeposite(int DepositeID)
        {
            int RecordEffected = 0;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_CalculateBalanceAfterDeposite
                                    @DepositeID";
                    using(SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@DepositeID", DepositeID);
                        RecordEffected = command.ExecuteNonQuery();
                    }
                }


            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error, "BankSystem", "System");
            }
            return RecordEffected > 0;
        }


    }
}
