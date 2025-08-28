using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Archive
{
    public partial class frmAllArchive : Form
    {
        public frmAllArchive()
        {
            InitializeComponent();
        }

        public Action OnClose;
        private void btnClose_Click(object sender, EventArgs e)
        {
            OnClose?.Invoke();
            this.Close();
        }

        private void frmAllArchive_Load(object sender, EventArgs e)
        {
            ctrlArchiveFor1.ctrlArchiveForPeople_Load();
        }

        private void ctrlArchiveFor1_Load(object sender, EventArgs e)
        {

        }
    }
}
