using DVLD_Buisness;
using System;
using DVLD_Buisness.Classes;
using System.Windows.Forms;
using iText.Layout.Splitting;

namespace DVLD.Communication.Phone.Controls
{
    public partial class ctrlShowPhoneLogInfo : UserControl
    {
        public ctrlShowPhoneLogInfo()
        {
            InitializeComponent();
        }
        clsCallLog _CallLog;

        void _LoadData()
        {
            #region SetCallerInfo
            switch (_CallLog.CallerType)
            {
                case clsCommunication.enFrom.ByPerson:
                    {
                        clsPerson CallerPersonInfo = clsPerson.Find(_CallLog.CallerID);
                        lblCallerName.Text = CallerPersonInfo.FirstName + " " + CallerPersonInfo.LastName;
                        lblCallerNumberPhone.Text = CallerPersonInfo.Phone;
                        lblCallerType.Text = _CallLog.GetStringOfFromType();
                        break;
                    }
                case clsCommunication.enFrom.ByUser:
                    {
                        clsUser CallerUserInfo  = clsUser.FindByUserID(_CallLog.CallerID);
                        lblCallerName.Text = CallerUserInfo.PersonInfo.FirstName + " " + CallerUserInfo.PersonInfo.LastName;
                        lblCallerNumberPhone.Text = CallerUserInfo.PersonInfo.Phone;
                        lblCallerType.Text = _CallLog.GetStringOfFromType();
                        break;
                    }
                case clsCommunication.enFrom.ByEmployee:
                    {
                        clsEmployee CallertEmployeeInfo = clsEmployee.FindByEmployeeID(_CallLog.CallerID);
                        lblCallerName.Text = CallertEmployeeInfo.PersonInfo.FirstName + " " + CallertEmployeeInfo.PersonInfo.LastName;
                        lblCallerNumberPhone.Text = CallertEmployeeInfo.PersonInfo.Phone;
                        lblCallerType.Text = _CallLog.GetStringOfFromType();
                        break;
                    }
            }

            
            #endregion

            lblStatus.Text = _CallLog.StringCallStatus;
            lblDuration.Text = _CallLog.Duration + " Second(s)";


            #region SetRecipientInfo

            switch (_CallLog.RecipientType)
            {
                case clsCommunication.enFor.ForPerson:
                    {
                        clsPerson RecipientPersonInfo = clsPerson.Find(_CallLog.RecipientID);
                        lblRecipientName.Text = RecipientPersonInfo.FirstName + " " + RecipientPersonInfo.LastName;
                        lblRecipientNumberPhone.Text = RecipientPersonInfo.Phone;
                        lblRecipientType.Text = _CallLog.GetRecipientType();
                        break;
                    }
                case clsCommunication.enFor.ForUser:
                    {
                        clsUser RecipientUserInfo = clsUser.FindByUserID(_CallLog.RecipientID);
                        lblRecipientName.Text = RecipientUserInfo.PersonInfo.FirstName + " " + RecipientUserInfo.PersonInfo.LastName;
                        lblRecipientNumberPhone.Text = RecipientUserInfo.PersonInfo.Phone;
                        lblRecipientType.Text = _CallLog.GetRecipientType();
                        break;
                    }
                case clsCommunication.enFor.ForEmployee:
                    {
                        clsEmployee RecipientEmployeeInfo = clsEmployee.FindByEmployeeID(_CallLog.RecipientID);
                        lblRecipientName.Text = RecipientEmployeeInfo.PersonInfo.FirstName + " " + RecipientEmployeeInfo.PersonInfo.LastName;
                        lblRecipientNumberPhone.Text = RecipientEmployeeInfo.PersonInfo.Phone;
                        lblRecipientType.Text = _CallLog.GetRecipientType();
                        break;
                    }
            }


            #endregion
           
            lblTimer.Text = _CallLog.CallTime.ToString("G");
            lblCallType.Text = _CallLog.StringCallType;

        }

        public void ctrlShowCallPhoneInfo_Load(int CallID)
        {
            _CallLog = clsCallLog.Find(CallID);
            if(_CallLog.CallID.HasValue)
            {
                _LoadData();
            }
            else
            {
                MessageBox.Show($"No Call Log with CallID [{CallID}]", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ctrlShowCallPhoneInfo_Load(object sender, EventArgs e)
        {

        }
    }
}
