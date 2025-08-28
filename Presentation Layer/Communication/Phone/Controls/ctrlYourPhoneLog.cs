using DVLD.Classes;
using DVLD.Employee;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Buisness.Classes;
using DVLD_Buisness.Global_Classes;
using Org.BouncyCastle.Cms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Communication.Phone.Controls
{
    public partial class ctrlYourPhoneLog : UserControl
    {
        public ctrlYourPhoneLog()
        {
            InitializeComponent();
        }

        void _FillPhoneLogFor(clsCommunication.enFor For,clsCallLog.enOrderType Type, int CallerID, clsCommunication.enFrom CallerType,
            int RecipientID, clsCommunication.enFor RecipientType,
            ref int MaxNumberOfPage, ref DataTable dt, int NumberOfPage)
        
        {
            switch (For)
            {
                case DVLD_Buisness.Classes.clsCommunication.enFor.None:
                    break;
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForPerson:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForCallLog(Type, CallerID, CallerType
                            , RecipientID, RecipientType));

                        if (Type == clsCallLog.enOrderType.YourOwnCallLog)
                            dt = clsPerson.GetCallLogsBy(Type,RecipientID, NumberOfPage, 3);
                        else
                            dt = clsPerson.GetCallLogsBy(Type,CallerID, NumberOfPage, 3);

                        break;
                    }
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForUser:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForCallLog(Type, CallerID, CallerType
                            , RecipientID, RecipientType));

                        if (Type == clsCallLog.enOrderType.YourOwnCallLog)
                            dt = clsUser.GetCallLogsBy(Type, RecipientID, NumberOfPage, 3);
                        else
                            dt = clsUser.GetCallLogsBy(Type, CallerID, NumberOfPage, 3);
                        break;
                    }
                    
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForCallLog(Type, CallerID, CallerType
                            , RecipientID, RecipientType));

                        if (Type == clsCallLog.enOrderType.YourOwnCallLog)
                            dt = clsEmployee.GetCallLogsBy(Type, RecipientID, NumberOfPage, 3);
                        else
                            dt = clsEmployee.GetCallLogsBy(Type, CallerID, NumberOfPage, 3);

                        break;
                    }
                default:
                    break;
            }
        }

        void _FillYourPhoneLogFor(clsCallLog.enFrom From,int CallerID, int NumberOfPage)
        {
            DataTable dt = new DataTable();
            int MaxNumberOfPage = 0;

            _FillPhoneLogFor(_RecipientType, clsCallLog.enOrderType.YourCallLog,
                CallerID, From, 0, _RecipientType, ref MaxNumberOfPage, ref dt, NumberOfPage);

            ctrlSwitchSearchYourPhoneLog.MaxNumberOfPage = MaxNumberOfPage;
            ctrlSwitchSearchYourPhoneLog.NumberOfPage = NumberOfPage;

            lblNumerOfRecordYourPhoneLog.Text = dt.Rows.Count.ToString();
            lblMessageWhenYourPhoneLogEmpty.Visible = dt.Rows.Count == 0;
            ctrlSwitchSearchYourPhoneLog.Visible = !lblMessageWhenYourPhoneLogEmpty.Visible;

            if (dt.Rows.Count > 0)
            {
                DataTable dtYourPhoneLog = dt.DefaultView
                .ToTable(false, "CallID", "RecipientID", "RecipientType",
                "CallTime", "PhoneNumber",
                "CallType", "Status");
                dgvYourPhoneLog.DataSource = dtYourPhoneLog;

                dgvYourPhoneLog.Columns[0].HeaderText = "Call ID";
                dgvYourPhoneLog.Columns[1].HeaderText = "Recipient ID";
                dgvYourPhoneLog.Columns[2].HeaderText = "Recipient Type";
                dgvYourPhoneLog.Columns[3].HeaderText = "Call Time";
                dgvYourPhoneLog.Columns[4].HeaderText = "PhoneNumber";
                dgvYourPhoneLog.Columns[5].HeaderText = "Call Type";
                dgvYourPhoneLog.Columns[6].HeaderText = "Status";
            }
            else
            {
                dgvYourPhoneLog.DataSource = null;
            }

            dgvYourPhoneLog.EnableHeadersVisualStyles = false;
            dgvYourPhoneLog.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }
        void _FillYourOwnPhoneLogFor(clsCallLog.enFor For, int RecipientID, int NumberOfPage)
        {
            DataTable dt = new DataTable();
            int MaxNumberOfPage = 0;

            _FillPhoneLogFor(For, clsCallLog.enOrderType.YourOwnCallLog, 0, clsCommunication.enFrom.None, RecipientID, For, ref MaxNumberOfPage, ref dt, NumberOfPage);
            ctrlSwitchSearchYourOwnPhoneLog.MaxNumberOfPage = MaxNumberOfPage;
            ctrlSwitchSearchYourOwnPhoneLog.NumberOfPage = NumberOfPage;

            ctrlSwitchSearchYourOwnPhoneLog.NumberOfPage = NumberOfPage;
            lblNumerOfRecordYourOwnPhoneLog.Text = dt.Rows.Count.ToString();
            lblMessageWhenYourOwnPhoneLogEmpty.Visible = dt.Rows.Count == 0;

            ctrlSwitchSearchYourOwnPhoneLog.Visible = dt.Rows.Count > 0;

            if (dt.Rows.Count > 0)
            {
                DataTable dtYourOwnPhoneCall = dt.DefaultView.
                    ToTable(false, "CallID", "CallerID", "CallerType",
                    "PhoneNumber", "CallType", "Status");
                dgvYourOwnPhonelog.DataSource = dtYourOwnPhoneCall;

                dgvYourOwnPhonelog.Columns[0].HeaderText = "Call ID";
                dgvYourOwnPhonelog.Columns[1].HeaderText = "Caller ID";
                dgvYourOwnPhonelog.Columns[2].HeaderText = "Caller Type";
                dgvYourOwnPhonelog.Columns[3].HeaderText = "PhoneNumber";
                dgvYourOwnPhonelog.Columns[4].HeaderText = "CallType";
                dgvYourOwnPhonelog.Columns[5].HeaderText = "Status";

            }
            else
            {
                dgvYourOwnPhonelog.DataSource = null;
            }
            dgvYourOwnPhonelog.EnableHeadersVisualStyles = false;
            dgvYourOwnPhonelog.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }
        int _ID;
        clsCommunication.enFor _RecipientType;
        clsCommunication.enFrom _CallerType;
        public void YourPhoneCall_For(int ID, clsCommunication.enFor RecipientType, clsCommunication.enFrom CallerType)
        {
            _ID = ID;
            _RecipientType = RecipientType;
            _CallerType = CallerType;

            _FillYourPhoneLogFor(CallerType, ID, 1);
            _FillYourOwnPhoneLogFor(RecipientType, ID, 1);

        }
        
        void _RefreshData()
        {
            YourPhoneCall_For(_ID, _RecipientType, _CallerType);
        }
      
        private void showPhoneLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPhoneLogInfo phoneLogInfo = new frmPhoneLogInfo((int)dgvYourPhoneLog.CurrentRow.Cells[0].Value);
            phoneLogInfo.ShowDialog();
        }

        private void ShowCallerInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int CallerID = (int)dgvYourOwnPhonelog.CurrentRow.Cells[1].Value;
            int CallerType = (int)dgvYourOwnPhonelog.CurrentRow.Cells[2].Value;
            switch (CallerType)
            {
                case 3:
                    {
                        frmEmployeeInfo employeeInfo = new frmEmployeeInfo(CallerID);
                        employeeInfo.ShowDialog();
                        break;
                    }
                case 2:
                    {
                        frmUserInfo userInfo = new frmUserInfo(CallerID);
                        userInfo.ShowDialog();
                        break;
                    }
                case 1:
                    {
                        frmShowPersonInfo personInfo = new frmShowPersonInfo(CallerID);
                        personInfo.ShowDialog();
                        break;
                    }
            }
            _RefreshData();
        }

        private void showRecipientInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int RecipientType =  Convert.ToInt32(dgvYourPhoneLog.CurrentRow.Cells[2].Value.ToString());
            int RecipientID = (int)dgvYourPhoneLog.CurrentRow.Cells[1].Value;
            switch (RecipientType)
            {
                case 3:
                    {
                        frmEmployeeInfo employeeInfo = new frmEmployeeInfo(RecipientID);
                        employeeInfo.ShowDialog();
                        break;
                    }
                case 2:
                    {
                        frmUserInfo userInfo = new frmUserInfo(RecipientID);
                        userInfo.ShowDialog();
                        break;
                    }
                case 1:
                    {
                        frmShowPersonInfo personInfo = new frmShowPersonInfo(RecipientID);
                        personInfo.ShowDialog();
                        break;
                    }
            }
            _RefreshData();
        }

        private void reconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int CallID = (int)dgvYourOwnPhonelog.CurrentRow.Cells[0].Value;
            clsCallLog CallLog = clsCallLog.Find(CallID);
            if(CallLog != null)
            {

            if (!(CallLog.RecipientType == clsCommunication.enFor.ForEmployee && CallLog.RecipientID == clsGlobal.CurrentEmployee.EmployeeID))
            {
                if (MessageBox.Show($"Are you sure do you want to reconnect for this call,\n by [{CallLog.RecipientID}, {CallLog.GetRecipientType()}] "
                    ,
                    "Continue"
                    , MessageBoxButtons.YesNo, MessageBoxIcon.Error) == DialogResult.No)
                    return;
            }
            frmCallPhone callPhone = new frmCallPhone(CallLog.CallerID, CallLog.CallerType, CallLog.RecipientID, CallLog.RecipientType);
            callPhone.ShowDialog();
            _RefreshData();

            }
        }

        private void tabYourPhoneLog_Click(object sender, EventArgs e)
        {

        }

        private void showPhoneLogDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPhoneLogInfo phoneLogInfo = new frmPhoneLogInfo((int)dgvYourOwnPhonelog.CurrentRow.Cells[0].Value);
            phoneLogInfo.ShowDialog();
        }
       

        private void deletePhoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show($"Are you sure do you want delete this Record With CallID{ (int)dgvYourPhoneLog.CurrentRow.Cells[0].Value } ? ","Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question )
                == DialogResult.No)
            {
                return;
            }
            if (clsCallLog.Delete((int)dgvYourPhoneLog.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("Deleted was successfully", "Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Deleted was failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }

        }

        private void deletePhoneLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show($"Are you sure do you want delete this Record With CallID{(int)dgvYourOwnPhonelog.CurrentRow.Cells[0].Value} ? ", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.No)
            {
                return;
            }
            if (clsCallLog.Delete((int)dgvYourOwnPhonelog.CurrentRow.Cells[0].Value))
            {
                MessageBox.Show("Deleted was successfully", "Successfully!", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            else
            {
                MessageBox.Show("Deleted was failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }

        private void dgvYourOwnPhonelog_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void cmsYourPhoneLog_Opening(object sender, CancelEventArgs e)
        {
            cmsYourPhoneLog.Enabled = !lblMessageWhenYourPhoneLogEmpty.Visible;
        }

        private void cmsYourOwnPhoneLog_Opening(object sender, CancelEventArgs e)
        {
            cmsYourOwnPhoneLog.Enabled = !lblMessageWhenYourOwnPhoneLogEmpty.Visible;
        }

        private void ctrlSwitchSearchYourPhoneLog_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillYourPhoneLogFor(_CallerType, _ID, e.CurrentNumberOfPage);

        }

        private void ctrlSwitchSearchYourOwnPhoneLog_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillYourOwnPhoneLogFor(_RecipientType, _ID, e.CurrentNumberOfPage);
        }

        private void ctrlYourCallPhoneLog_Load(object sender, EventArgs e)
        {

        }
    }
}
