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
    public class clsQuestionsData
    {
        public static int? AddNewQuestion(string Text, string ImagePath)
        {
            int QuestionID = -1;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_AddNewQuestion
                                    @QuestionID OUTPUT,
                                    @Text,
                                    @ImagePath";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionID", QuestionID);
                        command.Parameters.AddWithValue("@Text", Text);

                        if (!string.IsNullOrEmpty(ImagePath))
                            command.Parameters.AddWithValue("@ImagePath", ImagePath);
                        else
                            command.Parameters.AddWithValue("@ImagePath", System.DBNull.Value);

                        connection.Open();
                        object result = command.ExecuteScalar();
                        if (result != null && int.TryParse(result.ToString(), out int ID))
                        {
                            QuestionID = ID;
                        }

                    }
                }
            }

            catch (Exception ex)
            {
                //Console.WriteLine("Error: " + ex.Message);
                clsEventLog.WriteEventLog(ex.Message, EventLogEntryType.Error);
                return null;
            }
            if (QuestionID == -1)
                return null;
            return QuestionID;
        }

        public static bool Find(int QuestionID,ref string Text,ref string ImagePath)
        {
            bool Found = false;
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetQuestionInfoBy
                                    @QuestionID";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@QuestionID", QuestionID);
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                Found = true;
                                Text = (string)reader["Text"];
                                if (reader["ImagePath"] == DBNull.Value)
                                {
                                    ImagePath = null;
                                }
                                else
                                    ImagePath = (string)reader["ImagePath"];
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
            return Found;
        }
        public static DataTable GetFiveRndQuestions()
        {
            DataTable dtQuestions = new DataTable();
            try
            {

                using (SqlConnection connection = new SqlConnection(clsDataAccessSettings.ConnectionString))
                {

                    string query = @"exec SP_GetFiveRndQuestions";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        connection.Open();
                        using (SqlDataReader reader = command.ExecuteReader())
                        {
                            if (reader.HasRows)
                            {
                                dtQuestions.Load(reader);
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
            return dtQuestions;
        }

    }
}
