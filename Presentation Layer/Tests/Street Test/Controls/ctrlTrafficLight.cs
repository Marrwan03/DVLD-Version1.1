using DVLD.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DVLD.Tests.Street_Test.Controls
{
    public partial class ctrlTrafficLight : UserControl
    {
        public class clsEventHandler : EventArgs
        {
            public string ColorName { get; set; }
            public int ColorTime { get; set; }

            public clsEventHandler(string colorName, int colorTime)
            {
                ColorName = colorName;
                ColorTime = colorTime;
            }
        }
        public event EventHandler<clsEventHandler> OnRed;
        public event EventHandler<clsEventHandler> OnOrange;
        public event EventHandler<clsEventHandler> OnGreen;

        public enum enTrafficLight { Red, Orange, Green };
        public int GreenTime { get; set; }
        public int RedTime { get; set; }
        public int OrangeTime { get; set; }
        enTrafficLight _CurrentLight;
        public enTrafficLight CurrentLight
        {
            get { return _CurrentLight; }
            set
            {
                _CurrentLight = value;
                _DesignTrafficLight(_CurrentLight);
            }
        }

        void _DesignTrafficLight(enTrafficLight TrafficLight)
        {
            switch (TrafficLight)
            {
                case enTrafficLight.Red:
                    {
                        picTrafficLight.Image = Resources.Red;
                        lblTimer.ForeColor = Color.Red;
                        break;
                    }
                case enTrafficLight.Orange:
                    {
                        picTrafficLight.Image = Resources.Orange;
                        lblTimer.ForeColor = Color.Orange;
                        break;
                    }
                case enTrafficLight.Green:
                    {
                        picTrafficLight.Image = Resources.Green;
                        lblTimer.ForeColor = Color.Green;
                        break;
                    }
            }
        }
        int _MaximamTimer = 0;

        int _GetMaximamTimer()
        {
            switch (_CurrentLight)
            {
                case enTrafficLight.Red:
                    {
                        return RedTime;
                    }
                case enTrafficLight.Orange:
                    {
                        return OrangeTime;
                    }
                case enTrafficLight.Green:
                    {
                        return GreenTime;
                    }
            }
            return 0;
        }

        enTrafficLight _GetNextTrafficLight()
        {
            switch (_CurrentLight)
            {
                case enTrafficLight.Red:
                    {
                        OnGreen?.Invoke(this, new clsEventHandler("Green", GreenTime));
                        return enTrafficLight.Green;
                    }
                case enTrafficLight.Orange:
                    {
                        OnRed?.Invoke(this, new clsEventHandler("Red", RedTime));
                        return enTrafficLight.Red;
                    }
                case enTrafficLight.Green:
                    {
                        OnOrange?.Invoke(this, new clsEventHandler("Orange", OrangeTime));
                        return enTrafficLight.Orange;
                    }
            }
            return enTrafficLight.Red;
        }
        public void ctrlTrafficLight_Load()
        {
            timer1.Start();
        }
        public void ctrlTrafficLight_Off()
        {
            timer1.Stop();
        }
        public ctrlTrafficLight()
        {
            InitializeComponent();
        }

        private void ctrlTrafficLight_Load(object sender, EventArgs e)
        {
            _MaximamTimer = _GetMaximamTimer();
            RedTime = 10;
            OrangeTime = 5;
            GreenTime = 5;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_MaximamTimer == 0)
            {
                _CurrentLight = _GetNextTrafficLight();
                _DesignTrafficLight(_CurrentLight);
                _MaximamTimer = _GetMaximamTimer();
            }
            --_MaximamTimer;
            lblTimer.Text = _MaximamTimer.ToString();
        }
    }
}
