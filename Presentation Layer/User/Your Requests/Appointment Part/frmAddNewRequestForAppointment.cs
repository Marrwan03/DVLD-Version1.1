using DVLD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User.Your_Requests.Appointment_Part
{
    public partial class frmAddNewRequestForAppointment : Form
    {

        public int _UserID;
        public frmAddNewRequestForAppointment(int userID)
        {
            InitializeComponent();
            _UserID = userID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmAddNewRequestForAppointment_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlAddNewAppointment1.ctrlAddNewAppointment_Load(_UserID);
        }

        private void ctrlAddNewAppointment1_OnRequest(object sender, Controls.ctrlAddNewAppointment.clsRequestInfo e)
        {

           if(clsUtil.AddNewOrderInFile(ConfigurationManager.AppSettings["FileNameOfAppointmentsOrder"], e.ToString()))
            {
                if(MessageBox.Show("This Order is Entered Successfully", "Add New Appointment", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
                {
                    btnClose.PerformClick();
                }
            }
           else
            {
                MessageBox.Show("This Order is Failed Successfully", "Add New Appointment", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

        }
    }
}
