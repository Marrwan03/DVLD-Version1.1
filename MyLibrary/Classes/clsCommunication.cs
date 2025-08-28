using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness.Classes
{
    public  class clsCommunication
    {
        public enum enFor : byte { None, ForPerson = 1, ForUser, ForEmployee }
        public enFor RecipientType { get; set; }
        public string GetRecipientType()
        => _GetStringType((byte)RecipientType);

        public enum enFrom : byte { None, ByPerson = 1, ByUser = 2, ByEmployee = 3 }
       public enFrom FromType { get; set; }
        public string GetStringOfFromType()
             => _GetStringType((byte) FromType);

        string _GetStringType(byte Type)
        {
            switch (Type)
            {
                case 1:
                    return "Person";
                case 2:
                    return "User";
                case 3:
                    return "Employee";

            }
            return null;
        }
    }
}
