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
    public partial class frmListDeposite : Form
    {
        int _UserID;
        public frmListDeposite(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }

        private void frmListDeposite_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            ctrlGetOperationLogBy1.ctrlGetOperationLogBy_Load(Bank.Controls.ctrlGetOperationLogBy.enTypeOfOperation.Deposite, _UserID);
        }
    }
}
