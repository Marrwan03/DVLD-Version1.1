using DVLD_Buisness.Classes;
using DVLD_DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static DVLD_Buisness.clsCallLog;

namespace DVLD_Buisness
{
    public class clsCallLog : clsCommunication
    {
        enMode _Mode;
        enum enMode { Add, Update }
        public enum enOrderType {YourOwnCallLog=1, YourCallLog } 
        public int? CallID {  get; set; }
        public int CallerID { get; set; }
       public enFrom CallerType { get; set; }
        public string PhoneNumber { get; set; }
        public DateTime CallTime { get; set; }
        public int Duration { get; set; }
       public enum enCallType { VoiceCall=1, VedioCall, OnlineCall }
        public enCallType CallType { get; set; }
        public string StringCallType { get { return _GetStringCallType(); } }

        public enum enStatus { Completed=1, Missed, Rejected }
        public enStatus CallStatus { get; set;}

        public string StringCallStatus { get { return _GetStringCallStatus(); } }


        public int RecipientID { get; set; }
        public enFor RecipientType { get; set; }




        clsCallLog(int? callID, int callerID,enFrom callerType,  string phoneNumber,
            DateTime callTime, int duration, enCallType callType,
            enStatus callStatus, int recipientID, enFor recipientType) 
        {
            CallID = callID;
            this.CallerID = callerID;
            CallerType = callerType;
            FromType = CallerType;
            PhoneNumber = phoneNumber;
            CallTime = callTime;
            Duration = duration;
            CallType = callType;
            CallStatus = callStatus;
            RecipientID = recipientID;
            base.RecipientType = recipientType;
            this.RecipientType = recipientType;

            _Mode = enMode.Update;
        }
        public clsCallLog()
        {
            CallID = null;
            CallID = -1;
            CallerType = enFrom.None;
            PhoneNumber = null;
            CallTime = DateTime.MinValue;
            Duration = 0;
            CallType = 0;
            CallStatus = 0;
            RecipientID = -1;
            RecipientType  = enFor.None ;
         

            _Mode = enMode.Add;
        }
        string _GetStringCallStatus()
        {
            switch(CallStatus)
            {
                case enStatus.Completed:
                    {
                        return "Completed";
                    }
                    case enStatus.Missed:
                    {
                        return "Missed";
                    }
                    case enStatus.Rejected:
                    {
                        return "Rejected";
                    }
            }
            return null;
        }

        string _GetStringCallType()
        {
            switch(CallType)
            {
                case enCallType.VoiceCall:
                    {
                        return "Voice Call";
                    }
                case enCallType.VedioCall:
                    {
                        return "Vedio Call";
                    }
                    case enCallType.OnlineCall:
                    {
                        return "Online Call";
                    }
            }
            return null;
        }


        bool _AddNewCallLog()
        {
            this.CallID = clsCallLogs.AddNewCallLog(this.CallerID, (byte)this.CallerType, this.PhoneNumber,
                this.CallTime, this.Duration, (byte)this.CallType, (byte)this.CallStatus,
                this.RecipientID, (byte)this.RecipientType);
            return this.CallID.HasValue;
        }

        public bool Save()
        {
            switch(_Mode)
            {
                case enMode.Add:
                    {
                        if(_AddNewCallLog())
                        {
                            _Mode = enMode.Update;
                            return true;
                        }
                        break;
                    }
            }
            return false;
        }


        public static clsCallLog Find(int CallID)
        {
            int CallerID=0, Duration = 0;
            byte CallerType= 0;
            string PhoneNumber = null, Notes = null;
            DateTime CallTime= DateTime.MinValue;
            byte CallType = 0, Status=0;
            int RecipientID = -1;
            byte RecipientType = 0;

            if (clsCallLogs.FindCallLogBy(CallID, ref CallerID, ref CallerType, ref PhoneNumber, ref CallTime, ref Duration, ref CallType, ref Status,
                ref RecipientID, ref RecipientType))
            {
                return new clsCallLog(CallID, CallerID, (enFrom)CallerType, PhoneNumber, CallTime, Duration, (enCallType)CallType, 
                    (enStatus)Status, RecipientID, (enFor)RecipientType);
            }
            return null;
        }

        public static bool Delete(int CallID)
        {
            return clsCallLogs.DeleteCallLog(CallID);
        }

       


    }
}
