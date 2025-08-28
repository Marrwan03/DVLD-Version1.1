using DVLD_Buisness.Classes;
using DVLD_DataAccess;
using System;
using System.Data;

namespace DVLD_Buisness
{
    public class clsCreditCard
    {
        public enum enMode { Add, Update }
        enMode _Mode;
        public enum enTypeOfCreditCard
        {
            None, Visa, MasterCard,
            QNB, SAB,
            UnionPay, Discover
        }
        public static enTypeOfCreditCard GetTypeOfCreditCard(string typestringOfCreditCard)
        {
            enTypeOfCreditCard typeOfCreditCard;

            switch (typestringOfCreditCard)
            {
                case "Visa":
                    typeOfCreditCard = enTypeOfCreditCard.Visa;
                    break;
                case "MasterCard":
                    typeOfCreditCard = enTypeOfCreditCard.MasterCard; break;
                case "QNB":
                    typeOfCreditCard = enTypeOfCreditCard.QNB; break;
                case "SAB":
                    typeOfCreditCard = enTypeOfCreditCard.SAB; break;
                case "UnionPay":
                    typeOfCreditCard = enTypeOfCreditCard.UnionPay; break;
                case "Discover":
                    typeOfCreditCard = enTypeOfCreditCard.Discover; break;
                default:
                    typeOfCreditCard = enTypeOfCreditCard.None; break;
            }
            return typeOfCreditCard;

        }

        public static string GetStringTypeOfCreditCard(byte typeOfCreditCard)
        {
            switch ((enTypeOfCreditCard)typeOfCreditCard)
            {
                case enTypeOfCreditCard.Visa:
                    return "Visa";
                case enTypeOfCreditCard.MasterCard:
                    return "MasterCard";
                case enTypeOfCreditCard.QNB:
                    return "QNB";
                case enTypeOfCreditCard.SAB:
                    return "SAB";
                case enTypeOfCreditCard.UnionPay:
                    return "UnionPay";
                case enTypeOfCreditCard.Discover:
                    return "Discover";
                default:
                    return "Type";
            }
        }
        public string GetStringTypeOfCreditCard()
        {
            return GetStringTypeOfCreditCard((byte)this.CardType);
        }
        int? _CardID;
        public int? CardID { get { return _CardID; } }
        public int? UserID { get; set; }
        clsUser _UserInfo;
        public clsUser UserInfo { get { return _UserInfo; } }

        public string CardNumber { get; set; }
        public string CVV { get; set; }
        DateTime _IssueDate;
        public DateTime IssueDate { get { return _IssueDate; } }
        public decimal Balance { get; set; }
        public enTypeOfCreditCard CardType { get; set; }
        public bool IsActive { get; set; }

        bool _AddNewCreditCard()
        {
            this.CardNumber = clsSymmetric.ReverseValue(clsSymmetric.Encrypt(CardNumber, clsSymmetric.GetKeyValue()));
            this._CardID = clsCreditCardData.AddNewCreaditCard(this.UserID.Value, this.CardNumber, CVV, IssueDate, (byte)CardType, Balance, IsActive);
            return this.CardID.HasValue;
        }

        bool _UpdateCreditCard()
        {
            this.CardNumber = clsSymmetric.ReverseValue(clsSymmetric.Encrypt(CardNumber, clsSymmetric.GetKeyValue()));
            return clsCreditCardData.UpdateCreaditCardBy(CardID.Value, this.UserID.Value, this.CardNumber, CVV, IssueDate, (byte)CardType, Balance, IsActive);
        }

        public clsCreditCard(int cardID, int userID, string cardNumber, string cVV,
            DateTime issueDate, decimal balance, enTypeOfCreditCard cardType, bool isActive)
        {

            _CardID = cardID;
            UserID = userID;
            CardNumber = clsSymmetric.Decrypt(clsSymmetric.ReverseValue(cardNumber), clsSymmetric.GetKeyValue());
            CVV = cVV;
            _IssueDate = issueDate;
            Balance = balance;
            CardType = cardType;
            IsActive = isActive;

            _UserInfo = clsUser.FindByUserID(userID);
            _Mode = enMode.Update;
        }

        public clsCreditCard()
        {
            _CardID = null;
            CardNumber = null;
            CVV = null;
            UserID = null;
            _IssueDate = DateTime.Now;
            Balance = 0;
            CardType = enTypeOfCreditCard.None;
            IsActive = false;

            _Mode = enMode.Add;
        }

        public static clsCreditCard Find(int CardID)
        {
            int userID = 0;
            decimal balance = 0;
            string cardNumber = string.Empty, cVV = string.Empty;
            DateTime issueDate = DateTime.MinValue;
            byte CardType = 0;
            bool IsActive = false;

            if (clsCreditCardData.FindByCardID(CardID, ref userID, ref cardNumber, ref cVV,
                ref issueDate, ref CardType, ref balance, ref IsActive))
            {
                return new clsCreditCard(CardID, userID, cardNumber, cVV, issueDate, balance, (enTypeOfCreditCard)CardType, IsActive);
            }
            return null;
        }
        public static clsCreditCard Find(int UserID, enTypeOfCreditCard CardType)
        {
            int cardID = 0;
            decimal balance = 0;
            string cardNumber = string.Empty, cVV = string.Empty;
            DateTime issueDate = DateTime.MinValue;
            bool IsActive = false;

            if (clsCreditCardData.FindByUserIDAndCardType(ref cardID, UserID, ref cardNumber, ref cVV,
                ref issueDate, (byte)CardType, ref balance, ref IsActive))
            {
                return new clsCreditCard(cardID, UserID, cardNumber, cVV, issueDate, balance, CardType, IsActive);
            }
            return null;
        }
        public static DataTable FindByUserID(int UserID)
        {
            return clsCreditCardData.FindByUserID(UserID);
        }

        public static bool IsUserHasCreditCardWithSameTypeBy(int UserID, byte CardType)
        {
            return clsCreditCardData.IsUserHasCreditCardWithSameTypeBy(UserID, CardType);
        }

        public bool Save()
        {
            switch (_Mode)
            {
                case enMode.Add:
                    {
                        if (_AddNewCreditCard())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        break;
                    }
                case enMode.Update:
                    {
                        return _UpdateCreditCard();
                    }
            }
            return false;

        }

        public static bool IsCardNumberExists(string CardNumber)
        {
            return clsCreditCardData.IsCardNumberExists(clsSymmetric.ReverseValue(clsSymmetric.Encrypt(CardNumber, clsSymmetric.GetKeyValue())));
        }
        public static bool IsUserHasCreditCardBy(int UserID) => clsCreditCardData.IsUserHasCreditCardBy(UserID);

        public static bool Payment(float Fees, int UserID, byte CardType)
            => clsCreditCardData.Payment(Fees, UserID, CardType);

        public bool Payment(float Fess)
            => Payment(Fess, this.UserID.Value, (byte)this.CardType);
    }
}
