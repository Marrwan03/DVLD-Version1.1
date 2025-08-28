using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using DVLD.Classes;
using DVLD.Communication.Phone;
using DVLD.Email;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ListView;

namespace DVLD.People
{
    public partial class frmListPeople : Form
    {

        //only select the columns that you want to show in the grid
      private DataTable _dtPeople;

        private  void _RefreshPeoplList(int NumberOfPage=1)
        {
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsPerson.GetNumberOfRows(), 5);
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
            _dtPeople = clsPerson.GetPeopleBy(ctrlSwitchSearch1.NumberOfPage, 5);

            dgvPeople.DataSource = _dtPeople;
            lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
        }

        public frmListPeople()
        {
            InitializeComponent();
        }

        private void frmListPeople_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsPerson.GetNumberOfRows(), 5);
            _RefreshPeoplList(1);
            cbFilterBy.SelectedIndex = 0;
            lblWhenPeopleisEmpty.Visible = dgvPeople.Rows.Count == 0;
            ctrlSwitchSearch1.Visible = !lblWhenPeopleisEmpty.Visible;
            if (dgvPeople.Rows.Count > 0)
            {

                dgvPeople.Columns[0].HeaderText = "Person ID";

                dgvPeople.Columns[1].HeaderText = "National No.";


                dgvPeople.Columns[2].HeaderText = "First Name";

                dgvPeople.Columns[3].HeaderText = "Second Name";

                dgvPeople.Columns[4].HeaderText = "Third Name";

                dgvPeople.Columns[5].HeaderText = "Last Name";

                dgvPeople.Columns[6].HeaderText = "Date Of Birth";

                dgvPeople.Columns[7].HeaderText = "Gendor";

                dgvPeople.Columns[8].HeaderText = "Phone";

                dgvPeople.Columns[9].HeaderText = "Email";

                dgvPeople.Columns[10].HeaderText = "Nationality";
            }

            dgvPeople.EnableHeadersVisualStyles = false;
            dgvPeople.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;

        }
    
        private void txtFilterValue_TextChanged(object sender, EventArgs e)
        {
           
            string FilterColumn = "";
            //Map Selected Filter to real Column name 
            switch (cbFilterBy.Text)
            {
                case "Person ID":
                    FilterColumn = "PersonID";
                    break;

                case "National No.":
                    FilterColumn = "NationalNo";
                    break;

                case "First Name":
                    FilterColumn = "FirstName";
                    break;

                case "Second Name":
                    FilterColumn = "SecondName";
                    break;

                case "Third Name":
                    FilterColumn = "ThirdName";
                    break;

                case "Last Name":
                    FilterColumn = "LastName";
                    break;

                case "Nationality":
                    FilterColumn = "CountryName";
                    break;

                case "Gendor":
                    FilterColumn = "GendorCaption";
                    break;

                case "Phone":
                    FilterColumn = "Phone";
                    break;

                case "Email":
                    FilterColumn = "Email";
                    break;

                default:
                    FilterColumn = "None";
                    break;

            }

            //Reset the filters in case nothing selected or filter value conains nothing.
            if (txtFilterValue.Text.Trim() == "" || FilterColumn == "None")
            {
                ctrlSwitchSearch1.Visible = true;
                _dtPeople.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPeople.Rows.Count.ToString();
                return;
            }

            ctrlSwitchSearch1.Visible = false;
            if (FilterColumn == "PersonID")
                //in this case we deal with integer not string.
                
              _dtPeople.DefaultView.RowFilter = string.Format("[{0}] = {1}", FilterColumn, txtFilterValue.Text.Trim());
            else
             _dtPeople.DefaultView.RowFilter = string.Format("[{0}] LIKE '{1}%'", FilterColumn, txtFilterValue.Text.Trim());
       
         lblRecordsCount.Text= dgvPeople.Rows.Count.ToString();

        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {

           txtFilterValue.Visible = (cbFilterBy.Text != "None");
            if (txtFilterValue.Visible)
            {
                txtFilterValue.Text = "";
                txtFilterValue.Focus();
            }

        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonDetails))
            {
                clsGlobal.PermisionMessage("ShowPersonDetails");
                return;
            }
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            Form frm = new frmShowPersonInfo(PersonID);
            frm.ShowDialog();
        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.EditPersonPeople))
            {
                clsGlobal.PermisionMessage("EditPersonPeople");
                return;
            }
            Form frm = new frmAddUpdatePerson((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();

            _RefreshPeoplList();

        }

        bool WhenEmailError(string Message, int PersonID)
        {
                if (MessageBox.Show(Message, "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    if (MessageBox.Show("Do you want add email?", "Add Email", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.EditPersonPeople))
                        {
                            clsGlobal.PermisionMessage("EditPersonPeople");
                            return false;
                        }
                        frmAddUpdatePerson updatePerson = new frmAddUpdatePerson(PersonID);
                        updatePerson.ShowDialog();
                    }

                }
            return true;
        }

        private void sendEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailPeople))
            {
                clsGlobal.PermisionMessage("SendEmailPeople");
                return;
            }

            if (string.IsNullOrEmpty(clsGlobal.CurrentEmployee.PersonInfo.Email))
            {
                if(clsValidatoin.WhenEmailError("You don`t have Email", clsGlobal.CurrentEmployee.PersonInfo.PersonID.Value) )
                {
                    _RefreshPeoplList();
                }

            }

            clsPerson PersonInfo = clsPerson.Find((int)dgvPeople.CurrentRow.Cells[0].Value);
            if(PersonInfo != null)
            {
                if (PersonInfo.Email == null)
                {
                    if (clsValidatoin.WhenEmailError("This Person doesn`t have email", PersonInfo.PersonID.Value))
                    {
                        _RefreshPeoplList();
                    }
                    return;
                }
            }
            else
            {
                MessageBox.Show("This Person is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmSendUpdateEmail SendEmail = new frmSendUpdateEmail
                (clsGlobal.CurrentEmployee.EmployeeID.Value, clsEmail.enFrom.ByEmployee,
                PersonInfo.PersonID.Value, clsEmail.enFor.ForPerson);

            SendEmail.ShowDialog();

        }

        private void phoneCallToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallPeople))
            {
                clsGlobal.PermisionMessage("PhoneCallPeople");
                return;
            }
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmCallPhone callPhone = new frmCallPhone(clsGlobal.CurrentEmployee.EmployeeID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee,
                PersonID, DVLD_Buisness.Classes.clsCommunication.enFor.ForPerson);
            

            callPhone.ShowDialog();

        }

        private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DeletePersonPeople))
            {
                clsGlobal.PermisionMessage("DeletePersonPeople");
                return;
            }
            if (MessageBox.Show("Are you sure you want to delete Person [" + dgvPeople.CurrentRow.Cells[0].Value + "]", "Confirm Delete", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.OK)

            {
                clsPerson _Person = clsPerson.Find( (int)dgvPeople.CurrentRow.Cells[0].Value  );
                //Perform Delele and refresh
                if (clsPerson.DeletePerson((int)dgvPeople.CurrentRow.Cells[0].Value))
                {
                    clsUtil.DeleteOldImageFrom(_Person.ImagePath);
                    MessageBox.Show("Person Deleted Successfully.", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    _RefreshPeoplList();
                }

                else
                    MessageBox.Show("Person was not deleted because it has data linked to it.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }

        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewPerson))
            {
                clsGlobal.PermisionMessage("AddNewPerson");
                return;
            }
            Form frm = new frmAddUpdatePerson();
            frm.ShowDialog();

            _RefreshPeoplList();
        }

        private void btnAddPerson_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewPerson))
            {
                clsGlobal.PermisionMessage("AddNewPerson");
                return;
            }
            Form frm1 = new frmAddUpdatePerson();
            frm1.ShowDialog();
            _RefreshPeoplList();
        }

        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void dgvPeople_DoubleClick(object sender, EventArgs e)
        {
            Form frm = new frmShowPersonInfo((int)dgvPeople.CurrentRow.Cells[0].Value);
            frm.ShowDialog();
        }

        private void txtFilterValue_KeyPress(object sender, KeyPressEventArgs e)
        {
            //we allow number incase person id is selected.
            if (cbFilterBy.Text=="Person _PersonID")
              e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void cmsPeople_Opening(object sender, CancelEventArgs e)
        {
            bool EmailCondition=false;
            bool PhoneCondition=false;
            clsPerson Person = clsPerson.Find((int)dgvPeople.CurrentRow.Cells[0].Value);
            if (Person != null)
            {
                EmailCondition = Person.GetYourEmail(1, 1).Rows.Count > 0 || Person.GetYourMessages(1, 1).Rows.Count > 0;
                PhoneCondition = Person.GetCallLogsBy( clsCallLog.enOrderType.YourCallLog,1,1).Rows.Count > 0 || Person.GetCallLogsBy(clsCallLog.enOrderType.YourOwnCallLog, 1, 1).Rows.Count > 0;

            }
            showEmailToolStripMenuItem.Enabled = EmailCondition;
            callLogToolStripMenuItem.Enabled = PhoneCondition;

        }

        private void showEmailToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonEmail))
            {
                clsGlobal.PermisionMessage("ShowPersonEmail");
                return;
            }

            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmShowEmail showEmail = new frmShowEmail(PersonID, clsEmail.enFor.ForPerson, clsEmail.enFrom.ByPerson);


            showEmail.ShowDialog();

        }

        private void callLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserEmail))
            {
                clsGlobal.PermisionMessage("ShowUserEmail");
                return;
            }
            int PersonID = (int)dgvPeople.CurrentRow.Cells[0].Value;
            frmShowCallLog showCallLog = new frmShowCallLog(PersonID, DVLD_Buisness.Classes.clsCommunication.enFrom.ByPerson);
            
            showCallLog.ShowDialog();

        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _RefreshPeoplList(e.CurrentNumberOfPage);
        }
    }
}
