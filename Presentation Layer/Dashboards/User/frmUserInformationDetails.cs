using DVLD.Dashboards.User.Controls;
using DVLD.Properties;
using iText.StyledXmlParser.Jsoup.Nodes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Speech.Synthesis;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Dashboards.User
{
    public partial class frmUserInformationDetails : Form
    {
        int _PersonID;
        public frmUserInformationDetails(int PersonID)
        {
            InitializeComponent();
            _PersonID = PersonID;
        }
        private void frmUserInformationDetails_Load(object sender, EventArgs e)
        {
            ctrlPersonCard1.LoadPersonInfo(_PersonID);
            ctrlLicensesSection1.ctrlLicensesSection_Load(_PersonID);
            ctrlPaymentsSection1.ctrlPaymentsSection_Load(_PersonID);
           
        }

        public Action<string> OnChangedPicture;
        private void ctrlPersonCard1_OnChangedPicture(string obj)
        {
            OnChangedPicture?.Invoke(obj);
        }
    }
}
