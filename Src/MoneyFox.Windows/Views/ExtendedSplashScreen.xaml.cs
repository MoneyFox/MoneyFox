using System;
using System.Linq;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;
using Windows.ApplicationModel.Background;
using Windows.Foundation.Metadata;
using Windows.Globalization;
using Windows.System.UserProfile;
using Windows.UI.Core;
using Windows.UI.StartScreen;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using MoneyFox.Business.ViewModels;
using MoneyFox.Foundation.Constants;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MvvmCross.Core.ViewModels;
using MvvmCross.Platform;
using UniversalRateReminder;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     Displays an extra splash screen with a busy indicator to have more time to load the app
    ///     and inform the user that it's still responsive.
    /// </summary>
    public sealed partial class ExtendedSplashScreen
    {
        private const string TASK_NAMESPACE = "MoneyFox.Windows.Tasks";
        private const string CLEAR_PAYMENTS_TASK = "ClearPaymentsTask";
        private const string RECURRING_PAYMENT_TASK = "RecurringPaymentTask";


        internal bool Dismissed = false; // Variable to track splash screen dismissal status.
        internal Frame RootFrame;

        /// <summary>
        ///     Constructor
        /// </summary>
        public ExtendedSplashScreen(SplashScreen splashscreen)
        {
            InitializeComponent();
            var splash = splashscreen;

            if (splash != null)
            {
                // Register an event handler to be executed when the splash screen has been dismissed.
                splash.Dismissed += DismissedEventHandler;
            }

            // Create a Frame to act as the navigation context
            RootFrame = new Frame();
        }

        // Include code to be executed when the system has transitioned from the splash screen to the extended splash screen (application's first view).
        void DismissedEventHandler(SplashScreen sender, object e)
        {
            // Navigate away from the app's extended splash screen after completing setup operations here...
            if (!Dismissed)
            {
                Dismissed = true;
                var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, async () => 
                {
                    Window.Current.Content = new AppShell { Language = ApplicationLanguages.Languages[0] };
                    ApplicationLanguages.PrimaryLanguageOverride = GlobalizationPreferences.Languages[0];

                    var shell = (AppShell) Window.Current.Content;

                    // When the navigation stack isn't restored, navigate to the first page
                    // suppressing the initial entrance animation.
                    var setup = new Setup(shell.MyAppFrame);
                    setup.Initialize();

                    var start = Mvx.Resolve<IMvxAppStart>();
                    start.Start();

                    RegisterTasks();

                    shell.ViewModel = Mvx.Resolve<MenuViewModel>();

                    //If Jump Lists are supported, adds them
                    if (ApiInformation.IsTypePresent("Windows.UI.StartScreen.JumpList"))
                    {
                        await SetJumplist();
                    }

                    await CallRateReminder();
                });
            }
        }

        private async void RegisterTasks()
        {
            var backgroundAccessStatus = await BackgroundExecutionManager.RequestAccessAsync();
            if (backgroundAccessStatus == BackgroundAccessStatus.DeniedByUser)
            {
                await Mvx.Resolve<IDialogService>().ShowMessage(Strings.BackgroundAccessDeniedTitle,
                                                          Strings.BackgroundAccessDeniedByUserMessage);
            }
            else if (backgroundAccessStatus == BackgroundAccessStatus.DeniedByUser)
            {
                await Mvx.Resolve<IDialogService>().ShowMessage(Strings.BackgroundAccessDeniedTitle,
                                                                Strings.BackgroundAccessDeniedByPolicyMessage);
            }
            else
            {
                RegisterClearPaymentTask();
                RegisterRecurringPaymentTask();
                Mvx.Resolve<IBackgroundTaskManager>().StartBackupSyncTask();
            }
        }

        private void RegisterClearPaymentTask()
        {
            // Unregister existing task first.
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == CLEAR_PAYMENTS_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == CLEAR_PAYMENTS_TASK).Value.Unregister(true);
            }

            var builder = new BackgroundTaskBuilder
            {
                Name = CLEAR_PAYMENTS_TASK,
                TaskEntryPoint = String.Format("{0}.{1}", TASK_NAMESPACE, CLEAR_PAYMENTS_TASK)
            };

            // Task will be executed all 30 minutes
            builder.SetTrigger(new TimeTrigger(30, false));
            builder.Register();
        }

        private void RegisterRecurringPaymentTask()
        {
            // Unregister existing task first.
            if (BackgroundTaskRegistration.AllTasks.Any(task => task.Value.Name == RECURRING_PAYMENT_TASK))
            {
                BackgroundTaskRegistration.AllTasks.First(task => task.Value.Name == RECURRING_PAYMENT_TASK).Value.Unregister(true);
            }
            
            var builder = new BackgroundTaskBuilder
            {
                Name = RECURRING_PAYMENT_TASK,
                TaskEntryPoint = String.Format("{0}.{1}", TASK_NAMESPACE, RECURRING_PAYMENT_TASK)
            };

            // Task will be executed all 30 minutes
            builder.SetTrigger(new TimeTrigger(30, false));
            builder.Register();
        }

        private async Task SetJumplist()
        {
            var jumpList = await JumpList.LoadCurrentAsync();
            jumpList.Items.Clear();
            jumpList.SystemGroupKind = JumpListSystemGroupKind.None;

            var listItemAddIncome = JumpListItem.CreateWithArguments(Constants.ADD_INCOME_TILE_ID,
                Strings.AddIncomeLabel);
            listItemAddIncome.Logo = new Uri("ms-appx:///Assets/IncomeTileIcon.png");
            jumpList.Items.Add(listItemAddIncome);

            var listItemAddSpending = JumpListItem.CreateWithArguments(Constants.ADD_EXPENSE_TILE_ID,
                Strings.AddExpenseLabel);
            listItemAddSpending.Logo = new Uri("ms-appx:///Assets/SpendingTileIcon.png");
            jumpList.Items.Add(listItemAddSpending);

            var listItemAddTransfer = JumpListItem.CreateWithArguments(Constants.ADD_TRANSFER_TILE_ID,
                Strings.AddTransferLabel);
            listItemAddTransfer.Logo = new Uri("ms-appx:///Assets/TransferTileIcon.png");
            jumpList.Items.Add(listItemAddTransfer);

            await jumpList.SaveAsync();
        }

        private async Task CallRateReminder()
        {
            RatePopup.RateButtonText = Strings.YesLabel;
            RatePopup.CancelButtonText = Strings.NotNowLabel;
            RatePopup.Title = Strings.RateReminderTitle;
            RatePopup.Content = Strings.RateReminderText;

            await RatePopup.CheckRateReminderAsync();
        }
    }
}