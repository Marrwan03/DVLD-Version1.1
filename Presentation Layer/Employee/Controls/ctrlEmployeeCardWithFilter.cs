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
    public partial class ctrlEmployeeCardWithFilter : UserControl
    {
        public ctrlEmployeeCardWithFilter()
        {
            InitializeComponent();
        }
        public event Action<int> OnEmployeeAdded;
        public event Action<int> OnEmployeeSelected;
        int _EmployeeID;
        public int EmployeeID { get { return _EmployeeID; } }
        clsEmployee _EmployeeInfo;
        public clsEmployee EmployeeInfo { get { return _EmployeeInfo; } }
        bool _EnableFilter;
        public bool EnableFilter { get { return _EnableFilter; } 
            set 
            {
                _EnableFilter = value;
                gbFilter.Enabled = _EnableFilter;
            } }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
            {
                btnFind.PerformClick();
            }
            if (cbFilterBy.Text != "Username")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        void _FillData()
        {
            int EmployeeId = 0;
            switch (cbFilterBy.Text)
            {
                case "Employee ID":
                    {
                        EmployeeId = Convert.ToInt32(txtFilterValue.Text);
                        break;
                    }
                case "User ID":
                    {
                        EmployeeId = clsEmployee.FindEmployeeByUserID (Convert.ToInt32(txtFilterValue.Text) ).EmployeeID.Value;
                        break;
                    }
            }
            ctrlEmployeeCard1.ctrlEmployeeCard_Load(EmployeeId);
            if (OnEmployeeSelected!=null && EnableFilter)
                OnEmployeeSelected(ctrlEmployeeCard1.EmployeeID);
            
        }

        void DataBack(object sender, int EmployeeID)
        {
            _EmployeeID = EmployeeID;
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Text = EmployeeID.ToString();
            ctrlEmployeeCard1.ctrlEmployeeCard_Load(EmployeeID);

            OnEmployeeAdded?.Invoke(_EmployeeID);
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if(ctrlEmployeeCard1.EmployeeInfo != null)
            {
                if (MessageBox.Show("If you want add new employee, this Current EmpInfo will be remove\n\tAre you sure do you continue?",
                    "Note", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                    return;
            }
            frmAddUpdateEmployee addEmployee = new frmAddUpdateEmployee();
            addEmployee.DataBack += DataBack;
            addEmployee.ShowDialog();
        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                MessageBox.Show("You cannot find without set \n\t[EmployeeID Or UserID OR PersonID Or Username].",
                    "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFilterValue.Focus();
                return;
            }
            _FillData();
        }
      public  void ctrlEmployeeCardWithFilter_Load(int EmployeeID)
        {
            txtFilterValue.Text = EmployeeID.ToString();
            cbFilterBy.SelectedIndex = 0;
            _FillData();
        }
        private void ctrlEmployeeCardWithFilter_Load(object sender, EventArgs e)
        {

            cbFilterBy.SelectedIndex = 0;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = string.Empty;
        }

        private void ctrlEmployeeCard1_Load(object sender, EventArgs e)
        {

        }
    }
}
