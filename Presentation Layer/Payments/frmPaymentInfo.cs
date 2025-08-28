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
    public partial class frmPaymentInfo : Form
    {
        int _PaymentID;
        public frmPaymentInfo(int PaymentID)
        {
            InitializeComponent();
            _PaymentID = PaymentID;
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPaymentInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlBillInfo1.ctrlBillInfo_Load(_PaymentID);
        }
    }
}
