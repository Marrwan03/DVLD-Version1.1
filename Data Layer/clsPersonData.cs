using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_DataAccess
{
    public class clsPersonData
    {
       
        public static bool GetPersonInfoByID(int PersonID, ref string FirstName, ref string SecondName,
          ref string ThirdName, ref string LastName, ref string NationalNo, ref DateTime DateOfBirth,
           ref short Gendor,ref string Address,  ref string Phone, ref string Email,
           ref int NationalityCountryID, ref string ImagePath, ref bool IsDeleted)
        {
            bool isFound = false;
            
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                        connection.Open();
                    string query = @"exec SP_GetPersonInfoByID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];

                                //ThirdName: allows null in database so we should handle null
                                if (reader["ThirdName"] != DBNull.Value)
                                {
                                    ThirdName = (string)reader["ThirdName"];
                                }
                                else
                                {
                                    ThirdName = "";
                                }

                                LastName = (string)reader["LastName"];
                                NationalNo = (string)reader["NationalNo"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];


                                //Email: allows null in database so we should handle null
                                if (reader["Email"] != DBNull.Value)
                                {
                                    Email = (string)reader["Email"];
                                }
                                else
                                {
                                    Email = null;
                                }

                                NationalityCountryID = (int)reader["NationalityCountryID"];

                                //ImagePath: allows null in database so we should handle null
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                                else
                                {
                                    ImagePath = "";
                                }

                                IsDeleted = (bool)reader["IsDeleted"];

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

        public static bool GetPersonInfoByNationalNo(string NationalNo, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName,   ref DateTime DateOfBirth,
         ref short Gendor,ref string Address, ref string Phone, ref string Email,
         ref int NationalityCountryID, ref string ImagePath, ref bool IsDeleted)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetPersonInfoByNationalNo
                                    @NationalNo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@NationalNo", NationalNo);

                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {

                            if (reader.Read())
                            {
                                // The record was found
                                isFound = true;

                                PersonID = (int)reader["PersonID"];
                                FirstName = (string)reader["FirstName"];
                                SecondName = (string)reader["SecondName"];

                                //ThirdName: allows null in database so we should handle null
                                if (reader["ThirdName"] != DBNull.Value)
                                {
                                    ThirdName = (string)reader["ThirdName"];
                                }
                                else
                                {
                                    ThirdName = "";
                                }

                                LastName = (string)reader["LastName"];
                                DateOfBirth = (DateTime)reader["DateOfBirth"];
                                Gendor = (byte)reader["Gendor"];
                                Address = (string)reader["Address"];
                                Phone = (string)reader["Phone"];

                                //Email: allows null in database so we should handle null
                                if (reader["Email"] != DBNull.Value)
                                {
                                    Email = (string)reader["Email"];
                                }
                                else
                                {
                                    Email = "";
                                }

                                NationalityCountryID = (int)reader["NationalityCountryID"];

                                //ImagePath: allows null in database so we should handle null
                                if (reader["ImagePath"] != DBNull.Value)
                                {
                                    ImagePath = (string)reader["ImagePath"];
                                }
                                else
                                {
                                    ImagePath = "";
                                }

                            }
                            else
                            {
                                // The record was not found
                                isFound = false;
                            }
                            IsDeleted = (bool)reader["IsDeleted"];
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

        public static bool GetPersonInfoByPhone( string Phone, ref int PersonID, ref string FirstName, ref string SecondName,
        ref string ThirdName, ref string LastName, ref DateTime DateOfBirth,
         ref short Gendor, ref string Address, ref string NationalNo, ref string Email,
         ref int NationalityCountryID, ref string ImagePath, ref bool IsDeleted)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetPersonInfoByPhone
                                    @Phone";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Phone", Phone);

                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();

                        if (reader.Read())
                        {
                            // The record was found
                            isFound = true;

                            PersonID = (int)reader["PersonID"];
                            FirstName = (string)reader["FirstName"];
                            SecondName = (string)reader["SecondName"];

                            //ThirdName: allows null in database so we should handle null
                            if (reader["ThirdName"] != DBNull.Value)
                            {
                                ThirdName = (string)reader["ThirdName"];
                            }
                            else
                            {
                                ThirdName = "";
                            }

                            LastName = (string)reader["LastName"];
                            DateOfBirth = (DateTime)reader["DateOfBirth"];
                            Gendor = (byte)reader["Gendor"];
                            Address = (string)reader["Address"];
                            NationalNo = (string)reader["NationalNo"];

                            //Email: allows null in database so we should handle null
                            if (reader["Email"] != DBNull.Value)
                            {
                                Email = (string)reader["Email"];
                            }
                            else
                            {
                                Email = "";
                            }

                            NationalityCountryID = (int)reader["NationalityCountryID"];

                            //ImagePath: allows null in database so we should handle null
                            if (reader["ImagePath"] != DBNull.Value)
                            {
                                ImagePath = (string)reader["ImagePath"];
                            }
                            else
                            {
                                ImagePath = "";
                            }

                        }
                        else
                        {
                            // The record was not found
                            isFound = false;
                        }

                        IsDeleted = (bool)reader["IsDeleted"];


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

        public static int? AddNewPerson( string FirstName,  string SecondName,
           string ThirdName,  string LastName,  string NationalNo,  DateTime DateOfBirth,
           short Gendor, string Address,  string Phone,  string Email,
            int NationalityCountryID,  string ImagePath, bool IsDeleted)
        {
            //this function will return the new person id if succeeded and -1 if not.
            int? PersonID =0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();
                    string query = @"
                                    exec SP_AddNewPerson
                                    @PersonID output,
                                    @NationalNo,
                                    @FirstName,@SecondName,
                                    @ThirdName,@LastName,
                                    @DateOfBirth,@Gendor,
                                    @Address,@Phone,@Email,
                                    @NationalityCountryID,@ImagePath,
                                    @IsDeleted
                                    ";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@SecondName", SecondName);

                        if (ThirdName != "" && ThirdName != null)
                            command.Parameters.AddWithValue("@ThirdName", ThirdName);
                        else
                            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);

                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Phone", Phone);

                        if (Email != "" && Email != null)
                            command.Parameters.AddWithValue("@Email", Email);
                        else
                            command.Parameters.AddWithValue("@Email", System.DBNull.Value);

                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                        if (ImagePath != "" && ImagePath != null)
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                        command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

                        object result = command.ExecuteScalar();
                        if(result != null && int.TryParse(result.ToString(), out int pID))
                        {
                            PersonID = pID;
                        }
                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);

            }
            if (PersonID == 0)
            {
                PersonID = null;
            }



            return PersonID;
        }

        public static bool UpdatePerson(int? PersonID,  string FirstName, string SecondName,
           string ThirdName, string LastName, string NationalNo, DateTime DateOfBirth,
           short Gendor, string Address, string Phone, string Email,
            int NationalityCountryID, string ImagePath, bool IsDeleted)
        {

            int rowsAffected = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_UpdatePerson
                            @PersonID,
                            @NationalNo,
                            @FirstName,@SecondName,
                            @ThirdName,@LastName,
                            @DateOfBirth,@Gendor,
                            @Address,@Phone,@Email,
                            @NationalityCountryID,@ImagePath,
                            @IsDeleted";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);
                        command.Parameters.AddWithValue("@FirstName", FirstName);
                        command.Parameters.AddWithValue("@SecondName", SecondName);

                        if (ThirdName != "" && ThirdName != null)
                            command.Parameters.AddWithValue("@ThirdName", ThirdName);
                        else
                            command.Parameters.AddWithValue("@ThirdName", System.DBNull.Value);


                        command.Parameters.AddWithValue("@LastName", LastName);
                        command.Parameters.AddWithValue("@NationalNo", NationalNo);
                        command.Parameters.AddWithValue("@DateOfBirth", DateOfBirth);
                        command.Parameters.AddWithValue("@Gendor", Gendor);
                        command.Parameters.AddWithValue("@Address", Address);
                        command.Parameters.AddWithValue("@Phone", Phone);

                        if (Email != "" && Email != null)
                            command.Parameters.AddWithValue("@Email", Email);
                        else
                            command.Parameters.AddWithValue("@Email", System.DBNull.Value);

                        command.Parameters.AddWithValue("@NationalityCountryID", NationalityCountryID);

                        if (ImagePath != "" && ImagePath != null)
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                        command.Parameters.AddWithValue("@IsDeleted", IsDeleted);

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

        public async static Task< DataTable> GetAllPeople()
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {


                    string query =
                      @"exec SP_GetAllPeople";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        connection.Open();

                        using (SqlDataReader reader = await command.ExecuteReaderAsync())
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

        public static DataTable GetPeopleBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
            {
                    connection.Open();

                    string query =
                  @"exec SP_GetPeopleBy
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
        public static DataTable GetArchiveOfAllPeopleBy(int PageNumber, int RowPerPage)
        {

            DataTable dt = new DataTable();
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query =
                  @"exec SP_ArchiveOfAllPeopleBy
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

        public static int GetNumberOfRowsForPeople()
        {

            int Rows=0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query =
                  @"exec SP_GetNumberOfRowsForPeople";

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
        public static int GetNumberOfRowsForPeopleArchive()
        {

            int Rows = 0;
            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {
                    connection.Open();

                    string query =
                  @"exec SP_GetNumberOfRowsForPeopleArchive";

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

        public static bool DeletePerson(int PersonID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_DeletePersonByID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

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
        public static bool ReturnDeletedPerson(int PersonID)
        {

            int rowsAffected = 0;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_ReturnDeletedPerson
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

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

        public static bool IsPersonExist(int PersonID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsPersonExistByID
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        object Result = command.ExecuteScalar();
                        if (Result != null && bool.TryParse(Result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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

        public static bool IsPersonHasEmail(int PersonID)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsPersonHasEmail
                                    @PersonID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@PersonID", PersonID);

                        connection.Open();
                        object Result = command.ExecuteScalar();
                        if (Result != null && bool.TryParse(Result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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
        public static bool IsPersonExist(string NationalNo)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsPersonExistByNationalNo
                                    @NationalNo";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@NationalNo", NationalNo);

                        connection.Open();
                        object Result = command.ExecuteScalar();
                        if (Result != null && bool.TryParse(Result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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

        public static bool IsPhoneExists(string Phone)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsPersonExistByPhone
                                    @Phone";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Phone", Phone);

                        connection.Open();
                        object Result = command.ExecuteScalar();
                        if (Result != null && bool.TryParse(Result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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

        public static bool IsEmailExists(string Email)
        {
            bool isFound = false;

            try
            {
                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsPersonExistByEmail
                                    @Email";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@Email", Email);

                        connection.Open();
                        object Result = command.ExecuteScalar();
                        if (Result != null && bool.TryParse(Result.ToString(), out bool IsExists))
                        {
                            isFound = IsExists;
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
       
    }
}
