using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;

namespace DVLD_DataAccess
{
    public class clsEmployeesData
    {
        public static DataTable GetEmployeeBy(int PageNumber, int RowPerPage)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetEmployeeByPageNumber
                                    @PageNumber,
                                    @RowPerPage";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }

                    }
                }

            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return dt;
        }
        public static DataTable GetArchiveOfAllEmployeesBy(int PageNumber, int RowPerPage)
        {
            DataTable dt = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_ArchiveOfAllEmployeesBy
                                    @PageNumber,
                                    @RowPerPage";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@PageNumber", PageNumber);
                        command.Parameters.AddWithValue("@RowPerPage", RowPerPage);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            dt.Load(reader);
                        }

                    }
                }

            }
            catch (Exception e)
            {
                clsEventLog.WriteEventLog(e.Message, EventLogEntryType.Error);
            }
            return dt;
        }
        public static bool FindByEmployeeID( int EmployeeID,ref int UserID, ref DateTime HireDate,
           ref DateTime? ExitDate, ref long Permisions, ref decimal Monthltysalary,
           ref double BounsPerc, ref byte Status,
           ref int PersonID,ref string UserName,ref string Password, ref string Salt,
           ref byte BloodType, ref int CreatedUserByEmployeeID)
        {
            bool IsFound = false;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindEmployeeByEmpID
                                          @EmployeeID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.Read())
                            {
                           
                            UserID = (int)reader["UserID"];
                            HireDate = (DateTime)reader["HireDate"];
                            if (reader["ExitDate"] == DBNull.Value)
                            {
                                ExitDate = null;
                            }
                            else
                            {
                                ExitDate = (DateTime)reader["ExitDate"];
                            }

                            Permisions = (long)reader["Permisions"];
                            Monthltysalary = (decimal)reader["Monthltysalary"];
                            BounsPerc = (double)reader["BounsPerc"];

                            PersonID = (int)reader["PersonID"];
                            UserName = (string)reader["UserName"];
                            Password = (string)reader["Password"];
                            Salt = (string)reader["Salt"];
                            Status = (byte)reader["Status"];
                            BloodType = (byte)reader["BloodType"];
                            CreatedUserByEmployeeID = (int)reader["CreatedUserByEmployeeID"];

                            IsFound = true;
                            }
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error);

            }
            return IsFound;
        }
        public static bool FindByUserID(ref int EmployeeID, int UserID, ref DateTime HireDate, 
            ref DateTime? ExitDate, ref long Permisions, ref decimal Monthltysalary, ref double BounsPerc, ref byte Status,
          ref int PersonID, ref string UserName, ref string Password, ref string Salt,
           ref byte BloodType, ref int CreatedUserByEmployeeID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_FindEmployeeByUserID
                                    @UserID";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {


                                EmployeeID = (int)reader["EmployeeID"];
                                HireDate = (DateTime)reader["HireDate"];
                                if (reader["ExitDate"] == DBNull.Value)
                                {
                                    ExitDate = null;
                                }
                                else
                                {
                                    ExitDate = (DateTime)reader["ExitDate"];
                                }
                                Permisions = (long)reader["Permisions"];
                                Monthltysalary = (decimal)reader["Monthltysalary"];
                                BounsPerc = (double)reader["BounsPerc"];

                                PersonID = (int)reader["PersonID"];
                                UserName = (string)reader["UserName"];
                                Password = (string)reader["Password"];
                                Salt = (string)reader["Salt"];
                                Status = (byte)reader["Status"];
                                BloodType = (byte)reader["BloodType"];
                                CreatedUserByEmployeeID = (int)reader["CreatedUserByEmployeeID"];

                                IsFound = true;
                            }
                        }

                    }
                }

            }
            catch (Exception ex) 
            {
                clsEventLog.WriteEventLog(ex.Message, System.Diagnostics.EventLogEntryType.Error);

            }
            return IsFound;
        }

        public static bool ChangePermision(int EmployeeID, long Permisions)
        {
            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    string query = @"exec SP_ChangePermision
                                        @EmployeeID,
                                        @Permisions";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Permisions", Permisions);
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

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

        public static int? AddNewEmployee(int UserID,  DateTime HireDate,
             DateTime? ExitDate, long Permisions,  decimal Monthltysalary,  double BounsPerc,  byte Status)
        {
            int? EmployeeID=0 ;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_AddNewEmployee
                                    @EmployeeID output,
                                    @UserID,@HireDate, 
                                    @ExitDate,@Permisions,@Monthltysalary,
                                    @BounsPerc,@Status";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@HireDate", HireDate);
                        if(ExitDate == null)
                        {
                            command.Parameters.AddWithValue("@ExitDate", DBNull.Value);
                        }
                        else
                        {
                        command.Parameters.AddWithValue("@ExitDate", ExitDate);
                        }
                        command.Parameters.AddWithValue("@Permisions", Permisions);
                        command.Parameters.AddWithValue("@Monthltysalary", Monthltysalary);
                        command.Parameters.AddWithValue("@BounsPerc", BounsPerc);
                        command.Parameters.AddWithValue("@Status", Status);

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int EmpID))
                        {
                            EmployeeID = EmpID;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            if (EmployeeID == 0)
                EmployeeID = null;

            return EmployeeID;
        }

        public static bool UpdateEmployeeByID(int EmployeeID,int UserID, DateTime HireDate,
             DateTime? ExitDate,long Permisions,  decimal Monthltysalary, double BounsPerc, byte Status)
        {
            int RecordEffected = 0;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    //Permisions
                    string Query = @"exec SP_UpdateEmployeeByEmpID
                                          @EmployeeID,@UserID,
                                          @HireDate,@ExitDate, @Permisions,
                                          @Monthltysalary,@BounsPerc,@Status";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@HireDate", HireDate);
                        if(ExitDate.HasValue)
                        {
                            command.Parameters.AddWithValue("@ExitDate", ExitDate);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@ExitDate", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@Permisions", Permisions);
                        command.Parameters.AddWithValue("@Monthltysalary", Monthltysalary);
                        command.Parameters.AddWithValue("@BounsPerc", BounsPerc);
                        command.Parameters.AddWithValue("@Status", Status);

                        RecordEffected = command.ExecuteNonQuery();
                    }
                }

            }
            catch (Exception ex) 
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return RecordEffected > 0;
        }

        public static bool IsUserHasEmployeeAcc(int UserID)
        {
            bool IsExists = false;

            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_IsUserHasEmployeeAcc
                                    @UserID,
                                    @IsExists output";
                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        command.Parameters.AddWithValue("@IsExists", IsExists);

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool isexists))
                        {
                            IsExists = isexists;
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }

            return IsExists;

        }

        public static int GetNumberOfRowsForEmployee()
        {
            int NumberOfRows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetNumberOfRowsForEmployee";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Rows))
                        {
                            NumberOfRows = Rows;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }
        public static int GetNumberOfRowsForEmployeesArchive()
        {
            int NumberOfRows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetNumberOfRowsForEmployeesArchive";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int Rows))
                        {
                            NumberOfRows = Rows;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return NumberOfRows;
        }

        public static int? GetEmployeeIDByUserID(int UserID)
        {
            int? EmpID = null;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetEmployeeIDByUserID
                                    @UserID";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@UserID", UserID);
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int id))
                        {
                            EmpID = id;
                        }

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return EmpID;
        }

        public static decimal GetMaximumOfMonthltysalary()
        {
            decimal MaxOfMonthltysalary = 0;
            try
            {

                using(SqlConnection  connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_GetMaximumOfMonthltysalary";

                    using(SqlCommand command = new SqlCommand(Query,connection))
                    {

                        object result = command.ExecuteScalar();
                        if(result != null&& decimal.TryParse(result.ToString(), out decimal Max))
                        {
                            MaxOfMonthltysalary = Max;
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return MaxOfMonthltysalary;
        }

        public static bool DeleteEmployeeBy(int EmployeeID)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_DeleteEmployeeByID
                                       @EmployeeID";

                    using(SqlCommand command=new SqlCommand(Query,connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                        RecordEffected = command.ExecuteNonQuery();

                    }

                }
            }
            catch (Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return RecordEffected > 0;
        }

        public static bool IsEmployeeExists(int EmployeeID)
        {
            bool IsFound = false;
            try
            {
                using(SqlConnection connection = new SqlConnection( clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_IsEmployeeExists
                                    @EmployeeID";
                    using(SqlCommand cmd=new SqlCommand(Query,connection))
                    {
                        cmd.Parameters.AddWithValue("@EmployeeID", EmployeeID);

                        object result = cmd.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool IsExists))
                        {
                            IsFound = IsExists;
                        }

                    }
                }
            }
            catch(Exception ex)
            {
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return IsFound;
        }

        public static bool UpdateStatus(int EmployeeID, byte NewStatus)
        {
            int RecordEffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string Query = @"exec SP_UpdateStatusOfEmployee
                                    @EmployeeID,
                                    @NewStatus";

                    using (SqlCommand command = new SqlCommand(Query, connection))
                    {
                        command.Parameters.AddWithValue("@EmployeeID", EmployeeID);
                        command.Parameters.AddWithValue("@NewStatus", NewStatus);

                        RecordEffected = command.ExecuteNonQuery();

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
