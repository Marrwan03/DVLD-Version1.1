using Bank.Custome;
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

namespace Bank.Controls
{
    public partial class ctrlFindCreditCard : UserControl
    {

        public class clsCreditCardEventArg : EventArgs
        {
            public int UserID { get; }
           public clsCreditCard.enTypeOfCreditCard typeOfCreditCard { get;}

            public clsCreditCardEventArg(int userID, clsCreditCard.enTypeOfCreditCard typeOfCreditCard)
            {
                UserID = userID;
                this.typeOfCreditCard = typeOfCreditCard;
            }
        }



        public ctrlFindCreditCard()
        {
            InitializeComponent();
        }
        public clsCreditCard.enTypeOfCreditCard TypeOfCreditCard 
        {
            get 
            {
                return clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCards1.Text); 
            } 
        }
       public enum enFilterType { CardID, UserID}
        enFilterType _FilterType;
        public event EventHandler<clsCreditCardEventArg> OnSelected;
        int? _UserID;
        clsCreditCard _CreditCard;
        DataTable _dtAllCreditCards;
        private void ctrlFindCreditCard_Load(object sender, EventArgs e)
        {
            cbFiltering.SelectedIndex = 0;
           
        }
        public void ctrlFindCreditCard_Load(int ID, enFilterType filterType)
        {
            _FindDataBy(ID, filterType);
        }

        private void cbFiltering_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cbFiltering.Text)
            {
                case "Card ID":
                    {
                        _FilterType = enFilterType.CardID;
                        break;
                    }
                case "User ID":
                    {
                        _FilterType = enFilterType.UserID;
                        break;
                    }
            }
        }

        public bool gbFilterEnabled 
        {
            get 
            {
                return gbFilter.Enabled; 
            } 
            
            set 
            {
                gbFilter.Enabled = value;
            }
        }

        public bool customcbYourCreaditCardsEnabled 
        {
            get { return customcbYourCreaditCards1.Enabled; }
            set { customcbYourCreaditCards1.Enabled = value; }
        }

        public void RefreshData()
        {
            ctrlUserBankInfo1.RefreshData();
            customcbYourCreaditCards1.Clear();
            txtFilter.Focus();
        }

      

        private void customcbYourCreaditCards1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(_UserID.HasValue)
            {
                ctrlUserBankInfo1.ctrlUserBankInfo_Load(_UserID.Value,
    clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCards1.Text),
    ctrlUserBankInfo.enStatus.Recipient);
            }

        }

        private void txtFilter_KeyPress(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == (char)13)
                btnFilter.PerformClick();
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }
        bool _FindDataBy(int ID, enFilterType filterType)
        {
            int userid = 0;
            clsCreditCard.enTypeOfCreditCard typeOfCreditCard = clsCreditCard.enTypeOfCreditCard.None;
            return _FindDataBy(ID, filterType, ref userid, ref typeOfCreditCard);

        }
        bool _FindDataBy(int ID, enFilterType filterType, ref int UserID, ref clsCreditCard.enTypeOfCreditCard typeOfCreditCard)
        {
            switch (filterType)
            {
                case enFilterType.CardID:
                    {
                        _CreditCard = clsCreditCard.Find(ID);
                        
                        if (_CreditCard == null)
                        {
                            if (MessageBox.Show($"this cardID {ID} is not found", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                            {
                                RefreshData();
                                return false;
                            }
                        }
                        else
                        {
                            UserID = _CreditCard.UserID.Value;
                            typeOfCreditCard = _CreditCard.CardType;
                            ctrlUserBankInfo1.ctrlUserBankInfo_Load(ID, ctrlUserBankInfo.enStatus.Recipient);
                            customcbYourCreaditCards1.FillComboBoxBy(ID);
                        }
                        break;
                    }
                case enFilterType.UserID:
                    {
                        UserID = ID;
                        _dtAllCreditCards = clsCreditCard.FindByUserID(UserID);
                        if (_dtAllCreditCards == null)
                        {
                            if (MessageBox.Show($"this userID {UserID} is not found", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error) == DialogResult.OK)
                            {
                                RefreshData();
                                return false;
                            }
                        }

                        customcbYourCreaditCards1.Enabled = true;
                        customcbYourCreaditCards1.FillComboBoxBy(_dtAllCreditCards);
                        customcbYourCreaditCards1.SelectedIndex = 0;
                        typeOfCreditCard = clsCreditCard.GetTypeOfCreditCard(customcbYourCreaditCards1.Text);
                        ctrlUserBankInfo1.ctrlUserBankInfo_Load(UserID, typeOfCreditCard, ctrlUserBankInfo.enStatus.Recipient);
                        _UserID = UserID;
                        break;
                    }
            }
            return true;
        }

        private void btnFilter_Click(object sender, EventArgs e)
        {
            int UserID = 0;
            clsCreditCard.enTypeOfCreditCard typeOfCreditCard = clsCreditCard.enTypeOfCreditCard.None;
            if (_FindDataBy(Convert.ToInt32(txtFilter.Text), _FilterType, ref UserID, ref typeOfCreditCard))
                OnSelected?.Invoke(this, new clsCreditCardEventArg(UserID, typeOfCreditCard));
        }

        private void InitializeComponent()
        {
            this.gbFilter = new System.Windows.Forms.GroupBox();
            this.btnFilter = new Guna.UI2.WinForms.Guna2Button();
            this.txtFilter = new Guna.UI2.WinForms.Guna2TextBox();
            this.customcbYourCreaditCards1 = new Bank.Custome.CustomcbYourCreaditCards();
            this.cbFiltering = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.ctrlUserBankInfo1 = new Bank.Controls.ctrlUserBankInfo();
            this.gbFilter.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbFilter
            // 
            this.gbFilter.Controls.Add(this.btnFilter);
            this.gbFilter.Controls.Add(this.txtFilter);
            this.gbFilter.Controls.Add(this.customcbYourCreaditCards1);
            this.gbFilter.Controls.Add(this.cbFiltering);
            this.gbFilter.Controls.Add(this.label2);
            this.gbFilter.Controls.Add(this.label1);
            this.gbFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.gbFilter.ForeColor = System.Drawing.Color.White;
            this.gbFilter.Location = new System.Drawing.Point(3, 3);
            this.gbFilter.Name = "gbFilter";
            this.gbFilter.Size = new System.Drawing.Size(667, 112);
            this.gbFilter.TabIndex = 0;
            this.gbFilter.TabStop = false;
            this.gbFilter.Text = "Filter";
            // 
            // btnFilter
            // 
            this.btnFilter.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.btnFilter.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.btnFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.btnFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.btnFilter.FillColor = System.Drawing.Color.DarkSlateBlue;
            this.btnFilter.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.btnFilter.ForeColor = System.Drawing.Color.White;
            this.btnFilter.Image = global::Bank.Properties.Resources.FindCard;
            this.btnFilter.ImageSize = new System.Drawing.Size(64, 64);
            this.btnFilter.Location = new System.Drawing.Point(517, 23);
            this.btnFilter.Name = "btnFilter";
            this.btnFilter.Size = new System.Drawing.Size(121, 71);
            this.btnFilter.TabIndex = 5;
            this.btnFilter.Click += new System.EventHandler(this.btnFilter_Click);
            // 
            // txtFilter
            // 
            this.txtFilter.AutoRoundedCorners = true;
            this.txtFilter.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.txtFilter.BorderColor = System.Drawing.Color.White;
            this.txtFilter.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txtFilter.DefaultText = "";
            this.txtFilter.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.txtFilter.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.txtFilter.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFilter.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.txtFilter.FillColor = System.Drawing.Color.DarkSlateBlue;
            this.txtFilter.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFilter.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtFilter.ForeColor = System.Drawing.Color.White;
            this.txtFilter.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txtFilter.Location = new System.Drawing.Point(275, 42);
            this.txtFilter.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.txtFilter.Name = "txtFilter";
            this.txtFilter.PlaceholderForeColor = System.Drawing.Color.White;
            this.txtFilter.PlaceholderText = "";
            this.txtFilter.SelectedText = "";
            this.txtFilter.Size = new System.Drawing.Size(200, 35);
            this.txtFilter.TabIndex = 4;
            this.txtFilter.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtFilter_KeyPress);
            // 
            // customcbYourCreaditCards1
            // 
            this.customcbYourCreaditCards1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.customcbYourCreaditCards1.FormattingEnabled = true;
            this.customcbYourCreaditCards1.Location = new System.Drawing.Point(124, 65);
            this.customcbYourCreaditCards1.Name = "customcbYourCreaditCards1";
            this.customcbYourCreaditCards1.Size = new System.Drawing.Size(121, 26);
            this.customcbYourCreaditCards1.TabIndex = 3;
            this.customcbYourCreaditCards1.SelectedIndexChanged += new System.EventHandler(this.customcbYourCreaditCards1_SelectedIndexChanged);
            // 
            // cbFiltering
            // 
            this.cbFiltering.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbFiltering.FormattingEnabled = true;
            this.cbFiltering.Items.AddRange(new object[] {
            "Card ID",
            "User ID"});
            this.cbFiltering.Location = new System.Drawing.Point(125, 29);
            this.cbFiltering.Name = "cbFiltering";
            this.cbFiltering.Size = new System.Drawing.Size(121, 26);
            this.cbFiltering.TabIndex = 2;
            this.cbFiltering.SelectedIndexChanged += new System.EventHandler(this.cbFiltering_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(18, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(100, 18);
            this.label2.TabIndex = 1;
            this.label2.Text = "Card Type : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(85, 18);
            this.label1.TabIndex = 0;
            this.label1.Text = "Filter By : ";
            // 
            // ctrlUserBankInfo1
            // 
            this.ctrlUserBankInfo1.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.ctrlUserBankInfo1.Location = new System.Drawing.Point(85, 121);
            this.ctrlUserBankInfo1.Name = "ctrlUserBankInfo1";
            this.ctrlUserBankInfo1.Size = new System.Drawing.Size(492, 224);
            this.ctrlUserBankInfo1.TabIndex = 1;
            // 
            // ctrlFindCreditCard
            // 
            this.BackColor = System.Drawing.Color.DarkSlateBlue;
            this.Controls.Add(this.ctrlUserBankInfo1);
            this.Controls.Add(this.gbFilter);
            this.Name = "ctrlFindCreditCard";
            this.Size = new System.Drawing.Size(673, 370);
            this.Load += new System.EventHandler(this.ctrlFindCreditCard_Load);
            this.gbFilter.ResumeLayout(false);
            this.gbFilter.PerformLayout();
            this.ResumeLayout(false);

        }
    }
}
