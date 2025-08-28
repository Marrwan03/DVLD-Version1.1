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

namespace DVLD.User
{
    public partial class cbUserCards : ComboBox
    {
        public cbUserCards()
        {
            InitializeComponent();
        }

        public void Fill_cbUserCardsBy(int UserID)
        {
            DataTable dtAllCreditcard = clsCreditCard.FindByUserID(UserID);
            
            foreach (DataRow row in dtAllCreditcard.Rows)
            {
                this.Items.Add(clsCreditCard.GetStringTypeOfCreditCard((byte)row[5]));
            }

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
