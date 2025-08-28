using DVLD.Classes;
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

namespace DVLD.Communication.Phone
{
    public partial class frmCallPhoneWithFilter : Form
    {
        int _CallerID;
        clsCallLog.enFrom _CallerType;
        public frmCallPhoneWithFilter(int CallerID, clsCallLog.enFrom CallerType)
        {
            InitializeComponent();
            _CallerID = CallerID;
            _CallerType = CallerType;
        }
        //public delegate void MyDelegate(int?_UserID,int? _PersonID );
        //public event MyDelegate DataBack;
        public Action<int, clsCallLog.enFor> DataBack;
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void ctrlNumberBoard1_OnSearchPhoneNumber(object sender, Controls.ctrlNumberBoard.clsEventHandler e)
        {
            frmCallPhone callPhone = new frmCallPhone(_CallerID, _CallerType, e.RecipientID, e.RecipientType);
            callPhone.ShowDialog();
            DataBack?.Invoke(e.RecipientID, e.RecipientType);
            this.Close();
        }

        private void frmCallPhoneWithFilter_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlNumberBoard1.ctrlNumberBoard_Load();
        }
    }
}
