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

namespace DVLD.User.Controls
{
    public partial class ctrlYourCreaditCards : UserControl
    {
        public ctrlYourCreaditCards()
        {
            InitializeComponent();
        }

        int _UserID;
        bool _ShowBalance;
        DataTable _dtAllCreaditCards;

        void _FillCreaditCardBy(int NumberOfPage)
        {
            if(NumberOfPage > 0)
            {
                DataRow row = _dtAllCreaditCards.Rows[NumberOfPage - 1];
                ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
                ctrlCreditCard1.ShowBalance = _ShowBalance;
                ctrlCreditCard1.ctrlCreditCard_Load((int)row["CardID"]);
            }
           
        }

        public void ctrlYourCreaditCards_Load(int UserID, bool ShowBalance)
        {
            _UserID = UserID;
            _ShowBalance = ShowBalance;
            _dtAllCreaditCards = clsCreditCard.FindByUserID(UserID);
            if(_dtAllCreaditCards== null)
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

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillCreaditCardBy(e.CurrentNumberOfPage);
        }
    }
}
