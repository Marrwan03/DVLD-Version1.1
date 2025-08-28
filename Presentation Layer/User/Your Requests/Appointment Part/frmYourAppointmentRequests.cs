using DVLD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;

using System.Windows.Forms;

namespace DVLD.User.Your_Requests
{
    public partial class frmYourAppointmentRequests : Form
    {
        int _PersonID;
        public frmYourAppointmentRequests(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmYourAppointmentRequests_Load(object sender, EventArgs e)
        {
            ctrlAllTestAppointmentsInfo1.ctrlAllTestAppointmentInfo_Load(_PersonID);
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }

        private void ctrlAllTestAppointmentsInfo1_OnIssueLicense(object sender, Appointment_Part.Controls.ctrlAllTestAppointmentsInfo.clsIssueLicenseOrder e)
        {
            if (MessageBox.Show("Are you sure do you continue and issue a new license?", "Continue",
              MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.No)
            {
                return;
            }


            if (clsUtil.AddNewOrderInFile(ConfigurationManager.AppSettings["FileNameOfIssueLicenseOrder"], e.ToString()))
            {
                MessageBox.Show("The order is sended successfully", "Successful", MessageBoxButtons.OK, MessageBoxIcon.Information );
            }

        }
    }
}
