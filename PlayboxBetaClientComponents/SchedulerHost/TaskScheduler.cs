using SchedulerHost.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;
using Windows.UI.Xaml;

namespace SchedulerHost
{
    class TaskScheduler
    {
        public TaskScheduler()
        {
            LocalDataStore.CreateLocalConfigurationSettings();
            DispatcherTimerSetup();
        }

        ApplicationTrigger taskTrigger = null;
        DispatcherTimer dispatcherTimer;
        public bool taskRunning = false;
        public int runCount = 0;
        public void DispatcherTimerSetup()
        {
            dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += DispatcherTimerTick;
            dispatcherTimer.Interval = LocalDataStore.DispatcherTimerInterval;
            dispatcherTimer.Start();
        }
        void DispatcherTimerTick(object sender, object e)
        {
            if (IsScheduleTime())
            {
                if (!IsTaskRunning())
                    RegisterTask();
            }
            else
            {
                if (IsTaskRunning())
                    UnRegisterTask();
            }

        }

        private bool IsTaskRunning()
        {
            taskRunning = false;            
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == LocalDataStore.BackgroundTriggerName)
                {                    
                    taskRunning = true;
                }
            }            
            Debug.WriteLine("............................................................................................Datetime : " + DateTime.Now + " | IsTaskRunning:" + taskRunning.ToString() + " | Schedule time:" + LocalDataStore.SchedulerStartTime.ToString());
            return taskRunning;
        }

        private bool IsScheduleTime()
        {
            Debug.WriteLine("............................................................................................Datetime : " + DateTime.Now + " in IsScheduleTime");
            if (DateTime.Now.TimeOfDay > LocalDataStore.SchedulerStartTime && DateTime.Now.TimeOfDay < LocalDataStore.SchedulerEndTime)
                return true;
            else
                return false;
        }

        public void RegisterTask()
        {
            runCount++;
            Debug.WriteLine("............................................................................................Datetime : " + DateTime.Now + " in RegisterTask ...............Run Count : " + runCount);
            taskTrigger = new ApplicationTrigger();
            var task = SchedulerConfigurations.RegisterBackgroundTask(LocalDataStore.BackgroundTaskEntryPoint,
                                                                   LocalDataStore.BackgroundTriggerName,
                                                                   taskTrigger,
                                                                   null);
           // taskRunning = true;
            StartBackgroundTask();
        }

        private async void StartBackgroundTask()
        {
            // Reset the completion status
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(LocalDataStore.BackgroundTriggerName);

            //Signal the ApplicationTrigger
            var result = await taskTrigger.RequestAsync();

        }

        private void UnRegisterTask()
        {
            Debug.WriteLine("............................................................................................Datetime : " + DateTime.Now + " in UnRegisterTask");
            SchedulerConfigurations.UnregisterBackgroundTasks(LocalDataStore.BackgroundTriggerName);
            //taskRunning = false;

            Debug.WriteLine("********************************************************************************************New Schedule time at :" + DateTime.Now.AddMinutes(2).TimeOfDay.ToString());
            ApplicationDataContainer localSettings1;
            localSettings1 = ApplicationData.Current.LocalSettings;
            localSettings1.Values["SchedulerStartTime"] = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(2).Minute, 0);
            localSettings1.Values["SchedulerEndTime"] = new TimeSpan(DateTime.Now.Hour, DateTime.Now.AddMinutes(4).Minute, 0);

        }

    }
}
