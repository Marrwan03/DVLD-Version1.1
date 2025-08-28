using DVLD_Buisness.Classes;
using DVLD_DataAccess;
using System;
using System.Runtime.Remoting.Messaging;


namespace DVLD_Buisness
{
    public class clsEmail : clsCommunication
    {
        #region _Private
        enMode _Mode;
        enum enMode : byte { Add, Update}
        public enum enType : byte { YourEmail = 1, YourMessages }

        bool _AddNewMessage()
        {
            _EmailID = clsEmailsData.AddSendNewMessage(this.SenderID,(byte)this.FromType, this.Message,
                DateTime.Now, this.RecipientID, (byte)this.RecipientType);
            return _EmailID.HasValue;
        }
        bool _UpdateMessage()
        {
            return clsEmailsData.UpdateMessage(this.EmailID.Value,this.SenderID, (byte)this.FromType, this.Message,
                DateTime.Now, this.RecipientID, (byte)this.RecipientType);
        }

        #endregion

         int? _EmailID;
        public Nullable<int> EmailID { get { return _EmailID; } }
        public int SenderID { get; set; }
        public string SenderTypeString { get { return GetStringOfFromType(); } }

        (string Name, string Address) _GetInfoBy(int ID, byte Type)
        {
            switch (Type)
            {
                case 1:
                    {
                        clsPerson person = clsPerson.Find(ID);
                        if (person != null)
                            return (person.FullName, person.Email);
                        break;
                    }
                   
                case 2:
                    {
                        clsUser user = clsUser.FindByUserID(ID);
                        if(user != null)
                            return (user.PersonInfo.FullName, user.PersonInfo.Email);
                        break;
                    }
                    
                case 3:
                    {
                        clsEmployee employee = clsEmployee.FindByEmployeeID(ID);
                        if(employee != null)
                            return (employee.PersonInfo.FullName, employee.PersonInfo.Email);
                        break;
                    }
                    
            }
            return (null, null);
        }

        public (string Name, string Address) SenderInfo { get {return _GetInfoBy(SenderID, (byte)FromType); } }

        public string Message { get; set; }
        public DateTime MessageTime { get; set; }
        public int RecipientID { get; set; }
        public string RecipientTypeString { get { return GetRecipientType(); } }
        public (string Name, string Address) RecipientInfo { get { return _GetInfoBy(RecipientID, (byte)RecipientType); } }

        clsEmail(int? emailID,int senderID,enFrom senderType, string message,
             DateTime messageTime, int recipientID, enFor recipientType)
        {
            _EmailID = emailID;
            SenderID = senderID;
            FromType = senderType;
            Message = message;
            MessageTime = messageTime;
            RecipientID = recipientID;
            RecipientType = recipientType;            
            
            _Mode = enMode.Update;
        }

        public clsEmail()
        {
            _EmailID = null;
            SenderID = 0;
            FromType = enFrom.None;
            Message = null;
            MessageTime = DateTime.MinValue;
            RecipientID = 0;
            RecipientType = enFor.None;

            _Mode = enMode.Add;
        }

        public static clsEmail Find(int MessageID)
        {
            int senderID=0, recipientID = 0;
            byte senderType = 0, recipientType = 0;
            string message=null;
            DateTime time=DateTime.Now;
            if(clsEmailsData.FindEmailByID(MessageID,ref senderID, ref senderType,ref message,ref time,ref recipientID,ref recipientType))
            {
                return new clsEmail(MessageID, senderID, (enFrom)senderType, message, time, recipientID, (enFor)recipientType);
            }
            return null;
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    {
                        if(_AddNewMessage())
                            return true;
                        break;
                    }
                    case enMode.Update:
                    {
                        return _UpdateMessage();
                    }
            }
            return false;
        }

        public static bool IsRecipientExists(enFor For, int RecipientID)
        {
            switch (For)
            {
                case enFor.ForPerson:
                    {
                        return clsPerson.isPersonExist(RecipientID);
                    }
                case enFor.ForUser:
                    {
                        return clsUser.isUserExist(RecipientID);
                    }
                case enFor.ForEmployee:
                    {
                        return clsEmployee.IsEmplooyeeExists(RecipientID);
                    }
            }

            return false;
        }

        public static bool IsRecipientHasEmail(enFor For, int RecipientID)
        {
            switch (For)
            {
                case enFor.ForPerson:
                    {
                        return clsPerson.IsPersonHasEmail(RecipientID);
                    }
                case enFor.ForUser:
                    {
                        return clsUser.FindByUserID(RecipientID).PersonInfo.IsPersonHasEmail();
                    }
                case enFor.ForEmployee:
                    {
                        return clsEmployee.FindByEmployeeID(RecipientID).PersonInfo.IsPersonHasEmail();
                    }
            }

            return false;
        }

        public static bool DeleteMessage(int MessageID)
        {
            return clsEmailsData.DeleteMessage(MessageID);
        }

       

    }
}
