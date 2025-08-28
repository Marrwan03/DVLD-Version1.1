using DVLD.Applications;
using DVLD.Classes;
using DVLD.Employee;
using DVLD.People;
using DVLD.Tests;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using DVLD_Buisness.Interfaces;
using System;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Windows.Forms;
using static iText.StyledXmlParser.Jsoup.Select.Evaluator;

namespace DVLD.Archive.Controls
{
    public partial class ctrlArchiveFor : UserControl
    {
        public ctrlArchiveFor()
        {
            InitializeComponent();
        }
        enArchiveType _archiveType;
        public enum enArchiveType { Applications,Appointments, People, Users, Employees}
        DataTable _dtArchive;

        DataTable _GetdtArchive(int PageNumber, int RowPerPage)
        {
            switch (_archiveType)
            {
                case enArchiveType.Applications:
                    return clsApplication.GetArchiveOfAllApplicationsBy(PageNumber, RowPerPage);
                case enArchiveType.Appointments:
                    return clsTestAppointment.GetArchiveOfAllAppointmentsBy(PageNumber, RowPerPage);
                case enArchiveType.People:
                    return clsPerson.GetArchiveOfAllPeopleBy(PageNumber, RowPerPage);
                case enArchiveType.Users:
                    return clsUser.GetArchiveOfAllUsersBy(PageNumber, RowPerPage);
                case enArchiveType.Employees:
                    return clsEmployee.GetArchiveOfAllEmployeesBy(PageNumber, RowPerPage);
                default:
                    return null;
            }
        }

        void _FilldgvArchive(int PageNumber)
        {
            ctrlSwitchSearch1.NumberOfPage = PageNumber;
            _dtArchive = _GetdtArchive(PageNumber, 5);

            if(_dtArchive==null || ctrlSwitchSearch1.NumberOfPage==0)
            {
                lblNote.Visible = true;
                lblNumberOfRecord.Text = "0";
                dgvArchive.DataSource = null; 
                ctrlSwitchSearch1.Visible = false;
            }
            else
            {
                lblNote.Visible = _dtArchive.Rows.Count == 0;
                ctrlSwitchSearch1.Visible = !lblNote.Visible;
                lblNumberOfRecord.Text = _dtArchive.Rows.Count.ToString();
                dgvArchive.DataSource = _dtArchive;
            }
        }
        int _GetNumberOfRows()
        {
            switch (_archiveType)
            {
                case enArchiveType.Applications:
                    return clsApplication.GetNumberOfRowsForApplicationsArchive();
                case enArchiveType.Appointments:
                    return clsTestAppointment.GetNumberOfRowsForAppointmentsArchive();
                case enArchiveType.People:
                    return clsPerson.GetNumberOfRowsForPeopleArchive();
                case enArchiveType.Users:
                    return clsUser.GetNumberOfRowsForUsersArchive();
                case enArchiveType.Employees:
                    return clsEmployee.GetNumberOfRowsForEmployeesArchive();
            }
            return 0;
        }

        public void ctrlArchiveForPeople_Load()
        {
            cbArchiveType.SelectedIndex = 0;
        }

        private void ctrlArchiveForPeople_Load(object sender, EventArgs e)
        {
            dgvArchive.EnableHeadersVisualStyles = false;
            dgvArchive.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvArchive(e.CurrentNumberOfPage);
        }

        private void cbArchiveType_SelectedIndexChanged(object sender, EventArgs e)
        {
            _archiveType = (enArchiveType)cbArchiveType.SelectedIndex;
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(_GetNumberOfRows(), 5);
            _FilldgvArchive(1);
        }

        IReturn _GetReturnRecord(int ID)
        {
            switch (_archiveType)
            {
                case enArchiveType.Applications:
                    return clsApplication.FindBaseApplication(ID);
                case enArchiveType.Appointments:
                    return clsTestAppointment.Find(ID);
                case enArchiveType.People:
                    return clsPerson.Find(ID);
                case enArchiveType.Users:
                    return clsUser.FindByUserID(ID);
                default:
                    return clsEmployee.FindByEmployeeID(ID);

            }
        }

        private void returnToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReturnTheArchives))
            {
                clsGlobal.PermisionMessage("ReturnTheArchives");
                return;
            }

            int ID = (int)dgvArchive.CurrentRow.Cells[0].Value;
            if (MessageBox.Show($"Are you sure do you want return this record for {cbArchiveType.Text} ID: [{ID.ToString()}]", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            IReturn @return = _GetReturnRecord(ID);
           if( !@return.Return())
            {
                MessageBox.Show("Return this record was failed", "Failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
                
            }
            MessageBox.Show("Return this record was successfully", "Successfully",
                     MessageBoxButtons.OK, MessageBoxIcon.Information);
            cbArchiveType_SelectedIndexChanged(null, null);
        }

        Form _GetFormArchiveBy(int ID)
        {
            switch (_archiveType)
            {
                case enArchiveType.Applications:
                    return new frmApplicationBasicInfo(ID);
                case enArchiveType.Appointments:
                    return new frmTestAppointmentInfo(ID);
                case enArchiveType.People:
                    return new frmShowPersonInfo(ID);
                case enArchiveType.Users:
                    return new frmUserInfo(ID);
                default:
                    return new frmEmployeeInfo(ID);

            }
        }

        private void moreDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int ID = (int)dgvArchive.CurrentRow.Cells[0].Value;
          var frmDetails=  _GetFormArchiveBy (ID);
            frmDetails.Show();
            

        }

        private void cmsOpArchives_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            cmsOpArchives.Enabled = !lblNote.Visible;
        }
    }
}
