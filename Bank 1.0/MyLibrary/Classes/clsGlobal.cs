using DVLD_Buisness;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bank
{
    static class clsGlobal
    {
        public static clsUser CurrentUser { get; set; }

        public static Region CornerForm(int Width, int Height)
        {
            int radius = 30;
            GraphicsPath path = new GraphicsPath();
            path.StartFigure();
            path.AddArc(0, 0, radius, radius, 180, 90); // الزاوية العليا اليسرى
            path.AddArc(Width - radius, 0, radius, radius, 270, 90); // الزاوية العليا اليمنى
            path.AddArc(Width - radius, Height - radius, radius, radius, 0, 90); // الزاوية السفلى اليمنى
            path.AddArc(0, Height - radius, radius, radius, 90, 90); // الزاوية السفلى اليسرى
            path.CloseFigure();

           return new Region(path);
        }

    }
}
