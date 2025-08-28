using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Vision_Test.Controls
{
    public partial class ctrlVision_Test : UserControl
    {
        public ctrlVision_Test()
        {
            InitializeComponent();
        }


        public event EventHandler<GameInfo> OnFinishExam;

        clsRound _CurrentRound = new clsRound();

        public class GameInfo : EventArgs
        {
            public clsRound _Round { get; }

            public GameInfo(clsRound round)
            {
                _Round = round;
            }
        }


        public class clsRound
        {
            public byte MaxTimeForEachRound { get; set; }
            public byte Timer {  get; set; }
            public byte MaxNumberOfRound { get; set; }
            public byte NumberOfRound { get; set; } = 1;
            public byte NumberOfRightAnswer { get; set; }
            public byte NumberOfWrongAnswer { get; set; }
            public bool IsPass() => NumberOfRightAnswer > NumberOfWrongAnswer;

            public override string ToString()
            {
                return $"{MaxNumberOfRound} Round, Timer Each Round[{MaxTimeForEachRound}] Second\n{NumberOfRightAnswer} Right Answer, {NumberOfWrongAnswer} Wrong Answer\nFinal Result: {(IsPass() ? "Pass" : "Fail")}";
            }
        }

        public void ctrlVisionTest_Load(byte MaxNumberOfRound = 5, byte MaxTimeForEachRound = 30)
        {
            _CurrentRound.MaxNumberOfRound = MaxNumberOfRound;
            _CurrentRound.MaxTimeForEachRound = MaxTimeForEachRound;
            _CurrentRound.Timer = MaxTimeForEachRound;
            StatusOfChoices = false;
          //  Status(false);
            _RefreshData();

        }


         void _RefreshData()
        {
            SetRndCharForEachRows();
            SetRndColorForEachRows();    
            lblRound.Text = $"{_CurrentRound.NumberOfRound} / 5";
            lblTimer.Text = $"{_CurrentRound.Timer} S";
        }
        enum enColor { Red=1, Blue, Green, Yellow }
        char _GetRndCapitalChar(int From=65, int To= 90)
        {            
            Random rnd = new Random();
            return (char)rnd.Next(From, To);
        }

       public void SetRndCharForEachRows()
        {
            //Row1
            ctrlCircle1_1.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle1_2.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle1_3.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle1_4.TextOfCharctar = _GetRndCapitalChar().ToString();

            //Row2
            ctrlCircle2_1.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle2_2.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle3_3.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle4_4.TextOfCharctar = _GetRndCapitalChar().ToString();

            //Row3
            ctrlCircle3_1.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle3_2.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle3_3.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle3_4.TextOfCharctar = _GetRndCapitalChar().ToString();

            //Row4
            ctrlCircle4_1.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle4_2.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle4_3.TextOfCharctar = _GetRndCapitalChar().ToString();
            ctrlCircle4_4.TextOfCharctar = _GetRndCapitalChar().ToString();
        }

        enColor _GetRndEnumColor()
        {
            Random rnd = new Random();
            return (enColor)rnd.Next(1, 4);
        }

        Color _GetRndColor()
        {
            enColor C = _GetRndEnumColor();

            switch (C)
            {
                case enColor.Red:
                        return Color.Red;

                case enColor.Blue:
                    return Color.Blue;

                case enColor.Green:
                    return Color.Green;

                default:
                    return Color.Yellow;
            }


        }

       public void SetRndColorForEachRows()
        {
            Color color = _GetRndColor();
            //Row1
            ctrlCircle1_1.ColorOfControl = color;
            ctrlCircle1_2.ColorOfControl = color;
            ctrlCircle1_3.ColorOfControl = color;
            ctrlCircle1_4.ColorOfControl = color;

            //Row2
            ctrlCircle2_1.ColorOfControl = color;
            ctrlCircle2_2.ColorOfControl = color;
            ctrlCircle2_3.ColorOfControl = color;
            ctrlCircle2_4.ColorOfControl = color;

            //Row3
            ctrlCircle3_1.ColorOfControl = color;
            ctrlCircle3_2.ColorOfControl = color;
            ctrlCircle3_3.ColorOfControl = color;
            ctrlCircle3_4.ColorOfControl = color;

            //Row4
            ctrlCircle4_1.ColorOfControl = color;
            ctrlCircle4_2.ColorOfControl = color;
            ctrlCircle4_3.ColorOfControl = color;
            ctrlCircle4_4.ColorOfControl = color;
        }
        bool _StatusOfChoices;
        public bool StatusOfChoices 
        {
            get 
            {
                return _StatusOfChoices; 
            }
            set
            {
                _StatusOfChoices = value;
                Status(_StatusOfChoices);
            }
        }

        public void Status(bool Start)
        {
            //Row1
            ctrlCircle1_1.Turn = Start;
            ctrlCircle1_2.Turn = Start;
            ctrlCircle1_3.Turn = Start;
            ctrlCircle1_4.Turn = Start;

            //Row2
            ctrlCircle2_1.Turn = Start;
            ctrlCircle2_2.Turn = Start;
            ctrlCircle2_3.Turn = Start;
            ctrlCircle2_4.Turn = Start;

            //Row3
            ctrlCircle3_1.Turn = Start;
            ctrlCircle3_2.Turn = Start;
            ctrlCircle3_3.Turn = Start;
            ctrlCircle3_4.Turn = Start;

            //Row4
            ctrlCircle4_1.Turn = Start;
            ctrlCircle4_2.Turn = Start;
            ctrlCircle4_3.Turn = Start;
            ctrlCircle4_4.Turn = Start;
        }

        bool _CheckResult(ctrlCircle circle, string Answer) => circle.TextOfCharctar.ToLower() == Answer.ToString().ToLower();

        public (string QuestionAnswer, bool Answer) CheckResult(int Row,  int Col, string Answer)
        {
            if(Row == 1)
            {
                if (Col == 1)
                   return (ctrlCircle1_1.TextOfCharctar, _CheckResult(ctrlCircle1_1, Answer));
                else if (Col == 2)
                   return (ctrlCircle1_2.TextOfCharctar, _CheckResult(ctrlCircle1_2, Answer));
                else if(Col == 3)
                   return (ctrlCircle1_3.TextOfCharctar, _CheckResult(ctrlCircle1_3, Answer));
                else
                   return (ctrlCircle1_4.TextOfCharctar, _CheckResult(ctrlCircle1_4, Answer));
            }
            else if(Row == 2)
            {
                if (Col == 1)
                    return (ctrlCircle2_1.TextOfCharctar,_CheckResult(ctrlCircle2_1, Answer));
                else if (Col == 2)
                    return (ctrlCircle2_2.TextOfCharctar, _CheckResult(ctrlCircle2_2, Answer));
                else if (Col == 3)
                    return (ctrlCircle2_3.TextOfCharctar, _CheckResult(ctrlCircle2_3, Answer));
                else
                    return (ctrlCircle2_4.TextOfCharctar, _CheckResult(ctrlCircle2_4, Answer));
            }
            else if(Row == 3)
            {
                if (Col == 1)
                    return (ctrlCircle3_1.TextOfCharctar, _CheckResult(ctrlCircle3_1, Answer));
                else if (Col == 2)
                    return (ctrlCircle3_2.TextOfCharctar, _CheckResult(ctrlCircle3_2, Answer));
                else if (Col == 3)
                    return (ctrlCircle3_3.TextOfCharctar, _CheckResult(ctrlCircle3_3, Answer));
                else
                    return (ctrlCircle3_4.TextOfCharctar, _CheckResult(ctrlCircle3_4, Answer));
            }
            else
            {
                if (Col == 1)
                    return (ctrlCircle4_1.TextOfCharctar, _CheckResult(ctrlCircle4_1, Answer));
                else if (Col == 2)
                    return (ctrlCircle4_2.TextOfCharctar, _CheckResult(ctrlCircle4_2, Answer));
                else if (Col == 3)
                    return (ctrlCircle4_3.TextOfCharctar, _CheckResult(ctrlCircle4_3, Answer));
                else
                    return (ctrlCircle4_4.TextOfCharctar, _CheckResult(ctrlCircle4_4, Answer));
            }
        }

        public class clsQuestionInfo : EventArgs
        {
            public string YourAnswer { get;}
            public string QuestionAnswer { get;}
            public bool Result { get;}

            public clsQuestionInfo(string yourAnswer, string questionAnswer)
            {
                YourAnswer = yourAnswer;
                QuestionAnswer = questionAnswer;
                Result = YourAnswer.ToLower()==questionAnswer.ToLower();
            }
        }


        public event EventHandler<clsQuestionInfo> ResultOfEachRound;
        public void CheckUserAnswer(int Row, int Col, string Answer)
        {
            var QInfo = CheckResult(Row, Col, Answer.ToUpper());
            if (QInfo.Answer)
                _CurrentRound.NumberOfRightAnswer++;
            else
                _CurrentRound.NumberOfWrongAnswer++;

            RateUser.Value = _CurrentRound.NumberOfRightAnswer;
            ResultOfEachRound?.Invoke(this, new clsQuestionInfo(Answer, QInfo.QuestionAnswer));
        }


        int Row=0, Col=0;
        void _SetNewQuestion()
        {
            SwitchAnswer.Checked = false;
            txtAnswer.Text = "";
            lblTimer.ForeColor = Color.Black;
            _RefreshData();

            Random rnd = new Random();
            Row = rnd.Next(1, 4);
            Col = rnd.Next(1, 4);
            lblIndex.Text = $"[{Row}, {Col}]";
        }

        void _NextRound()
        {

            txtAnswer.Text = "";
            if (_CurrentRound.NumberOfRound == _CurrentRound.MaxNumberOfRound)
            {
                Status(false);
                gbExam.Enabled = false;
                timer1.Stop();
                lblResult.Text = _CurrentRound.IsPass() ? "True" : "False";
                btnStart.Text = "Finish";
                OnFinishExam?.Invoke(this,new GameInfo(_CurrentRound));
                lblTimer.Text = _CurrentRound.MaxTimeForEachRound.ToString();
                lblRound.Text = $"{_CurrentRound.NumberOfRound} / {_CurrentRound.MaxNumberOfRound}";
                return;
            }

            if (_CurrentRound.NumberOfRound < 5)
                _CurrentRound.NumberOfRound++;

            _CurrentRound.Timer = _CurrentRound.MaxTimeForEachRound;
            lblRound.Text = $"{_CurrentRound.NumberOfRound} / {_CurrentRound.MaxNumberOfRound}";

            _SetNewQuestion();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {

            if(_CurrentRound.Timer > 0)
            {
                _CurrentRound.Timer -= 1;
               
                if (_CurrentRound.Timer < 20)
                    lblTimer.ForeColor = Color.Orange;
                else if(_CurrentRound.Timer < 10)
                    lblTimer.ForeColor = Color.Red;

            }
            else
            {
                if(!SwitchAnswer.Checked)
                {
                    CheckUserAnswer(Row, Col, txtAnswer.Text);
                }
                _NextRound();
                
            }
               
            lblTimer.Text = $"{_CurrentRound.Timer} S";
        }

        private void txtAnswer_KeyPress(object sender, KeyPressEventArgs e)
        {
            
            e.Handled = char.IsLetter(e.KeyChar) && txtAnswer.Text.Length==1;

        }

        private void SwitchAnswer_CheckedChanged(object sender, EventArgs e)
        {
            if(SwitchAnswer.Checked)
            {
            CheckUserAnswer(Row, Col, txtAnswer.Text);
            _NextRound() ;
            }
        }

        private void txtAnswer_TextChanged(object sender, EventArgs e)
        {
            SwitchAnswer.Enabled = txtAnswer.Text.Length==1;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            Status(true);
            _SetNewQuestion();
            timer1.Start();
            pQuestion.Enabled = true;
            btnStart.Enabled = false;
        }
    }
}
