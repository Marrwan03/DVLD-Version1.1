using DVLD.Classes;
using DVLD.Controls;
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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace DVLD.Employee
{
    public partial class ctrlUserCardWithFilter : UserControl
    {
        public ctrlUserCardWithFilter()
        {
            InitializeComponent();
        }

        public event Action<int> OnFindUser;
        public event Action<int> OnAddUser;
        int _UserID;
        public int? UserID { get { return ctrlUserCard1.UserID.Value; } }

        bool _EnableUserFilter = true;
        public bool EnableUserFilter 
        {
            set 
            {
                _EnableUserFilter = value;
                gbFilter.Enabled = _EnableUserFilter;
            }
            get
            {
                return _EnableUserFilter;
                
            }
        }
        void FindNow()
        {
            switch(cbFilterBy.Text)
            {
                case "User ID":
                    {
                        ctrlUserCard1.LoadUserInfo(Convert.ToInt32(txtFilterValue.Text), ctrlUserCard.enLoadUserInfo.ByUserID);
                        break;
                    }
                case "Person ID":
                    {
                        ctrlUserCard1.LoadUserInfo(Convert.ToInt32(txtFilterValue.Text), ctrlUserCard.enLoadUserInfo.ByPersonID);
                        break;
                    }
                case "Username":
                    {
                        ctrlUserCard1.LoadUserInfo(Convert.ToInt32(txtFilterValue.Text), ctrlUserCard.enLoadUserInfo.ByUsername);
                        break;
                    }
            }
            if (EnableUserFilter && OnFindUser != null)
            {
                OnFindUser(ctrlUserCard1.UserID.Value);
            }
        }

        public void LoadUserInfo(int Search, ctrlUserCard.enLoadUserInfo loadUserInfo)
        {
            cbFilterBy.SelectedIndex = Convert.ToInt32(loadUserInfo);
            txtFilterValue.Text = Search.ToString();

            FindNow();
        }



        private void gbFilter_Enter(object sender, EventArgs e)
        {
           
        }

        private void ctrlUserCard1_Load(object sender, EventArgs e)
        {
            
        }

        private void txtFilterUser_Validating(object sender, CancelEventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                e.Cancel = false;
                errorProvider1.SetError(txtFilterValue, "This textbox is required to find user account");
            }
            else
            {
                e.Cancel = true;
                errorProvider1.SetError(txtFilterValue, null);
            }
        }

        private void txtFilterUser_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Check if the pressed key is Enter(character code 13)
            if (e.KeyChar == (char)13)
            {

                btnFind.PerformClick();
            }

            //this will allow only digits if person id is selected
            if (cbFilterBy.Text == "Person ID" || cbFilterBy.Text == "Employee ID")
                e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);

        }

        private void btnFind_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(txtFilterValue.Text))
            {
                MessageBox.Show("You cannot Search without [UserID or PersonID or Username]", "Error!",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                txtFilterValue.Focus();
                return;
            }

            int? UserID = 0;
            if (cbFilterBy.Text == "Person ID")
                UserID = clsUser.GetUserIDByPersonID(Convert.ToInt32(txtFilterValue.Text));
            else if (cbFilterBy.Text == "Username")
                UserID = clsUser.FindByUsername(txtFilterValue.Text).UserID;
            else
                UserID = Convert.ToInt32(txtFilterValue.Text);

                int? EmployeeID = clsEmployee.GetEmployeeIDByUserID(UserID.Value);
            if (EmployeeID.HasValue)
            {
                MessageBox.Show("This Person has Employee Acc, Go to Employee`s section and search about him.", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

                FindNow();
        }

       public void LoadUserInfoByUserID(int ID)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Text = ID.ToString();
            ctrlUserCard1.LoadUserInfo(ID, ctrlUserCard.enLoadUserInfo.ByUserID);
            OnAddUser?.Invoke(ID);
        }

        private void btnAddNewUser_Click(object sender, EventArgs e)
        {
            if(!clsGlobal.CurrentEmployee.CheckAccessPermision(clsEmployee.enPermision.AddNewUser))
            {
                clsGlobal.PermisionMessage("AddNewUser");
                return;
            }

            frmAddUpdateUser addUpdateUser = new frmAddUpdateUser();
            addUpdateUser.DataBack += LoadUserInfoByUserID;
            addUpdateUser.ShowDialog();

        }

        private void cbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtFilterValue.Text = "";
            txtFilterValue.Focus();

        }

        private void ctrlUserCardWithFilter_Load(object sender, EventArgs e)
        {
            cbFilterBy.SelectedIndex = 0;
            txtFilterValue.Focus();
            EnableUserFilter = true;
        }
    }
}
