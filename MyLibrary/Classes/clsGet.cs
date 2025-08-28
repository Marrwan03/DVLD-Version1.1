using DVLD_Buisness.Classes;
using DVLD_DataAccess;
using DVLD_DataAccess.MyFunSQL;

using System.Collections.Generic;

using System.IO;


namespace DVLD_Buisness.Global_Classes
{
    public class clsGet
    {
        public static int GetMaximamPage(int NumberOfRows, int DivRowsBy=3)
        {
            return clsGets.GetMaximamPage(NumberOfRows, DivRowsBy);
        }

        public static int GetMaxNumberOfRowsForEmail(clsEmail.enType Type,
        int SenderID, clsCommunication.enFrom SenderType, int RecipientID, clsCommunication.enFor RecipientType)
            => clsGets.GetMaxNumberOfRowsForEmail((byte)Type, SenderID, (byte)SenderType, RecipientID, (byte)RecipientType);

        public static int GetMaxNumberOfRowsForCallLog(clsCallLog.enOrderType Type,
            int CallerID, clsCommunication.enFrom CallerType,
            int RecipientID, clsCommunication.enFor RecipientType)
            => clsGets.GetMaxNumberOfRowsForCallLog((byte)Type,CallerID, (byte)CallerType,RecipientID, (byte)RecipientType);

       


    }
}
