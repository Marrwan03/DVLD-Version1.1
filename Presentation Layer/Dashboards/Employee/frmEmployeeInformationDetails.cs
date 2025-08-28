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

namespace DVLD.Dashboards.Employee
{
    public partial class frmEmployeeInformationDetails : Form
    {
        int _EmployeeID;
        clsEmployee _CurrentEmployee;
        public frmEmployeeInformationDetails(int EmployeeID)
        {
            InitializeComponent();
            _EmployeeID = EmployeeID;
        }

        private void frmEmployeeInformationDetails_Load(object sender, EventArgs e)
        {
            _CurrentEmployee = clsEmployee.FindByEmployeeID(_EmployeeID);
            if (_CurrentEmployee != null)
            {
                ctrlPersonCard1.LoadPersonInfo(_CurrentEmployee.PersonID);
                ctrlCreatedSection1.ctrlCreatedSection_Load(_CurrentEmployee.EmployeeID.Value);

            }
            else
            {
                MessageBox.Show($"There isn`t employee with this ID[{_EmployeeID}].", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
