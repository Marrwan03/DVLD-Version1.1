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

namespace DVLD.User.Status.Control
{
    public partial class ctrlAllTestsInfo : UserControl
    {
        public ctrlAllTestsInfo()
        {
            InitializeComponent();
        }

        int _LDLAppID;
        List<int> _AllTestsID;

        void _FillctrlTestInfo(int PageNumber)
        {
            ctrlTestInfo1.ctrlTestInfo_Load(_AllTestsID[PageNumber - 1]);
        }
        bool _Visible;
        bool Visible { get { return _Visible; }
            set 
            {
                _Visible = value;
                ctrlSwitchSearch1.Visible = _Visible;
                ctrlTestInfo1.Visible = _Visible;
            }
        }

       public void ctrlAllTestsInfo_Load(int LDLAppID)
        {
            _LDLAppID = LDLAppID;
            _AllTestsID = clsTest.GetAllTestsIDBy(_LDLAppID);
            Visible = _AllTestsID.Count > 0;
            if (Visible)
            {
                ctrlSwitchSearch1.MaxNumberOfPage = _AllTestsID.Count;
                ctrlSwitchSearch1.NumberOfPage = 1;
                _FillctrlTestInfo(ctrlSwitchSearch1.NumberOfPage);
            }
           
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FillctrlTestInfo(e.CurrentNumberOfPage);
        }
    }
}
