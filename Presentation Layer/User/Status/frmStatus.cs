using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.User.Status
{
    public partial class frmStatus : Form
    {
        int _PersonID;
        public frmStatus(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }

        private void frmStatus_Load(object sender, EventArgs e)
        {
            ctrlStatus1.ctrlStatus_Load(_PersonID);
        }

        public Action OnClose;
       private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }
    }
}
