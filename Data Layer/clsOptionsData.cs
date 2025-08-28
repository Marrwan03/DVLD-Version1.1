using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD_DataAccess
{
    public class clsOptionsData
    {
        public static int? AddNewOptionFor(int QuestionID, string OptionText, bool IsCorrect)
        {
            int OptionID = -1;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewOptionFor
                                    @OpitionID output,
                                    @QuestionID,
                                    @OptionText,
                                    @IsCorrect";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.AddWithValue("@OpitionID", OptionID);
                        command.Parameters.AddWithValue("@QuestionID", QuestionID);
                        command.Parameters.AddWithValue("@OptionText", OptionText);
                        command.Parameters.AddWithValue("@IsCorrect", IsCorrect);
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            OptionID = ID;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            if (OptionID == -1)
                return null;
            return OptionID;
        }

        public static bool IsOptionCorrectBy(int QuestionID, string OpitionText)
        {
            bool IsCorrect = false;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_IsOptionCorrectBy
                                    @QuestionID,
                                    @OptionText";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionID", QuestionID);
                        command.Parameters.AddWithValue("@OptionText", OpitionText);
                        connection.Open();

                        object result = command.ExecuteScalar();
                        if (result != null && bool.TryParse(result.ToString(), out bool Correct))
                        {
                            IsCorrect = Correct;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return IsCorrect;
        }

        public static DataTable GetOptionsFor(int  QuestionID)
        {
            DataTable result = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"SP_GetOptionsFor
                                    @QuestionID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionID", QuestionID);
                        connection.Open();

                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            if(reader.HasRows)
                            {
                                result.Load(reader);
                            }
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
            }
            return result;
        }

    }
}
