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

namespace DVLD.Employee
{
    public partial class frmFindEmployee : Form
    {
        public frmFindEmployee()
        {
            InitializeComponent();
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlEmployeeCardWithFilter1_OnEmployeeAdded(int obj)
        {
            notifyIcon1.BalloonTipText = "Added New Employee";
            notifyIcon1.BalloonTipTitle = $"New Employee ID : {obj}";
            
        }

        private void ctrlEmployeeCardWithFilter1_OnEmployeeSelected(int obj)
        {
            notifyIcon1.BalloonTipText = "Update Employee";
            notifyIcon1.BalloonTipTitle = $"Update Employee ID : {obj}";
        }

        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            
        }

        private void frmFindEmployee_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
        }
    }
}
