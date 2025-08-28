using DVLD.Properties;
using DVLD_Buisness;
using Guna.UI2.WinForms;
using System;
using System.Drawing;
using System.IO;
using System.Media;
using System.Threading;
using System.Windows.Forms;

namespace DVLD.Communication.Phone.Controls
{
    public partial class ctrlCallPhone : UserControl
    {
        public ctrlCallPhone()
        {
            InitializeComponent();
        }

        public class clsEventArgs : EventArgs 
        {
            public string NumberPhone {  get; set; }
            public int Duration { get; set; }

            public clsEventArgs(string numberPhone, int duration)
            {
                NumberPhone = numberPhone;
                Duration = duration;
            }
        }


        public event Action<clsEventArgs> OnCallOff;
       public enum enStatusMobile { On, Off}

        enStatusMobile _StatusMobile;
        public enStatusMobile StatusMobile { get { return _StatusMobile; } set { _StatusMobile = value; 
            
            switch (_StatusMobile)
                {
                    case enStatusMobile.On:
                        picMobileScreen.Image = Resources.RunMobileScreen;
                        break;
                        case enStatusMobile.Off:
                        picMobileScreen.Image = Resources.MobileScreen;
                        break;
                }
            } }
        int _RecipientID=-1;
        clsCallLog.enFor _RecipientType = DVLD_Buisness.Classes.clsCommunication.enFor.None;
        int _Duration = 0;

        void _LoadDataBy(int PersonID)
        {
            clsPerson person = clsPerson.Find(PersonID);
            if(person != null)
            {
                if(File.Exists(person.ImagePath))
                {
                    picCaller.ImageLocation = person.ImagePath;
                }
                else
                    picCaller.Image = Resources.Person_32;

                lblNumberPhone.Text = person.Phone;
                lblCountryCode.Text = person.CountryInfo.CountryCode;
                lblCountryName.Text = person.CountryInfo.CountryName;
                lblName.Text = person.FirstName + " " + person.LastName;
            }
            else
            {
                if(MessageBox.Show("No Person Account With RecipientPersonID("+PersonID+").", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    picCaller.Image = Resources.Delete_32_2;
                    lblNumberPhone.Text = null;
                    lblCountryCode.Text = null;
                    lblCountryName.Text = null;
                    lblName.Text = null;
                }
               

            }
        }

        void _LoadData()
        {
            int PersonID=0;

            switch (_RecipientType)
            {
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForPerson:
                    {
                        PersonID = clsPerson.Find(_RecipientID).PersonID.Value;
                        break;
                    }
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForUser:
                    {
                        PersonID = clsUser.FindByUserID(_RecipientID).PersonID;
                        break;
                    }
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee:
                    {
                        PersonID = clsEmployee.FindByEmployeeID(_RecipientID).PersonID; break;
                    }
            }
            _LoadDataBy(PersonID);
        }

        public void ctrlCallPhone_Load(int RecipientID, clsCallLog.enFor RecipientType)
        {
            sp.Play();
            picMobileScreen.Image = Resources.RunMobileScreen;
            tmDuration.Start();
            tmDot.Start();
            _RecipientID = RecipientID;
            _RecipientType = RecipientType;
            _LoadData();
        }
        //C:\Users\lenovo\source\repos\DVLD_Original\DVLD\bin\Debug\huawei-ringtone-559.wav
        void _ChangePicture(Guna2CirclePictureBox pictureBox, Bitmap MuteImage, Bitmap UnMuteImage1)
        {
            if (pictureBox.Tag.ToString() == "?")
            {
                pictureBox.Tag = "!";
                pictureBox.Image = MuteImage;
            }
            else
            {
                pictureBox.Tag = "?";
                pictureBox.Image = UnMuteImage1;
            }
        }

        private void guna2CirclePictureBox3_Click(object sender, EventArgs e)
        {
            _ChangePicture(picYourVoice, Resources.MuteYourVoice, Resources.UnMuteYourVoice);
        }

        SoundPlayer sp = new SoundPlayer(Directory.GetCurrentDirectory()+"\\huawei-ringtone-559.wav");

        private void picVoice_Click(object sender, EventArgs e)
        {
            _ChangePicture(picVoice, Resources.MuteVoice, Resources.UnMuteVoice);

            if(picVoice.Tag == "?")
                sp.Play();
            else
                sp.Stop();

        }

        private void ctrlCallPhone_Load(object sender, EventArgs e)
        {

        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if(tmDot.Enabled)
            {
                if(lblDot.Text == "...")
                {
                    lblDot.Text = "";
                }
                lblDot.Text += ".";
            }
        }

        private void picCallOff_Click(object sender, EventArgs e)
        {
            tmDuration.Stop();
            tmDot.Stop();
            sp.Stop();
            SoundPlayer soundOff = new SoundPlayer(Directory.GetCurrentDirectory() + "\\_2LINg0OyK0.wav");
            soundOff.Play();
            Thread.Sleep(3000);
            picMobileScreen.Image = Resources.MobileScreen;
            if (MessageBox.Show("The call is over.", "Over!", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                soundOff.Stop();
                this.Enabled = false;
                OnCallOff(new clsEventArgs(lblNumberPhone.Text, _Duration));
            }
        }

        private void gbCallPhone_Enter(object sender, EventArgs e)
        {

        }

        private void tmDuration_Tick(object sender, EventArgs e)
        {
            _Duration++;
        }

        private void picMobileScreen_Click(object sender, EventArgs e)
        {

        }
    }
}
