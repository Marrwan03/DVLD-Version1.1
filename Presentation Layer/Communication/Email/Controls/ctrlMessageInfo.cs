using DVLD.Classes;
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
    public partial class ctrlMessageInfo : UserControl
    {
        public ctrlMessageInfo()
        {
            InitializeComponent();
        }

        public void ctrlMessageInfo_Load(int SenderID, clsEmail.enFrom From, int RecipientID, clsEmail.enFor For)
        {
            //Edit you should has SenderID
            switch (From)
            {
                case clsEmail.enFrom.ByPerson:
                    {
                        clsPerson SenderPersonInfo = clsPerson.Find(SenderID);
                        lblSenderName.Text = SenderPersonInfo.FirstName + " " + SenderPersonInfo.LastName;
                        lblSenderEmail.Text = SenderPersonInfo.Email;
                        lblSenderType.Text = "Person";
                        break;
                    }
                    case clsEmail.enFrom.ByUser:
                    {
                        clsUser SenderUserInfo = clsUser.FindByUserID(SenderID);
                        lblSenderName.Text = SenderUserInfo.PersonInfo.FirstName + " " + SenderUserInfo.PersonInfo.LastName;
                        lblSenderEmail.Text = SenderUserInfo.PersonInfo.Email;
                        lblSenderType.Text = "User";
                        break;
                    }
                    case clsEmail.enFrom.ByEmployee:
                    {
                        clsEmployee SenderEmployeeInfo = clsEmployee.FindByEmployeeID(SenderID);
                        lblSenderName.Text = SenderEmployeeInfo.PersonInfo.FirstName + " " + SenderEmployeeInfo.PersonInfo.LastName;
                        lblSenderEmail.Text = SenderEmployeeInfo.PersonInfo.Email;
                        lblSenderType.Text = "Employee";
                        break;
                    }
            }
            switch (For)
            {
                case clsEmail.enFor.ForPerson:
                    {
                        clsPerson PersonInfo = clsPerson.Find(RecipientID);
                        lblRecipientName.Text = PersonInfo.FirstName + " " + PersonInfo.LastName;
                        lblRecipientEmail.Text = PersonInfo.Email;
                        lblRecipientType.Text = "Person";
                        break;
                    }
                case clsEmail.enFor.ForUser:
                    {
                        clsUser RecipientUserInfo = clsUser.FindByUserID(RecipientID);
                        lblRecipientName.Text = RecipientUserInfo.PersonInfo.FirstName + " " + RecipientUserInfo.PersonInfo.LastName;
                        lblRecipientEmail.Text = RecipientUserInfo.PersonInfo.Email;
                        lblRecipientType.Text = "User";
                        break;
                    }
                case clsEmail.enFor.ForEmployee:
                    {
                        clsEmployee RecipientEmployeeInfo = clsEmployee.FindByEmployeeID(RecipientID);
                        lblRecipientName.Text = RecipientEmployeeInfo.PersonInfo.FirstName + " " + RecipientEmployeeInfo.PersonInfo.LastName;
                        lblRecipientEmail.Text = RecipientEmployeeInfo.PersonInfo.Email;
                        lblRecipientType.Text = "Employee";
                        break;
                       
                    }
            
            }
        }


        public void ctrlMessageInfo_Load(int MessageID)
        {
            clsEmail emailInfo = clsEmail.Find(MessageID);
            if (emailInfo != null)
            {
                lblMessageID.Text = emailInfo.EmailID.ToString();
                lblMessageTime.Text = emailInfo.MessageTime.ToString("G");

              ctrlMessageInfo_Load(emailInfo.SenderID, emailInfo.FromType, emailInfo.RecipientID, emailInfo.RecipientType);

                if(File.Exists(emailInfo.Message))
                {
                    picMessage.Visible = true;
                    picMessage.ImageLocation = emailInfo.Message;

                    lblMessageText.Visible = false;
                }
                else
                {
                    lblMessageText.Visible = true;
                    lblMessageText.Text = emailInfo.Message;

                    picMessage.Visible = false;
                }

            }
            else
            {
                MessageBox.Show("This MessageID isn`t found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


        }

        private void lblMessageText_Click(object sender, EventArgs e)
        {

        }

        private void gbMessageInfo_Enter(object sender, EventArgs e)
        {

        }
    }
}
