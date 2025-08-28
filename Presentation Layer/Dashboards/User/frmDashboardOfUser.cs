using DVLD.All_Licenses;
using DVLD.Classes;
using DVLD.Communication.Phone;
using DVLD.Dashboards.User;
using DVLD.Email;
using DVLD.Employee;
using DVLD.Payments;
using DVLD.User;
using DVLD.User.Status;
using DVLD.User.Your_Requests;
using Guna.UI2.WinForms;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Dashboards
{
    public partial class frmDashboardOfUser : Form
    {
        frmNewLogin _NewLogin;
        public frmDashboardOfUser(frmNewLogin NewLogin)
        {
            InitializeComponent();
            _NewLogin = NewLogin;
            this.Load += frmDashboardOfUser_Load;
        }
        [DllImport("user32.DLL", EntryPoint = "ReleaseCapture")]
        extern static void ReleaseCapture();
        [DllImport("user32.DLL", EntryPoint = "SendMessage")]
        extern static void SendMessage(System.IntPtr hwnd, int wmsg, int wparam, int iparam);

        private void pTop_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 0x112, 0xf012, 0);
        }
        void _ShowFormInCentre(object frm)
        {
            if (pCenter.Controls.Count > 0)
            {
                pCenter.Controls.RemoveAt(0);
            }

            Form form = frm as Form;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.BringToFront();
            pCenter.Tag = form;
            pCenter.Controls.Add(form);
            form.Show();

        }
        void _ChangeText(Guna2Button btn, string Text)
        {
            if (btn.Text == Text)
                btn.Text = null;
            else
                btn.Text = Text;
        }

        void _HideText()
        {
            _ChangeText(btnAllLicenses, "All Licenses");
            _ChangeText(btnYourRequests, "Your Requests");
            _ChangeText(btnPayments, "Payments");
            _ChangeText(btnStatus, "Status");
            _ChangeText(btnAccountSettings, "Account Settings");
        }
        void _HideAllPanels()
        {
            pRequestsMenue.Visible = false;
            pAccountSettingsMenue.Visible = false;
        }

        private void ctrlWindowState1_Close()
        {
            Application.Exit();
        }

        private void ctrlWindowState1_maximize()
        {
            this.WindowState = FormWindowState.Maximized;
        }

        private void ctrlWindowState1_minimize()
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void ctrlWindowState1_RestoreDown()
        {
            this.WindowState = FormWindowState.Normal;
        }

        private void frmDashboardOfUser_Load(object sender, EventArgs e)
        {
            ctrlDashboardDataInfo1.ctrlDashboardDataInfo_Load
                (clsGlobal.CurrentUser.UserID.Value,
                MyGeneralUserControl.ctrlDashboardDataInfo.enLoginType.User);

            _ShowUserInfoInCentre();
            this.Region = clsGlobal.CornerForm(Width, Height);
        }

        private void btnCurrentUserInfo2_Click(object sender, EventArgs e)
        {
            frmUserInfo userInfo = new frmUserInfo(clsGlobal.CurrentUser.UserID.Value);
            userInfo.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(userInfo); 
        }

      

        private void btnChangePassword2_Click_1(object sender, EventArgs e)
        {
            frmChangePassword changePassword = new frmChangePassword(clsGlobal.CurrentUser.UserID.Value);
            changePassword.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(changePassword);
        }

        private void btnYourEmail2_Click(object sender, EventArgs e)
        {
            frmShowEmail showEmail =
                new frmShowEmail(clsGlobal.CurrentUser.UserID.Value,
                DVLD_Buisness.Classes.clsCommunication.enFor.ForUser,
                DVLD_Buisness.Classes.clsCommunication.enFrom.ByUser);
            showEmail.OnClose += _ShowUserInfoInCentre;

            _ShowFormInCentre(showEmail);
        }

        private void btnYourCallLog2_Click(object sender, EventArgs e)
        {
            frmShowCallLog showCallLog = new frmShowCallLog(clsGlobal.CurrentUser.UserID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByUser);
            showCallLog.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(showCallLog);
        }

        private void btnSignOut_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentUser = null;
            _NewLogin.Show();
            this.Close();
        }

        byte WidthSmall = 89;
        short WidthBig = 381;
        
        private void picMenue_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthBig)
                pMenue.Width = WidthSmall;
            else
                pMenue.Width = WidthBig;

            _HideText();
            _HideAllPanels();
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {

            if (pMenue.Width == WidthSmall)
                picMenue_Click(null, null);

            _HideAllPanels();
            pAccountSettingsMenue.Visible = true;
        }

        void _RefreshData(string NewPath)
        {
            ctrlDashboardDataInfo1.ImagePath = NewPath;
        }

        void _ShowUserInfoInCentre()
        {
            
            frmUserInformationDetails userInformationDetails =
                new frmUserInformationDetails(clsGlobal.CurrentUser.PersonID);
            userInformationDetails.OnChangedPicture += _RefreshData;
            _ShowFormInCentre(userInformationDetails);
        }

        private void btnAllLicenses_Click(object sender, EventArgs e)
        {
            _HideAllPanels();
            frmAllLicenses allLicenses = new frmAllLicenses(clsGlobal.CurrentUser.UserID.Value);
            allLicenses.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(allLicenses);
        }

        private void btnYourWallet_Click(object sender, EventArgs e)
        {
            frmYourCreaditCards yourCreaditCards = new DVLD.User.frmYourCreaditCards(clsGlobal.CurrentUser.UserID.Value, true);
            yourCreaditCards.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(yourCreaditCards);
        }

        private void btnYourRequests_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthSmall)
                picMenue_Click(null, null);
            _HideAllPanels();
            pRequestsMenue.Visible = true;
            //_ShowFormInCentre(new frmYourRequests(clsGlobal.CurrentUser.PersonID));
        }

        private void btnApplicationsRequestsPart_Click(object sender, EventArgs e)
        {
            frmYourApplicationRequests yourApplicationRequests = new frmYourApplicationRequests(clsGlobal.CurrentUser.PersonID);
            yourApplicationRequests.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(yourApplicationRequests);
        }

        private void btnAppointmentPart_Click(object sender, EventArgs e)
        {
            frmYourAppointmentRequests yourAppointmentRequests = new frmYourAppointmentRequests(clsGlobal.CurrentUser.PersonID);
            yourAppointmentRequests.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(yourAppointmentRequests);
        }

        private void btnPayments_Click(object sender, EventArgs e)
        {
            frmAllPayments allPayments = new frmAllPayments(clsGlobal.CurrentUser.PersonID);
            allPayments.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(allPayments); 
        }

        private void btnStatus_Click(object sender, EventArgs e)
        {
            frmStatus status = new frmStatus(clsGlobal.CurrentUser.PersonID);
            status.OnClose += _ShowUserInfoInCentre;
            _ShowFormInCentre(status);
        }
    }
}
