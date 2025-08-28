using DVLD.Applications;
using DVLD.Classes;
using DVLD.User;
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

namespace DVLD.All_Licenses
{
    public partial class frmAllLicenses : Form
    {
        int _UserID;
        public frmAllLicenses(int UserID)
        {
            InitializeComponent();
            _UserID = UserID;
        }
        clsUser _UserInfo;

      

        private void frmAllLicenses_Load(object sender, EventArgs e)
        {
            _UserInfo = clsUser.FindByUserID(_UserID);
            this.Region = clsGlobal.CornerForm(Width, Height);
            if(!clsDriver.IsExistsBy(_UserID))
            {
               if( MessageBox.Show($"You don`t have any [Local - International] Licenses to show them.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                {
                    ctrlDriverLicenses1.Enabled = false;
                    ctrlYourLicenses1.Enabled=false;
                    ctrlYourLicenses1.ResetAllInfo();
                    return;
                }
               
            }
            ctrlDriverLicenses1.LoadInfoByPersonID(_UserInfo.PersonID);
            ctrlYourLicenses1.ctrlYourLicenses_Load(_UserID);

        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }
    }
}
