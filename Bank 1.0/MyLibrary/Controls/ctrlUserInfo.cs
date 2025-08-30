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
    public partial class ctrlUserInfo : UserControl
    {
        public ctrlUserInfo()
        {
            InitializeComponent();
        }

        public void ctrlUserInfo_Load(string Username)
        {
            lblUsername.Text = Username;
            lblCurrentDate.Text = DateTime.Now.ToString("dd/MM/yyyy");
        }

    }
}
