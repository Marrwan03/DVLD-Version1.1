using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.MyGeneralUserControl
{
    public partial class ctrlDashboardDataInfo : UserControl
    {
        string _ImagePath;
        public string ImagePath
        {
            get
            {
                return _ImagePath;
            }
            set
            {
                _ImagePath = value;
                if(_ImagePath != null)
                    picPhoto.Load(_ImagePath);

            }
        }


        public enum enLoginType { User, Employee}

        enLoginType _loginType;
        public enLoginType LoginType 
        {
            get { return _loginType; } 
            set 
            {
                _loginType = value;

                switch (_loginType)
                {
                    case enLoginType.User:
                        lblLoginType.Text = "User : ";
                        break;
                    case enLoginType.Employee:
                        lblLoginType.Text = "Employee : ";
                        break;

                }

            }
        }
        public string NameOf { get { return lblName.Text; } }
        public ctrlDashboardDataInfo()
        {
            InitializeComponent();
        }
        public void ctrlDashboardDataInfo_Load(int LoginID, enLoginType loginType)
        {
            timer1.Start();
            this.LoginType = loginType;

            string Name = "Empty";
            string ImagePath = null;
            short Gendor = 0;

            if(loginType == enLoginType.User)
            {
                clsUser user = clsUser.FindByUserID(LoginID);
                if (user != null)
                {
                    Name = user.PersonInfo.FirstName + " " + user.PersonInfo.LastName;
                    ImagePath = user.PersonInfo.ImagePath;
                    Gendor = user.PersonInfo.Gendor;
                   
                }
            }
            else
            {
                clsEmployee employee = clsEmployee.FindByEmployeeID(LoginID);
                if (employee != null)
                {
                    Name = employee.PersonInfo.FirstName + " " + employee.PersonInfo.LastName;
                    ImagePath = employee.PersonInfo.ImagePath;
                    Gendor = employee.PersonInfo.Gendor;
                }
            }

            lblName.Text = Name;

            if(string.IsNullOrEmpty(ImagePath))
            {
                if( Gendor == 0)
                {
                    picPhoto.Image = Resources.Male_512;
                }
                else
                {
                    picPhoto.Image = Resources.Female_512;
                }
                
            }
            picPhoto.ImageLocation = ImagePath;

        }
        private void ctrlDashboardDataInfo_Load(object sender, EventArgs e)
        {
            
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            lblTimer.Text = DateTime.Now.ToString("hh:mm:ss d/M/yyyy");
        }
    }
}
