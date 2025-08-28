
using System.Diagnostics;
namespace DVLD_DataAccess
{
    public class clsEventLog
    {
        /// <summary>
        /// This Procejer to write EventLog when Something happiens
        /// </summary>
        /// <param name="SourceName">SourceName in Event Viewer</param>
        /// <param name="LogName">LogName in Event Viewer</param>
        /// <param name="Message">Message in Event Viewer</param>
        /// <param name="eventLogEntryType">Choice The Type {You should inculde Diagnostics library}</param>
        public static void WriteEventLog(string SourceName, string LogName, string Message, EventLogEntryType eventLogEntryType)
        {
            if (!EventLog.Exists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
            EventLog.WriteEntry(SourceName, Message, eventLogEntryType);
        }
        /// <summary>
        /// This Procejer to write EventLog when Something happiens
        /// </summary>
        /// <param name="Message">Message in Event Viewer</param>
        /// <param name="eventLogEntryType">~Choice The Type {You should inculde Diagnostics library}</param>
        /// <param name="SourceName">~this is the source of error by defult "DVDLApp" </param>
        /// /// <param name="LogName">~this is the LogName of error by defult "ApplicationApplication" </param>
        public static void WriteEventLog(string Message, EventLogEntryType eventLogEntryType,
            string SourceName = "DVDLApp", string LogName = "Application")
        {
            if (!EventLog.SourceExists(SourceName))
            {
                EventLog.CreateEventSource(SourceName, LogName);
            }
            EventLog.WriteEntry(SourceName, Message, eventLogEntryType);
        }
    }
}
