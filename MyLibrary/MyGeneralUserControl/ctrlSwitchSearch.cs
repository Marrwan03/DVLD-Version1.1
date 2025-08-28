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
    public partial class ctrlSwitchSearch : UserControl
    {
        public ctrlSwitchSearch()
        {
            InitializeComponent();
        }

      public  class clsEVentHandler : EventArgs
        {
            public int CurrentNumberOfPage { get; }

            public clsEVentHandler(int currentNumberOfPage)
            {
                CurrentNumberOfPage = currentNumberOfPage;
               
            }
        }


        public event EventHandler<clsEVentHandler> ChangePageToRight;
        public event EventHandler<clsEVentHandler> ChangePageToLeft;

        int _MaxNumberOfPage;
        public int MaxNumberOfPage { get { return _MaxNumberOfPage; } set 
            {
                _MaxNumberOfPage = value;
                ctrlSwitchSearch_Load(null, null);
            } }
        int _NumberOfPage;
       public int NumberOfPage {
            get {  return _NumberOfPage; }
            set
            {
                _NumberOfPage = value;
                if (_NumberOfPage > MaxNumberOfPage)
                    _NumberOfPage = 0;

                btnNumber.Text = _NumberOfPage.ToString();
            } }
        enum enTurn { Right, Left}

        void _CalculateNumberOfPage(enTurn turn)
        {
            if (NumberOfPage >= MaxNumberOfPage && turn != enTurn.Left)
            {
                NumberOfPage = 1;
                ChangePageToRight(this, new clsEVentHandler(NumberOfPage));
            }
            else
            {
                switch(turn) 
                {
                    case enTurn.Left:
                        {
                            if(NumberOfPage > 1)
                            {
                                NumberOfPage -= 1;
                                ChangePageToLeft(this, new clsEVentHandler(NumberOfPage));
                            }
                            else
                            {
                                NumberOfPage = MaxNumberOfPage;
                                ChangePageToLeft(this, new clsEVentHandler(NumberOfPage));
                            }
                            break;
                        }
                        default:
                        {
                            NumberOfPage += 1;
                            ChangePageToRight(this, new clsEVentHandler(NumberOfPage));
                            break;
                        }
                }
            }
            
        }

        private void btnRight_Click(object sender, EventArgs e)
        {
            _CalculateNumberOfPage(enTurn.Right);
        }

        private void ctrlSwitchSearch_Load(object sender, EventArgs e)
        {
            btnRight.Enabled = MaxNumberOfPage > 1;
            btnLeft.Enabled = btnRight.Enabled;

            btnNumber.Text = NumberOfPage.ToString();

        }

        private void btnLeft_Click(object sender, EventArgs e)
        {
            _CalculateNumberOfPage(enTurn.Left);
        }
    }
}
