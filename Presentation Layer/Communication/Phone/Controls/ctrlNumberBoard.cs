using DVLD_Buisness;
using Guna.UI2.WinForms;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static DVLD_Buisness.Classes.clsCommunication;

namespace DVLD.Communication.Phone.Controls
{
    public partial class ctrlNumberBoard : UserControl
    {
        public ctrlNumberBoard()
        {
            InitializeComponent();
          
        }

        public class clsEventHandler : EventArgs
        {
            public string Phone { get; }
            public int RecipientID { get; }
            public clsCallLog.enFor RecipientType { get; }

            public clsEventHandler( string phone, int recipientID, clsCallLog.enFor recipientType)
            {

                Phone = phone;
                RecipientID = recipientID;
                RecipientType = recipientType;

            }
        }


        public event EventHandler<clsEventHandler> OnSearchPhoneNumber;


        void _SetNumber(Guna2CircleButton btn)
        {
            lblPhoneNumber.Text += btn.Text;
        }

        public void btn_Click(object sender, EventArgs e)
        {
            Guna2CircleButton btn = (Guna2CircleButton)sender;
            _SetNumber(btn);
        }

      
        private void btnDelete_Click(object sender, EventArgs e)
        {
            StringBuilder sb = new StringBuilder();
            if(lblPhoneNumber.Text.Length >0)
            {
            sb.Append (lblPhoneNumber.Text);
            sb.Remove(lblPhoneNumber.Text.Length-1,1);
            lblPhoneNumber.Text = sb.ToString();
            }
        }

     

       

        private void btnSearch_Click(object sender, EventArgs e)
        {
            if(string.IsNullOrEmpty(lblPhoneNumber.Text) )
            {
                MessageBox.Show("You have to fill PhoneNumber to search about it", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int RecipientID;
            clsCallLog.enFor RecipientType;
            clsPerson person = clsPerson.FindBy(lblPhoneNumber.Text);
            if(person == null)
            {
                MessageBox.Show($"This Number {lblPhoneNumber.Text} isn`t here, Set another number", "Not Found", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            RecipientID = person.PersonID.Value;
            RecipientType = enFor.ForPerson;

            clsUser user = clsUser.FindByPersonID(person.PersonID.Value);
            if (user != null)
            {
                RecipientID = user.UserID.Value;
                RecipientType = enFor.ForUser;

                clsEmployee employee = clsEmployee.FindByEmployeeID(user.UserID.Value);
                if(employee != null)
                {
                    RecipientID = employee.EmployeeID.Value;
                    RecipientType = enFor.ForEmployee;
                }
            }

            OnSearchPhoneNumber?.Invoke(this, new clsEventHandler(lblPhoneNumber.Text, RecipientID, RecipientType));
        }

        public void btn_MouseEnter(object sender, EventArgs e)
        {
            Guna2CircleButton btn = (Guna2CircleButton)sender;
            btn.FillColor = Color.CadetBlue;
        }

        public void btn_MouseLeave(object sender, EventArgs e)
        {
            Guna2CircleButton btn = (Guna2CircleButton)sender;
            btn.FillColor = Color.White;
        }

        private void picMobileScreen_Click(object sender, EventArgs e)
        {

        }

        private void cbCountries1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblNumberCode.Text = clsCountry.Find(cbCountries1.Text).CountryCode;
        }

        public void ctrlNumberBoard_Load()
        {
            cbCountries1.FillCountriesInComoboBox();
            cbCountries1.SelectedIndex= cbCountries1.FindString("Saudi Arabia");
        }

        private void ctrlNumberBoard_Load(object sender, EventArgs e)
        {

        }
    }
}
