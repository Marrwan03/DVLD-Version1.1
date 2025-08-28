using DVLD.Classes;
using DVLD.Controls.ApplicationControls;
using DVLD.Properties;
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
using static DVLD_Buisness.clsTestType;

namespace DVLD.Tests
{
    public partial class frmListTestAppointments: Form
    {

        private DataTable _dtLicenseTestAppointments;
        private int _LocalDrivingLicenseApplicationID ;
        private clsTestType.enTestType _TestType = clsTestType.enTestType.VisionTest;

        public frmListTestAppointments(int LocalDrivingLicenseApplicationID, clsTestType.enTestType TestType)
        {
            InitializeComponent();
            _LocalDrivingLicenseApplicationID= LocalDrivingLicenseApplicationID;
            _TestType = TestType;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void _LoadTestTypeImageAndTitle()
        {
            switch (_TestType)
            {

                case clsTestType.enTestType.VisionTest:
                    {
                        lblTitle.Text = "Vision Test Appointments";
                        this.Text= lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Vision_512;
                        break;
                    }

                case clsTestType.enTestType.WrittenTest:
                    {
                        lblTitle.Text = "Written Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.Written_Test_512;
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        lblTitle.Text = "Street Test Appointments";
                        this.Text = lblTitle.Text;
                        pbTestTypeImage.Image = Resources.driving_test_512;
                        break;
                    }
            }
        }

        void _FilldgvLicenseTestAppointments(int NumberOfPage)
        {
            ctrlSwitchSearchForAppointment.NumberOfPage = NumberOfPage;
            _dtLicenseTestAppointments = clsTestAppointment.
                GetApplicationTestAppointmentsPerTestTypeBy(_LocalDrivingLicenseApplicationID, _TestType, ctrlSwitchSearchForAppointment.NumberOfPage, 3);
            dgvLicenseTestAppointments.DataSource = _dtLicenseTestAppointments;
            lblRecordsCount.Text = dgvLicenseTestAppointments.Rows.Count.ToString();
        }

        private void frmListTestAppointments_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            _LoadTestTypeImageAndTitle();


            ctrlDrivingLicenseApplicationInfo1.LoadApplicationInfoByLocalDrivingAppID(_LocalDrivingLicenseApplicationID);
            ctrlSwitchSearchForAppointment.MaxNumberOfPage = clsGet.GetMaximamPage(clsTestAppointment.
                GetNumberOfRowsForAppTestAppointmentsPerTestTypeBy(_LocalDrivingLicenseApplicationID, _TestType), 3);

            _FilldgvLicenseTestAppointments(1);
            lblMessageWhenAppointmentsListisEmpty.Visible = dgvLicenseTestAppointments.Rows.Count == 0;
            ctrlSwitchSearchForAppointment.Visible = !lblMessageWhenAppointmentsListisEmpty.Visible;

            if (dgvLicenseTestAppointments.Rows.Count>0)
            {
                dgvLicenseTestAppointments.Columns[0].HeaderText = "Appointment ID";
                dgvLicenseTestAppointments.Columns[1].HeaderText = "Appointment Date";
                dgvLicenseTestAppointments.Columns[2].HeaderText = "Total Fees";
                dgvLicenseTestAppointments.Columns[3].HeaderText = "Status";
            }

            dgvLicenseTestAppointments.EnableHeadersVisualStyles = false;
            dgvLicenseTestAppointments.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        private void btnAddNewAppointment_Click(object sender, EventArgs e)
        {

            clsLocalDrivingLicenseApplication localDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);


            if (localDrivingLicenseApplication.IsThereAnActiveScheduledTest( _TestType))
            {
                MessageBox.Show("Person Already have an active appointment for this test, You cannot add new appointment", "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

         

            //---
             clsTest LastTest = localDrivingLicenseApplication.GetLastTestPerTestType( _TestType);

            if (LastTest == null)
            {
                frmScheduleTest frm1 = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType);
                frm1.ShowDialog();
                //refresh
                frmListTestAppointments_Load(null, null);
                return;
            }
            
            //if person already passed the test s/he cannot retak it.
            if (LastTest.TestResult == true)
            {
                MessageBox.Show("This person already passed this test before, you can only retake faild test", "Not Allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmScheduleTest frm2 = new frmScheduleTest
                (LastTest.TestAppointmentInfo.LocalDrivingLicenseApplicationID, _TestType);
            frm2.ShowDialog();
            //refresh
            frmListTestAppointments_Load(null, null);
            //---

        }

        private void editToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if(!clsGlobal.CurrentEmployee.CheckAccessPermision( clsEmployee.enPermision.EditAppointment))
            {
                clsGlobal.PermisionMessage("EditAppointment");
                return;
            }

            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            frmScheduleTest frm = new frmScheduleTest(_LocalDrivingLicenseApplicationID, _TestType, TestAppointmentID);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);

        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;

            frmTakeTest frm = new frmTakeTest(TestAppointmentID, _TestType);
            frm.ShowDialog();
            frmListTestAppointments_Load(null, null);
        }

        private void cmsApplications_Opening(object sender, CancelEventArgs e)
        {

        }

        private void ctrlSwitchSearchForAppointment_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvLicenseTestAppointments(e.CurrentNumberOfPage);
        }

        private void statusToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value;
            clsTestAppointment StatusTestAppointment = clsTestAppointment.Find(TestAppointmentID);
            if (StatusTestAppointment != null)
            {
                string Message = "This Appointment is Locked";
                if (StatusTestAppointment.IsPaid())
                {
                    Message = $"This Appointment is ready to take test For {StatusTestAppointment.LocalDrivingLicenseApplicationInfo.PersonFullName}";

                    if (StatusTestAppointment.RetakeTestApplicationID.HasValue)
                    {
                        if (StatusTestAppointment.RetakeTestAppInfo.IsPaid())
                         Message=   $"This Appointment is ready to take Appointment For {StatusTestAppointment.LocalDrivingLicenseApplicationInfo.PersonFullName}";
                        else
                          Message=  $"This Appointment doesn`t ready to take Appointment For {StatusTestAppointment.LocalDrivingLicenseApplicationInfo.PersonFullName},\nHe has to pay RetakeApplicationFees";
                    }
                }
                else
                    Message = $"This Appointment doesn`t ready to take test For {StatusTestAppointment.LocalDrivingLicenseApplicationInfo.PersonFullName},\nHe has to pay AppointmentFees";


                MessageBox.Show(Message, "Status",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);

                frmTestAppointmentInfo testAppointmentInfo = new frmTestAppointmentInfo(TestAppointmentID);
                testAppointmentInfo.ShowDialog();

            }
            else
            {
                MessageBox.Show($"This Appointment with[{TestAppointmentID}]ID doesn`t found", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                
            }
        }

       
        private void DeleteStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteAppointment))
            {
                clsGlobal.PermisionMessage("DeleteAppointment");
                return;
            }

            if(clsTestAppointment.UpdateStatus((int)dgvLicenseTestAppointments.CurrentRow.Cells[0].Value,
                clsTestAppointment.enStatus.Deleted))
            {
                MessageBox.Show("Test Appointment deleted successfully :-)", "Deleted successfully",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Test Appointment deleted Failed :-(", "Deleted failed",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmListTestAppointments_Load(null, null);

        }
    }
}
