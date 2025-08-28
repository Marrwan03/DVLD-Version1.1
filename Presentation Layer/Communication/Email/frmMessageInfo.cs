using DVLD.Classes;
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

namespace DVLD.Email
{
    public partial class frmMessageInfo : Form
    {
        int _MessageID;
       
        public frmMessageInfo(int messageID)
        {
            InitializeComponent();
            _MessageID = messageID;
        }

        private void frmMessageInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlMessageInfo1.ctrlMessageInfo_Load(_MessageID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
