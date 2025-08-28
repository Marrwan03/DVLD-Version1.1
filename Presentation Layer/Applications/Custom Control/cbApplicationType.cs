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

namespace DVLD.Applications.Custom_Control
{
    public partial class cbApplicationType : ComboBox
    {
        public cbApplicationType()
        {
            InitializeComponent();
        }

        public void Load_cbApplicationType()
        {
           DataTable dtAppType = clsApplicationType.GetAllApplicationTypes();
            foreach (DataRow item in dtAppType.Rows)
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
