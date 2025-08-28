using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Employee
{
    public partial class ctrlEmployeePermisions : UserControl
    {

        public ctrlEmployeePermisions()
        {
            InitializeComponent();
        }

        enMode _Mode;
        public enum enMode
        {
            Add, Update, Show
        }

        void EnableAllControls(bool enable)
        {
            
            tabDrivingLicensesServices.Enabled = enable;
            tabManageApplicationTypes.Enabled = enable;
            tabOthers.Enabled = enable;
            #region tabManageLocalDrivingLicenseApp
            chbShowApplicationDetails.Enabled = enable;
            chbEditApplication.Enabled = enable;
            chbIssueLicenseFirstTime.Enabled = enable;
            chbShowLicenseLDL.Enabled = enable;
            chbShowPersonLicenseHistory.Enabled = enable;
            chbSechduleTests.Enabled = enable;
            chbDeleteApplication.Enabled = enable;
            #endregion
            tabOther2.Enabled = enable;
            tabManageInternationalDrivingLicenseApp.Enabled = enable;
            tabManageTestTypes.Enabled = enable;
            tabAdvanced.Enabled = enable;

            tabPeople.Enabled = enable;

            tabUsers.Enabled = enable;

            tabEmployee.Enabled = enable;

           //btnSave.Enabled = enable;
           // chbGiveFullAccess.Enabled = enable;
        }

        public enMode Mode
        {
            get
            {
                return _Mode;

            }

            set
            {
                _Mode = value;

                switch (_Mode)
                {
                    case enMode.Add:
                        {
                            //lblTitle.Text = "Add Permision";
                            EnableAllControls(true);
                            break;
                        }
                    case enMode.Update:
                        {
                           // lblTitle.Text = "Update Permision";
                            EnableAllControls(true);
                            break;
                        }
                    case enMode.Show:
                        {
                          //  lblTitle.Text = "Show Permision";
                          
                            EnableAllControls(false);
                            break;
                        }
                }


            }

        }
        public event Action<long> OnEmployeePermisionChanged;
        clsEmployee _EmployeeInfo;
        public clsEmployee EmployeeInfo { get { return _EmployeeInfo; } }
        long _Permision = 0;
        public long Permision { get { return _Permision; } }
        public void ctrlUserPermision_Load(long Permisions)
        {
            if (Permisions == -1)
            {
                _Permision = Permisions;
                chbGiveFullAccess.Checked = true;
                CheckAllCheckBox(true);
                return;
            }


            chbRenewDrivingLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.RenewDrivingLicense, Permisions);
                chbReplacementforLostorDamagedLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ReplacementforLostorDamagedLicense, Permisions);
                chbReleaseDetainedLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ReleaseDetainedLicense, Permisions);
                chbDetainedLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DetainLicense, Permisions);
                

                chbEditApplicationType.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditApplicationType, Permisions);

                chbAddNewLocalLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewLocalLicense, Permisions);

                chbEditApplication.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditApplication, Permisions);
                chbIssueLicenseFirstTime.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.IssueLicenseFirstTime, Permisions);
                chbShowLicenseLDL.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseLDL, Permisions);
            chbShowPersonLicenseHistory.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonLicenseHistory, Permisions);
            chbShowApplicationDetails.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowApplicationDetails, Permisions);
                chbDeleteApplication.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteApplication, Permisions);
                chbSechduleTests.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.SechduleTests, Permisions);
   

                chbAddNewInternationalLicense.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewInternationalLicense, Permisions);
            chbShowLicenseInt.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowLicenseInt, Permisions);

            chbEditTestType.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditTestType, Permisions);

            chbReturntheArchives.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ReturnTheArchives, Permisions);
            chbEditAppointments.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditAppointment, Permisions);
            chbDeleteAppointments.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteAppointment, Permisions);

            chbDeletePersonPeople.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DeletePersonPeople, Permisions);
                chbAddNewPerson.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewPerson, Permisions);
                chbEditPersonPeople.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditPersonPeople, Permisions);
                chbSendEmailPeople.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailPeople, Permisions);
                chbPhoneCallPeople.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallPeople, Permisions);
            chbShowPersonEmail.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonEmail, Permisions);
            chbShowCallLogPeople.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPersonCallLog, Permisions);


            chbShowUserDetails.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserDetails, Permisions);
                chbEditUser.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditUser, Permisions);
                chbChangePasswordUser.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ChangePasswordUser, Permisions);
                
                chbAddNewUser.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewUser, Permisions);
                chbDeleteUser.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteUser, Permisions);
                chbSendEmailUsers.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailUsers, Permisions);
                chbPhoneCallUsers.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallUsers, Permisions);
            chbShowCallLogUsers.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserEmail, Permisions);
            chbShowCallLogUsers.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowUserCallLog, Permisions);

            chbShowEmployeeDetails.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowEmployeesDetails, Permisions);
            chbEditEmployee.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.EditEmployee, Permisions);
            chbAddNewEmployee.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewEmployee, Permisions);
            chbDeleteEmployee.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.DeleteEmployee, Permisions);
            chbShowPermisions.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowPermisions, Permisions);
            chbSetPermisions.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.SetPermision, Permisions);
            chbSendEmailEmployees.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.SendEmailEmployees, Permisions);
            chbShowEmailEmployees.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowEmployeesEmail, Permisions);
            chbPhoneCallEmployees.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.PhoneCallEmployees, Permisions);
            chbShowCallLogEmployees.Checked = clsEmployee.CheckAccessPermision(clsEmployee.enPermision.ShowEmployeesCallLog, Permisions);

        }
        private void ctrlUserPermisions_Load(object sender, EventArgs e)
        {

        }
        void CheckAllCheckBox(bool Checked)
        {
            CheckApplication(Checked);

            CheckPeopleGroupbox(Checked);

            CheckUsersGroupbox(Checked);

            CheckEmployeesGroupbox(Checked);

        }

        void CheckManageLocalLicenseGroupbox(bool Checked)
        {
            chbEditApplication.Checked = Checked;
            chbShowApplicationDetails.Checked = Checked;
            chbDeleteApplication.Checked = Checked;
            chbIssueLicenseFirstTime.Checked = Checked;
            chbSechduleTests.Checked = Checked;
            chbShowLicenseLDL.Checked = Checked;
            chbShowPersonLicenseHistory.Checked = Checked;
        }

        
        void CheckEmployeesGroupbox(bool Checked)
        {
            chbShowEmployeeDetails.Checked = Checked;
            chbEditEmployee.Checked =        Checked;
            chbAddNewEmployee.Checked =      Checked;
            chbDeleteEmployee.Checked =      Checked;
            chbShowPermisions.Checked =      Checked;
            chbSetPermisions.Checked =       Checked;
            chbSendEmailEmployees.Checked =  Checked;
            chbShowEmailEmployees.Checked =  Checked;
            chbPhoneCallEmployees.Checked =  Checked;
            chbShowCallLogEmployees.Checked = Checked;
        }
       
        void CheckPeopleGroupbox(bool Checked)
        {
            chbShowPersonDetailsPeople.Checked = Checked;
            chbEditPersonPeople.Checked = Checked;
            chbDeletePersonPeople.Checked = Checked;
            chbSendEmailPeople.Checked = Checked;
            chbAddNewPerson.Checked = Checked;
            chbPhoneCallPeople.Checked = Checked;
            chbShowPersonEmail.Checked = Checked;
            chbShowCallLogPeople.Checked = Checked;
        }

        void CheckUsersGroupbox(bool Checked)
        {
            chbShowUserDetails.Checked = Checked;
            chbAddNewUser.Checked = Checked;
            chbEditUser.Checked = Checked;
            chbDeleteUser.Checked = Checked;
            chbChangePasswordUser.Checked = Checked;
            chbSendEmailUsers.Checked = Checked;
            chbPhoneCallUsers.Checked = Checked;
            chbShowUserEmail.Checked = Checked;
            chbShowCallLogUsers.Checked = Checked;
        }

      

        void CheckApplication(bool Checked)
        {
            #region Driving Licenses Services Groupbox
            chbRenewDrivingLicense.Checked = Checked;
            chbReplacementforLostorDamagedLicense.Checked = Checked;
            #endregion
            #region Detain Licenses Groupbox
            chbDetainedLicense.Checked = Checked;
            chbReleaseDetainedLicense.Checked = Checked;
            #endregion
            #region Application Type Groupbox
            chbEditApplicationType.Checked = Checked;
            #endregion
            #region Manage Application Groupbox

            chbAddNewLocalLicense.Checked = Checked;
            CheckManageLocalLicenseGroupbox(Checked);

            chbAddNewInternationalLicense.Checked = Checked;
            chbShowLicenseInt.Checked = Checked;
            #endregion
            #region TestTypes
            chbEditTestType.Checked = Checked;
            #endregion
            #region Advance
            chbReturntheArchives.Checked = Checked;
            chbEditAppointments.Checked = Checked;
            chbDeleteAppointments.Checked = Checked;
            #endregion
        }

        long SumOfPermisionApplication()
        {
            long sum = 0;
            sum += clsEmployee.GetValueOfPermision(chbRenewDrivingLicense.Checked, clsEmployee.enPermision.RenewDrivingLicense);
            sum += clsEmployee.GetValueOfPermision(chbReplacementforLostorDamagedLicense.Checked, clsEmployee.enPermision.ReplacementforLostorDamagedLicense);
            sum += clsEmployee.GetValueOfPermision(chbEditApplication.Checked, clsEmployee.enPermision.EditApplication);
            sum += clsEmployee.GetValueOfPermision(chbIssueLicenseFirstTime.Checked, clsEmployee.enPermision.IssueLicenseFirstTime);
            sum += clsEmployee.GetValueOfPermision(chbShowApplicationDetails.Checked, clsEmployee.enPermision.ShowApplicationDetails);
            sum += clsEmployee.GetValueOfPermision(chbDeleteApplication.Checked, clsEmployee.enPermision.DeleteApplication);
            sum += clsEmployee.GetValueOfPermision(chbSechduleTests.Checked, clsEmployee.enPermision.SechduleTests);
            sum += clsEmployee.GetValueOfPermision(chbShowLicenseLDL.Checked, clsEmployee.enPermision.ShowLicenseLDL);
            sum += clsEmployee.GetValueOfPermision(chbShowPersonLicenseHistory.Checked, clsEmployee.enPermision.ShowPersonLicenseHistory);

            sum += clsEmployee.GetValueOfPermision(chbAddNewLocalLicense.Checked, clsEmployee.enPermision.AddNewLocalLicense);
            sum += clsEmployee.GetValueOfPermision(chbShowLicenseInt.Checked, clsEmployee.enPermision.ShowLicenseInt);
            sum += clsEmployee.GetValueOfPermision(chbAddNewInternationalLicense.Checked, clsEmployee.enPermision.AddNewInternationalLicense);

            sum += clsEmployee.GetValueOfPermision(chbReleaseDetainedLicense.Checked, clsEmployee.enPermision.ReleaseDetainedLicense);
            
            sum += clsEmployee.GetValueOfPermision(chbEditApplicationType.Checked, clsEmployee.enPermision.EditApplicationType);
            sum += clsEmployee.GetValueOfPermision(chbEditTestType.Checked, clsEmployee.enPermision.EditTestType);

            sum+= clsEmployee.GetValueOfPermision(chbReturntheArchives.Checked, clsEmployee.enPermision.ReturnTheArchives);
            sum += clsEmployee.GetValueOfPermision(chbDeleteAppointments.Checked, clsEmployee.enPermision.DeleteAppointment);
            sum += clsEmployee.GetValueOfPermision(chbEditAppointments.Checked, clsEmployee.enPermision.EditAppointment);

            return sum;
        }

        long SumOfPermisionPeople()
        {

            long sum = 0;
            sum += clsEmployee.GetValueOfPermision(chbDeletePersonPeople.Checked, clsEmployee.enPermision.DeletePersonPeople);
            sum += clsEmployee.GetValueOfPermision(chbAddNewPerson.Checked, clsEmployee.enPermision.AddNewPerson);
            sum += clsEmployee.GetValueOfPermision(chbEditPersonPeople.Checked, clsEmployee.enPermision.EditPersonPeople);
            sum += clsEmployee.GetValueOfPermision(chbSendEmailPeople.Checked, clsEmployee.enPermision.SendEmailPeople);
            sum += clsEmployee.GetValueOfPermision(chbPhoneCallPeople.Checked, clsEmployee.enPermision.PhoneCallPeople);
            sum += clsEmployee.GetValueOfPermision(chbShowPersonEmail.Checked, clsEmployee.enPermision.ShowPersonEmail);
            sum += clsEmployee.GetValueOfPermision(chbShowCallLogPeople.Checked, clsEmployee.enPermision.ShowPersonCallLog);
            return sum;
        }

        long SumOfPermisionUsers()
        {
            long sum = 0;
            sum += clsEmployee.GetValueOfPermision(chbShowUserDetails.Checked, clsEmployee.enPermision.ShowUserDetails);
            sum += clsEmployee.GetValueOfPermision(chbEditUser.Checked, clsEmployee.enPermision.EditUser);
            sum += clsEmployee.GetValueOfPermision(chbChangePasswordUser.Checked, clsEmployee.enPermision.ChangePasswordUser);
            sum += clsEmployee.GetValueOfPermision(chbAddNewUser.Checked, clsEmployee.enPermision.AddNewUser);
            sum += clsEmployee.GetValueOfPermision(chbDeleteUser.Checked, clsEmployee.enPermision.DeleteUser);
            sum += clsEmployee.GetValueOfPermision(chbSendEmailUsers.Checked, clsEmployee.enPermision.SendEmailUsers);
            sum += clsEmployee.GetValueOfPermision(chbPhoneCallUsers.Checked, clsEmployee.enPermision.PhoneCallUsers);
            sum += clsEmployee.GetValueOfPermision(chbShowUserEmail.Checked, clsEmployee.enPermision.ShowUserEmail);
            sum += clsEmployee.GetValueOfPermision(chbShowUserEmail.Checked, clsEmployee.enPermision.ShowUserCallLog);
            return sum;
        }

        long SumOfPermisionEmployees()
        {
            long sum = 0;

            sum += clsEmployee.GetValueOfPermision(chbShowEmployeeDetails.Checked, clsEmployee.enPermision.ShowEmployeesDetails);
            sum += clsEmployee.GetValueOfPermision(chbEditEmployee.Checked, clsEmployee.enPermision.EditEmployee);
            sum += clsEmployee.GetValueOfPermision(chbAddNewEmployee.Checked, clsEmployee.enPermision.AddNewEmployee);
            sum += clsEmployee.GetValueOfPermision(chbDeleteEmployee.Checked, clsEmployee.enPermision.DeleteEmployee);
            sum += clsEmployee.GetValueOfPermision(chbSendEmailEmployees.Checked, clsEmployee.enPermision.SendEmailEmployees);
            sum += clsEmployee.GetValueOfPermision(chbShowPermisions.Checked, clsEmployee.enPermision.ShowPermisions);
            sum += clsEmployee.GetValueOfPermision(chbSetPermisions.Checked, clsEmployee.enPermision.SetPermision);
            sum += clsEmployee.GetValueOfPermision(chbPhoneCallEmployees.Checked, clsEmployee.enPermision.PhoneCallEmployees);
            sum += clsEmployee.GetValueOfPermision(chbShowCallLogEmployees.Checked, clsEmployee.enPermision.ShowEmployeesCallLog);
            sum += clsEmployee.GetValueOfPermision(chbShowEmailEmployees.Checked, clsEmployee.enPermision.ShowEmployeesEmail);

            return sum;
        }
       

        private void btnSave_Click(object sender, EventArgs e)
        {
            if(_Mode == enMode.Show)
            {
                MessageBox.Show("You are in show mode you cannot save anything.\n\n\tJust Watch!", "Show Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }    
            if (MessageBox.Show("Are you sure do you want save data?", "Confirm!", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            { return; }


            if (!chbGiveFullAccess.Checked)
            {
                long Permision = SumOfPermisionApplication();
                Permision += SumOfPermisionPeople();
                Permision += SumOfPermisionUsers();
                Permision += SumOfPermisionEmployees();
                _Permision = Permision;
            }
            else
            {
                _Permision = -1;
            }

            if (_EmployeeInfo != null)
                _EmployeeInfo.ChangePermision(_Permision);

            if (OnEmployeePermisionChanged != null)
                OnEmployeePermisionChanged(_Permision);

            _Mode = enMode.Update;

            //EnableAllControls(false);
        }

        private void chbGiveFullAccess_CheckedChanged(object sender, EventArgs e)
        {
            if(_Mode == enMode.Show)
            {
                MessageBox.Show("You are in show mode you cannot Change anything.\n\n\tJust Watch!", "Show Mode", MessageBoxButtons.OK, MessageBoxIcon.Information);
               
                return;
            }
               

            if (chbGiveFullAccess.Checked)
            {
                CheckAllCheckBox(true);
                //chbGiveFullAccess.Checked = true;
               // _Permision = -1;
            }
            else
            {
                CheckAllCheckBox(false);
                _Permision = 0;
            }
        }
    }
}
