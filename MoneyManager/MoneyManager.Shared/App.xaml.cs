using System;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Navigation;
using MoneyManager.Business.Logic;
using MoneyManager.Business.Logic.Tile;
using MoneyManager.Tasks.TransactionsWp;
using Xamarin;

namespace MoneyManager {
    public sealed partial class App {
#if WINDOWS_PHONE_APP
        private TransitionCollection transitions;
#endif

        public App() {
            InitializeComponent();
            Suspending += OnSuspending;
        }

        protected override async void OnLaunched(LaunchActivatedEventArgs e) {
#if DEBUG
            if (!Insights.IsInitialized) {
                Insights.Initialize("599ff6bfdc79368ff3d5f5629a57c995fe93352e");
            }
#endif
            var rootFrame = Window.Current.Content as Frame;

            // Do not repeat app initialization when the Window already has content,
            // just ensure that the window is active
            if (rootFrame == null) {
                // Create a Frame to act as the navigation context and navigate to the first page
                rootFrame = new Frame {CacheSize = 1};

                if (e.PreviousExecutionState == ApplicationExecutionState.Terminated) {
                    // TODO: Load state from previously suspended application
                }

                // Place the frame in the current Window
                Window.Current.Content = rootFrame;
            }

            if (rootFrame.Content == null) {
#if WINDOWS_PHONE_APP
                // Removes the turnstile navigation for startup.
                if (rootFrame.ContentTransitions != null) {
                    transitions = new TransitionCollection();
                    foreach (Transition c in rootFrame.ContentTransitions) {
                        transitions.Add(c);
                    }
                }

                rootFrame.ContentTransitions = null;
                rootFrame.Navigated += RootFrame_FirstNavigated;
#endif

                // When the navigation stack isn't restored navigate to the first page,
                // configuring the new page by passing required information as a navigation
                // parameter
                if (!rootFrame.Navigate(typeof (MainPage), e.Arguments)) {
                    throw new Exception("Failed to create initial page");
                }
            }

            // Ensure the current window is active
            Window.Current.Activate();
            TileHelper.DoNavigation(e.TileId);

            RecurringTransactionLogic.CheckRecurringTransactions();
            await TransactionLogic.ClearTransactions();

            BackgroundTaskLogic.RegisterBackgroundTask();

            Tile.UpdateMainTile();
        }

#if WINDOWS_PHONE_APP

        /// <summary>
        ///     Restores the content transitions after the app has launched.
        /// </summary>
        /// <param name="sender">The object where the handler is attached.</param>
        /// <param name="e">Details about the navigation event.</param>
        private void RootFrame_FirstNavigated(object sender, NavigationEventArgs e) {
            var rootFrame = sender as Frame;
            rootFrame.ContentTransitions = transitions ?? new TransitionCollection {new NavigationThemeTransition()};
            rootFrame.Navigated -= RootFrame_FirstNavigated;
        }

#endif

        /// <summary>
        ///     Invoked when application execution is being suspended.  Application state is saved
        ///     without knowing whether the application will be terminated or resumed with the contents
        ///     of memory still intact.
        /// </summary>
        /// <param name="sender">The source of the suspend request.</param>
        /// <param name="e">Details about the suspend request.</param>
        private void OnSuspending(object sender, SuspendingEventArgs e) {
            SuspendingDeferral deferral = e.SuspendingOperation.GetDeferral();

            Tile.UpdateMainTile();

            // TODO: Save application state and stop any background activity
            deferral.Complete();
        }
    }
}