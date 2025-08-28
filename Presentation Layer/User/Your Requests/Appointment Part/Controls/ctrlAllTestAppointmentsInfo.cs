using DVLD.Applications;
using DVLD.Classes;
using DVLD.Payments;
using DVLD.Tests;
using DVLD.Tests.Vision_Test;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User.Your_Requests.Appointment_Part.Controls
{
    public partial class ctrlAllTestAppointmentsInfo : UserControl
    {
        public ctrlAllTestAppointmentsInfo()
        {
            InitializeComponent();
        }
        int _PersonID;
        DataTable _dtYourAllTestAppointment;
        void _FilldgvYourAllTestAppointment(int PageNumber)
        {
            ctrlSwitchSearch1.NumberOfPage = PageNumber;
            _dtYourAllTestAppointment = clsTestAppointment.GetApplicationTestAppointmentsBy(_PersonID, PageNumber, 8);
            lblNote.Visible = _dtYourAllTestAppointment.Rows.Count == 0;
            lblNumberOfRecord.Text = _dtYourAllTestAppointment.Rows.Count.ToString();
            dgvYourAllTestAppointment.DataSource = _dtYourAllTestAppointment;
            if(dgvYourAllTestAppointment.Rows.Count > 0)
            {
                dgvYourAllTestAppointment.Columns[0].HeaderText = "Test App ID";
                dgvYourAllTestAppointment.Columns[0].Width = 50;
                dgvYourAllTestAppointment.Columns[1].HeaderText = "Class Name";
                dgvYourAllTestAppointment.Columns[2].HeaderText = "Test Type";
                dgvYourAllTestAppointment.Columns[3].HeaderText = "App Date";
                dgvYourAllTestAppointment.Columns[4].HeaderText = "Retake Test App ID";
                dgvYourAllTestAppointment.Columns[4].Width = 95;
                dgvYourAllTestAppointment.Columns[5].HeaderText = "Paid Fees";
                dgvYourAllTestAppointment.Columns[5].Width = 75;
                dgvYourAllTestAppointment.Columns[6].HeaderText = "Payment ID";
                dgvYourAllTestAppointment.Columns[7].HeaderText = "Status";
                dgvYourAllTestAppointment.Columns[7].Width = 75;
                dgvYourAllTestAppointment.Columns[8].HeaderText = "Test Result";
                dgvYourAllTestAppointment.Columns[8].Width = 75;
                dgvYourAllTestAppointment.Columns[9].HeaderText = "Create By";
                dgvYourAllTestAppointment.Columns[9].Width = 75;
            }
        }

        void _RefreshData()
        {
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsTestAppointment.GetNumberOfRowsForAppTestAppointmentsBy(_PersonID), 8);
            _FilldgvYourAllTestAppointment(1);
        }

        public void ctrlAllTestAppointmentInfo_Load(int PersonID)
        {
            _PersonID = PersonID;
            _RefreshData();
            if(lblNote.Visible)
            {
                pFilter.Enabled = false;
                pSort.Enabled = false;
            }

            cbUserCards1.Fill_cbUserCardsBy(clsUser.FindByPersonID(PersonID).UserID.Value);

            if(cbUserCards1.Items.Count > 0)
            {
                cbUserCards1.SelectedIndex = 0;
            }
            else
            {
                cbUserCards1.Text = "Empty";
                payItToolStripMenuItem.Enabled = false;
            }

        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvYourAllTestAppointment(e.CurrentNumberOfPage);
        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (cbFilterBy.Text != "None" && cbFilterBy.Text != "IsLocked")
                e.Handled = !char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar);
        }

        private void cbSortByTestType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlSwitchSearch1.Visible = cbSortByTestType.Text == "All";
            if (ctrlSwitchSearch1.Visible)
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = "";
                _FilldgvYourAllTestAppointment(1);
            }
            else
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "TestTypeTitle", cbSortByTestType.Text);
            }
        }

        void _VisibleControlsFor(bool VisiblecbIsLocked, bool VisibletxtFilter)
        {
            cbIsLocked.Visible = VisiblecbIsLocked;
            txtFilter.Visible = VisibletxtFilter;
        }

        private void cbFilterBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFilterBy.Text)
            {
                case "None":
                    {
                        _VisibleControlsFor(false, false);
                        if(_dtYourAllTestAppointment != null)
                        {
                        _dtYourAllTestAppointment.DefaultView.RowFilter = "";
                        _FilldgvYourAllTestAppointment(1);
                        }
                        txtFilter.Text = null;
                        break;
                    }
                case "Is Locked":
                    {
                        _VisibleControlsFor(true, false);
                        break;
                    }
                    default:
                    {
                        _VisibleControlsFor(false, true);
                        break;
                    }
            }

        }

        private void cbIsLocked_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool Visible = cbIsLocked.Text == "All";
            ctrlSwitchSearch1.Visible = Visible;

            if(Visible)
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = "";
                _FilldgvYourAllTestAppointment(1);
                return;
            }
            string ValueOfLocked="";
            switch(cbIsLocked.Text)
            {
                case "Yes":
                    {
                        ValueOfLocked = "1";
                        break;
                    }
                case "No":
                    {
                        ValueOfLocked = "0";
                        break;
                    }
            }
            _dtYourAllTestAppointment.DefaultView.RowFilter = string.Format("[{0}] = {1}", "IsLocked", ValueOfLocked);
            lblNumberOfRecord.Text = dgvYourAllTestAppointment.Rows.Count.ToString();
        }

        private void txtFilter_TextChanged(object sender, EventArgs e)
        {
            string FilterColumn = "";
            switch (cbFilterBy.Text)
            {
                case "Test App ID":
                    {
                        FilterColumn = "TestAppointmentID";
                        break;
                    }
                case "Retake App ID":
                    {
                        FilterColumn = "RetakeTestApplicationID";
                        break;
                    }
                case "Payment ID":
                    {
                        FilterColumn = "PaymentID";
                        break;
                    }
                default:
                    {
                        FilterColumn = "None";
                        break;
                    }
            }
            ctrlSwitchSearch1.Visible = FilterColumn == "None";
            if(ctrlSwitchSearch1.Visible || string.IsNullOrWhiteSpace(txtFilter.Text))
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = "";
                _FilldgvYourAllTestAppointment(1);
            }
            else
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", FilterColumn, txtFilter.Text.Trim());
                lblNumberOfRecord.Text = dgvYourAllTestAppointment.Rows.Count.ToString();
            }

        }

        private void rbFilter_CheckedChanged(object sender, EventArgs e)
        {
            pFilter.Visible = rbFilter.Checked;
            pSort.Visible = rbSort.Checked;
            if(pSort.Visible)
            {
                rbClassType.Checked = true;
                cbSortByClassType.SelectedIndex = 0;
                cbSortByTestType.SelectedIndex = 0;
            }
                
        }

        private void ctrlAllTestAppointmentInfo_Load(object sender, EventArgs e)
        {
            rbFilter.Checked = true;
            cbFilterBy.SelectedIndex = 0;

            dgvYourAllTestAppointment.EnableHeadersVisualStyles = false;
            dgvYourAllTestAppointment.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
        }

        private void cbSortByClassType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ctrlSwitchSearch1.Visible = cbSortByClassType.Text == "All";
            if (ctrlSwitchSearch1.Visible)
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = "";
                _FilldgvYourAllTestAppointment(1);
            }
            else
            {
                _dtYourAllTestAppointment.DefaultView.RowFilter = string.Format("[{0}] = '{1}'", "ClassName", cbSortByClassType.Text);
                lblNumberOfRecord.Text = dgvYourAllTestAppointment.Rows.Count.ToString();
            }
        }

        private void rbClassType_CheckedChanged(object sender, EventArgs e)
        {
            cbSortByClassType.Visible = rbClassType.Checked;
            cbSortByTestType.Visible = rbTestType.Checked;
           
        }

        clsLocalDrivingLicenseApplication _CurrentLocalDrivingLicenseApplication;
        private void cmsAllTestAppointmentInfo_Opening(object sender, CancelEventArgs e)
        {
            payItToolStripMenuItem.Enabled = dgvYourAllTestAppointment.CurrentRow.Cells[6].Value == DBNull.Value;
            showBillInfoToolStripMenuItem.Enabled = !payItToolStripMenuItem.Enabled;
            takeTestToolStripMenuItem.Enabled = !payItToolStripMenuItem.Enabled && dgvYourAllTestAppointment.CurrentRow.Cells[7].Value.ToString() == "Not Locked";
            testInfoToolStripMenuItem.Enabled = !takeTestToolStripMenuItem.Enabled && dgvYourAllTestAppointment.CurrentRow.Cells[7].Value.ToString() == "Locked";
            retakeAppInfoToolStripMenuItem.Enabled = dgvYourAllTestAppointment.CurrentRow.Cells[4].Value != DBNull.Value;
             _CurrentLocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID
                (clsTestAppointment.Find((int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value).LocalDrivingLicenseApplicationID);

            issueNewLicenseToolStripMenuItem.Enabled =
               _CurrentLocalDrivingLicenseApplication.PassedAllTests()
                && !_CurrentLocalDrivingLicenseApplication.IsLicenseIssued(); 
        }

        private void payItToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (cbUserCards1.Items.Count == -1)
            {
                MessageBox.Show("You don`t have any creditcard to continue,\nyou should create new Account in Bank system", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            frmPaymentOf paymentOfAppointment = new frmPaymentOf
                ((int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value,
                (float)((decimal)dgvYourAllTestAppointment.CurrentRow.Cells[5].Value),
                Payments.Controls.ctrlPaymentFor.enPaymentFor.Appointment,
                clsCreditCard.GetTypeOfCreditCard(cbUserCards1.Text));

            paymentOfAppointment.ShowDialog();
            _RefreshData();
        }

        private void showAppointmenInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value;
            frmTestAppointmentInfo testAppointmentInfo = new frmTestAppointmentInfo(TestAppointmentID);
            testAppointmentInfo.ShowDialog();
        }

        private void takeTestToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppointmentID = (int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value;
            string TestType = (string)dgvYourAllTestAppointment.CurrentRow.Cells[2].Value;
            frmTakeTest takeTest = null;
            switch (TestType)
            {
                case "Vision Test":
                    {
                        takeTest = new frmTakeTest(TestAppointmentID, clsTestType.enTestType.VisionTest);
                        break;
                    }
                case "Written (Theory) Test":
                    {
                        takeTest = new frmTakeTest(TestAppointmentID, clsTestType.enTestType.WrittenTest);
                        break;
                    }
                case "Practical (Street) Test":
                    {
                        takeTest = new frmTakeTest(TestAppointmentID, clsTestType.enTestType.StreetTest);
                        break;
                    }
            }
            takeTest.ShowDialog();
            _RefreshData();
        }

        private void showBillInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int PaymentID = (int)dgvYourAllTestAppointment.CurrentRow.Cells[6].Value;
            frmPaymentInfo paymentInfo = new frmPaymentInfo(PaymentID);
            paymentInfo.ShowDialog();
        }

        private void picAddNewAppointment_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure do want add new Appointment?", "Confirm!",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            frmAddNewRequestForAppointment addNewRequestForAppointment = new frmAddNewRequestForAppointment(clsUser.FindByPersonID(_PersonID).UserID.Value);
            addNewRequestForAppointment.ShowDialog();
            

        }

        private void testInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int TestAppID = (int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value;
            int? TestID = clsTestAppointment.Find(TestAppID).TestID;
            if(TestID.HasValue)
            {
                frmTestInfo testInfo = new frmTestInfo(TestID.Value);
                testInfo.ShowDialog();
            }
            else
            {
                MessageBox.Show($"we cannot find any test with your TestAppID[{TestAppID}]",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void retakeAppInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RetakeAppID = (int)dgvYourAllTestAppointment.CurrentRow.Cells[4].Value;

            frmApplicationBasicInfo applicationBasicInfo = new frmApplicationBasicInfo(RetakeAppID);
            applicationBasicInfo.ShowDialog();

        }

        public class clsIssueLicenseOrder : EventArgs
        {
            public int UserID { get; set; }
            public int LDLAppID { get; }
            public DateTime DateTime { get; }

            public clsIssueLicenseOrder(int userID, int lDLAppID, DateTime dateTime)
            {
                UserID = userID;
                LDLAppID = lDLAppID;
                DateTime = dateTime;
            }

            public override string ToString()
            {
                return $"{UserID}#//#{LDLAppID}#//#{this.DateTime.ToString("f")}";
            }

            public static clsIssueLicenseOrder FromString(string str)
            {
                string[]result = str.Split(new string[] { "#//#"}, StringSplitOptions.None);
                return new clsIssueLicenseOrder(Convert.ToInt32(result[0]), Convert.ToInt32(result[1]), Convert.ToDateTime(result[2]));
            }

        }
        public event EventHandler<clsIssueLicenseOrder> OnIssueLicense;

        private void issueNewLicenseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(clsUtil.IsUserHasIssueLicenseOrderBy(
                clsUser.FindByPersonID(_CurrentLocalDrivingLicenseApplication.ApplicationInfo.ApplicantPersonID)
                .UserID.Value))
            {
                MessageBox.Show("You already have an order, Please wait for a few minutes.","Wait",MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            OnIssueLicense?.Invoke(this, new clsIssueLicenseOrder(clsUser.FindByPersonID(_PersonID).UserID.Value,
                clsTestAppointment.Find((int)dgvYourAllTestAppointment.CurrentRow.Cells[0].Value).LocalDrivingLicenseApplicationID, DateTime.Now));

        }
    }
}
