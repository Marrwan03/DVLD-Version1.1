using DVLD.Classes;
using DVLD.Communication.Phone;
using DVLD.Email;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DVLD.Employee
{
    public partial class frmListEmployees : Form
    {
        public frmListEmployees()
        {
            InitializeComponent();
        }
        DataTable _dtEmployees;
        event Action<int> NoneMode;
        void _FilldgvEmployees(int NumberOfPage)
        {
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
            _dtEmployees = clsEmployee.GetEmployeeBy(ctrlSwitchSearch1.NumberOfPage, 3);
            lblRecordsCount.Text = _dtEmployees.Rows.Count.ToString();
            lblWhenEmployeeListareEmpty.Visible = _dtEmployees.Rows.Count <= 0;

            if(_dtEmployees.Rows.Count > 0 ) 
            {
                dgvEmployees.DataSource = _dtEmployees;
            }
        }

        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void frmListEmployees_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            NoneMode += _FilldgvEmployees;
            cbFilterBy.SelectedIndex = 0;
            NumricUDown.Maximum = clsEmployee.GetMaximumOfMonthltysalary();
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsEmployee.GetNumberOfRows());
            _FilldgvEmployees(1);



            dgvEmployees.EnableHeadersVisualStyles = false;
            dgvEmployees.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvEmployees(e.CurrentNumberOfPage);
        }

        private void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewEmployee))
            {
                clsGlobal.PermisionMessage("AddNewEmployee");
                return;
            }
            frmAddUpdateEmployee frmAddEmployee = new frmAddUpdateEmployee();
            frmAddEmployee.ShowDialog();
            frmListEmployees_Load(null, null);
        }

        void _ControlVisible(bool pFilterOfMonthlySalaryBool, bool cbIsActiveBool, bool txtFilterValueBool)
        {
            pFilterOfMonthlySalary.Visible = pFilterOfMonthlySalaryBool;
            cbStatus.Visible = cbIsActiveBool;
            txtFilterValue.Visible = txtFilterValueBool;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "None":
                    {
                        _ControlVisible(false, false, false);
                        NoneMode(1);
                        break;
                    }
                case "Monthly Salary":
                    {

                        _ControlVisible(true, false, false);
                        break;
                    }
                case "Status":
                    {
                        cbStatus.SelectedIndex = 0;
                        _ControlVisible(false, true, false);
                        break;
                    }
                    default:
                    {
                        _ControlVisible(false, false, true);
                        break;
                    }

            }

        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterName;

            switch (cbFilterBy.Text)
            {
                case "Employee ID":
                    {
                        FilterName = "EmployeeID";
                        break;
                    }
                default:
                    {
                        FilterName = "UserID";
                        break;
                    }
            }

            if(string.IsNullOrEmpty( txtFilterValue.Text.Trim()))
            {
                ctrlSwitchSearch1.Visible = true;
                _dtEmployees.DefaultView.RowFilter ="";
                lblRecordsCount.Text = _dtEmployees.Rows.Count.ToString();
                NoneMode(1);
                return;
            }
            ctrlSwitchSearch1.Visible = false;
            _dtEmployees.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterName, txtFilterValue.Text);
            lblRecordsCount.Text = _dtEmployees.Rows.Count.ToString();


        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            string FilterName = "Status";
            string FilterValue = cbStatus.Text;
            

            if(cbStatus.Text == "All")
            {
                _dtEmployees.DefaultView.RowFilter = "";
                ctrlSwitchSearch1.Visible = true;
                NoneMode(1);
                return;
            }
            else
            {
                ctrlSwitchSearch1.Visible = false;
                _dtEmployees.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterName, FilterValue);
            }
            lblRecordsCount.Text = _dtEmployees.Rows.Count.ToString();
        }

        private void NumricUDown_ValueChanged(object sender, EventArgs e)
        {
            if(NumricUDown.Value == NumricUDown.Maximum)
            {
                NumricUDown.Value = 0;
            }


            if(NumricUDown.Value == 0)
            {
                ctrlSwitchSearch1.Visible = true;
                NoneMode(1);
                return;
            }
            ctrlSwitchSearch1.Visible = false;
            _dtEmployees.DefaultView.RowFilter = string.Format("[{0}] <= {1}", "Monthltysalary", NumricUDown.Value.ToString());
            lblRecordsCount.Text = _dtEmployees.Rows.Count.ToString();
        }

        private void showEmployeeInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!clsGlobal.CurrentEmployee.CheckAccessPermision( clsEmployee.enPermision.ShowEmployeesDetails))
            {
                clsGlobal.PermisionMessage("ShowEmployeesDetails");
                return;
            }
            frmEmployeeInfo employeeInfo = new frmEmployeeInfo((int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value);
            employeeInfo.ShowDialog();
        }

        private void addNewEmployeeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewEmployee))
            {
                clsGlobal.PermisionMessage("AddNewEmployee");
                return;
            }
            frmAddUpdateEmployee addnewemployee = new frmAddUpdateEmployee();
            addnewemployee.ShowDialog();
            frmListEmployees_Load(null, null);
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.EditEmployee))
            {
                clsGlobal.PermisionMessage("EditEmployee");
                return;
            }
            frmAddUpdateEmployee UpdateEmployee = new frmAddUpdateEmployee((int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value);
            UpdateEmployee.ShowDialog();
            frmListEmployees_Load(null, null);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteEmployee))
            {
                clsGlobal.PermisionMessage("DeleteEmployee");
                return;
            }

            int EmpID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;

            if (MessageBox.Show($"Are you sure do you want delete this employee with [{EmpID}] ID ? ",
                "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            if(clsEmployee.DeleteEmployeeBy(EmpID))
            {
                MessageBox.Show($"Employee with [{EmpID}] ID is deleted", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show($"Employee with [{EmpID}] ID is not deleted", "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            frmListEmployees_Load(null, null);
        }

        private void showPerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPermisions))
            {
                clsGlobal.PermisionMessage("ShowPermisions");
                return;
            }
            frmShowPermisions ShowPermision = new frmShowPermisions((int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value);
            ShowPermision.ShowDialog();
        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallEmployees))
            {
                clsGlobal.PermisionMessage("PhoneCallEmployees");
                return;
            }

            int EmpID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;
            frmCallPhone callPhone = new frmCallPhone(clsGlobal.CurrentEmployee.EmployeeID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee,
                EmpID, DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee);
            callPhone.ShowDialog();

        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailEmployees))
            {
                clsGlobal.PermisionMessage("SendEmailEmployees");
                return;
            }

            if (string.IsNullOrEmpty(clsGlobal.CurrentEmployee.PersonInfo.Email))
            {
                if (clsValidatoin.WhenEmailError("You don`t have Email", clsGlobal.CurrentEmployee.PersonInfo.PersonID.Value))
                {
                    frmListEmployees_Load(null, null);
                }

                return;
            }

            int EmployeeID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;
            clsEmployee employee = clsEmployee.FindByEmployeeID(EmployeeID);
            if(string.IsNullOrEmpty(employee.PersonInfo.Email))
            {
                if (clsValidatoin.WhenEmailError("This User doesn`t have email", employee.PersonInfo.PersonID.Value))
                {
                    frmListEmployees_Load(null, null);
                }
                return;
            }
            


            frmSendUpdateEmail SendEmail = new frmSendUpdateEmail(clsGlobal.CurrentEmployee.EmployeeID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee,
                EmployeeID, DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee);
            SendEmail.ShowDialog();
            frmListEmployees_Load(null, null);
        }

        private void cmsEmployee_Opening(object sender, CancelEventArgs e)
        {
            int EmpID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;

            clsEmployee EmployeeInfo = clsEmployee.FindByEmployeeID(EmpID);
            if( EmployeeInfo != null )
            {
                showEmailToolStripMenuItem.Enabled = (EmployeeInfo.GetYourEmail(1,1).Rows.Count > 0 || EmployeeInfo.GetYourMessages(1,1).Rows.Count>0);
                callLogToolStripMenuItem.Enabled = 
                    (EmployeeInfo.GetCallLogsBy(clsCallLog.enOrderType.YourCallLog,1,1).Rows.Count > 0 
                    ||
                    EmployeeInfo.GetCallLogsBy(clsCallLog.enOrderType.YourOwnCallLog,1,1).Rows.Count > 0);
            }

        }

        private void showEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowEmployeesEmail))
            {
                clsGlobal.PermisionMessage("ShowEmployeesEmail");
                return;
            }
            int EmployeeID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;
            frmShowEmail showEmail = new frmShowEmail(EmployeeID, DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee);
            showEmail.ShowDialog();


        }

        private void callLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowEmployeesCallLog))
            {
                clsGlobal.PermisionMessage("ShowEmployeesCallLog");
                return;
            }
            int EmployeeID = (int)dgvEmployees.CurrentRow.Cells["EmployeeID"].Value;
            frmShowCallLog showCallLog = new frmShowCallLog(EmployeeID, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee);
            showCallLog.ShowDialog();
        }
    }
}
