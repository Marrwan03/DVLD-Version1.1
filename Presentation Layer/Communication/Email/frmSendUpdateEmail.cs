using DVLD.Classes;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Email
{
    public partial class frmSendUpdateEmail : Form
    {
        enum enMode { Send=1, Update=2}
        enMode _Mode;
        public frmSendUpdateEmail(int SenderID,clsEmail.enFrom SenderType,int RecipientID,clsEmail.enFor RecipientType)
        {
            InitializeComponent();
            _RecipientID = RecipientID;
          _RecipientType = RecipientType;
            _SenderID = SenderID;
            _SenderType = SenderType;

            _Mode = enMode.Send;
        }

        public frmSendUpdateEmail(int MessageID)
        {
            InitializeComponent();
            _MessageID = MessageID;
           
            _Mode = enMode.Update;
        }
        public frmSendUpdateEmail(int SenderID, clsEmail.enFrom SenderType)
        {
            InitializeComponent();
            _SenderID = SenderID;
            _SenderType = SenderType;

            _Mode = enMode.Send;
        }

        int _SenderID;
        clsEmail.enFrom _SenderType;
        int? _RecipientID;
        clsEmail.enFor _RecipientType = clsEmail.enFor.None;
       //clsEmail.enModeSend _modeSend;
        clsEmail EmailInfo;
        int? _MessageID;

        void _SetMode()
        {
            switch (_Mode)
            {
                case enMode.Send:
                    {
                        if (_RecipientID.HasValue)
                            ctrlSendEditMessageWithInfo1.SendMessage_Load(_SenderID, _SenderType, _RecipientID.Value, _RecipientType);
                       
                        lblTitle.Text = "Send Message";
                        break;
                    }
                case enMode.Update:
                    {
                        EmailInfo = clsEmail.Find(_MessageID.Value);
                        if (EmailInfo != null)
                        {
                            _SenderID = EmailInfo.SenderID;
                            _SenderType = EmailInfo.FromType;
                            _RecipientID = EmailInfo.RecipientID;
                            _RecipientType = EmailInfo.RecipientType;
                        }
                        else
                        {

                            MessageBox.Show("Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        
                        ctrlSendEditMessageWithInfo1.SendMessage_Load(_SenderID, _SenderType, _MessageID.Value);
                        lblTitle.Text = "Update Message";
                        break;
                    }
            }
        }

        void _ResetForm()
        {
           _SetMode();
            switch (_RecipientType)
            {
                case clsEmail.enFor.ForPerson:
                    {
                        pSenderType.Enabled = false;
                        rbPerson.Checked = true;
                        clsPerson Person = clsPerson.Find(_RecipientID.Value);
                        ctrlPersonCardWithFilter1.LoadPersonInfo(Person.PersonID.Value);
                        ctrlPersonCardWithFilter1.FilterEnabled = false;
                        break;
                    }
                case clsEmail.enFor.ForUser:
                    {
                        pSenderType.Enabled = false;
                        rbUser.Checked = true;
                        clsUser User = clsUser.FindByUserID(_RecipientID.Value);
                        ctrlUserCardWithFilter1.LoadUserInfo(User.UserID.Value, DVLD.Controls.ctrlUserCard.enLoadUserInfo.ByUserID);
                        ctrlUserCardWithFilter1.EnableUserFilter = false;
                        break;
                    }
                case clsEmail.enFor.ForEmployee:
                    {
                        pSenderType.Enabled = false;
                        rbEmployee.Checked = true;
                        clsEmployee Employee = clsEmployee.FindByEmployeeID(_RecipientID.Value);
                        ctrlEmployeeCardWithFilter1.ctrlEmployeeCardWithFilter_Load(Employee.EmployeeID.Value);
                        ctrlUserCardWithFilter1.EnableUserFilter = false;
                        break;
                    }
                default:
                    {
                        tabSendMessage.Enabled = false;
                        rbPerson.Checked = true;
                        break;
                    }
            }


        }

        private void frmSendEmail_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _ResetForm();
        }

      

      

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlSendMessageWithInfo1_OnSendMessage(int obj)
        {
            _MessageID = obj;
            clsEmail Email = clsEmail.Find(_MessageID.Value);
            if (Email == null)
            {
                MessageBox.Show("Email is not Found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            MessageBox.Show("Message Email ID is [ " + Email.EmailID?.ToString() + " ]", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);

            ctrlSendEditMessageWithInfo1.EnableSendMessageGroup = false;
            switch (Email.RecipientType)
            {
                case clsEmail.enFor.ForPerson:
                    {
                        ctrlPersonCardWithFilter1.FilterEnabled = false;
                        break;
                    }
                case clsEmail.enFor.ForUser:
                    {
                        ctrlUserCardWithFilter1.EnableUserFilter = false;
                        break;
                    }
                case clsEmail.enFor.ForEmployee:
                    {
                        ctrlEmployeeCardWithFilter1.EnableFilter = false;
                        break;
                    }
            }
            _Mode = enMode.Update;
            _SetMode();
            btnClose.Focus();
        }

        private void ctrlSendMessageWithInfo1_Load(object sender, EventArgs e)
        {

        }

        public void rb_CheckedChanged(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.Visible = rbPerson.Checked;
            ctrlUserCardWithFilter1.Visible = rbUser.Checked;
            ctrlEmployeeCardWithFilter1.Visible = rbEmployee.Checked;
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

            if(!_RecipientID.HasValue && _Mode != enMode.Update)
            {
                MessageBox.Show("You have to get [Person OR Employee] Account to continue", "Empty!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            _ResetForm();


            tcMessage.SelectedTab = tcMessage.TabPages["tabSendMessage"];
            tabSendMessage.Enabled = true;

        }

        void _FillRecipientInfo(int RecipientID, clsEmail.enFor RecipientType)
        {
            _RecipientID = RecipientID;
            _RecipientType = RecipientType;
        }

        private void ctrlUserCardWithFilter1_OnFindUser(int obj)
        {
            _FillRecipientInfo(obj, clsEmail.enFor.ForUser);
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _FillRecipientInfo(obj, clsEmail.enFor.ForPerson);
        }

        private void ctrlUserCardWithFilter1_OnAddUser(int obj)
        {
            _FillRecipientInfo(obj , clsEmail.enFor.ForUser);
        }

        private void ctrlPersonCardWithFilter1_OnPersonAdded(int obj)
        {
            _FillRecipientInfo(obj, clsEmail.enFor.ForPerson);
        }

        private void tabSendMessage_Click(object sender, EventArgs e)
        {

        }

        private void ctrlEmployeeCardWithFilter1_OnEmployeeAdded(int obj)
        {
            _FillRecipientInfo(obj, clsEmail.enFor.ForPerson);
        }

        private void ctrlEmployeeCardWithFilter1_OnEmployeeSelected(int obj)
        {
            _FillRecipientInfo(obj, clsEmail.enFor.ForEmployee);
        }
    }
}
