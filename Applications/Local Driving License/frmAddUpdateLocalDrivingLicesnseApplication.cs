using DVLD.Classes;
using DVLD.Controls;
using DVLD.People;
using DVLD.Properties;
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
using static System.Net.Mime.MediaTypeNames;

namespace DVLD.Applications
{


    public partial class frmAddUpdateLocalDrivingLicesnseApplication : Form
    {

        public enum enMode { AddNew = 0, Update = 1 };

        private enMode _Mode;
        private int _LocalDrivingLicenseApplicationID = -1;
        private int _SelectedPersonID = -1;
        clsLocalDrivingLicenseApplication _LocalDrivingLicenseApplication;
        int _LicenseClass;

        public frmAddUpdateLocalDrivingLicesnseApplication()
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int PersonID, int LicenseClass)
        {
            InitializeComponent();
            _Mode = enMode.AddNew;
            _SelectedPersonID = PersonID;
            _LicenseClass = LicenseClass;
        }

        public frmAddUpdateLocalDrivingLicesnseApplication(int LocalDrivingLicenseApplicationID)
        {
            InitializeComponent();

            _Mode = enMode.Update;
            _LocalDrivingLicenseApplicationID = LocalDrivingLicenseApplicationID;

        }

        private void _FillLicenseClassesInComoboBox()
        {
            DataTable dtLicenseClasses = clsLicenseClass.GetAllLicenseClasses();

            foreach (DataRow row in dtLicenseClasses.Rows)
            {
                cbLicenseClass.Items.Add(row["ClassName"]);
            }
        }

        private void _ResetDefualtValues()
        {
            //this will initialize the reset the defaule values
            _FillLicenseClassesInComoboBox();


            if (_Mode == enMode.AddNew)
            {

                lblTitle.Text = "New Local Driving License Application";
                this.Text = "New Local Driving License Application";
                _LocalDrivingLicenseApplication = new clsLocalDrivingLicenseApplication();
               // ctrlPersonCardWithFilter1.FilterFocus();
                tpApplicationInfo.Enabled = _SelectedPersonID != -1;
                if(tpApplicationInfo.Enabled )
                {
                    LoadPersonDataBy(_SelectedPersonID);
                    cbLicenseClass.SelectedIndex = _LicenseClass;
                }
                else
                {
                cbLicenseClass.SelectedIndex = 2;
                }
                lblFees.Text = clsApplicationType.Find((int)clsApplication.enApplicationType.NewDrivingLicense).Fees.ToString();
                lblApplicationDate.Text = DateTime.Now.ToShortDateString();
                lblCreatedByEmployee.Text = clsGlobal.CurrentEmployee.PersonInfo.FirstName;

            }
            else
            {
                lblTitle.Text = "Update Local Driving License Application";
                this.Text = "Update Local Driving License Application";

                tpApplicationInfo.Enabled = true;
                btnSave.Enabled = true;


            }
            
        }

        public void LoadPersonDataBy(int PersonID)
        {
            _SelectedPersonID = PersonID;          
            ctrlPersonCardWithFilter1.LoadPersonInfo(_SelectedPersonID);
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            lblAge.Text = clsPerson.Find(_SelectedPersonID).Age.ToString();
            btnSave.Enabled = true;
            tpApplicationInfo.Enabled = true;
            tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
        }

        private void _LoadData()
        {
            
            ctrlPersonCardWithFilter1.FilterEnabled = false;
            _LocalDrivingLicenseApplication = clsLocalDrivingLicenseApplication.FindByLocalDrivingAppLicenseID(_LocalDrivingLicenseApplicationID);

            if (_LocalDrivingLicenseApplication == null)
            {
                MessageBox.Show("No Application with ID = " + _LocalDrivingLicenseApplicationID, "Application Not Found", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                this.Close();

                return;
            }

            ctrlPersonCardWithFilter1.LoadPersonInfo(_LocalDrivingLicenseApplication.ApplicationInfo.ApplicantPersonID);
            lblLocalDrivingLicebseApplicationID.Text = _LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID.ToString();
            lblApplicationDate.Text = clsFormat.DateToShort(_LocalDrivingLicenseApplication.ApplicationInfo.ApplicationDate);
            // _LocalDrivingLicenseApplication.LicenseClassInfo.ClassName
            cbLicenseClass.SelectedIndex = cbLicenseClass.FindString(clsLicenseClass.Find(_LocalDrivingLicenseApplication.LicenseClassID).ClassName);
            lblFees.Text = _LocalDrivingLicenseApplication.ApplicationInfo.PaidFees.ToString();
            lblAge.Text = _LocalDrivingLicenseApplication.ApplicationInfo.PersonInfo.Age.ToString();
            if (_LocalDrivingLicenseApplication.ApplicationInfo.IsPaid())
            {
                lblIsPaid.Text = "Yes";
            }
            else
                lblIsPaid.Text = "No";
            lblCreatedByEmployee.Text = clsEmployee.FindByEmployeeID(_LocalDrivingLicenseApplication.ApplicationInfo.CreatedByEmployeeID).PersonInfo.FirstName;

        }

        private void DataBackEvent(object sender, int PersonID)
        {
            // Handle the data received
            _SelectedPersonID = PersonID;
            ctrlPersonCardWithFilter1.LoadPersonInfo(PersonID);


        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Load(object sender, EventArgs e)
        {
            _ResetDefualtValues();

            if (_Mode == enMode.Update)
            {
                _LoadData();
            }
            this.Region = clsGlobal.CornerForm(Width, Height);
        }

        private void btnApplicationInfoNext_Click(object sender, EventArgs e)
        {
            if (_Mode == enMode.Update)
            {
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
                return;
            }

            //incase of add new mode.
            if (ctrlPersonCardWithFilter1.PersonID != -1)
            {
                lblAge.Text = clsPerson.Find(_SelectedPersonID).Age.ToString();
                btnSave.Enabled = true;
                tpApplicationInfo.Enabled = true;
                tcApplicationInfo.SelectedTab = tcApplicationInfo.TabPages["tpApplicationInfo"];
            }

            else
            {
                MessageBox.Show("Please Select a Person", "Select a Person", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ctrlPersonCardWithFilter1.FilterFocus();
            }
        }

        //bool IsLicenseCorrectForHim()
        //{
        //    return ctrlPersonCardWithFilter1.SelectedPersonInfo.Age >= clsLicenseClass.Find(cbLicenseClass.Text).MinimumAllowedAge;
        //}

        private void btnSave_Click(object sender, EventArgs e)
        {
            if (!this.ValidateChildren())
            {
                //Here we dont continue becuase the form is not valid
                MessageBox.Show("Some fileds are not valide!, put the mouse over the red icon(s) to see the erro", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            if (!clsLicense.IsLicenseCorrectForHim(ctrlPersonCardWithFilter1.SelectedPersonInfo.Age, cbLicenseClass.Text))
            {
                MessageBox.Show($"Person cannot have this license,\n\n This license has upper than {clsLicenseClass.Find(cbLicenseClass.Text).MinimumAllowedAge}"
                    , "Not allowed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            int? ActiveApplication = clsApplication.GetActiveApplicationID(_SelectedPersonID,
                clsApplication.enApplicationType.NewDrivingLicense);

            if(ActiveApplication.HasValue)
            {
                _LocalDrivingLicenseApplication.ApplicationInfo = clsApplication.FindBaseApplication(ActiveApplication.Value);
                if (!_LocalDrivingLicenseApplication.ApplicationInfo.IsPaid())
                {
                    MessageBox.Show("This user doesn`t Pay this application,\nUser has to pay it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    lblIsPaid.Text = "No";
                    return;
                }
                else
                {
                    _LocalDrivingLicenseApplication.LicenseClassID = cbLicenseClass.FindString(cbLicenseClass.Text)+1;
                    _LocalDrivingLicenseApplication.ApplicationID = ActiveApplication.Value;
                    if (_LocalDrivingLicenseApplication.Save())
                    {
                        MessageBox.Show($"Add new Local driving application with[{_LocalDrivingLicenseApplication.LocalDrivingLicenseApplicationID}] ID successfully",
                            "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        lblIsPaid.Text = "Yes";
                        _Mode = enMode.Update;
                        _ResetDefualtValues();
                        clsUtil.ChangeOrderOfApplicationsToPaidFor
                            (_LocalDrivingLicenseApplication.ApplicationInfo.PaymentInfo.CreditCardInfo.UserID.Value,
                            _LocalDrivingLicenseApplication.ApplicationInfo.ApplicationTypeID, 
                            clsUtil.enOperationOrder.delete);
                        
                    }
                }

            }
            else
            {
                _LocalDrivingLicenseApplication.ApplicationInfo.ApplicantPersonID = ctrlPersonCardWithFilter1.PersonID.Value;
                _LocalDrivingLicenseApplication.ApplicationInfo.ApplicationDate = DateTime.Now;
                _LocalDrivingLicenseApplication.ApplicationInfo.ApplicationTypeID = 1;
                _LocalDrivingLicenseApplication.ApplicationInfo.ApplicationStatus = clsApplication.enApplicationStatus.New;
                _LocalDrivingLicenseApplication.ApplicationInfo.LastStatusDate = DateTime.Now;
                _LocalDrivingLicenseApplication.ApplicationInfo.PaidFees = Convert.ToSingle(lblFees.Text);
                _LocalDrivingLicenseApplication.ApplicationInfo.PaymentID = null;//in this step you create new application and he has to pay it
                _LocalDrivingLicenseApplication.ApplicationInfo.CreatedByEmployeeID = clsGlobal.CurrentEmployee.EmployeeID.Value;
                if(_LocalDrivingLicenseApplication.ApplicationInfo.Save())
                {
                    MessageBox.Show($"Add new Application with ApplicationID[{_LocalDrivingLicenseApplication.ApplicationInfo.ApplicationID}],\nThis User has to Pay it to continue.",
                        "Saved New Application", MessageBoxButtons.OK, MessageBoxIcon.Information);
                   
                }
                else
                {
                    MessageBox.Show($"Failed Add Application.",
                       "Failed New Application", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                lblIsPaid.Text = "No";
                return;
            }
            

           
        }

        private void ctrlPersonCardWithFilter1_OnPersonSelected(int obj)
        {
            _SelectedPersonID = obj;

        }

        private void frmAddUpdateLocalDrivingLicesnseApplication_Activated(object sender, EventArgs e)
        {
            ctrlPersonCardWithFilter1.FilterFocus();
        }

        private void cbLicenseClass_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbLicenseClass.Text)
            {
                case "Class 1 - Small Motorcycle":
                    {
                        picLicenseClass.Image= Resources.small_Motorcycle;
                        break;
                    }
                case "Class 2 - Heavy Motorcycle License":
                    {
                        picLicenseClass.Image = Resources.Heavy_Motorcycle;
                        break;
                    }
                case "Class 3 - Ordinary driving license":
                    {
                        picLicenseClass.Image = Resources.Ordinary_driving;
                        break;
                    }
                case "Class 4 - Commercial":
                    {
                        picLicenseClass.Image = Resources.Commercial_Vehicle;
                        break;
                    }
                case "Class 5 - Agricultural":
                    {
                        picLicenseClass.Image = Resources.Agricultural_Vehicle;
                        break;
                    }
                case "Class 6 - Small and medium bus":
                    {
                        picLicenseClass.Image = Resources.Small_and_medium_bus;
                        break;
                    }
                case "Class 7 - Truck and heavy vehicle":
                    {
                        picLicenseClass.Image = Resources.Truck_and_heavy_vehicle;
                        break;
                    }


            }



        }

        private void picLicenseClass_Click(object sender, EventArgs e)
        {
            //All Record`s Data with is available
            clsLicenseClass CurrentLicenseClass = clsLicenseClass.Find(cbLicenseClass.Text);
            string Message1 = $"------------------------------------------\n\n" +
                $"Name: {CurrentLicenseClass.ClassName},\nMinimumAllowedAge: {CurrentLicenseClass.MinimumAllowedAge},\nYour Age: {ctrlPersonCardWithFilter1.SelectedPersonInfo.Age}" +
                $"\n\n------------------------------------------\n\n";
            string Message2 = "";
            if (clsLicense.IsLicenseCorrectForHim(ctrlPersonCardWithFilter1.SelectedPersonInfo.Age, cbLicenseClass.Text))
            {
                //available
                Message2 = "This License is Available for you :-)";
            }
            else
            {
                Message2 = "This License isn`t Available for you :-(";
            }

            MessageBox.Show(Message1 + Message2 + "\n\n------------------------------------------", $"{CurrentLicenseClass.ClassName} Info", MessageBoxButtons.OK, MessageBoxIcon.Information);



        }


       public Action OnClosed;

        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClosed?.Invoke();
            this.Close();
        }

        private void ctrlPersonCardWithFilter1_OnPersonAdded(int obj)
        {
           if( MessageBox.Show($"Added New Person with this ID[{obj}].", "Add New Person", MessageBoxButtons.OK, MessageBoxIcon.Information) == DialogResult.OK)
            {
                ctrlPersonCardWithFilter1.LoadPersonInfo(obj);
            }           
        }

        private void lblTitle_Click(object sender, EventArgs e)
        {

        }

        
    }
}
