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

namespace DVLD.MyCustomeControls
{
    public partial class cbTestType : ComboBox
    {

        public void FillcbTestType()
        {
            DataTable dtTestTypes = clsTestType.GetAllTestTypes();

            if(dtTestTypes != null && dtTestTypes.Rows.Count>0)
            {

                foreach (DataRow TestType in dtTestTypes.Rows)
                {
                    this.Items.Add(TestType["TestTypeTitle"]);
                }

            }

        }

        public cbTestType()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
