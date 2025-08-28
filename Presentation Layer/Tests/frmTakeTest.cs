using DVLD.Classes;
using DVLD_Buisness;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace DVLD.Tests
{
    public partial class frmTakeTest: Form
    {
      
        private int _AppointmentID;
        private clsTestType.enTestType _TestType;
        //849, 670
        public clsTestType.enTestType TestType 
        {
            get
            {
                return _TestType;
            }
            set
            {
                _TestType = value;

                switch(_TestType)
                {
                    case clsTestType.enTestType.WrittenTest:
                        {
                            this.Size = new Size(849, 670);
                            ctrlSetYourNote1.Location = new Point(177, 528);
                            break;
                        }
                        default:
                        {
                            this.Size = new Size(1389, 630);
                            ctrlSetYourNote1.Location = new Point(888, 528);
                            break;
                        }
                }

            }
        }

        private int _TestID = -1;
        private clsTest _CurrentTest;


        public frmTakeTest(int AppointmentID,clsTestType.enTestType testType )
        {
            InitializeComponent();
            _AppointmentID= AppointmentID;
            TestType = testType;
            _CurrentTest = new clsTest();
        }

        private void frmTakeTest_Load(object sender, EventArgs e)
        {
            btnSave.Enabled = false;
            this.Region = clsGlobal.CornerForm(Width, Height);
            ctrlTakeTest1.ctrlTakeTest_Load(_AppointmentID, TestType);
            #region Hide
            //ctrlSecheduledTest1.TestTypeID = _TestType;
            //ctrlSecheduledTest1.LoadInfo(_AppointmentID);

            //if (ctrlSecheduledTest1.TestAppointmentID==-1)
            //    btnSave.Enabled = false;
            //else
            //    btnSave.Enabled = true;


            //int _TestID = ctrlSecheduledTest1.TestID;
            //if (_TestID != -1)
            //{
            //    _CurrentTest = clsTest.Find(_TestID);

            //    if (_CurrentTest.TestResult)
            //        rbPass.Checked = true;
            //    else
            //        rbFail.Checked = true;
            //       txtNotes.Text = _CurrentTest.Notes;

            //    lblUserMessage.Visible = true;
            //    rbFail.Enabled=false; 
            //    rbPass.Enabled=false;
            //}

            //else
            //    _CurrentTest = new clsTest();
            #endregion
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {

          if (  MessageBox.Show("Are you sure you want to save?",
                      "Confirm", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)==DialogResult.No
             )
            {
                return;
            }

            _CurrentTest.TestAppointmentID = _AppointmentID;
            
            if (_CurrentTest.Save())
            {
                MessageBox.Show("Data Saved Successfully.", "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
                //Refresh
                ctrlTakeTest1.ctrlSecheduledTest1.LoadInfo(_AppointmentID);
                ctrlSetYourNote1.Enabled = false;
                btnSave.Enabled = false;
                ctrlTakeTest1.Enabled = false;
                ctrlTakeTest1.ctrlSecheduledTest1.Enabled = true;
            }
            else
                MessageBox.Show("Error: Data Is not Saved Successfully.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

        }

        private void ctrlSecheduledTest1_Load(object sender, EventArgs e)
        {

        }

        private void rbFail_CheckedChanged(object sender, EventArgs e)
        {
            
        }

        private void ctrlTakeTest1_OnFinishTest(object sender, Controls.ctrlTakeTest.TestInfo e)
        {
            _CurrentTest.TestResult = e.TestResult;
            ctrlSetYourNote1.Enabled = true;
            btnSave.Enabled = true;
        }

        private void ctrlSetYourNote1_SetNote(object sender, Controls.ctrlSetYourNote.clsNote e)
        {

            if(MessageBox.Show("Are you sure do uou want to save this note?", "Confirm", MessageBoxButtons.OK, MessageBoxIcon.Question) == DialogResult.OK)
            {
            _CurrentTest.Notes = e.Note;
            }
        }
    }
}
