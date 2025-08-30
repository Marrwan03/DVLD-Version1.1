using DataLogic2;
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

namespace Bank
{
    public partial class frmListWithdraw : Form
    {
        int _UserID;

        public frmListWithdraw(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmWithdraw_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlGetOperationLogBy1.ctrlGetOperationLogBy_Load(Bank.Controls.ctrlGetOperationLogBy.enTypeOfOperation.Withdraw, _UserID);

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

       
    }
}
