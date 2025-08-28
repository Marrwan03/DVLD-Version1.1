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

namespace DVLD.Tests.Controls
{
    public partial class ctrlTestInfo : UserControl
    {
        public ctrlTestInfo()
        {
            InitializeComponent();
        }
        int _TestID;
        clsTest _TestInfo;
        public void ctrlTestInfo_Load(int TestID)
        {
            _TestID = TestID;
            _TestInfo = clsTest.Find(_TestID);
            if(_TestInfo != null)
            {
                ctrlSecheduledTest1.LoadInfo(_TestInfo.TestAppointmentID);
                lblTestResult.Text = (_TestInfo.TestResult ? "Pass" : "Fail");
                lblNote.Text = (string.IsNullOrEmpty(_TestInfo.Notes) ? "NULL" : _TestInfo.Notes);
            }
            else
            {
                MessageBox.Show($"This testID[{_TestID}] is not found", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

    }
}
