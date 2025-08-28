using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsOption
    {
        int? _OptionID;
        public int? OptionID { get { return _OptionID; }  }

        public int QuestionID { set; get; }
        clsQuestion _QuestionInfo;
        public clsQuestion QuestionInfo { get { return _QuestionInfo; } }
       
        public string  OptionText { set; get; }
        public bool IsCorrect { set; get; }

        public clsOption() 
        {
            _OptionID = null;
            QuestionID = 0;
            OptionText = null;
            IsCorrect = false;
        }

        public clsOption(int optionID,int questionID, string optionText, bool isCorrect)
        {
            _OptionID = optionID;
            QuestionID = questionID;
            OptionText = optionText;
            IsCorrect = isCorrect;

            this._QuestionInfo = clsQuestion.Find(questionID);
        }

        bool _AddNewOption()
        {
            this._OptionID = clsOptionsData.AddNewOptionFor(this.QuestionID, this.OptionText, this.IsCorrect);
            return OptionID.HasValue;
        }


        public bool Save()
        {
            return _AddNewOption();
        }    

        public static bool IsOptionCorrectBy(int QuestionID, string OpitionText) => clsOptionsData.IsOptionCorrectBy(QuestionID, OpitionText);


    }
}
