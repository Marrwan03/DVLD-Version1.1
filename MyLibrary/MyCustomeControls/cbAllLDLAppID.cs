using DVLD_Buisness;
using System.Data;
using System.Windows.Forms;

namespace DVLD.MyCustomeControls
{
    public partial class cbAllLDLAppID : ComboBox
    {

        public void FillcbAllLDLAppIDBy(int PersonID)
        {
            DataTable dtAllLDLApp = clsLocalDrivingLicenseApplication.GetAllLocalDrivingLicenseApplicationsBy(PersonID);

            if(dtAllLDLApp != null &&dtAllLDLApp.Rows.Count > 0)
            {
                foreach (DataRow row in dtAllLDLApp.Rows)
                {
                    this.Items.Add(row["LocalDrivingLicenseApplicationID"]);
                }
            }

        }

        public cbAllLDLAppID()
        {
            InitializeComponent();
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
        }
    }
}
