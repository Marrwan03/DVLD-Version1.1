using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Configuration;

namespace DVLD.User
{
    public partial class frmAddNewRequestForApp : Form
    {
        int _UserID;
        public frmAddNewRequestForApp(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }
        public event Action<int> AddNewOrder;
        private void frmAddNewOrderForApp_Load(object sender, EventArgs e)
        {
            this.Region = clsGlobal.CornerForm(Width, Height);

            ctrlAddNewApplication1.ctrlAddNewApplication_Load(_UserID);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }


        private void ctrlAddNewApplication1_OrderInfoChanged_1(object sender, Applications.Controls.ctrlAddNewApplication.OrderInfo e)
        {

            clsUtil.AddNewOrderInFile(ConfigurationManager.AppSettings["FileNameOfApplicationsOrder"], e.CurrentInfo.ToString()) ;
            AddNewOrder?.Invoke(clsUser.FindByUserID(_UserID).PersonID);
            this.Close() ;
        }

        private void ctrlAddNewApplication1_Load(object sender, EventArgs e)
        {

        }
    }
}
