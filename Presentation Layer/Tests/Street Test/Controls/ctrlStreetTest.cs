using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Media;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD.Applications.Controls.ctrlAddNewApplication;

namespace DVLD.Tests.Street_Test.Controls
{
    public partial class ctrlStreetTest : UserControl
    {
        public bool EnableGbExam { get { return gbExamInfo.Enabled; } set { gbExamInfo.Enabled = value; } }
        public class clsExamInfo : EventArgs
        {
            public int NumberOfWarning { get; }
            public int MinutesOfExam { get; }
            public int MinutesYouTakeIt {  get; }
            public bool IsPass() => NumberOfWarning > 0;

            public clsExamInfo(int numberOfWarning, int minutesOfExam, int minutesYouTakeIt) 
            {
                NumberOfWarning = numberOfWarning;
                MinutesOfExam = minutesOfExam;
                MinutesYouTakeIt = minutesYouTakeIt;
                IsPass();

            }

            public override string ToString()
            {
                string s = $"[{NumberOfWarning}] عدد المخالفات المتبقيه" +
                    $"\n[{MinutesYouTakeIt} m] الزمن المقضي" +
                    $"\n[{MinutesOfExam} m] الزمن الافتراضي" +
                    $"\n[{(IsPass() ? "ناجح" : "راسب")}] النتيجة";
                return s;
            }
        }

        public event EventHandler<clsExamInfo> OnFinishExam;

        int _NumberOfMeter;
        public int NumberOfMeter
        {
            get
            {
                return _NumberOfMeter;
            }
            set
            {
                _NumberOfMeter = value;
                if (_NumberOfMeter>=0&&_NumberOfMeter <= 220)
                {
                    lblCarMeter.Text = _NumberOfMeter.ToString();
                    BarOfMeter.Value = _NumberOfMeter;
                    btnPush.Text = "Push";
                }
                else
                    btnPush.Text = "Max";
            }
        }

        int _NumberOfWarning = 0;
        public int NumberOfWarning
        {
            get { return _NumberOfWarning; }
            set
            {
                _NumberOfWarning = value;
                lblNumberofwarnings.Text = _NumberOfWarning.ToString();
            }
        }

        int _MinutesOfExam=0;
        public void ctrlStreet_Load(int numberOfWarning=3, int MinutesOfExam=1)
        {
            NumberOfWarning = numberOfWarning;
            _MinutesOfExam = MinutesOfExam;
           
        }

        BitArray _Passed = new BitArray(4,true);

        void _IncreaseSpeedOfCar()
        {
            //حدد موقع السياره
            int x = picCar.Location.X;
            int y = picCar.Location.Y;
            
            
            if ((y-13 == 519) && (x >= 90 && x < 703) )
            {               
                picCar.Location = new Point(x+=4, y);
                picCar.Image = Resources.FrontStraightCar;
                if (ctrlTrafficLight1.CurrentLight == ctrlTrafficLight.enTrafficLight.Red)
                {
                    _CheckTrafficLight();
                }
            }
            else if(x >= 703 && _Passed[0])
            {
                picCar.Image = Resources.FrontSideCar;
                picCar.Location = new Point(791, 446);
                _Passed[0] = false; 
            }
            else if(x-10 == 781 && (y <= 446 && y > 76) )
            {
                picCar.Location = new Point(x, y-=4);
            }
            else if(y <=76 && _Passed[1])
            {
                picCar.Image = Resources.BackStraightCar;
                picCar.Location = new Point(722, 15);
                _Passed[1] = false;
            }
            else if(y-15==0 && (x <= 722 && x> 90))
            {
                picCar.Location = new Point(x-=4, y);
                if (ctrlTrafficLight2.CurrentLight == ctrlTrafficLight.enTrafficLight.Red)
                {
                    _CheckTrafficLight();
                }
            }
            else if(x<=90 && _Passed[2])
            {
                picCar.Image = Resources.BackSideCar;
                picCar.Location = new Point(31, 76);
                _Passed[2] = false;
            }
            else if(x-10 == 21 && (y >= 76 &&  y < 446))
            {
                picCar.Location = new Point(x, y+=5);

                if (ctrlTrafficLight3.CurrentLight == ctrlTrafficLight.enTrafficLight.Red)
                {
                    _CheckTrafficLight();
                }

            }
            else if(y == 446 && _Passed[3])
            {
                picCar.Image = Resources.FrontStraightCar;
                picCar.Location = new Point(90, 532);

                _Passed.SetAll(true);

            }

        }

        
        void _RaiseNotification()
        {
            NotificationForTrafficLight.Icon = SystemIcons.Warning;
            NotificationForTrafficLight.BalloonTipIcon = ToolTipIcon.Warning;
            NotificationForTrafficLight.BalloonTipTitle = "مخالفة مرورية";
            NotificationForTrafficLight.BalloonTipText = 
                $"[{picCar.Location.X}, {picCar.Location.Y}] تم قيد مخالفة في موقع"+
                $"\n[{_NumberOfWarning}] عدد المخالفات الباقيه";
               

            NotificationForTrafficLight.ShowBalloonTip(2000);
        }

        bool _IsPassTrafficLight()
        {
            //set location of three cars then check location car if (Equal && traffic light ==red) raise notification
            if(picCar.Location.Y-13 == 519) //In Front Straight Line
            {
                return picCar.Location.X >= 430 && picCar.Location.X <= 510;
            }
            else if (picCar.Location.Y - 15 == 0) //In Back Straight Line 
            {
                return picCar.Location.X <= 605 && picCar.Location.X >= 535;
            }
            else if(picCar.Location.X -10 == 21)
            {
                return picCar.Location.Y >=95 && picCar.Location.Y <= 220;
            }
            return false;

        }

        public ctrlStreetTest()
        {
            InitializeComponent();
        }        
        public event Action<int> OnWarning;

        void _CheckTrafficLight()
        {
            if (_IsPassTrafficLight())
            {
                NumberOfWarning--;
                _RaiseNotification();
                OnWarning?.Invoke(NumberOfWarning);
            }
            if (NumberOfWarning == 0)
            {
                timerofExam.Stop();
                TimeSpan span = TimeSpan.FromSeconds(_Seconds);
                ctrlTrafficLight1.ctrlTrafficLight_Off();
                ctrlTrafficLight2.ctrlTrafficLight_Off();
                ctrlTrafficLight3.ctrlTrafficLight_Off();
                OnFinishExam?.Invoke(this, new clsExamInfo(NumberOfWarning, _MinutesOfExam, span.Minutes));
            }

        }

      

        private void btnStart_Click(object sender, EventArgs e)
        {
            btnStart.Enabled = false;
            pOperationOfCar.Visible = true;
            ctrlTrafficLight1.ctrlTrafficLight_Load();
            ctrlTrafficLight2.ctrlTrafficLight_Load();
            ctrlTrafficLight3.ctrlTrafficLight_Load();
            timerofExam.Start();
        }
        int _Seconds = 0;
        private void timerofExam_Tick(object sender, EventArgs e)
        {
            _Seconds++;
            TimeSpan span = TimeSpan.FromSeconds(_Seconds);
            string Time = span.ToString(@"hh\:mm\:ss");    
            lblTimer.Text = Time;
            lblTimer.Refresh();

            if(span.Minutes == _MinutesOfExam)
            {
                timerofExam.Stop();
                 OnFinishExam?.Invoke(this, new clsExamInfo(NumberOfWarning, _MinutesOfExam, span.Minutes));
            }

        }


        SoundPlayer sp = new SoundPlayer(Directory.GetCurrentDirectory() +"\\CarSound.wav");
        private void btnPush_MouseEnter_1(object sender, EventArgs e)
        {
           timerOfCarIncrease.Start();
            sp.Play();
            timerOfCarDecrease.Stop();
            lblCarStatus.Text = "Running";
        }

      
        private void timerofcar_Tick(object sender, EventArgs e)
        {
            NumberOfMeter += 1;
            _IncreaseSpeedOfCar();
        }

        private void timerOfCarDecrease_Tick(object sender, EventArgs e)
        {
            NumberOfMeter -= 1;
        }

        private void btnPush_MouseLeave_1(object sender, EventArgs e)
        {
            sp.Stop();
            timerOfCarDecrease.Start();
            timerOfCarIncrease.Stop();
            lblCarStatus.Text = "Stop";
        }

        private void ctrlTrafficLight2_OnRed(object sender, ctrlTrafficLight.clsEventHandler e)
        {
            
        }

        private void ctrlStreetTest_Load(object sender, EventArgs e)
        {

        }
    }
}
