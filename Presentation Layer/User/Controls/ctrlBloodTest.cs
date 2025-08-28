using DVLD.Properties;
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

namespace DVLD.Employee
{
    public partial class ctrlBloodTest : UserControl
    {
        public class clsBloodTest : EventArgs
        {
            public int NumberOfBloodTest { get; }
            public clsUser.enBloodType BloodType { get; }
            public string BloodTypeString { get; }


            public clsBloodTest(int numberOfBloodTest, clsUser.enBloodType bloodType, string bloodTypeString)
            {
                NumberOfBloodTest = numberOfBloodTest;
                BloodType = bloodType;
                BloodTypeString = bloodTypeString;
            }
        }

        clsUser.enBloodType _BloodType= clsUser.enBloodType.None;
        public clsUser.enBloodType BloodType 
        {
            get 
            {
                return _BloodType; 
            }
            set 
            {
                _BloodType = value;
                btnDrawBlood.Text = _GetBloodType(_BloodType);
            } 
        }
        public enum enHand { Open, Close}
        enHand _HandStatus = enHand.Close;
        public enHand HandStatus
        {
            get { return _HandStatus; }
            set
            {
                _HandStatus = value;
                switch (_HandStatus)
                {
                    case enHand.Open:
                        {
                            picHand.Image = Resources.OpenHand; break;
                        }
                    default:
                        {
                            picHand.Image = Resources.CloseHand;
                            break;
                        }
                }

            }
        }
        int _NumberOfBloodInNeedle = 0;
        public int NumberOfBloodInNeedle 
        {
            get 
            {
                return _NumberOfBloodInNeedle;
            }
            set
            {
                _NumberOfBloodInNeedle = value;
                pbBlood.Value = _NumberOfBloodInNeedle; 
            } 
        }
        public event EventHandler<clsBloodTest> CompletedBloodTest;


        string _GetBloodType(clsUser.enBloodType BloodType) => clsUser.GetBloodType(BloodType);

        public ctrlBloodTest()
        {
            InitializeComponent();
        }
        public void ctrlBloodTest_Load(clsUser.enBloodType bloodType)
        {
           this.NumberOfBloodInNeedle = 100;
            HandStatus = enHand.Close;
            BloodType = bloodType;
            btnDrawBlood.Enabled = false;
        }
        private void ctrlBloodTest_Load(object sender, EventArgs e)
        {

        }

        private void tmBloodTest_Tick(object sender, EventArgs e)
        {
            NumberOfBloodInNeedle++;
            if (NumberOfBloodInNeedle == 100)
            {
                picHand.Image = Resources.CloseHand;
                btnDrawBlood.Enabled = false;
                btnDrawBlood.Text = "Finish";
                clsUser.enBloodType BloodType = clsUser.GetRandomeBloodType();
                CompletedBloodTest(this, new clsBloodTest(NumberOfBloodInNeedle, BloodType, clsUser.GetBloodType(BloodType)));
            }
        }

        private void btnDrawBlood_MouseDown(object sender, MouseEventArgs e)
        {
            NumberOfBloodInNeedle += 25;

            if (NumberOfBloodInNeedle == 100)
            {
                tmBloodTest.Stop();
                picHand.Image = Resources.CloseHand;
                btnDrawBlood.Enabled = false;
                btnDrawBlood.Text = "Finish";
                clsUser.enBloodType BloodType = clsUser.GetRandomeBloodType();
                CompletedBloodTest(this, new clsBloodTest(NumberOfBloodInNeedle, BloodType, clsUser.GetBloodType(BloodType)));
            }
        }

        private void btnDrawBlood_MouseLeave(object sender, EventArgs e)
        {
            picHand.Image = Resources.CloseHand;
            tmBloodTest.Stop();
        }

        private void btnDrawBlood_MouseHover(object sender, EventArgs e)
        {

        }

        private void btnDrawBlood_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void btnDrawBlood_MouseEnter(object sender, EventArgs e)
        {
            picHand.Image = Resources.OpenHand;
            tmBloodTest.Start();
        }

        private void btnDrawBlood_Click(object sender, EventArgs e)
        {

        }
    }
}
