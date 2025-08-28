using DVLD.Applications;
using DVLD.Tests;
using DVLD_Buisness;
using DVLD_Buisness.Global_Classes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Payments.Controls
{
    public partial class ctrlPayments : UserControl
    {
        int _PersonID;
        clsPayment.enPaymentType _paymentType;
        DataTable _dtAllPayments;

        public ctrlPayments()
        {
            InitializeComponent();
        }

        void _FilldgvPayments(int NumberOfPage)
        {
            ctrlSwitchSearch1.NumberOfPage = NumberOfPage;
            _dtAllPayments = clsPayment.GetAllPaymentsBy(_paymentType, _PersonID, NumberOfPage, 7);
            dgvPayments.DataSource = _dtAllPayments;
            lblRecordsCount.Text = dgvPayments.Rows.Count.ToString();
        }

        void _PraperTheControl()
        {
            ctrlSwitchSearch1.MaxNumberOfPage = clsGet.GetMaximamPage(clsPayment.NumberOfPaymentsBy(_paymentType, _PersonID), 7);
            _FilldgvPayments(1);
            lblWhenPaymentsEmpty.Visible = dgvPayments.Rows.Count == 0;
            ctrlSwitchSearch1.Visible = !lblWhenPaymentsEmpty.Visible;
            SwitchCardType.Checked = false;
        }

        public void ctrlPayments_Load(int PersonID)
        {
            _PersonID = PersonID;
            cbPaymentType.SelectedIndex = 0;
            cbUserCards1.Fill_cbUserCardsBy(clsUser.FindByPersonID(PersonID).UserID.Value);
            _PraperTheControl();
            dgvPayments.EnableHeadersVisualStyles = false;
            dgvPayments.ColumnHeadersDefaultCellStyle.BackColor = Color.LightSteelBlue;
        }

        private void cbPaymentType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPaymentType.Text == "Application")
            {
                showToolStripMenuItem.Text = "Show Application Info";
                _paymentType = clsPayment.enPaymentType.Application;
            }
            else
            {
                showToolStripMenuItem.Text = "Show Appointment Info";
                _paymentType = clsPayment.enPaymentType.Appointment;
            }

            _PraperTheControl();
        }

        private void ctrlSwitchSearch1_ChangePageToLeft(object sender, MyGeneralUserControl.ctrlSwitchSearch.clsEVentHandler e)
        {
            _FilldgvPayments(e.CurrentNumberOfPage);
        }

        private void cbUserCards1_SelectedIndexChanged(object sender, EventArgs e)
        {
            _dtAllPayments.DefaultView.RowFilter 
                = string.Format("[{0}] = '{1}'", "CardType", cbUserCards1.Text);

            lblRecordsCount.Text = dgvPayments.Rows.Count.ToString();
        }

        private void SwitchCardType_CheckedChanged(object sender, EventArgs e)
        {
            pCardType.Enabled = SwitchCardType.Checked;
            ctrlSwitchSearch1.Visible = !SwitchCardType.Checked;

            if (ctrlSwitchSearch1.Visible)
            {
                ctrlSwitchSearch1.Visible = true;
                _dtAllPayments.DefaultView.RowFilter = "";
                lblRecordsCount.Text = dgvPayments.Rows.Count.ToString();
            }

        }

        private void cmsPayments_Opening(object sender, CancelEventArgs e)
        {
            cmsPayments.Enabled = !lblWhenPaymentsEmpty.Visible
                && dgvPayments.Rows.Count > 0;
        }

        private void showBillInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmPaymentInfo paymentInfo 
                = new frmPaymentInfo((int)dgvPayments.CurrentRow.Cells[0].Value);
            paymentInfo.ShowDialog();

        }

        private void showToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if(_paymentType == clsPayment.enPaymentType.Application)
            {
                frmApplicationBasicInfo applicationBasicInfo
                = new frmApplicationBasicInfo((int)dgvPayments.CurrentRow.Cells[1].Value);
                applicationBasicInfo.ShowDialog();
            }
            else
            {
                frmTestAppointmentInfo testAppointmentInfo
                = new frmTestAppointmentInfo((int)dgvPayments.CurrentRow.Cells[1].Value);
                testAppointmentInfo.ShowDialog();
            }
            
        }

        private void ctrlPayments_Load(object sender, EventArgs e)
        {

        }
    }
}
