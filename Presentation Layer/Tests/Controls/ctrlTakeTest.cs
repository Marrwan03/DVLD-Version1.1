using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Controls
{
    public partial class ctrlTakeTest : UserControl
    {
        DataTable _dtQuestionsOfWrittenTest;
        public class TestInfo : EventArgs
        {
            public int TestAppointmentID { get; }
            public clsTestType.enTestType TestTypeID { get; }
            public bool TestResult { get; }

            public TestInfo(int testAppointmentID, clsTestType.enTestType testTypeID, bool testResult)
            {
                TestAppointmentID = testAppointmentID;
                TestTypeID = testTypeID;
                TestResult = testResult;
            }
        }

       public event EventHandler<TestInfo> OnFinishTest;

        void _VisibleOfControls(bool VisibleVisionTest, bool VisibleWrittenTest, bool VisibleStreetTest)
        {
            if (VisibleWrittenTest)
            {
                this.Size = new Size(826, 471);
                ctrlSecheduledTest1.Location = new Point(302, 3);
            }
            else if(VisibleStreetTest)
            {
                this.Size = new Size(1389, 630);
            ctrlSecheduledTest1.Location = new Point(865, 4);
            }
            else
            {
                this.Size = new Size(1389, 469);
                ctrlSecheduledTest1.Location = new Point(865, 4);
            }

            ctrlVision_Test1.Visible = VisibleVisionTest;
            ctrlWrittenTest1.Visible = VisibleWrittenTest;          
            ctrlStreetTest1.Visible = VisibleStreetTest;

        }

        public clsTestType.enTestType TestTypeID 
        {
            get 
            {
                return ctrlSecheduledTest1.TestTypeID;
            }
            set
            {
                ctrlSecheduledTest1.TestTypeID = value;

                switch(ctrlSecheduledTest1.TestTypeID)
                {
                    case clsTestType.enTestType.VisionTest:
                        {
                            _VisibleOfControls(true, false, false);
                            break;
                        }
                        case clsTestType.enTestType.WrittenTest:
                        {
                            _VisibleOfControls(false, true, false);
                            break;
                        }
                        case clsTestType.enTestType.StreetTest:
                        {
                            _VisibleOfControls(false, false, true);
                            break;
                        }
                }

            }
        }

        public ctrlTakeTest()
        {
            InitializeComponent();
            //TestTypeID = clsTestType.enTestType.WrittenTest;
        }
        int _TestAppointmentID;
        public int TestAppointmentID { get { return _TestAppointmentID; } }
        public void ctrlTakeTest_Load(int testAppointmentID, clsTestType.enTestType testTypeID)
        {
            _TestAppointmentID = testAppointmentID;
            TestTypeID = testTypeID;
            ctrlSecheduledTest1.TestTypeID = testTypeID;
            ctrlSecheduledTest1.LoadInfo(_TestAppointmentID);

            switch(ctrlSecheduledTest1.TestTypeID)
            {
               case clsTestType.enTestType.VisionTest:
               {
                   ctrlVision_Test1.ctrlVisionTest_Load();
                   break;
               }
               case clsTestType.enTestType.WrittenTest:
               {
                   _dtQuestionsOfWrittenTest = clsQuestion.GetFiveRndQuestions();
                   _SetNewQuestionForWrittenTest(1);
                   break;
               }
               case clsTestType.enTestType.StreetTest:
               {
                   ctrlStreetTest1.ctrlStreet_Load();
                   break;
               }
            }

        }

        void _SetNewQuestionForWrittenTest(int NumberOfQuestion)
        {
            if (NumberOfQuestion <= 5)
                ctrlWrittenTest1.ctrlQuestionWithOptions_Load((int)_dtQuestionsOfWrittenTest.Rows[NumberOfQuestion - 1][0], NumberOfQuestion);
            else
                ctrlWrittenTest1.FinishTheExam();
        }

        private void ctrlWrittenTest1_OnFinishQuestion(bool obj)
        {
            OnFinishTest?.Invoke(this, new TestInfo(TestAppointmentID, TestTypeID, obj));
        }

        private void ctrlVision_Test1_OnFinishExam(object sender, Vision_Test.Controls.ctrlVision_Test.GameInfo e)
        {

            OnFinishTest?.Invoke(this, new TestInfo(TestAppointmentID, TestTypeID, e._Round.IsPass()));
        }

        private void ctrlStreetTest1_OnFinishExam(object sender, Street_Test.Controls.ctrlStreetTest.clsExamInfo e)
        {
           if( MessageBox.Show(
               $"NumberOfWarning: {e.NumberOfWarning}," +
               $" \nMinutesOfExam: {e.MinutesOfExam}," +
               $" \nMinutesYouTakeIt: {e.MinutesYouTakeIt}," +
               $"\nResult: {e.IsPass()}", "Exam Info", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ctrlStreetTest1.EnableGbExam = false;
            }
            OnFinishTest?.Invoke(this, new TestInfo(TestAppointmentID, TestTypeID, e.IsPass()));
        }

        void _RaiseRoundInfo(string BalloonTipText)
        {
            NotifecationForTest.Icon = SystemIcons.Application;
            NotifecationForTest.BalloonTipIcon = ToolTipIcon.Info;
            NotifecationForTest.BalloonTipTitle = "Notification For Round";
            NotifecationForTest.BalloonTipText = BalloonTipText;
            NotifecationForTest.ShowBalloonTip(1000);
        }
        private void ctrlVision_Test1_ResultOfEachRound(object sender, Vision_Test.Controls.ctrlVision_Test.clsQuestionInfo e)
        {
            string Text = $"Your Answer: {e.YourAnswer}," +
                $"\nQuestion Answer: {e.QuestionAnswer}," +
                $"\nResult: {(e.Result? "True" : "False")}";

            _RaiseRoundInfo(Text);
        }

        private void ctrlWrittenTest1_OnCheckQuestion(object sender, Written_Test.Controls.ctrlWrittenTest.clsQuestionInfo e)
        {
            string Text = $"[{e.UserAnswer}] ايجابتك هي" +
                $"\n[{e.QuestionAnswer}] الاجابة الصحيحة هي" +
                $"\n[{(e.QuestionAnswer == e.UserAnswer ? "صح" : "غلط")}] النتيجة";
            _RaiseRoundInfo(Text);
        }

        private void ctrlWrittenTest1_OnNextQuestion()
        {
            ctrlWrittenTest1.NumberOfQuestion++;
            _SetNewQuestionForWrittenTest(ctrlWrittenTest1.NumberOfQuestion);
        }

        private void ctrlStreetTest1_OnWarning(int obj)
        {
            MessageBox.Show($"you pass traffic light\nRemaining of warning[{obj}]");
        }

        private void ctrlTakeTest_Load(object sender, EventArgs e)
        {

        }
    }
}
