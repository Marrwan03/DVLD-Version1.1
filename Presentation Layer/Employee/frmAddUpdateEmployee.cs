using DVLD.Classes;
using DVLD_Buisness;
using iText.Forms.Xfdf;
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
    public partial class frmAddUpdateEmployee : Form
    {
        public frmAddUpdateEmployee()
        {
            InitializeComponent();
            _Mode = enMode.Add;
        }
        public frmAddUpdateEmployee(int EmployeeID)
        {
            InitializeComponent();
            _EmployeeID = EmployeeID;
            _Mode = enMode.Update;
        }
       public delegate void MyDelegate(object Sender,int EmployeeID);
        public event MyDelegate DataBack;
        clsEmployee _EmployeeInfo;
        int _EmployeeID;
        public int EmployeeID { get { return _EmployeeID; } }

        enum enMode { Add, Update};
        enMode _Mode;

        void _RefreshData()
        {
            if(_Mode == enMode.Add)
            {
                lblTitle.Text = "Add New Employee";
                tbLoginInfo.Enabled = false;
                ctrlUserCardWithFilter1.Focus();
                _EmployeeInfo = new clsEmployee();
            }
            else
            {
                lblTitle.Text = "Update Employee";
                tbLoginInfo.Enabled = true;
                btnSave.Enabled = true;
            }

            lblEmployeeID.Text = "???";
            lblHireDate.Text = clsFormat.DateToShort(DateTime.Now);
            lblExitDate.Text = "Null";
            txtMonthlySalary.Text = "0";
            lblPermisions.Text = "0";
            lblUserID.Text = "0";
            chbIsActive.Checked = false;
            ctrlEmployeePermisions1.Mode = Employee.ctrlEmployeePermisions.enMode.Add;
        }

        void _LoadData()
        {
            _EmployeeInfo = clsEmployee.FindByEmployeeID(_EmployeeID);
            if(_EmployeeInfo == null )
            {
                MessageBox.Show($"No employee with id {_EmployeeID}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ctrlUserCardWithFilter1.EnableUserFilter = false;
            ctrlUserCardWithFilter1.LoadUserInfoByUserID(_EmployeeInfo.UserID.Value);
            lblEmployeeID.Text = _EmployeeInfo.EmployeeID.ToString();
            lblHireDate.Text = clsFormat.DateToShort(_EmployeeInfo.HireDate);
            txtMonthlySalary.Text = _EmployeeInfo.Monthltysalary.ToString();
            lblPermisions.Text = _EmployeeInfo.Permisions.ToString();
            lblUserID.Text = _EmployeeInfo.UserID.ToString();
            chbIsActive.Checked = _EmployeeInfo.Status == clsUser.enStatus.Active;

            ctrlEmployeePermisions1.ctrlUserPermision_Load(_EmployeeInfo.Permisions);
            ctrlEmployeePermisions1.Mode = Employee.ctrlEmployeePermisions.enMode.Update;
        }
        long _Permisions;

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tbLoginInfo.Enabled = true;
                tcEmployyeInfo.SelectedTab = tcEmployyeInfo.TabPages["tbLoginInfo"];
                return;
            }

            if (!ctrlUserCardWithFilter1.UserID.HasValue)
            {
                MessageBox.Show("You Have to set Employee to continue...", "Empty Employee!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbLoginInfo.Enabled = false;
                ctrlUserCardWithFilter1.Focus();
                return;
            }

            if (clsEmployee.IsUserHasEmployeeAcc(ctrlUserCardWithFilter1.UserID.Value))
            {
                MessageBox.Show("This Employee is already has Account.", "error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbLoginInfo.Enabled = false;
                ctrlUserCardWithFilter1.Focus();
                return;
            }
            tbLoginInfo.Enabled = true;
            tcEmployyeInfo.SelectedTab = tcEmployyeInfo.TabPages["tbLoginInfo"];
            lblUserID.Text = ctrlUserCardWithFilter1.UserID.Value.ToString();
            btnSave.Enabled = true;

        }

        private void frmAddUpdateEmployee_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            chbIsActive.Checked = true;
            _RefreshData();
            if(_Mode == enMode.Update)
            {
                _LoadData();
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro",
                    "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;

            }

            if(Convert.ToDecimal( txtMonthlySalary.Text) <= 0 )
            {
                MessageBox.Show("You don`t set any Monthly Salary for this employee!", "Non Salary", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMonthlySalary.Focus();
                return;
            }

            if(ctrlEmployeePermisions1.Permision == 0)
            {
                MessageBox.Show("You don`t set any permision for this employee!", "Non Permision", MessageBoxButtons.OK, MessageBoxIcon.Error);
                tbPermisions.Focus();
                ctrlEmployeePermisions1.Focus();
                return;
            }

            _EmployeeInfo.UserID = ctrlUserCardWithFilter1.UserID.Value;            
            _EmployeeInfo.ExitDate = null;
            _EmployeeInfo.Permisions = _Permisions;
            _EmployeeInfo.Monthltysalary = Convert.ToDecimal(txtMonthlySalary.Text);
            _EmployeeInfo.BounsPerc = 0;
            _EmployeeInfo.Status = chbIsActive.Checked? clsUser.enStatus.Active: clsUser.enStatus.NotActive;
            if(_Mode == enMode.Add)
                _EmployeeInfo.HireDate = DateTime.Now;

            if (_EmployeeInfo.Save())
            {
                _EmployeeID = _EmployeeInfo.EmployeeID.Value;
                lblEmployeeID.Text = _EmployeeInfo.EmployeeID.Value.ToString();
                lblPermisions.Text = _Permisions.ToString();
               
                ctrlEmployeePermisions1.Mode = Employee.ctrlEmployeePermisions.enMode.Update;
                lblTitle.Text = "Update Employee";
                DataBack?.Invoke(this, EmployeeID);
                if(_Mode == enMode.Update)
                    MessageBox.Show($"Updated EmployeeID : [{_EmployeeInfo.EmployeeID.Value}] successfully", "Updated Successfully!",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                else
                    MessageBox.Show($"Added Employee successfully with EmpID : [{_EmployeeInfo.EmployeeID.Value}]", "Added Successfully!",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);

                _Mode = enMode.Update;
                ctrlUserCardWithFilter1.EnableUserFilter = false;
            }
            else
            {
                MessageBox.Show($"Added Employee Failed ", "Added Failed!",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);


            }

        }

        

        private void txtMonthlySalary_KeyPress(object sender, KeyPressEventArgs e)
        {
            
        }

        private void txtMonthlySalary_Validating(object sender, CancelEventArgs e)
        {
            if (!clsValidatoin.ValidateFloat(txtMonthlySalary.Text))
            {
                e.Cancel = true;
                errorProvider1.SetError(txtMonthlySalary, "Invalid Salary Format");
            }
            else
            {
                errorProvider1.SetError(txtMonthlySalary, null);
            }
        }

        private void ctrlUserCardWithFilter1_OnFindUser(int obj)
        {
            lblUserID.Text = ctrlUserCardWithFilter1.UserID.Value.ToString();
        }

        private void txtMonthlySalary_TextChanged(object sender, EventArgs e)
        {

        }

        private void ctrlEmployeePermisions1_OnUserPermisionChanged(long obj)
        {
            _Permisions = obj;
            MessageBox.Show($"Your permision is : {obj}");
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        private void ctrlUserCardWithFilter1_OnAddUser(int obj)
        {
            
        }
    }
}
