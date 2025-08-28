using DVLD_Buisness;
using Guna.UI2.WinForms;
using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.Tests.Written_Test.Controls
{
    public partial class ctrlWrittenTest : UserControl
    {
        int _NumberOfQuestion=1;
        public int NumberOfQuestion 
        {
            
            get
            {
                return _NumberOfQuestion;
            }
            set
            {
                _NumberOfQuestion = value;
                if(_NumberOfQuestion <= 5)
                {
                    lblNumberOfQuestion.Text = $"[{_NumberOfQuestion} - 5]";
                }
                
            }
        }

        public string QuestionText
        {
            get
            {
                return lblQuestionText.Text;
            }
            set
            {
                lblQuestionText.Text = value;
            }
        }

        public Guna2CirclePictureBox SetQuestionPicture 
        {
            get
            {
                
                return picQuestionPicture;
            }
            set
            {
                picQuestionPicture = value;
            }
        }

        public string OptionOne
        {
            get
            {
                return rbOption2.Text;
            }
            set
            {
                rbOption2.Text = value;
            }
        }

        public string OptionTwo 
        {
            get
            {
                return rbOption1.Text;
            }
            set { rbOption1.Text = value; }
        
        }
        public string OptionThree
        {
            get
            {
                return rbOption3.Text;
            }
            set { rbOption3.Text = value; }

        }

        RadioButton _UserOption=null;
        DataRow _RightOption;

        

        public ctrlWrittenTest()
        {
            InitializeComponent();

        }
        int _QuestionID;
        public int QuestionID { get { return _QuestionID; } }
        clsQuestion _CurrentQuestion;
       public clsQuestion CurrentQuestion { get { return _CurrentQuestion; } }

        DataRow _GetRightOption()
        {
            foreach (DataRow row in CurrentQuestion.Options.Rows)
            {
                if ((bool)row[3])
                    return row;
            }
            return null;
        }

        RadioButton _GetRightrbOption()
        {
            if (rbOption2.Text == _RightOption[2].ToString())
                return rbOption2;
            else if (rbOption1.Text == _RightOption[2].ToString())
                return rbOption1;
            return rbOption3;
        }

        bool _GetResultOfTest() => Rate.Value >= 3;
        bool _GetResultOfQuestion() => _RightOption[2].ToString() == _UserOption.Text;

        public class clsQuestionInfo : EventArgs
        {
            public string UserAnswer { get; }
            public string QuestionAnswer { get; }

            public clsQuestionInfo(string userAnswer, string questionAnswer)
            {
                UserAnswer = userAnswer;
                QuestionAnswer = questionAnswer;
            }
        }


        public event EventHandler<clsQuestionInfo> OnCheckQuestion;
        public event Action OnNextQuestion;
        public event Action<bool> OnFinishQuestion;

        void _LoadQuestion()
        {
            rbOption1.BackColor = this.BackColor;
            rbOption2.BackColor = this.BackColor;
            rbOption3.BackColor = this.BackColor;

            if (File.Exists(_CurrentQuestion.ImagePath))
            {
                picQuestionPicture.Visible = true;
                picQuestionPicture.ImageLocation = _CurrentQuestion.ImagePath;
            }
            else
            {
                picQuestionPicture.Visible = false;
            }

            QuestionText = CurrentQuestion.Text;
            _RightOption = _GetRightOption();
            DataTable dtOptions = CurrentQuestion.Options;
            OptionOne = dtOptions.Rows[0][2].ToString();
            OptionTwo = dtOptions.Rows[1][2].ToString();
            OptionThree = dtOptions.Rows[2][2].ToString();
        }

       public void FinishTheExam()
        {
            btnNext.Text = "أنتهت الاسئلة";
            btnCheck.Enabled = false;
            btnNext.Enabled = false;
            OnFinishQuestion?.Invoke(_GetResultOfTest());
        }

        public void ctrlQuestionWithOptions_Load(int questionID, int numberOfQuestion)//NumberOfQuestion declare int with validation if int != Data.count
        {
            _QuestionID = questionID ;
            _CurrentQuestion = clsQuestion.Find(_QuestionID);
            _NumberOfQuestion = numberOfQuestion;
            if (NumberOfQuestion > 5)
            {
                FinishTheExam();
                return;
            }

            if (_CurrentQuestion != null )
            {
                btnNext.Enabled = false;
                _LoadQuestion();
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            btnCheck.Enabled = true;
            btnNext.Enabled = false;
            //_UserOption = null;
            OnNextQuestion?.Invoke();
        }
        
        private void rbOption1_CheckedChanged(object sender, EventArgs e)
        {
            _UserOption = (RadioButton)sender;
        }

        private void btnCheck_Click(object sender, EventArgs e)
        {
            if(_UserOption == null)
            {
                MessageBox.Show("يحب عليك اختيار اجابة لهذا السؤال", "خطأ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            btnCheck.Enabled = false;
            btnNext.Enabled = true;
            bool result = _GetResultOfQuestion();
            RadioButton rbRightOption = _GetRightrbOption();
            rbRightOption.BackColor = Color.Green;
            if(!result)
            {
                Rate.Value -= 1;
                _UserOption.BackColor = Color.Red;
            }
            OnCheckQuestion?.Invoke(this, new clsQuestionInfo(_UserOption.Text, rbRightOption.Text));
        }

        private void rbOption1_MouseEnter(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            rb.BackColor = Color.LightSteelBlue;
        }

        private void rbOption1_MouseLeave(object sender, EventArgs e)
        {
            RadioButton rb = (RadioButton)sender;
            rb.BackColor = this.BackColor;
        }
    }
}
