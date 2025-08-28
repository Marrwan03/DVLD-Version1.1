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
    public partial class cbTypeOfCreditCard : ComboBox
    {
        public cbTypeOfCreditCard()
        {
            InitializeComponent();
        }
        HashSet<string> Set = new HashSet<string>()
            {
                "Visa",
                "MasterCard",
                "QNB",
                "SAB",
                "UnionPay",
                "Discover"
            };
        public void FillTypeOfCreditCard()
        {
            
            foreach (string s in Set)
            {
                this.Items.Add(s);  
            }
        }

        public clsCreditCard.enTypeOfCreditCard GetTypeOfCreditCard(string typestringOfCreditCard)
        {
            return clsCreditCard.GetTypeOfCreditCard(typestringOfCreditCard);

        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
