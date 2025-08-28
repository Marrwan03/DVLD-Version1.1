using DVLD.Classes;
using DVLD.Properties;
using DVLD_Buisness;
using System;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Email
{
    public partial class ctrlSendEditMessageWithInfo : UserControl
    {
        public event Action<int> OnSendMessage;
        protected virtual void SendMessage(int messageID)
        { OnSendMessage?.Invoke(messageID); }
        public ctrlSendEditMessageWithInfo()
        {
            InitializeComponent();
        }

        int _SenderID;
        clsEmail.enFrom _SenderType;

        int _RecipientID;
        clsEmail.enFor _RecipientType;
        
       public bool EnableSendMessageGroup 
        {
            set 
            {
                gbSendMessage.Enabled = value;
            }
        }
        string _Message;
       int? _MessageID;
        clsEmail EmailInfo;
       

        public void SendMessage_Load(int SenderID,clsEmail.enFrom SenderType,int RecipientID, clsEmail.enFor RecipientType)
        {
            _SenderID = SenderID;
            _RecipientID = RecipientID;
            _SenderType = SenderType;
            _RecipientType = RecipientType;

            ctrlMessageInfo1.ctrlMessageInfo_Load(_SenderID, _SenderType, _RecipientID, _RecipientType);

            EmailInfo = new clsEmail();
        }

        public void SendMessage_Load(int SenderID, clsEmail.enFrom SenderType, int MessageID)
        {
            _SenderID = SenderID;
            _SenderType = SenderType;
            _MessageID = MessageID;
            EmailInfo = clsEmail.Find(_MessageID.Value);

            if (!_MessageID.HasValue || EmailInfo == null)
            {
                MessageBox.Show("This Message Id Or Message is not here, Check it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            _RecipientID = EmailInfo.RecipientID;
            _RecipientType=EmailInfo.RecipientType;

            if(File.Exists(EmailInfo.Message))
            {
                txtMessage.Visible = false;
                llblRermoveImage.Visible = true;
                picMessage.Visible = true;

                picMessage.ImageLocation = EmailInfo.Message;
            }
            else
            {
                picMessage.Visible = false;
                txtMessage.Visible = true;
                llblRermoveImage.Visible = false;

                txtMessage.Text = EmailInfo.Message;
            }

            ctrlMessageInfo1.ctrlMessageInfo_Load(_MessageID.Value);
            
        }

        bool _HandlePictureMessage(ref string SourceFile)
        {

            if (EmailInfo.EmailID.HasValue)
            {
                if (File.Exists(EmailInfo.Message))
                {
                    File.Delete(EmailInfo.Message);
                }
            }
           
            return clsUtil.CopyImageToProjectImagesFolder(ref SourceFile, @"C:\DVLD-Email-Images\");
        }
        private void picSendImage_Click(object sender, EventArgs e)
        {


            openFileDialog1.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.gif;*.bmp";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                // Process the selected file
                string selectedFilePath = openFileDialog1.FileName;
                txtMessage.Visible = false;
                picMessage.Visible = true;
                picMessage.ImageLocation = selectedFilePath;
                llblRermoveImage.Visible = true;
                // ...
            }
        }

        private  void picSendMessage_Click(object sender, EventArgs e)
        {

            if((txtMessage.Text == "Set Message ..." || txtMessage.Text=="") && picMessage.ImageLocation == null)
            {
                MessageBox.Show("You don`t have any message to send", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtMessage.Focus();
                picSendImage.Focus();
                return;
            }

            if(string.IsNullOrEmpty(picMessage.ImageLocation))
            {
                EmailInfo.Message = txtMessage.Text;
            }
            else
            {
                _Message = picMessage.ImageLocation;
                if(!_HandlePictureMessage(ref _Message))
                {
                    MessageBox.Show("The operation of save image is failed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EmailInfo.Message = _Message;
            }

            EmailInfo.SenderID = _SenderID;
            EmailInfo.FromType = _SenderType;
            EmailInfo.RecipientID = _RecipientID;
            EmailInfo.RecipientType = _RecipientType;

            bool Exists = File.Exists(EmailInfo.Message);
            string appPassword = null;
            //You should set your own appPassword from this URL
            //[ https://accounts.google.com/v3/signin/challenge/pwd?TL=ALgCv6zozrf6fk3k9UyNyiVsjFWCyobuqX6g20Ty3EExocFfqFkZiKHKUw6qtbK8&authuser=0&cid=2&continue=https%3A%2F%2Fmyaccount.google.com%2Fapppasswords&flowName=GlifWebSignIn&followup=https%3A%2F%2Fmyaccount.google.com%2Fapppasswords&ifkv=AdBytiMfeIsqYC5lEEnXykXllHO8jjxCxKbjw35_iqjB73uNtO5L2kS75OVkli-vRDZ_JKEM9aiosg&osid=1&rart=ANgoxcezpWZp63g3DxYg_EYtv2CqPEk72sKC1ArqrPpiSRuArVLeFNO3RhzbsMRCUQAe_hLl8x4M7Uk3aI2itEPUUQ7qj9w--5LY6aP9PQb8ZOOs1e_qfx8&rpbg=1&service=accountsettings]
            if (!clsUtil.SendEmailSMTP(EmailInfo.SenderInfo.Address, appPassword,
                EmailInfo.RecipientInfo.Address, "DVLD-System", Exists?"Picture" : EmailInfo.Message,
                Exists? EmailInfo.Message : null))
            {
                MessageBox.Show("الإعدادات غير صحيحة. تحقق من:\n1. كلمة مرور التطبيق\n2. التطبيقات الأقل أماناً\n3. اتصال الإنترنت");
                return;
            }


            if ( !EmailInfo.Save())
            {
                MessageBox.Show("The message hasn`t arrived :-(", "Fail", MessageBoxButtons.OK, MessageBoxIcon.Error);
                gbSendMessage.Focus();
                return;
            }

            MessageBox.Show("The message has arrived :-)", "Successfully", MessageBoxButtons.OK, MessageBoxIcon.Information);
            _MessageID = EmailInfo.EmailID;
            if (OnSendMessage != null)
                SendMessage(_MessageID ?? -1);

            ctrlMessageInfo1.ctrlMessageInfo_Load(_MessageID ?? -1);
        }

        private void llblRermoveImage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            if(_MessageID.HasValue)
            {
                string Message = clsEmail.Find(_MessageID.Value).Message;

                if(File.Exists(Message))
                {
                    File.Delete(Message);
                }
            }
            picMessage.ImageLocation = null;
            llblRermoveImage.Visible=false;
            picMessage.Visible=false;
            txtMessage.Text = "Set Message ...";
            txtMessage.Visible = true;

        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {
          
        }

        private void ctrlSendMessage_Load(object sender, EventArgs e)
        {




        }

        private void gbSendMessage_Enter(object sender, EventArgs e)
        {

        }

        private void txtMessage_MouseEnter(object sender, EventArgs e)
        {
            if (txtMessage.Text == "Set Message ...")
                txtMessage.Text = null;
        }
    }
}
