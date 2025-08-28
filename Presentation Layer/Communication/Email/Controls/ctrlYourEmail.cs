using DVLD.Classes;
using DVLD.Employee;
using DVLD.People;
using DVLD_Buisness;
using DVLD_Buisness.Classes;
using DVLD_Buisness.Global_Classes;
using Org.BouncyCastle.Cms;
using System;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DVLD.Email
{
    public partial class ctrlYourEmail : UserControl
    {
        public ctrlYourEmail()
        {
            InitializeComponent();
        }
      

        void _FillEmailInfoBy(clsEmail.enFor For, clsEmail.enType TypeOfEmail,
            int SenderID, clsCommunication.enFrom SenderType,
            int RecipientID, clsCommunication.enFor RecipientType,
            ref int MaxNumberOfPage, ref DataTable dt, int NumberOfPage)
        {
            switch (For)
            {
                case DVLD_Buisness.Classes.clsCommunication.enFor.ForPerson:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForEmail(TypeOfEmail, SenderID, SenderType
                            , RecipientID, RecipientType), 6);

                        if(TypeOfEmail == clsEmail.enType.YourEmail)
                            dt = clsPerson.GetYourEmailBy(RecipientID, NumberOfPage, 6);
                        else
                            dt = clsPerson.GetYourMessagesBy(SenderID, NumberOfPage, 6);

                        break;
                    }

                case DVLD_Buisness.Classes.clsCommunication.enFor.ForUser:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForEmail(TypeOfEmail, SenderID, SenderType
                            , RecipientID, RecipientType), 6);

                        if (TypeOfEmail == clsEmail.enType.YourEmail)
                            dt = clsUser.GetYourEmailBy(RecipientID, NumberOfPage, 6);
                        else
                            dt = clsUser.GetYourMessagesBy(SenderID, NumberOfPage, 6);

                        break;
                    }

               case DVLD_Buisness.Classes.clsCommunication.enFor.ForEmployee:
                    {
                        MaxNumberOfPage =
                            clsGet.GetMaximamPage
                            (clsGet.GetMaxNumberOfRowsForEmail(TypeOfEmail, SenderID, SenderType
                            , RecipientID, RecipientType), 6);

                        if (TypeOfEmail == clsEmail.enType.YourEmail)
                            dt = clsEmployee.GetYourEmailBy(RecipientID, NumberOfPage, 6);
                        else
                            dt = clsEmployee.GetYourMessagesBy(SenderID, NumberOfPage, 6);

                        break;
                    }

            }
        }

        void _FilldgvYourEmailFor(clsEmail.enFor For,int RecipientID, int NumberOfPage)
        {
            DataTable dt = new DataTable();
            int MaxNumberOfPage = 0;

            _FillEmailInfoBy(For, clsEmail.enType.YourEmail, 0, clsCommunication.enFrom.None, RecipientID, For, ref MaxNumberOfPage, ref dt, NumberOfPage);
            ctrlSwitchSearchYourEmail.MaxNumberOfPage = MaxNumberOfPage;
            ctrlSwitchSearchYourEmail.NumberOfPage = NumberOfPage;
                

            lblNumerOfRecordEmail.Text = dt.Rows.Count.ToString();

            lblMessageWhenEmailEmpty.Visible = dt.Rows.Count == 0;
            ctrlSwitchSearchYourEmail.Visible = !lblMessageWhenEmailEmpty.Visible;

            if (dt.Rows.Count > 0)
            {
             DataTable dtEmail = dt.DefaultView.ToTable(false, "EmailID",
                 "SenderID", "SenderType", "Message", "Time");

                dgvYourEmail.DataSource = dtEmail;
                dgvYourEmail.Columns[0].HeaderText = "ID";
                dgvYourEmail.Columns[1].HeaderText = "Sender ID";
                dgvYourEmail.Columns[2].HeaderText = "Sender Type";
                dgvYourEmail.Columns[3].HeaderText = "Message";
                dgvYourEmail.Columns[4].HeaderText = "Time";
            }
            else
            {
                dgvYourEmail.DataSource = null;

            }

            dgvYourEmail.EnableHeadersVisualStyles = false;
            dgvYourEmail.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;

        }

        void _FilldgvYourMessagesFor(clsEmail.enFor For, int SenderID,clsEmail.enFrom SenderType, int NumberOfPage)
        {
            DataTable dt= new DataTable();
            int MaxNumberOfPage = 0;
            _FillEmailInfoBy(For, clsEmail.enType.YourMessages, SenderID, SenderType, 0, _RecipientType, ref MaxNumberOfPage, ref dt, NumberOfPage);
            ctrlSwitchSearchYourMessages.MaxNumberOfPage = MaxNumberOfPage;
            ctrlSwitchSearchYourMessages.NumberOfPage = NumberOfPage;

            lblNumerOfRecordMessages.Text = dt.Rows.Count.ToString(); 
            lblMessageWhenMessageEmpty.Visible = dt.Rows.Count == 0;
            ctrlSwitchSearchYourMessages.Visible = !lblMessageWhenMessageEmpty.Visible;

            if (dt.Rows.Count > 0)
            {
                DataTable dtMessages = dt.DefaultView.ToTable(false, "EmailID", "Message",
                    "Time", "RecipientID", "RecipientType");
                dgvYourMessages.DataSource = dtMessages;

                dgvYourMessages.Columns[0].HeaderText = "ID";
                dgvYourMessages.Columns[1].HeaderText = "Message";
                dgvYourMessages.Columns[2].HeaderText = "Time";
                dgvYourMessages.Columns[3].HeaderText = "Recipient ID";
                dgvYourMessages.Columns[4].HeaderText = "Recipient Type";

            }
            else
            {
                dgvYourMessages.DataSource = null;
            }

            dgvYourMessages.EnableHeadersVisualStyles = false;
            dgvYourMessages.ColumnHeadersDefaultCellStyle.BackColor = Color.Black;
        }

        int _ID;
        clsEmail.enFor _RecipientType;
        clsEmail.enFrom _SenderType;
        public void YourEmail_For(int ID, clsEmail.enFor RecipientType, clsEmail.enFrom SenderType)
        {
            _ID = ID;
            _SenderType = SenderType;
            _RecipientType = RecipientType;

            _FilldgvYourEmailFor(RecipientType, _ID, 1);
            _FilldgvYourMessagesFor(RecipientType, _ID,SenderType, 1);
        }

       public void RefreshData()
        {
            YourEmail_For(_ID, _RecipientType, _SenderType);
        }

        Form _GetFormInfoBy(byte Type, int ID)
        {
            Form frm;
            switch (Type)
            {
                case 1:
                    {
                        frm = new frmShowPersonInfo(ID);
                        break;
                    }
                case 2:
                    {
                        frm = new frmUserInfo(ID);
                        break;
                    }
                default:
                    {
                        frm = new frmEmployeeInfo(ID);
                        break;
                    }
            }
            return frm;
        }

        private void showSenderInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = _GetFormInfoBy
                ((byte)dgvYourEmail.CurrentRow.Cells[2].Value,
                (int)dgvYourEmail.CurrentRow.Cells[1].Value);
        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form frm = _GetFormInfoBy
                ((byte)dgvYourMessages.CurrentRow.Cells[4].Value,
                (int)dgvYourMessages.CurrentRow.Cells[3].Value);
            frm.ShowDialog();
        }

        private void showDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMessageInfo frmMessageInfo = new frmMessageInfo((int)dgvYourEmail.CurrentRow.Cells[0].Value);
            frmMessageInfo.ShowDialog();
        }

        private void dgvYourEmail_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ReplyStripMenuItem1_Click(object sender, EventArgs e)
        {
          
            clsEmail MessageInfo = clsEmail.Find((int)dgvYourEmail.CurrentRow.Cells[0].Value);
            if (MessageInfo != null)
            {
                frmSendUpdateEmail sendEmail = new frmSendUpdateEmail(MessageInfo.EmailID.Value);
                sendEmail.ShowDialog();
                RefreshData();
            }
        }

        private void showMessageDetailsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMessageInfo messageInfo = new frmMessageInfo((int)dgvYourMessages.CurrentRow.Cells[0].Value);
            messageInfo.ShowDialog();
        }

        private void deleteMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {

            if (MessageBox.Show("Are you sure do you want delete this Message", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsEmail MessageInfo = clsEmail.Find((int)dgvYourMessages.CurrentRow.Cells[0].Value);
                if (File.Exists(MessageInfo.Message))
                {
                    File.Delete(MessageInfo.Message);
                }

                if (clsEmail.DeleteMessage((int)dgvYourMessages.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Message deleted successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    //Refresh
                   RefreshData();

                }
            }
        }

        private void editMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int MesssageID = (int)dgvYourMessages.CurrentRow.Cells[0].Value;
            clsEmail MessageInfo = clsEmail.Find(MesssageID);
            if (MessageInfo != null)
            {
                frmSendUpdateEmail sendEmail = new frmSendUpdateEmail(MessageInfo.EmailID.Value);
                sendEmail.ShowDialog();
                RefreshData();
            }
        }

        private void cmsEmail_Opening(object sender, CancelEventArgs e)
        {
            cmsEmail.Enabled = dgvYourEmail.Rows.Count > 0;
            bool ShowOperations = clsGlobal.CurrentEmployee == null;
            ReplyStripMenuItem1.Enabled = ShowOperations;
            deleteMessageToolStripMenuItem1.Enabled = ShowOperations;
        }

        private void cmsMessages_Opening(object sender, CancelEventArgs e)
        {
            cmsMessages.Enabled = dgvYourMessages.Rows.Count > 0;
            bool ShowOperations = clsGlobal.CurrentEmployee == null;
            editMessageToolStripMenuItem.Enabled = ShowOperations;
            deleteMessageToolStripMenuItem.Enabled= ShowOperations;
        }

        private void ctrlYourEmail_Load(object sender, EventArgs e)
        {

        }

        private void ctrlSwitchSearchYourEmail_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvYourEmailFor(_RecipientType, _ID, e.CurrentNumberOfPage);
        }

        private void ctrlSwitchSearchYourEmail_ChangePageToRight(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
           _FilldgvYourEmailFor(_RecipientType, _ID, e.CurrentNumberOfPage);
        }

        private void ctrlSwitchSearchYourMessages_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvYourMessagesFor(_RecipientType, _ID,_SenderType, e.CurrentNumberOfPage);
        }

        private void ctrlSwitchSearchYourMessages_ChangePageToRight(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvYourMessagesFor(_RecipientType,_ID,_SenderType, e.CurrentNumberOfPage);
        }

        private void deleteMessageToolStripMenuItem1_Click(object sender, EventArgs e)
        {

            if(MessageBox.Show("Are you sure do you want delete this Message", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                clsEmail MessageInfo = clsEmail.Find((int)dgvYourEmail.CurrentRow.Cells[0].Value);
                if (File.Exists(MessageInfo.Message))
                {
                    File.Delete(MessageInfo.Message);
                }

                if(clsEmail.DeleteMessage((int)dgvYourEmail.CurrentRow.Cells[0].Value))
                {
                    MessageBox.Show("Message deleted successfully", "Delete", MessageBoxButtons.OK, MessageBoxIcon.Information);

                   
                    RefreshData();
                }
            }


        }
    }
}
