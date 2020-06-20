using SchedulerHost.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel.Background;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace SchedulerHost
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        ApplicationTrigger trigger = null;
        public MainPage()
        {
            this.InitializeComponent();
            LocalDataStore.CreateLocalConfigurationSettings();
            TaskScheduler taskScheduler = new TaskScheduler();
        }

        private void RegisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            trigger = new ApplicationTrigger();
            var task = SchedulerConfigurations.RegisterBackgroundTask(LocalDataStore.BackgroundTaskEntryPoint,
                                                                   LocalDataStore.BackgroundTriggerName,
                                                                   trigger,
                                                                   null);
            // AttachProgressAndCompletedHandlers(task);
            //SignalBackgroundTask(null, null);
            // UpdateUI();
        }

        /// <summary>
        /// Unregister a ApplicationTriggerTask.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void UnregisterBackgroundTask(object sender, RoutedEventArgs e)
        {
            SchedulerConfigurations.UnregisterBackgroundTasks(LocalDataStore.BackgroundTriggerName);
           
        }

        private async void SignalBackgroundTask(object sender, RoutedEventArgs e)
        {
            // Reset the completion status
            var settings = ApplicationData.Current.LocalSettings;
            settings.Values.Remove(LocalDataStore.BackgroundTriggerName);

            //Signal the ApplicationTrigger
            var result = await trigger.RequestAsync();
            //BackgroundTaskSample.ApplicationTriggerTaskResult = "Signal result: " + result.ToString();
            //UpdateUI();
        }

        /// <summary>
        /// Attach progress and completed handers to a background task.
        /// </summary>
        /// <param name="task">The task to attach progress and completed handlers to.</param>
        private void AttachProgressAndCompletedHandlers(IBackgroundTaskRegistration task)
        {
            task.Progress += new BackgroundTaskProgressEventHandler(OnProgress);
            task.Completed += new BackgroundTaskCompletedEventHandler(OnCompleted);
        }

        /// <summary>
        /// Handle background task progress.
        /// </summary>
        /// <param name="task">The task that is reporting progress.</param>
        /// <param name="e">Arguments of the progress report.</param>
        private void OnProgress(IBackgroundTaskRegistration task, BackgroundTaskProgressEventArgs args)
        {
            //var ignored = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () =>
            //{
            //    var progress = "Progress: " + args.Progress + "%";
            //    BackgroundTaskSample.ApplicationTriggerTaskProgress = progress;
            //    UpdateUI();
            //});
        }

        /// <summary>
        /// Handle background task completion.
        /// </summary>
        /// <param name="task">The task that is reporting completion.</param>
        /// <param name="e">Arguments of the completion report.</param>
        private void OnCompleted(IBackgroundTaskRegistration task, BackgroundTaskCompletedEventArgs args)
        {
            //UpdateUI();
        }


    }
}
