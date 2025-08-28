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

namespace DVLD.Applications
{
    public partial class frmApplicationBasicInfo : Form
    {
        int _AppID;
        public frmApplicationBasicInfo(int AppID)
        {
            InitializeComponent();
            _AppID = AppID;
        }

        private void frmApplicationBasicInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlApplicationBasicInfo1.LoadApplicationInfo(_AppID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
