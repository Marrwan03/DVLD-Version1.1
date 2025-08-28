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
    public partial class ctrlSetYourNote : UserControl
    {
        public ctrlSetYourNote()
        {
            InitializeComponent();
        }

        public class clsNote : EventArgs
        {
            public string Note { get; }
            public clsNote(string note)
            {
                Note = note;
            }

        }

        public event EventHandler<clsNote> SetNote; 
        private void btnConfirm_Click(object sender, EventArgs e)
        {
            if (txtNote.Text != "NULL")
                SetNote?.Invoke(this, new clsNote(txtNote.Text));
        }

        private void txtNote_TextChanged(object sender, EventArgs e)
        {

        }



        private void txtNote_Enter(object sender, EventArgs e)
        {
            if (txtNote.Text == "NULL")
            {
                txtNote.Text = "";
            }
        }

        private void txtNote_Leave(object sender, EventArgs e)
        {
            if (txtNote.Text.Trim() == "")
            {
                txtNote.Text = "NULL";
            }
        }
    }
}
