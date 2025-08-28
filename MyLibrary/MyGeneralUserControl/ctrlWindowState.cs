using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.MyGeneralUserControl
{
    public partial class ctrlWindowState : UserControl
    {
        public ctrlWindowState()
        {
            InitializeComponent();
        }
        [
            Category("Events WindowState"),
            Description("Set the operation when WindowState is close")
            
            ]
        public event Action Close;
        [
           Category("Events WindowState"),
           Description("Set the operation when WindowState is RestoreDown")

           ]
        public event Action RestoreDown;
        [
           Category("Events WindowState"),
           Description("Set the operation when WindowState is maximize")

           ]
        public event Action maximize;
        [
           Category("Events WindowState"),
           Description("Set the operation when WindowState is minimize")

           ]
        public event Action minimize;
        private void picRestoreDown_Click(object sender, EventArgs e)
        {
            picmaximize.Visible = true;
            picRestoreDown.Visible = false;
            RestoreDown?.Invoke();
        }

        private void picmaximize_Click(object sender, EventArgs e)
        {

            picmaximize.Visible = false;
            picRestoreDown.Visible = true;
            maximize?.Invoke();
        }

        private void picClose_Click(object sender, EventArgs e)
        {
            Close?.Invoke();
        }

        private void picMinimize_Click(object sender, EventArgs e)
        {
            minimize?.Invoke();
        }
    }
}
