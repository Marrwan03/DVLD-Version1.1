using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using DVLD_Buisness;
using Microsoft.Win32;
using System.Drawing.Drawing2D;
using System.Drawing;
using DVLD.Applications.Controls;
using DVLD.User.Your_Requests.Appointment_Part.Controls;
using System.Configuration;
using System.Linq;

namespace DVLD.Classes
{
    internal static  class clsGlobal
    {
        public static clsUser CurrentUser;
      
        public static clsEmployee CurrentEmployee { set; get; }

       
        public static  void PermisionMessage(string NameOfPart)
        {
            //Set All Admin 
            //Admin has -1 Permision and 281,474,976,710,654.
            MessageBox.Show($"You don`t have permision to open this part[{NameOfPart}],\n\nTalk with Employee to Give you permision.",
                "Don`t Have Pemision", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

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
