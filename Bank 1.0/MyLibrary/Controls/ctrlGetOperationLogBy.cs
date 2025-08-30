using DataLogic2;
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
    public partial class ctrlGetOperationLogBy : UserControl
    {
        public enum enTypeOfOperation { Deposite, Withdraw}
        enTypeOfOperation _TypeOfOperation;
        public enTypeOfOperation TypeOfOperation
        {
            get
            {
                return _TypeOfOperation;
            }
            set
            {
                _TypeOfOperation = value;
                switch (_TypeOfOperation)
                {
                    case enTypeOfOperation.Deposite:
                        {
                            lblTitle.Text = "Deposite Log";
                            break;
                        }
                        case enTypeOfOperation.Withdraw:
                        {
                            lblTitle.Text = "Withdraw Log";
                            break;
                        }
                        
                }

            }
        }
        int _UserID;
        DataTable _dtOperationData;
        DataTable _dtAllYourCards;
        public ctrlGetOperationLogBy()
        {
            InitializeComponent();
        }

        void _FilldgvOperationLogBy()
        {
            _GetdtOperationData((int)_dtAllYourCards.Rows[customcbYourCreaditCards1.SelectedIndex]["CardID"]);
            lblMessageForEmptyData.Visible = _dtOperationData.Rows.Count == 0;
            lblRecordNumber.Text = _dtOperationData.Rows.Count.ToString();

            if(_dtOperationData.Rows.Count > 0)
            {
                dgvOperationLog.DataSource = _dtOperationData;
            }
            else
            {
                dgvOperationLog.DataSource = null;
            }

        }

        void _GetdtOperationData(int CardID)
        {
            switch (_TypeOfOperation)
            {
                case enTypeOfOperation.Deposite:
                    {
                        _dtOperationData = clsDeposite.GetDepositeLogBy(CardID);
                        break;
                    }
                case enTypeOfOperation.Withdraw:
                    {
                        _dtOperationData = clsDeposite.GetWithdrawLogBy(CardID);
                        break;
                    }
            }
        }

        string _GetPerfix()
        {
            string Perfix = "";
            switch (_TypeOfOperation)
            {
                case enTypeOfOperation.Deposite:
                    {
                        Perfix = "Deposite";
                        break;
                    }
                case enTypeOfOperation.Withdraw:
                    {
                        Perfix = "Withdraw";
                        break;
                    }
            }
            return Perfix;
        }

        public void ctrlGetOperationLogBy_Load(enTypeOfOperation typeOfOperation, int UserID)
        {
            _UserID = UserID;
            _dtAllYourCards = clsCreditCard.FindByUserID(_UserID);
            this.TypeOfOperation = typeOfOperation;

            if (_dtAllYourCards.Rows.Count > 0)
            {
                customcbYourCreaditCards1.FillComboBoxBy(_dtAllYourCards);
                customcbYourCreaditCards1.SelectedIndex = 0;                
            }
            lblMessageForEmptyData.Text += _GetPerfix();
        }

        private void ctrlGetOperationLogBy_Load(object sender, EventArgs e)
        {          
            dgvOperationLog.EnableHeadersVisualStyles = false;
            dgvOperationLog.ColumnHeadersDefaultCellStyle.BackColor = Color.SlateBlue;
        }

        private void customcbYourCreaditCards1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _FilldgvOperationLogBy();
        }
    }
}
