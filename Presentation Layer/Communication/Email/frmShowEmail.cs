using DVLD.Classes;
using DVLD_Buisness;
using DVLD_Buisness.Classes;
using System;
using System.Windows.Forms;

namespace DVLD.Email
{
    public partial class frmShowEmail : Form
    {
        int _ID;
        clsEmail.enFor _RecipientType;
        clsEmail.enFrom _SenderType;


        public frmShowEmail(int ID, clsEmail.enFor RecipientType, clsEmail.enFrom SenderType)
        {
            InitializeComponent();
            _ID = ID;
            _RecipientType = RecipientType;
            _SenderType = SenderType;
        }

        string _GetNameOfEmail()
        {
            switch (_SenderType)
            {
                case clsCommunication.enFrom.ByPerson:
                    return clsPerson.Find(_ID).FirstName;
                case clsCommunication.enFrom.ByUser:
                    return clsUser.FindByUserID(_ID).PersonInfo.FirstName;
                case clsCommunication.enFrom.ByEmployee:
                    return clsEmployee.FindByEmployeeID(_ID).PersonInfo.FirstName;
            }
            return null;
        }

        private void frmShowEmail_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            lblEmailName.Text = _GetNameOfEmail()+"`s Email";

            ctrlYourEmail1.YourEmail_For(_ID, _RecipientType, _SenderType);
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }

        private void picAddMessageEmail_Click(object sender, EventArgs e)
        {
            frmSendUpdateEmail SendEmail = new frmSendUpdateEmail(_ID,_SenderType);
            SendEmail.ShowDialog();
            frmShowEmail_Load(null, null);
        }
    }
}
