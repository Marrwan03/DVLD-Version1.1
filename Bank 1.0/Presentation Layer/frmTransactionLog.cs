using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Bank
{
    public partial class frmTransactionLog : Form
    {
        int _UserID;
        public frmTransactionLog(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmTransactionLog_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            ctrlGetOperationLogByForWithdraw1.ctrlGetOperationLogBy_Load(Bank.Controls.ctrlGetOperationLogBy.enTypeOfOperation.Withdraw, _UserID);
            ctrlGetOperationLogByForDeposite2.ctrlGetOperationLogBy_Load(Bank.Controls.ctrlGetOperationLogBy.enTypeOfOperation.Deposite, _UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
