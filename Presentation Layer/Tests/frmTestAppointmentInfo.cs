using DVLD.Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmTestAppointmentInfo : Form
    {
        int _AppointmentID;
        public frmTestAppointmentInfo(int AppointmentID)
        {
            InitializeComponent();
            _AppointmentID = AppointmentID;
        }

        private void frmTestAppointmentInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlAppointmentInfo1.ctrlAppointmentInfo_Load(_AppointmentID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
