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

namespace DVLD.Dashboards.Employee.Controls
{
    public partial class ctrlCreatedSection : UserControl
    {
        int _EmployeeID;
        public ctrlCreatedSection()
        {
            InitializeComponent();
        }

        public void ctrlCreatedSection_Load(int EmployeeID)
        {
            _EmployeeID = EmployeeID;
            clsEmployee employee = clsEmployee.FindByEmployeeID(_EmployeeID);
            if (employee != null)
            {
                lblNumberOfCreatedLocalLicenses.Text = employee.GetNumberOfCreatedLocalLicense().ToString();
                lblNumberOfCreatedInternationalicenses.Text = employee.GetNumberOfCreatedInternationalLicense().ToString();
                lblNumberOfCreatedApplication.Text = employee.GetNumberOfCreatedApp().ToString();
                lblNumberOfCreatedAppointment.Text = employee.GetNumberOfCreatedAppointment().ToString();
            }
            else
            {
                MessageBox.Show($"There isn`t any employee with this ID[{_EmployeeID}]", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
