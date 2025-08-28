using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Windows.Forms;

namespace DVLD.Employee
{
    public partial class frmShowPermisions : Form
    {
        int _EmployeeID;
        public frmShowPermisions(int EmployeeID)
        {
            InitializeComponent();
            _EmployeeID = EmployeeID;
        }
        clsEmployee _EmployeeInfo;
        private void frmShowPermisions_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _EmployeeInfo = clsEmployee.FindByEmployeeID(_EmployeeID);
            if(_EmployeeInfo == null )
            {
                MessageBox.Show($"There isn`t employee with this id [ {_EmployeeID} ].", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            lblTitle.Text = $"Show ( {_EmployeeInfo.PersonInfo.FirstName} ) Permissions";
            ctrlEmployeePermisions2.Mode = Employee.ctrlEmployeePermisions.enMode.Show;
            ctrlEmployeePermisions2.ctrlUserPermision_Load(_EmployeeInfo.Permisions);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
