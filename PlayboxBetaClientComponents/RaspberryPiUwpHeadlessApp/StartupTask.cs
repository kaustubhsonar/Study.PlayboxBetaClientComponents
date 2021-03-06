﻿using System;
using System.Diagnostics;
using System.Threading;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Diagnostics;

namespace RaspberryPiUwpHeadlessApp
{
    public sealed class StartupTask : IBackgroundTask
    {
        private const string loggingChannelId = "4bd2826e-54a1-4ba9-bf63-92b73ea1ac4a";
        private BackgroundTaskDeferral taskDeferral;
        public void Run(IBackgroundTaskInstance taskInstance)
        {
            using (var loggingChannel = new LoggingChannel("my provider", null, new Guid(loggingChannelId)))
            {
                loggingChannel.LogMessage("Back ground task started");
                if (taskInstance == null)
                {
                    return;
                }
                taskDeferral = taskInstance.GetDeferral();
                LocalDataStore.CreateLocalConfigurationSettings();
               // taskInstance.Canceled += new BackgroundTaskCanceledEventHandler(OnCanceled);
                var processor = new Processor(loggingChannel);
                processor.Process();                
                Thread.Sleep(28800000);//ToDo-Temporary provision to run this service for a specified period(8 hours)..Should add Scheduler logic.
                taskDeferral.Complete();
                loggingChannel.LogMessage("Task completed at " + DateTime.UtcNow.ToString());
            }
        }
        private void OnCanceled(IBackgroundTaskInstance sender, BackgroundTaskCancellationReason reason)
        {
            //
            // Indicate that the background task is canceled.
            //
            //_cancelRequested = true;
            //_cancelReason = reason;

            Debug.WriteLine("Background " + sender.Task.Name + " Cancel Requested...");
        }
    }
}
