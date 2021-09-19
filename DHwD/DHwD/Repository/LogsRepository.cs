using System;
using System.Diagnostics;
using DHwD.Repository.Interfaces;
using Microsoft.AppCenter.Crashes;

namespace DHwD.Repository
{
    public class LogsRepository : ILogs
    {
        public LogsRepository()
        {
        }

        public void LogError(Exception ex)
        {
            AppCenterTrackError(ex);
            ConsoleWrite(ex);
        }

        private void ConsoleWrite(Exception ex)
        {
            Debug.WriteLine(" @@@@ Exception  @@@@");
            Debug.WriteLine(" @@@@ Exception Message  @@@@");
            Debug.WriteLine(ex.Message);
            Debug.WriteLine(" @@@@ Exception Data  @@@@");
            Debug.WriteLine(ex.Data);
            Debug.WriteLine(" @@@@ Exception StackTrace  @@@@");
            Debug.WriteLine(ex.StackTrace);
        }

        private void AppCenterTrackError(Exception ex)
        {
            try
            {
                Crashes.TrackError(ex);
            }
            catch (Exception)
            { }
        }
    }
}
