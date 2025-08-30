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

namespace Bank.Custome
{
    public partial class CustomcbYourCreaditCards : ComboBox
    {
        public CustomcbYourCreaditCards()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }

        public void FillComboBoxBy(DataTable dtAllCreadiCards)
        {
            for (int i = 0; i < dtAllCreadiCards.Rows.Count; i++)
            {
                var Row = dtAllCreadiCards.Rows[i];
                 byte item = (byte)Row["CardType"];
                string TypeOfCard = clsCreditCard.GetStringTypeOfCreditCard(item);
                if(this.FindString(TypeOfCard) == -1)
                    this.Items.Add(TypeOfCard);

            }
        }

        public void FillComboBoxBy(int CardID)
        {
            clsCreditCard creditCard = clsCreditCard.Find(CardID);

            if(creditCard != null)
            {
                this.Items.Add(creditCard.GetStringTypeOfCreditCard());
            }

            

        }

        public void Clear()
        {
            this.Items.Clear();
        }

    }
}
