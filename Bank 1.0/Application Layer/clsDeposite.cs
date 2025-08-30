using DVLD_Buisness;
using System;
using DataAccess;
using System.Data;

namespace DataLogic2
{
    public class clsDeposite
    {        
        public enum enMode { Add, Update}
        enMode _Mode;
        public int? DepositeID { get; private set;}
        public int?  FromCardID { get;  set; }
        clsCreditCard _FromCardInfo { get; set; }
        public clsCreditCard FromCardInfo { get { return _FromCardInfo; } }
        public DateTime DateTime { get; set; }
        public decimal Amount { get; set; }
        public int? ToCardID { get; set; }
        clsCreditCard _ToCardInfo { get; set; }
        public clsCreditCard ToCardInfo { get { return _ToCardInfo; } }
        public string Note {  get; set; }

        bool _AddNewDeposite()
        {
            this.DepositeID = clsDepositeData.AddNewDeposite(this.FromCardID.Value, this.DateTime,
                this.Amount, this.ToCardID.Value, this.Note);

            return this.DepositeID.HasValue;
        }
        bool _UpdateDeposite()
        {
            return clsDepositeData.UpdateDeposite(this.DepositeID.Value, this.FromCardID.Value, this.DateTime,
                this.Amount, this.ToCardID.Value, this.Note);
        }

        public clsDeposite(int depositeID, int fromCardID, DateTime dateTime, decimal amount, int toCardID, string note)
        {
            DepositeID = depositeID;
            FromCardID = fromCardID;
            DateTime = dateTime;
            Amount = amount;
            ToCardID = toCardID;
            Note = note;

            _FromCardInfo = clsCreditCard.Find(FromCardID.Value);
            _ToCardInfo = clsCreditCard.Find(ToCardID.Value);
            _Mode = enMode.Update;
        }
        public clsDeposite()
        {
            DepositeID = null;
            FromCardID = null;
            DateTime = DateTime.Now;
            Amount = 0;
            ToCardID = null;
            Note = null;

            _Mode = enMode.Add;
        }

        public static clsDeposite FindLastDepositeBy(int FromCardID)
        {
            int depositeID = 0;
            decimal amount = 0;
            int toCardID = 0;
            string note = null;
            DateTime datetime = DateTime.Now;

            if (clsDepositeData.FindLastDepositeBy(ref depositeID, FromCardID,ref datetime, ref amount, ref toCardID, ref note))
            {
                return new clsDeposite(depositeID, FromCardID, datetime, amount, toCardID, note);
            }
            return null;
        }

        public static clsDeposite FindLastWithdrawBy(int ToCardID)
        {
            int depositeID = 0;
            decimal amount = 0;
            int fromCardID = 0;
            string note = null;
            DateTime datetime = DateTime.Now;

            if (clsDepositeData.FindLastWithdrawBy(ref depositeID,ref fromCardID, ref datetime, ref amount, ToCardID, ref note))
            {
                return new clsDeposite(depositeID,fromCardID, datetime, amount, ToCardID, note);
            }
            return null;
        }

        public static clsDeposite Find(int depositeID)
        {
            int FromCardID = 0;
            DateTime datetime = DateTime.Now;
            decimal amount = 0;
            int toCardID = 0;
            string note = null;

            if (clsDepositeData.Find( depositeID,ref FromCardID,ref datetime, ref amount, ref toCardID, ref note))
            {
                return new clsDeposite(depositeID, FromCardID, datetime, amount, toCardID, note);
            }
            return null;
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    {
                        if(_AddNewDeposite())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        break;
                    }
                    case enMode.Update:
                    {
                        return _UpdateDeposite();
                    }
            }
            return false;

        }
        public static DataTable GetWithdrawLogBy(int CardID)
        {
            return clsDepositeData.GetWithdrawLogBy(CardID);
        }
        public static DataTable GetDepositeLogBy(int CardID)
        {
            return clsDepositeData.GetDepositeLogBy(CardID);
        }
        public static int GetNumberOfDepositeBy(int CardID)
        {
            return clsDepositeData.GetNumberOfDepositeBy(CardID);
        }
        public static int GetNumberOfWithdrawBy(int CardID)
        {
            return clsDepositeData.GetNumberOfWithdrawBy(CardID);
        }
        public static bool CalculateBalanceAfterDeposite(int DepositeID)
        {
            return clsDepositeData.CalculateBalanceAfterDeposite(DepositeID);
        }
        public bool CalculateBalanceAfterDeposite()
        {
            return CalculateBalanceAfterDeposite(this.DepositeID.Value);
        }

    }
}
