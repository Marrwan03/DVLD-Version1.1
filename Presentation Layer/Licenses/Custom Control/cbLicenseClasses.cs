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

namespace DVLD.Licenses.Custom_Control
{
    public partial class cbLicenseClasses : ComboBox
    {
        public cbLicenseClasses()
        {
            InitializeComponent();
        }

        public void cbLicenseClasses_Load()
        {
            DataTable dtcbLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow item in dtcbLicenseClasses.Rows)
            {
                this.Items.Add(item[1]);
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
