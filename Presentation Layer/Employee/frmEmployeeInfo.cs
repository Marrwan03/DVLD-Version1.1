using DVLD.Classes;
using System;
using System.Windows.Forms;

namespace DVLD.Employee
{
    public partial class frmEmployeeInfo : Form
    {
        int _EmployeeID;
        public frmEmployeeInfo(int EmployeeID)
        {
            InitializeComponent();
            _EmployeeID = EmployeeID;
        }

        private void frmEmployeeInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlEmployeeCard1.ctrlEmployeeCard_Load(_EmployeeID);
        }

        public Action OnClosed;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }
    }
}
