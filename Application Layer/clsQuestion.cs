using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsQuestion
    {
       public enum enMode { Add, Update}

        public enMode Mode;

        private int? _QuestionID;
        public int? QuestionID { get { return _QuestionID; } }

        public string Text { set;get; }

        public string ImagePath { set; get; }

        DataTable _Options;
        public DataTable Options { get { return _Options; } }

        public static clsQuestion Find(int questionID)
        {
            string text = "", ImagePath = "";

            if(clsQuestionsData.Find(questionID, ref text, ref ImagePath))
            {
                return new clsQuestion(questionID, text, ImagePath);
            }
            return null;

        }

        public clsQuestion()
        {
            Text = string.Empty;
            ImagePath = string.Empty;
            Mode = enMode.Add;
        }

        public clsQuestion(int? QuestionID,string text, string imagePath)
        {
            _QuestionID = QuestionID;
            Text = text;
            ImagePath = imagePath;
            Mode = enMode.Update;
            _Options = GetOptionsForThisQuestion();
        }

        bool _AddNewQuestion()
        {
            this._QuestionID = clsQuestionsData.AddNewQuestion(this.Text, this.ImagePath);
            return QuestionID.HasValue;
        }

        public static DataTable GetOptionsFor(int QuestionID) => clsOptionsData.GetOptionsFor(QuestionID);

        public DataTable GetOptionsForThisQuestion() => GetOptionsFor(this.QuestionID.Value);

        public bool Save()
        {
            switch(Mode)
            {
                case enMode.Add:
                    {
                        if(_AddNewQuestion())
                        {
                            Mode = enMode.Update;
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }

        public static DataTable GetFiveRndQuestions() => clsQuestionsData.GetFiveRndQuestions();


    }
}
