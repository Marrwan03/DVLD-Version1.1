using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DVLD_Buisness
{
    public class clsPayment
    {
        public enum enPaymentType : byte { Application=1 , Appointment }
        public int? PaymentID { get; set; }
        public int? CardId { get; set; }
        public clsCreditCard CreditCardInfo { get; set; }
        public decimal Amount { get; set; }
        public DateTime DateTime { get; set; }
        bool _AddNewPayment()
        {
            this.PaymentID = clsPaymentsData.AddNewPayment(this.CardId.Value, this.Amount, this.DateTime);
            return this.PaymentID.HasValue;
        }
        public clsPayment(int? paymentID, int cardId, decimal amount, DateTime dateTime)
        {
            PaymentID = paymentID;
            CardId = cardId;
            Amount = amount;
            DateTime = dateTime;
            CreditCardInfo = clsCreditCard.Find(cardId);
        }
        public clsPayment() 
        {
            PaymentID = null;
            CardId = null;
            Amount = 0;
            DateTime = DateTime.Now;
        }
        public bool Save()
        {
            return _AddNewPayment();    
        }
        public static clsPayment Find(int PaymentID)
        {
            int CardID = -1;
            decimal Amount = 0;
            DateTime DateTime = DateTime.Now;
            if(clsPaymentsData.Find(PaymentID, ref CardID, ref Amount, ref DateTime))
            {
                return new clsPayment(PaymentID, CardID, Amount, DateTime);
            }
            return null;
        }
        public static DataTable FindAllPaymentsBy(int UserID)
        {
            return clsPaymentsData.FindAllPaymentsBy(UserID);
        }
        public static DataTable GetAllPaymentsBy(enPaymentType PaymentType, int PersonID, int PageNumber, int RowPerPage)
            => clsPaymentsData.GetAllPaymentsBy((byte)PaymentType, PersonID, PageNumber, RowPerPage);
        public static int NumberOfPaymentsBy(enPaymentType PaymentType, int PersonID)
           => clsPaymentsData.NumberOfPaymentsBy((byte)PaymentType, PersonID);
    }
}
