using DVLD.Classes;
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

namespace DVLD.Employee.Controls
{
    public partial class ctrlEmployeeCard : UserControl
    {
        int _EmployeeID;
        clsEmployee _EmployeeInfo;

        public int EmployeeID { get { return _EmployeeID; } }

        public clsEmployee EmployeeInfo { get { return _EmployeeInfo; } }

        public ctrlEmployeeCard()
        {
            InitializeComponent();
            
        }

        void _ResetData()
        {
            ctrlPersonCard1.ResetPersonInfo();

            lblEmployeeID.Text = "???";
            lblUserID.Text = "???";
            lblHireDate.Text = "???";
            lblIsActive.Text = "???";

            llblShowPermisions.Enabled = false;
            llblShowUserInfo.Enabled = false;
        }

        private void llblShowPermisions_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            _EmployeeInfo = clsEmployee.FindByEmployeeID(_EmployeeID);
            if (_EmployeeInfo == null)
            {
                MessageBox.Show($"There isn`t employee with this id [ {_EmployeeID} ].", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            frmShowPermisions showPermisions = new frmShowPermisions(_EmployeeID);
            showPermisions.Show();
        }

        public void ctrlEmployeeCard_Load(int EmployeeID)
        {
            _EmployeeID = EmployeeID;
            _EmployeeInfo = clsEmployee.FindByEmployeeID(EmployeeID);
            if ( _EmployeeInfo == null )
            {
                MessageBox.Show($"There isn`t Employee With this ID {_EmployeeID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                _ResetData();
                return;
            }
            ctrlPersonCard1.LoadPersonInfo(_EmployeeInfo.PersonInfo.PersonID.Value);
            lblEmployeeID.Text = _EmployeeInfo.EmployeeID.ToString();
            lblUserID.Text = _EmployeeInfo.UserID.ToString();
            lblHireDate.Text = clsFormat.DateToShort(_EmployeeInfo.HireDate);
            lblIsActive.Text = _EmployeeInfo.Status== clsUser.enStatus.Active ? "True" : "False";

            llblShowPermisions.Enabled = true;
            llblShowUserInfo.Enabled = true;
        }

        private void ctrlEmployeeCard_Load(object sender, EventArgs e)
        {
            _ResetData();
        }

        private void llblShowUserInfo_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(_EmployeeInfo.UserID.Value);
            userInfo.Show();
        }
    }
}
