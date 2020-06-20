using System;
using Windows.Storage;
using System.Net.NetworkInformation;

namespace SchedulerHost.Data
{
    public static class LocalDataStore
    {
        private static ApplicationDataContainer localSettings;

        public static void CreateLocalConfigurationSettings()
        {
            localSettings = ApplicationData.Current.LocalSettings;
            //localSettings.Values["SchedulerStartTime"] = new TimeSpan(14, 28, 00);
            //localSettings.Values["SchedulerEndTime"] = new TimeSpan(14, 29, 00);
            localSettings.Values["SchedulerStartTime"] = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(1).Minute, 0);
            localSettings.Values["SchedulerEndTime"] = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, 0);
            localSettings.Values["BackgroundTaskEntryPoint"] = "RaspberryPiUwpHeadlessApp.StartupTask";
            localSettings.Values["BackgroundTaskName"] = "StartupTask";
            localSettings.Values["BackgroundTriggerName"] = "ApplicationTrigger";
            localSettings.Values["DispatcherTimerInterval"] = new TimeSpan(0, 0, 15);
        }

        public static TimeSpan SchedulerStartTime => (TimeSpan)localSettings.Values["SchedulerStartTime"];
        public static TimeSpan SchedulerEndTime => (TimeSpan)localSettings.Values["SchedulerEndTime"];
        public static TimeSpan DispatcherTimerInterval => (TimeSpan)localSettings.Values["DispatcherTimerInterval"];
        public static String BackgroundTaskEntryPoint => (String)localSettings.Values["BackgroundTaskEntryPoint"];
        public static String BackgroundTaskName => (String)localSettings.Values["BackgroundTaskName"];
        public static String BackgroundTriggerName => (String)localSettings.Values["BackgroundTriggerName"];

    }
}
