using SchedulerHost.Data;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using Windows.Storage;

namespace SchedulerHost
{
    class SchedulerConfigurations
    {
        public static bool applicationTriggerTaskRegistered = false;

        /// <summary>
        /// Register a background task with the specified taskEntryPoint, name, trigger,
        /// and condition (optional).
        /// </summary>
        /// <param name="taskEntryPoint">Task entry point for the background task.</param>
        /// <param name="name">A name for the background task.</param>
        /// <param name="trigger">The trigger for the background task.</param>
        /// <param name="condition">An optional conditional event that must be true for the task to fire.</param>
        public static BackgroundTaskRegistration RegisterBackgroundTask(String taskEntryPoint, String name, IBackgroundTrigger trigger, IBackgroundCondition condition, BackgroundTaskRegistrationGroup group = null)
        {
            UnregisterBackgroundTasks(name);
            if (TaskRequiresBackgroundAccess(name))
            {
                // If the user denies access, the task will not run.
                var requestTask = BackgroundExecutionManager.RequestAccessAsync();
            }
            var builder = new BackgroundTaskBuilder();
            builder.Name = name;
            builder.TaskEntryPoint = taskEntryPoint;
            builder.SetTrigger(trigger);

            if (condition != null)
            {
                builder.AddCondition(condition);
                builder.CancelOnConditionLoss = true;
            }

            BackgroundTaskRegistration task = builder.Register();
            UpdateBackgroundTaskRegistrationStatus(name, true);

            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(name);
            return task;
        }
        
        /// <summary>
        /// Unregister background tasks with specified name.
        /// </summary>
        /// <param name="name">Name of the background task to unregister.</param>
        public static void UnregisterBackgroundTasks(String name, BackgroundTaskRegistrationGroup group = null)
        {
            foreach (var cur in BackgroundTaskRegistration.AllTasks)
            {
                if (cur.Value.Name == name)
                {
                    cur.Value.Unregister(true);
                }
            }
            UpdateBackgroundTaskRegistrationStatus(name, false);
        }

        private void AttachProgressAndCompletedHandlers(IBackgroundTaskRegistration task)
        {
            task.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
            task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }
        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {

        }

        /// <summary>
        /// Handle background task completion.
        /// </summary>
        /// <param name="task">The task that is reporting completion.</param>
        /// <param name="e">Arguments of the completion report.</param>
        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            Debug.WriteLine("Datetime : " + DateTime.Now + " in OnCompleted");
            //UnregisterBackgroundTasks(LocalDataStore.BackgroundTriggerName);
            //UpdateBackgroundTaskRegistrationStatus(LocalDataStore.BackgroundTriggerName, false);
        }


        /// <summary>
        /// Store the registration status of a background task with a given name.
        /// </summary>
        /// <param name="name">Name of background task to store registration status for.</param>
        /// <param name="registered">TRUE if registered, FALSE if unregistered.</param>
        public static void UpdateBackgroundTaskRegistrationStatus(String name, bool registered)
        {
            if (name == LocalDataStore.BackgroundTriggerName)
                applicationTriggerTaskRegistered = registered;
        }

        /// <summary>
        /// Get the registration / completion status of the background task with
        /// given name.
        /// </summary>
        /// <param name="name">Name of background task to retreive registration status.</param>
        public static String GetBackgroundTaskStatus(String name)
        {
            var registered = false;
            if (name == LocalDataStore.BackgroundTriggerName)
                registered = applicationTriggerTaskRegistered;

            var status = registered ? "Registered" : "Unregistered";

            object taskStatus;
            var settings = ApplicationData.Current.LocalSettings;
            if (settings.Values.TryGetValue(name, out taskStatus))
            {
                status += " - " + taskStatus.ToString();
            }

            return status;
        }

        /// <summary>
        /// Determine if task with given name requires background access.
        /// </summary>
        /// <param name="name">Name of background task to query background access requirement.</param>
        public static bool TaskRequiresBackgroundAccess(String name)
        {
            if (name == LocalDataStore.BackgroundTriggerName)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
