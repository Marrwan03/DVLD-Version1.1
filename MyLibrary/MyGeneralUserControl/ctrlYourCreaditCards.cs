using DVLD.Employee;
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

namespace MyControlLibrary
{
    public partial class ctrlYourCreaditCards : UserControl
    {
        public ctrlYourCreaditCards()
        {
            InitializeComponent();
        }
        int _UserID;
        DataTable _dtAllCreaditCards;

        void _FillCreaditCardBy(int NumberOfPage)
        {
            DataRow row = _dtAllCreaditCards.Rows[NumberOfPage - 1];
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
            ctrlCreadiCard1.ctrlCreditCard_Load((int)row["CardID"]);
        }

        public void ctrlYourCreaditCards_Load(int UserID)
        {
            _UserID = UserID;
            _dtAllCreaditCards = clsCreditCard.FindByUserID(UserID);
            if (_dtAllCreaditCards.Rows.Count == 0)
            {
                ctrlSwitchSearch1.Enabled = false;
                return;
            }
            ctrlSwitchSearch1.MaxNumberOfPage = _dtAllCreaditCards.Rows.Count;
            ctrlSwitchSearch1.NumberOfPage = 1;
            _FillCreaditCardBy(ctrlSwitchSearch1.NumberOfPage);
        }
        private void ctrlYourCreaditCards_Load(object sender, EventArgs e)
        {

        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, DVLD.MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillCreaditCardBy(e.CurrentNumberOfPage);
        }
    }
}
