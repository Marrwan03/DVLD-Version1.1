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

namespace DVLD.User
{
    public partial class frmYourApplicationRequests : Form
    {
        int _PersonID;
        public frmYourApplicationRequests(int PersonID)
        {
            
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmYourOrders_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlYourDrivingLicenseApplications1.ctrlYourDrivingLicenseApplications_Load(_PersonID);
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }
    }
}
