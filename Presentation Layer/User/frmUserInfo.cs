using DVLD.Classes;
using System;
using System.Windows.Forms;

namespace DVLD.Employee
{
    public partial class frmUserInfo: Form
    {
        private int _UserID;

        public frmUserInfo(int UserID)
        {
            InitializeComponent();
            _UserID=UserID;
        }
        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }

        private void frmUserInfo_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlUserCard1.LoadUserInfo(_UserID, DVLD.Controls.ctrlUserCard.enLoadUserInfo.ByUserID);

        }
        
    }
}
