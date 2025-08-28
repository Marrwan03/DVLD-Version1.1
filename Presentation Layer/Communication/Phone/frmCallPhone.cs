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
using static Guna.UI2.Native.WinApi;

namespace DVLD.Communication.Phone
{
    public partial class frmCallPhone : Form
    {
        int _CallerID;
        clsCallLog.enFrom _CallerType;
        int _RecipientID;
        clsCallLog.enFor _RecipientType;
        int Duration = 0;
        
        public frmCallPhone(int CallerID,clsCallLog.enFrom CallerType, int RecipientID, clsCallLog.enFor RecipientType)
        {
            InitializeComponent();
           _CallerID = CallerID;
            _CallerType = CallerType;
            _RecipientID = RecipientID;
            _RecipientType = RecipientType;
            Duration = 0;

        }

        private void frmCallPhone_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlCallPhone1.ctrlCallPhone_Load(_RecipientID, _RecipientType);
           
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlCallPhone1_OnCallOff(Controls.ctrlCallPhone.clsEventArgs obj)
        {
            clsCallLog _CallLog = new clsCallLog();
            _CallLog.CallerID = _CallerID;
            _CallLog.CallerType = _CallerType;
            _CallLog.PhoneNumber = obj.NumberPhone;
            _CallLog.Duration = obj.Duration;
            _CallLog.CallTime = DateTime.Now.AddSeconds(-Duration);
            _CallLog.CallType = clsCallLog.enCallType.VoiceCall;
            _CallLog.CallStatus = clsCallLog.enStatus.Missed;
            _CallLog.RecipientID = _RecipientID;
            _CallLog.RecipientType = _RecipientType;

            if (_CallLog.Save())
            {
                if (MessageBox.Show("The call has been saved", "Save", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                    this.Close();
            }
            else
            {
                MessageBox.Show("The call hasn`t been saved", "Save", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }

        private void ctrlCallPhone1_Load(object sender, EventArgs e)
        {

        }
    }
}
