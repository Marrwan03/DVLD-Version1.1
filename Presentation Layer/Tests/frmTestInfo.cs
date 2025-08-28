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

namespace DVLD.Tests
{
    public partial class frmTestInfo : Form
    {
        int _TestID;
        public frmTestInfo(int testID)
        {
            InitializeComponent();
            _TestID = testID;
        }

        private void frmTestInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlTestInfo1.ctrlTestInfo_Load(_TestID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
