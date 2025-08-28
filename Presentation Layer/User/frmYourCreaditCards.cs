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

namespace DVLD.User
{
    public partial class frmYourCreaditCards : Form
    {
        int _UserID;
        bool _ShowBalance;
        public frmYourCreaditCards(int UserID, bool ShowBalance)
        {
            InitializeComponent();
            _UserID = UserID;
            _ShowBalance = ShowBalance;
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }

        private void frmYourCreaditCards_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            _FillctrlYourCreaditCards_LoadBy(_UserID);
        }

        void _FillctrlYourCreaditCards_LoadBy(int UserID)
        {
            ctrlYourCreaditCards1.ctrlYourCreaditCards_Load(UserID, _ShowBalance);
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            frmAddUpdateCreditCard AddCreditCard = new frmAddUpdateCreditCard();
            AddCreditCard.OnAddNewCreditCard += _FillctrlYourCreaditCards_LoadBy;
            AddCreditCard.ShowDialog();

        }

        private void ctrlYourCreaditCards1_Load(object sender, EventArgs e)
        {

        }
    }
}
