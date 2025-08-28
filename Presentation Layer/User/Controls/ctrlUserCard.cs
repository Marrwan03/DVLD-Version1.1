using DVLD.Properties;
using DVLD.Employee;
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

namespace DVLD.Controls
{
    public partial class ctrlUserCard : UserControl
    {

        private clsUser _User;
        private int? _UserID = null;
        private int? _PersonID = null;
        private string _Username = null;

       public enum enLoadUserInfo { ByUserID,  ByPersonID, ByUsername }

        public int? UserID
        {
            get { return _UserID.Value; }
        }
        public int? PersonID { get { return _PersonID.Value; } }
        public string Username { get { return _Username; } }
            

        public ctrlUserCard()
        {
            InitializeComponent();
        }

        public void LoadUserInfo(int Search, enLoadUserInfo loadUserInfo)
        {
            switch (loadUserInfo)
            {
                case enLoadUserInfo.ByUserID:
                    {
                        _UserID = Search;
                        _User = clsUser.FindByUserID(_UserID.Value);
                        break;
                    }
                    case enLoadUserInfo.ByPersonID:
                    {
                        _PersonID = Search;
                        _User = clsUser.FindByPersonID(_PersonID.Value);
                        break;

                    }
                    case enLoadUserInfo.ByUsername:
                    {
                        _Username = Search.ToString();
                        _User = clsUser.FindByUsername(_Username);
                        break;
                    }
            }

            

            if (_User == null)
            {
                _ResetUserInfo();
                MessageBox.Show("No User with UserID = " + UserID.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _UserID = _User.UserID;
            _FillUserInfo();
        }
       

        private void _FillUserInfo()
        {

            ctrlPersonCard1.LoadPersonInfo(_User.PersonID);
            lblUserID.Text = _User.UserID.ToString();
            lblUserName.Text = _User.UserName.ToString();

            lblIsActive.Text = _User.Status == clsUser.enStatus.Active ? "Yes" : "No";

            lblBloodType.Text = _User.GetBloodType();
        }

        private void _ResetUserInfo()
        {
            
            ctrlPersonCard1.ResetPersonInfo();
            lblUserID.Text = "[???]";
            lblUserName.Text = "[???]";
            lblIsActive.Text = "[???]";
        }

        private void ctrlPersonCard1_Load(object sender, EventArgs e)
        {

        }

        private void ctrlUserCard_Load(object sender, EventArgs e)
        {

        }

       

        private void groupBox1_Enter(object sender, EventArgs e)
        {

        }
    }
}
