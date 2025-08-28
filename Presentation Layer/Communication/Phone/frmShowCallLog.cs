using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Communication.Phone
{
    public partial class frmShowCallLog : Form
    {
        int _CallerID;
        clsCallLog.enFrom _CallerType;
        public frmShowCallLog(int CallerID, clsCallLog.enFrom CallerType )
        {
            InitializeComponent();
           _CallerID = CallerID;
           _CallerType = CallerType;

        }
        string _GetFirstname()
        {
            switch (_CallerType)
            {
                case DVLD_Buisness.Classes.clsCommunication.enFrom.ByPerson:
                    {
                        return clsPerson.Find(_CallerID).FirstName;
                    }
                case DVLD_Buisness.Classes.clsCommunication.enFrom.ByUser:
                    {
                        return clsUser.FindByUserID(_CallerID).PersonInfo.FirstName;
                    }
                default:
                    {
                        return clsEmployee.FindByEmployeeID(_CallerID).PersonInfo.FirstName;
                    }
            }

        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }
        
        private void picAddCallPhone_Click(object sender, EventArgs e)
        {
            
            frmCallPhoneWithFilter callPhoneWithFilter = new frmCallPhoneWithFilter(_CallerID, _CallerType);
            callPhoneWithFilter.ShowDialog();
            
            frmShowCallLog_Load(null, null);
        }

        private void frmShowCallLog_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            byte CallerType = (byte)_CallerType;
            clsCallLog.enFor RecipientType = (DVLD_Buisness.Classes.clsCommunication.enFor)CallerType;

            ctrlYourPhoneLog1.YourPhoneCall_For(_CallerID, RecipientType, _CallerType);
            lblPhoneLogName.Text = _GetFirstname() +"`s Phone Log";
        }

        private void ctrlYourPhoneLog1_Load(object sender, EventArgs e)
        {

        }
    }
}
