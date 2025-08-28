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

namespace DVLD.Payments
{
    public partial class frmAllPayments : Form
    {
        int _PersonID;
        public frmAllPayments(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmAllPayments_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlPayments1.ctrlPayments_Load(_PersonID);
        }
        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }
    }
}
