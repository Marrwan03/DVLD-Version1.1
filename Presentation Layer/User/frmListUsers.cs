using DVLD.Classes;
using DVLD.Communication.Phone;
using DVLD.Email;
using DVLD.People;
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

namespace DVLD.Employee
{
    public partial class frmListUsers : Form
    {
        private static DataTable _dtAllUsers ;

        public frmListUsers()
        {
            InitializeComponent();
        }
        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        void _FilldgvUsers(int NumberOfPage)
        {
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
           
            _dtAllUsers = clsUser.GetUsersBy(NumberOfPage, 5);
            dgvUsers.DataSource = _dtAllUsers;
            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void frmListUsers_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsUser.GetNumberOfRows(), 5);
            _FilldgvUsers(1);
            cbFilterBy.SelectedIndex = 0;
            lblWhenUsersListareEmpty.Visible = dgvUsers.Rows.Count == 0;
            ctrlSwitchSearch1.Visible = !lblWhenUsersListareEmpty.Visible;

            if (dgvUsers.Rows.Count > 0)
            {
                dgvUsers.Columns[0].HeaderText = "User ID";
                dgvUsers.Columns[1].HeaderText = "Person ID";
                dgvUsers.Columns[2].HeaderText = "Full Name";
                dgvUsers.Columns[3].HeaderText = "UserName";
                dgvUsers.Columns[4].HeaderText = "Status";
            }

            dgvUsers.EnableHeadersVisualStyles = false;
            dgvUsers.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (cbFilterBy.Text == "Status")
            {
                txtFilterValue.Visible= false;
                cbIsActive.Visible = true;
                cbIsActive.Focus();
                cbIsActive.SelectedIndex = 0;
            } 
            
            else
            {                
                txtFilterValue.Visible = (cbFilterBy.Text !="None") ;
                cbIsActive.Visible = false;
                if (cbFilterBy.Text == "None")
                    txtFilterValue.Enabled = false;
                else
                    txtFilterValue.Enabled = true;

                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }
        }

        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "User ID":
                    FilterColumn = "UserID";
                    break;
                case "UserName":
                    FilterColumn = "UserName";
                    break;

                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

        
                case "Full Name":
                    FilterColumn = "FullName";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                ctrlSwitchSearch1.Visible = true;
                _dtAllUsers.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
                return;
            }
            ctrlSwitchSearch1.Visible = false;
            if (FilterColumn != "FullName" && FilterColumn != "UserName")
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());

            lblRecordsCount.Text = dgvUsers.Rows.Count.ToString();
        }

        private void cbIsActive_SelectedIndexChanged(object sender, EventArgs e)
        {

             
          string FilterColumn = "Status";
          string FilterValue =cbIsActive.Text;

            if (FilterValue == "All")
            {
                _dtAllUsers.DefaultView.RowFilter = "";
                ctrlSwitchSearch1.Visible = true;
            }
            else
            {
                //in this case we deal with numbers not string.
                _dtAllUsers.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, FilterValue);
                ctrlSwitchSearch1.Visible = false;
            }

            lblRecordsCount.Text = _dtAllUsers.Rows.Count.ToString();


        }

        private void btnAddUser_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewUser))
            {
                clsGlobal.PermisionMessage("AddNewUser");
                return;
            }
            frmAddUpdateUser Frm1 = new frmAddUpdateUser ();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);  
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.EditUser))
            {
                clsGlobal.PermisionMessage("EditUser");
                return;
            }
            frmAddUpdateUser Frm1 = new frmAddUpdateUser((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewUser))
            {
                clsGlobal.PermisionMessage("AddNewUser");
                return;
            }
            frmAddUpdateUser Frm1 = new frmAddUpdateUser();
            Frm1.ShowDialog();
            frmListUsers_Load(null, null);

        }

        private void dgvUsers_CellContentDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
           
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserDetails))
            {
                clsGlobal.PermisionMessage("ShowUserDetails");
                return;
            }
            frmUserInfo Frm1 = new frmUserInfo((int)dgvUsers.CurrentRow.Cells[0].Value);
            Frm1.ShowDialog();
           
        }

        private void ChangePasswordtoolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ChangePasswordUser))
            {
                clsGlobal.PermisionMessage("ChangePasswordUser");
                return;
            }
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmChangePassword Frm1 = new frmChangePassword(UserID);
            Frm1.ShowDialog();

        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id or user id is selected.
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "User ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteUser))
            {
                clsGlobal.PermisionMessage("DeleteUser");
                return;
            }
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            if (clsUser.DeleteUser(UserID))
            {
                MessageBox.Show("User has been deleted successfully", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                frmListUsers_Load(null, null);
            }

            else
                MessageBox.Show("User is not delted due to data connected to it.", "Faild", MessageBoxButtons.OK, MessageBoxIcon.Error);


            


        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailUsers))
            {
                clsGlobal.PermisionMessage("SendEmailUsers");
                return;
            }

            if(string.IsNullOrEmpty( clsGlobal.CurrentEmployee.PersonInfo.Email))
            {
              if(  clsValidatoin.WhenEmailError("You don`t have Email", clsGlobal.CurrentEmployee.PersonInfo.PersonID.Value))
                {
                    frmListUsers_Load(null, null);
                }

                return;
            }

            clsUser UserInfo = clsUser.FindByUserID((int)dgvUsers.CurrentRow.Cells[0].Value);
            if (UserInfo.PersonInfo.Email == null)
            {
                if (clsValidatoin.WhenEmailError("This User doesn`t have email", UserInfo.PersonInfo.PersonID.Value))
                {
                    frmListUsers_Load(null, null);
                }
                return;
            }

           

            frmSendUpdateEmail sendEmail = new frmSendUpdateEmail(clsGlobal.CurrentEmployee.EmployeeID.Value, clsEmail.enFrom.ByEmployee,
                (int)dgvUsers.CurrentRow.Cells[0].Value, clsEmail.enFor.ForUser);
            sendEmail.ShowDialog();


        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallUsers))
            {
                clsGlobal.PermisionMessage("PhoneCallUsers");
                return;
            }
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmCallPhone callPhone = new frmCallPhone(clsGlobal.CurrentEmployee.EmployeeID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee,
                UserID, DVLD_Buisness.Classes.clsCommunication.enFor.ForUser);
            callPhone.ShowDialog();
        }

        private void showEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserEmail))
            {
                clsGlobal.PermisionMessage("ShowUserEmail");
                return;
            }

            frmShowEmail showEmail = new frmShowEmail((int)dgvUsers.CurrentRow.Cells[0].Value, clsEmail.enFor.ForUser, clsEmail.enFrom.ByUser);
            showEmail.ShowDialog();

        }

        private void cmsUsers_Opening(object sender, CancelEventArgs e)
        {
            clsUser User = clsUser.FindByUserID((int)dgvUsers.CurrentRow.Cells[0].Value);
            if(User != null)
            {
                showEmailToolStripMenuItem.Enabled = (User.GetYourEmail(1,1).Rows.Count > 0 || User.GetYourMessages(1,1).Rows.Count > 0);
                callLogToolStripMenuItem.Enabled = (User.GetCallLogsBy(clsCallLog.enOrderType.YourOwnCallLog, 1, 1).Rows.Count > 0 
                    || User.GetCallLogsBy(clsCallLog.enOrderType.YourCallLog,1,1).Rows.Count > 0);
            }
           

        }

        private void callLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserCallLog))
            {
                clsGlobal.PermisionMessage("ShowUserCallLog");
                return;
            }
            int UserID = (int)dgvUsers.CurrentRow.Cells[0].Value;
            frmShowCallLog showCallLog = new frmShowCallLog(UserID, DVLD_Buisness.Classes.clsCommunication.enFrom.ByUser);
            showCallLog.ShowDialog();
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvUsers(e.CurrentNumberOfPage);
        }
    }
}
