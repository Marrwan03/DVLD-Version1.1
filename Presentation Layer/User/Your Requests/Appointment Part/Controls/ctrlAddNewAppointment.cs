using DVLD.Classes;
using DVLD.MyCustomeControls;
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
using static DVLD.Applications.Controls.ctrlAddNewApplication;
using static DVLD_Buisness.clsLicense;

namespace DVLD.User.Your_Requests.Appointment_Part.Controls
{
    public partial class ctrlAddNewAppointment : UserControl
    {
        public ctrlAddNewAppointment()
        {
            InitializeComponent();
        }

        public class clsRequestInfo : EventArgs
        {
            public int UserID { get; set; }
            public int LDLAppID { get; }
            public clsTestType.enTestType TestType { get; }
            public DateTime Datetime { get; set; }
            public clsRequestInfo(int userID,int lDLAppID, clsTestType.enTestType testType, DateTime dateTime)
            {
                UserID = userID;
                LDLAppID = lDLAppID;
                TestType = testType;
                Datetime = dateTime;
            }

            public override string ToString()
            {
                return $"{UserID}#//#{LDLAppID}#//#{(int)TestType}#//#{Datetime}";
            }

            public static clsRequestInfo FromString(string str)
            {
                string[] result = str.Split(new string[] { "#//#" }, StringSplitOptions.None);

                return new clsRequestInfo(Convert.ToInt32(result[0]), Convert.ToInt32(result[1]),
                    (clsTestType.enTestType)Convert.ToInt32(result[2]),Convert.ToDateTime(result[3]));
            }

        }
        public event EventHandler<clsRequestInfo> OnRequest;

        int _UserID;
        clsUser _UserInfo;
        int _LDLAppID;
        clsTestType.enTestType _TestType;



        public void ctrlAddNewAppointment_Load(int UserID)
        {
            _UserID = UserID;
            _UserInfo = clsUser.FindByUserID(UserID);
            if(_UserInfo != null )
            {
                lblFullName.Text = _UserInfo.PersonInfo.FullName;
                cbAllLDLAppID1.FillcbAllLDLAppIDBy(_UserInfo.PersonInfo.PersonID.Value);
                if (cbAllLDLAppID1.Items.Count > 0)
                {
                    cbTestType1.FillcbTestType();
                    cbAllLDLAppID1.SelectedIndex = 0;
                    cbTestType1.SelectedIndex = 0;
                }
                else
                {

                   if( MessageBox.Show("You don`t have a license Appliction ID", "Error!",
                        MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                    {
                        cbAllLDLAppID1.Enabled = false;
                        cbTestType1.Enabled = false;
                        btnSendRequest.Enabled = false;
                    }
                }
            }

        }

        bool _AcceptRequest()
        {
            List<clsRequestInfo> AllRequests = clsUtil.GetAllNewOrdersOfAppointments();
            if(AllRequests.Count >0 )
            {
                if (AllRequests.Exists(x => x.UserID == _UserID && x.LDLAppID == _LDLAppID && x.TestType == _TestType))
                 return false;
            }

            bool Accept = true;
            switch (_TestType)
            {
                case clsTestType.enTestType.VisionTest:
                    {
                        if(clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.VisionTest))
                        {
                            Accept = false;
                        }
                        break;
                    }
                case clsTestType.enTestType.WrittenTest:
                    {
                        if (!clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.VisionTest)
                            || clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.WrittenTest))
                        {
                            Accept = false;
                        }
                        break;
                    }
                case clsTestType.enTestType.StreetTest:
                    {
                        if (!clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.VisionTest)
                            || !clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.WrittenTest)
                            || clsLocalDrivingLicenseApplication.DoesPassTestType(_LDLAppID, clsTestType.enTestType.StreetTest))
                        {
                            Accept = false;
                        }
                        break;
                    }
            }
            return Accept;
        }

        private void btnSendRequest_Click(object sender, EventArgs e)
        {
            //set validations
            if(!_AcceptRequest())
            {
                MessageBox.Show($"You cannot choice this TestType[{cbTestType1.Text}],\nBecause you already test it and pass OR You don`t do the last tests",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                cbTestType1.Focus();
                return; 
            }

            if (MessageBox.Show("Are you sure for this Request?", "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
                return;

            OnRequest?.Invoke(this, new clsRequestInfo(_UserID,_LDLAppID, _TestType, DateTime.Now));

        }

        private void cbAllLDLAppID1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _LDLAppID = Convert.ToInt32(cbAllLDLAppID1.Text);
        }

        private void cbTestType1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _TestType = clsTestType.Find(cbTestType1.Text).ID;
        }
    }
}
