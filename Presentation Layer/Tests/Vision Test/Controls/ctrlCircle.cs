using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Vision_Test.Controls
{
    public partial class ctrlCircle : UserControl
    {
        public ctrlCircle()
        {
            InitializeComponent();
        }
        public Font SizeOfFont 
        {

            set
            {

                lblCharctar.Font = value;
            }
            get
            {
                return lblCharctar.Font; 
            }
        }

        public bool Turn
        {
            get { return guna2WinProgressIndicator1.Enabled; }
            set
            {
                if(value)
                {
                    guna2WinProgressIndicator1.Start();
                }
                else
                    guna2WinProgressIndicator1.Stop();

            }
        }

        public Color ColorOfControl 
        {
            get { return guna2WinProgressIndicator1.ProgressColor; }

            set
            {
                guna2WinProgressIndicator1.ProgressColor = value;
            }
        }

        public string TextOfCharctar 
        {
            set
            {
                lblCharctar.Text = value;
            }
            get
            {
                return lblCharctar.Text;
            }
        }

    }
}
