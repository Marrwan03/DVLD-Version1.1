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

namespace DVLD.Communication.Phone
{
    public partial class frmPhoneLogInfo : Form
    {
        int _CallID;
        public frmPhoneLogInfo(int CallID)
        {
            InitializeComponent();
            _CallID = CallID;

        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void frmPhoneLogInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlShowPhoneLogInfo1.ctrlShowCallPhoneInfo_Load(_CallID);
        }
    }
}
