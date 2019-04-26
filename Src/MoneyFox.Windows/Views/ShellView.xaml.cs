using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MoneyFox.Foundation.Resources;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    /// <summary>
    ///     An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ShellView : IViewFor<ShellViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(ShellViewModel), typeof(ShellView), null);

        public ShellView()
        {
            InitializeComponent();

            ViewModel = new ShellViewModel();
            this.WhenActivated(disposables => {
                // Bind the view model router to RoutedViewHost.Router property.
                this.OneWayBind(ViewModel, x => x.Router, x => x.RoutedViewHost.Router)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.CanGoBack, v => v.NavView.IsBackEnabled)
                    .DisposeWith(disposables);
            });
        }

        private void NavView_ItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked)
            {
                RoutedViewHost.Router.Navigate.Execute(new SettingsViewModel(ViewModel.HostScreen));
            }

            if (args.InvokedItem.Equals(Strings.AccountsTitle))
            {
                RoutedViewHost.Router.Navigate.Execute(new AccountListViewModel(ViewModel.HostScreen));
            }
            else if (args.InvokedItem.Equals(Strings.CategoriesTitle))
            {
                RoutedViewHost.Router.Navigate.Execute(new CategoryListViewModel(ViewModel.HostScreen));
            }
            else if (args.InvokedItem.Equals(Strings.StatisticsTitle))
            {
                //RoutedViewHost.Router.Navigate.Execute(new AccountListViewModel(HostScreen));
            }
            else if (args.InvokedItem.Equals(Strings.BackupTitle))
            {
                //RoutedViewHost.Router.Navigate.Execute(new AccountListViewModel(HostScreen));
            }
            else if (args.InvokedItem.Equals(Strings.SettingsTitle))
            {
                //RoutedViewHost.Router.Navigate.Execute(new AccountListViewModel(HostScreen));
            }
            else if (args.InvokedItem.Equals(Strings.AboutTitle))
            {
                //RoutedViewHost.Router.Navigate.Execute(new AccountListViewModel(HostScreen));
            }
        }

        public ShellViewModel ViewModel {
            get => (ShellViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (ShellViewModel) value;
        }
    }
}