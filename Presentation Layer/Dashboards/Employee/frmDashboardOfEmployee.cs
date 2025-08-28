using System;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using DVLD.Employee;
using DVLD.People;
using DVLD.Properties;
using DVLD.Applications;
using DVLD.Classes;
using DVLD_Buisness;
using DVLD.Applications.International_License;
using DVLD.Licenses;
using DVLD.Applications.ReplaceLostOrDamagedLicense;
using DVLD.Applications.Rlease_Detained_License;
using DVLD.Tests;
using DVLD.Applications.Detain_License;
using DVLD.Drivers;
using DVLD.Email;
using DVLD.Communication.Phone;
using System.Drawing;
using DVLD.Applications.Controls;
using System.Collections.Generic;
using static DVLD.Applications.Controls.ctrlAddNewApplication;
using DVLD.User.Your_Requests.Appointment_Part.Controls;
using DVLD.DriverLicense;
using DVLD.Dashboards.Employee;
using DVLD.Archive;

namespace DVLD
{
 
    public partial class frmDashboardOfEmployee : Form
    {
        frmNewLogin _frmLogin;
        public frmDashboardOfEmployee(frmNewLogin frmLogin)
        {
            InitializeComponent();
            _HidePanles();
            _frmLogin = frmLogin;

        }
        

         void _ShowEmployeeInfoInCentre()
        {
            _ShowFormInCentre(new frmEmployeeInformationDetails(clsGlobal.CurrentEmployee.EmployeeID.Value));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _HidePanles();
           _ShowNewAllOrdersInfo();
            ctrlDashboardDataInfo1.ctrlDashboardDataInfo_Load(clsGlobal.CurrentEmployee.EmployeeID.Value,
            MyGeneralUserControl.ctrlDashboardDataInfo.enLoginType.Employee);
            _ShowEmployeeInfoInCentre();
        }

        void _HideSubApplications()
        {
            if(pDrivingLicensesServices.Visible )
            {
                
                _HideSubDrivingLicensesServices();

                pDrivingLicensesServices.Visible = false;
                picArrowDrivingLicensesServices.Image = Resources.Down;
            }

            if(pManageApplications.Visible)
            {
                pManageApplications.Visible = false;
                btnManageApplications.Tag = "Down";
                btnManageApplications.Image = Resources.Down_32;
            }
            if(pDetainLicenses.Visible)
            {
                pDetainLicenses.Visible = false;
                btnDetainLicenses.Tag = "Down";
                btnDetainLicenses.Image = Resources.Down_32;
            }
            
        }

        void _HideSubDrivingLicensesServices()
        {
            if(pNewDrivingLicense.Visible)
            {
                pNewDrivingLicense.Visible = false;
                picArrowNewDrivingLicense.Image = Resources.Down;
            }
        }

        void _HidePanles()
        {
            if(pApplications.Visible)
            {
                pApplications.Visible = false;
                picArrowApplications.Image = Resources.Down;
                _HideSubApplications();
            }

            if (pAccountSettings.Visible)
            {
                pAccountSettings.Visible = false;              
            }

        }

        void _ChangeText(Button btn, string Text)
        {
            if (btn.Text == Text)
                btn.Text = null;
            else
                btn.Text = Text;
        }

        void _HideText()
        {
            _ChangeText(btnApplications, "Applications");
            _ChangeText(btnPeople, "People");
            _ChangeText(btnDrivers, "Drivers");
            _ChangeText(btnUsers, "Users");
            _ChangeText(btnEmployees, "Employees");
            _ChangeText(btnArchive, "Archive");
            _ChangeText(btnAccountSettings, "Account Settings");
        }


        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        void _RaiseTheOrdersOfUsers(object Control,string Tittle, string Text)
        {
            NotifyIcon Notification = Control as NotifyIcon;

            Notification.Icon = SystemIcons.Application;
            Notification.BalloonTipIcon = ToolTipIcon.Info;
            Notification.BalloonTipTitle = Tittle;
            Notification.BalloonTipText = Text;
            Notification.ShowBalloonTip(2000);
        }
        void _ShowNewAllOrdersInfo()
        {
            _AllOrdersInfoOfApp = clsUtil.GetAllNewOrdersOfApplications();
            if (_AllOrdersInfoOfApp.Count > 0)
            {
                foreach (var OrderInfo in _AllOrdersInfoOfApp)
                {
                    _RaiseTheOrdersOfUsers(NotificationForAddNewApp,"Notification For New Application Order",
                        $"\nBy UserID[{OrderInfo.UserID}], App Type[{clsApplication.GetStringOfApplicationType((clsApplication.enApplicationType)OrderInfo.ApplicationType)}]," +
                        $"\nTime : {OrderInfo.Datetime.ToString("g")}," +
                        $"\nIsPaid? {(OrderInfo.IsPaid ? "Yes" : "No")}");

                    _CurrentOrderOfApp = OrderInfo;
                }
            }

            _AllOrdersInfoOfAppointment = clsUtil.GetAllNewOrdersOfAppointments();
            if(_AllOrdersInfoOfAppointment.Count > 0)
            {
                foreach (var OrderInfo in _AllOrdersInfoOfAppointment)
                {
                    _RaiseTheOrdersOfUsers(NotificationForAddNewAppointment,"Notification For New Appointment Order",
                        $"By UserID[{OrderInfo.UserID}], LDLAppID[{OrderInfo.LDLAppID}]," +
                        $"\nTest Type[{OrderInfo.TestType}],"+
                        $"\nTime : {OrderInfo.Datetime.ToString("g")}."); 

                    _CurrentOrderOfAppointment = OrderInfo;
                }
            }

            _AllOrdersInfoOfIssueLiense = clsUtil.GetAllNewOrdersOfFirstIssueLicense();
            if(_AllOrdersInfoOfIssueLiense.Count > 0)
            {
                foreach (var OrderInfo in _AllOrdersInfoOfIssueLiense)
                {
                     _RaiseTheOrdersOfUsers(NotificationForIssueLicense, "Notification For New Appointment Order",
                    $"UserID: [{OrderInfo.UserID}], LDLAppID[{OrderInfo.LDLAppID}],"+
                    $"\nTime : {OrderInfo.DateTime.ToString("g")}.");

                    _CurrentOrderOfIssueLiense= OrderInfo;
                }
               
            }

        }
        private void TimerForAddNewApp_Tick(object sender, EventArgs e)
        {
            
        }

        List<ctrlAddNewApplication.NewOrderInfo> _AllOrdersInfoOfApp;
        ctrlAddNewApplication.NewOrderInfo _CurrentOrderOfApp;

        List<ctrlAddNewAppointment.clsRequestInfo> _AllOrdersInfoOfAppointment;
        ctrlAddNewAppointment.clsRequestInfo _CurrentOrderOfAppointment;

        List<ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder> _AllOrdersInfoOfIssueLiense;
        ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder _CurrentOrderOfIssueLiense;
        private void picMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
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
            
           if(pCenter.Controls.Count > 0)
            {
                pCenter.Controls.RemoveAt(0);
            }
            
            Form form =  frm as Form;
            form.TopLevel = false;
            form.Dock = DockStyle.Fill;
            form.BringToFront();
            pCenter.Tag = form;
            pCenter.Controls.Add(form);
            form.Show();
            
        }
        enum enStatus { Down, Up}
        void _ChangePictureBy(PictureBox pic, enStatus Status)
        {
            switch(Status)
            {
                case enStatus.Down:
                    {
                        pic.Tag = "Down";
                        pic.Image = Resources.Down; break;
                    }
                default:
                    {
                        pic.Tag = "Up";
                        pic.Image = Resources.Up; break;
                    }
            }
            }
        void _ChangePictureBy(PictureBox pic)
        {
            if(pic.Tag.ToString() == "Down")
            {
                _ChangePictureBy(pic, enStatus.Up);
            }
            else
            {
                _ChangePictureBy(pic, enStatus.Down);
            }
        }

        private void btnApplication_Click_1(object sender, EventArgs e)
        {
            if(pMenue.Width == WidthSmall)
                picMenue_Click(null, null);

            _HidePanles();
            _HideSubApplications();
            if(picArrowApplications.Tag.ToString() == "Down")
            {
                pApplications.Visible = true;
                picArrowDrivingLicensesServices.Visible = true;
            }   
            _ChangePictureBy(picArrowApplications);
        }

        private void btnPeople_Click(object sender, EventArgs e)
        {

        }

        private void button3_Click(object sender, EventArgs e)
        {
            _ShowFormInCentre(new frmShowPersonInfo(1));
        }

        private void btnDrivingLicensesServices_Click(object sender, EventArgs e)
        {
            _HideSubApplications();

            if (picArrowDrivingLicensesServices.Tag.ToString() == "Down")
            {
                pDrivingLicensesServices.Visible = true;
                picArrowNewDrivingLicense.Visible = true;

            }

           
            _ChangePictureBy(picArrowDrivingLicensesServices);
        }

        private void btnNewDrivingLicense_Click(object sender, EventArgs e)
        {
            _HideSubDrivingLicensesServices();
            if(picArrowNewDrivingLicense.Tag.ToString() == "Down")
            {
                pNewDrivingLicense.Visible = true;
            }           
            _ChangePictureBy(picArrowNewDrivingLicense);
        }

        private void btnRenewDrivingLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.RenewDrivingLicense))
            {
                clsGlobal.PermisionMessage("RenewDrivingLicense");
                return;
            }
            _HideSubDrivingLicensesServices();
            frmRenewLocalDrivingLicenseApplication renewLocalDrivingLicenseApplication 
                = new frmRenewLocalDrivingLicenseApplication();
            renewLocalDrivingLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(renewLocalDrivingLicenseApplication);

        }

        private void btnReplacementforLostorDamagedLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReplacementforLostorDamagedLicense))
            {
                clsGlobal.PermisionMessage("ReplacementforLostorDamagedLicense");
                return;
            }
            _HideSubDrivingLicensesServices();
            frmReplaceLostOrDamagedLicenseApplication replaceLostOrDamagedLicenseApplication
                = new frmReplaceLostOrDamagedLicenseApplication();
            replaceLostOrDamagedLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(replaceLostOrDamagedLicenseApplication);

        }

        private void btnReleaseDetainedDrivingLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReleaseDetainedLicense))
            {
                clsGlobal.PermisionMessage("ReleaseDetainedLicense");
                return;
            }
            _HideSubDrivingLicensesServices();
            frmReleaseDetainedLicenseApplication releaseDetainedLicenseApplication
                = new frmReleaseDetainedLicenseApplication();
            releaseDetainedLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(releaseDetainedLicenseApplication);

        }

        private void btnRetakeTest_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageLocalLicenses))
            {
                clsGlobal.PermisionMessage("ManageLocalLicenses");
                return;
            }
            _HideSubDrivingLicensesServices();
            frmListLocalDrivingLicesnseApplications listLocalDrivingLicesnseApplications
               = new frmListLocalDrivingLicesnseApplications(); 
            listLocalDrivingLicesnseApplications.OnClosed+= _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listLocalDrivingLicesnseApplications);

        }

        private void btnManageApplications_Click(object sender, EventArgs e)
        {
            _HideSubApplications();
            if (btnManageApplications.Tag.ToString() == "Down")
            {
              pManageApplications.Visible = true;
              btnManageApplications.Tag = "Up";
                btnManageApplications.Image = Resources.Up_32;
            }
            else
            {
                btnManageApplications.Tag = "Down";
                btnManageApplications.Image = Resources.Down_32;
            }
            
        }

        private void btnDetainLicenses_Click(object sender, EventArgs e)
        {
            _HideSubApplications();
            if (btnDetainLicenses.Tag.ToString() == "Down")
            {
                pDetainLicenses.Visible = true;
                btnDetainLicenses.Tag = "Up";
                btnDetainLicenses.Image = Resources.Up_32;
            }
            else
            {
                btnDetainLicenses.Tag = "Down";
                btnDetainLicenses.Image = Resources.Down_32;
            }
        }

      

        private void btnManageApplicationTypes_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageApplicationTypes))
            {
                clsGlobal.PermisionMessage("ManageApplicationTypes");
                return;
            }
            _HideSubApplications();
            frmListApplicationTypes listApplicationTypes = new frmListApplicationTypes();
            listApplicationTypes.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listApplicationTypes);

        }

        private void btnManageTestTypes_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageTestTypes))
            {
                clsGlobal.PermisionMessage("ManageTestTypes");
                return;
            }
            _HideSubApplications();
            frmListTestTypes listTestTypes = new frmListTestTypes();
            listTestTypes.OnClosed += _ShowEmployeeInfoInCentre;
           _ShowFormInCentre(listTestTypes);

        }



        private void btnPeople_Click_1(object sender, EventArgs e)
        {
             if(pMenue.Width == WidthSmall)
                picMenue_Click(null, null);
            _HidePanles();
            frmListPeople listPeople = new frmListPeople();
            listPeople.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listPeople);
        }

        private void btnDrivers_Click(object sender, EventArgs e)
        {
             if(pMenue.Width == WidthSmall)
                picMenue_Click(null, null);
            _HidePanles();
            frmListDrivers listDrivers = new frmListDrivers();
            listDrivers.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listDrivers);
        }

        private void btnUsers_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthSmall)
                picMenue_Click(null, null);
            _HidePanles();
            frmListUsers listUsers = new frmListUsers();
            listUsers.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listUsers);

        }

        byte WidthSmall = 89;
        short WidthBig = 381;
        private void picMenue_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthBig)
            {
                pMenue.Width = WidthSmall;
            }
            else
                pMenue.Width = WidthBig;

            _HidePanles();
            _HideText();
            _ShowNewAllOrdersInfo();
        }

        private void btnAccountSettings_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthSmall)
                picMenue_Click(null, null);

            _HidePanles();
            pAccountSettings.Visible = true;
        }

        private void btnLocalLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewLocalLicense))
            {
                clsGlobal.PermisionMessage("AddNewLocalLicense");
                return;
            }
            frmAddUpdateLocalDrivingLicesnseApplication addUpdateLocalDrivingLicesnseApplication
                = new frmAddUpdateLocalDrivingLicesnseApplication();
            addUpdateLocalDrivingLicesnseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(addUpdateLocalDrivingLicesnseApplication);
        }

        private void btnInternationalLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewInternationalLicense))
            {
                clsGlobal.PermisionMessage("AddNewInternationalLicense");
                return;
            }
            frmNewInternationalLicenseApplication newInternationalLicenseApplication
                = new frmNewInternationalLicenseApplication();
            newInternationalLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(newInternationalLicenseApplication);
        }

        private void btnLocalDrivingLicenseApplications_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageLocalLicenses))
            {
                clsGlobal.PermisionMessage("ManageLocalLicenses");
                return;
            }
            frmListLocalDrivingLicesnseApplications listLocalDrivingLicesnseApplications
                = new frmListLocalDrivingLicesnseApplications();
            listLocalDrivingLicesnseApplications.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listLocalDrivingLicesnseApplications);

        }

        private void btnInternationalLicenseApplications_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageInternationalLicenses))
            {
                clsGlobal.PermisionMessage("ManageInternationalLicenses");
                return;
            }
            frmListInternationalLicesnseApplications listInternationalLicesnseApplications
                = new frmListInternationalLicesnseApplications();
            listInternationalLicesnseApplications.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listInternationalLicesnseApplications);

        }

        private void btnManageDetainedLicenses_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ManageDetainLicense))
            {
                clsGlobal.PermisionMessage("ManageDetainLicense");
                return;
            }
            frmListDetainedLicenses listDetainedLicenses =
                new frmListDetainedLicenses();
            listDetainedLicenses.OnClosed += _ShowEmployeeInfoInCentre;

            _ShowFormInCentre(listDetainedLicenses);

        }

        private void btnDetainLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.DetainLicense))
            {
                clsGlobal.PermisionMessage("DetainLicense");
                return;
            }
            frmDetainLicenseApplication detainLicenseApplication = new frmDetainLicenseApplication();
            detainLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(detainLicenseApplication);

        }

        private void btnReleaseDetainedLicense_Click(object sender, EventArgs e)
        {
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReleaseDetainedLicense))
            {
                clsGlobal.PermisionMessage("ReleaseDetainedLicense");
                return;
            }
            frmReleaseDetainedLicenseApplication releaseDetainedLicenseApplication = new frmReleaseDetainedLicenseApplication();
            releaseDetainedLicenseApplication.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(releaseDetainedLicenseApplication);

        }

        private void btnCurrentUserInfo_Click(object sender, EventArgs e)
        {
            frmEmployeeInfo employeeInfo
                = new frmEmployeeInfo(clsGlobal.CurrentEmployee.EmployeeID ?? -1);
            employeeInfo.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(employeeInfo);
        }

        private void btnChangePassword_Click(object sender, EventArgs e)
        {
            frmChangePassword changePassword
                = new frmChangePassword(clsGlobal.CurrentEmployee.UserID.Value);
            changePassword.OnClose += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(changePassword);
        }

        private void btnYourEmail_Click(object sender, EventArgs e)
        {
            frmShowEmail showEmail =
                new frmShowEmail(clsGlobal.CurrentEmployee.EmployeeID.Value, clsEmail.enFor.ForEmployee, clsEmail.enFrom.ByEmployee);
            showEmail.OnClose += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(showEmail);
        }

        private void btnYourCallLog_Click(object sender, EventArgs e)
        {
            frmShowCallLog showCallLog
                = new frmShowCallLog(clsGlobal.CurrentEmployee.EmployeeID.Value, DVLD_Buisness.Classes.clsCommunication.enFrom.ByEmployee);
            showCallLog.OnClose += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(showCallLog);
        }
       
        private void btnSignOut_Click(object sender, EventArgs e)
        {
            clsGlobal.CurrentEmployee = null;
           
            _frmLogin.Show();

            this.Close();
        }

        private void ctrlBloodTest1_CompletedBloodTest(object sender, ctrlBloodTest.clsBloodTest e)
        {
            MessageBox.Show($"NumberOfBlood : {e.NumberOfBloodTest} %,\nBlood Type : {e.BloodTypeString}");
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void btnEmployees_Click(object sender, EventArgs e)
        {
            if (pMenue.Width == WidthSmall)
                picMenue_Click(null, null);
            _HidePanles();
            frmListEmployees listEmployees = new frmListEmployees();
            listEmployees.OnClosed += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(listEmployees);
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

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            if(MessageBox.Show($"This application Will add by:\nEmployeeID {clsGlobal.CurrentEmployee.EmployeeID.Value}",
                "Add New Application", MessageBoxButtons.OKCancel, MessageBoxIcon.Question ) == DialogResult.Cancel)
            {
                return;
            }
            int ActiveLicenseID = clsLicense.GetActiveLicenseIDByPersonID(_CurrentOrderOfApp.UserInfo.PersonID, _CurrentOrderOfApp.LicenseClass);
            int LastLicenseID = clsLicense.GetLastLicenseID
                (_CurrentOrderOfApp.UserInfo.PersonID,
                _CurrentOrderOfApp.LicenseClass);

            if (_CurrentOrderOfApp.LicenseType == clsDetainedLicense.enLicenseType.International)
            {
                ActiveLicenseID = clsInternationalLicense
                    .GetActiveInternationalLicenseIDBy
                    (clsDriver.FindByPersonID(_CurrentOrderOfApp.UserInfo.PersonID).DriverID.Value
                    , ActiveLicenseID);

                LastLicenseID = clsInternationalLicense.GetLastIntLicenseID
                    (_CurrentOrderOfApp.UserInfo.PersonID,
                _CurrentOrderOfApp.LicenseClass);
            }

            switch ((clsApplication.enApplicationType)_CurrentOrderOfApp.ApplicationType)
            {
                    case clsApplication.enApplicationType.NewDrivingLicense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewLocalLicense))
                        {
                            clsGlobal.PermisionMessage("AddNewLocalLicense");
                            return;
                        }
                        frmAddUpdateLocalDrivingLicesnseApplication AddNewLocalDrivingLicesnseApplication
                            = new frmAddUpdateLocalDrivingLicesnseApplication(_CurrentOrderOfApp.UserInfo.PersonID, _CurrentOrderOfApp.LicenseClass-1);
                        _ShowFormInCentre(AddNewLocalDrivingLicesnseApplication);
                        return;
                    }
                    case clsApplication.enApplicationType.RenewDrivingLicense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.RenewDrivingLicense))
                        {
                            clsGlobal.PermisionMessage("RenewDrivingLicense");
                            return;
                        }
                        frmRenewLocalDrivingLicenseApplication renewLocalDrivingLicenseApplication = new frmRenewLocalDrivingLicenseApplication();
                        renewLocalDrivingLicenseApplication.frmRenewLocalDrivingLicenseApplication_Load
                            (ActiveLicenseID, _CurrentOrderOfApp.LicenseType == clsDetainedLicense.enLicenseType.Local);
                        _ShowFormInCentre(renewLocalDrivingLicenseApplication);
                        break;
                    }
                    case clsApplication.enApplicationType.ReplaceLostDrivingLicense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReplacementforLostorDamagedLicense))
                        {
                            clsGlobal.PermisionMessage("ReplacementforLostorDamagedLicense");
                            return;
                        }
                        frmReplaceLostOrDamagedLicenseApplication ReplaceLostOrDamagedLicenseApplication = new frmReplaceLostOrDamagedLicenseApplication();
                        ReplaceLostOrDamagedLicenseApplication.frmReplaceLostOrDamagedLicenseApplication_Load(ActiveLicenseID,
                            _CurrentOrderOfApp.LicenseType == clsDetainedLicense.enLicenseType.Local,
                            true);
                        _ShowFormInCentre(ReplaceLostOrDamagedLicenseApplication);
                        break;
                    }
                    case clsApplication.enApplicationType.ReplaceDamagedDrivingLicense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReplacementforLostorDamagedLicense))
                        {
                            clsGlobal.PermisionMessage("ReplacementforLostorDamagedLicense");
                            return;
                        }
                        frmReplaceLostOrDamagedLicenseApplication ReplaceLostOrDamagedLicenseApplication = new frmReplaceLostOrDamagedLicenseApplication();
                        ReplaceLostOrDamagedLicenseApplication.frmReplaceLostOrDamagedLicenseApplication_Load(ActiveLicenseID,
                            _CurrentOrderOfApp.LicenseType == clsDetainedLicense.enLicenseType.Local, false);
                        _ShowFormInCentre(ReplaceLostOrDamagedLicenseApplication);
                        break;
                    }
                    case clsApplication.enApplicationType.ReleaseDetainedDrivingLicsense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.ReleaseDetainedLicense))
                        {
                            clsGlobal.PermisionMessage("ReleaseDetainedLicense");
                            return;
                        }
                        frmReleaseDetainedLicenseApplication ReleaseDetainedLicenseApplication
                            = new frmReleaseDetainedLicenseApplication(LastLicenseID, 
                            _CurrentOrderOfApp.LicenseType == clsDetainedLicense.enLicenseType.Local);
                        _ShowFormInCentre(ReleaseDetainedLicenseApplication);
                        break;
                    }
                    case clsApplication.enApplicationType.NewInternationalLicense:
                    {
                        if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewInternationalLicense))
                        {
                            clsGlobal.PermisionMessage("AddNewInternationalLicense");
                            return;
                        }
                        frmNewInternationalLicenseApplication NewInternationalLicenseApplication = new frmNewInternationalLicenseApplication(ActiveLicenseID);
                        _ShowFormInCentre(NewInternationalLicenseApplication);
                        break;
                    }

            }


        }

        private void NotificationForAddNewApp_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void NotificationForAddNewAppointment_BalloonTipClicked(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This Appointment Will add by:\nEmployeeID {clsGlobal.CurrentEmployee.EmployeeID.Value}",
               "Add New Appointment", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.SechduleTests))
            {
                clsGlobal.PermisionMessage("SechduleTests");
                return;
            }

            frmScheduleTest scheduleTest = new frmScheduleTest(_CurrentOrderOfAppointment.LDLAppID, _CurrentOrderOfAppointment.TestType);
            _ShowFormInCentre(scheduleTest);

        }

        private void NotificationForAddNewAppointment_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void NotificationForIssueLicense_MouseDoubleClick(object sender, MouseEventArgs e)
        {

        }

        private void NotificationForIssueLicense_BalloonTipClicked(object sender, EventArgs e)
        {
            if (MessageBox.Show($"This Appointment Will add by:\nEmployeeID {clsGlobal.CurrentEmployee.EmployeeID.Value}",
              "Issue New License", MessageBoxButtons.OKCancel, MessageBoxIcon.Question) == DialogResult.Cancel)
            {
                return;
            }
            if (!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.IssueLicenseFirstTime))
            {
                clsGlobal.PermisionMessage("IssueLicenseFirstTime");
                return;
            }

            frmIssueDriverLicenseFirstTime issueDriverLicenseFirstTime = new frmIssueDriverLicenseFirstTime(_CurrentOrderOfIssueLiense.LDLAppID);
            _ShowFormInCentre(issueDriverLicenseFirstTime);
        }

        private void btnArchive_Click(object sender, EventArgs e)
        {
           frmAllArchive frmAllArchive = new frmAllArchive();
            frmAllArchive.OnClose += _ShowEmployeeInfoInCentre;
            _ShowFormInCentre(frmAllArchive);
            
        }

        private void NotificationForIssueLicense_MouseDoubleClick_1(object sender, MouseEventArgs e)
        {

        }
    }
}
