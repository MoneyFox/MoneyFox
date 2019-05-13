using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using Windows.UI.Xaml;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyModifyPaymentUserControl : ReactiveUserControl<ModifyPaymentViewModel> { }

    public sealed partial class ModifyPaymentUserControl : MyModifyPaymentUserControl
    {
        public ModifyPaymentUserControl()
        {
            this.InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.AccountHeader, v => v.ComboBoxTargetAccount.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.ChargedAccounts, v => v.ComboBoxChargedAccount.Items).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.ChargedAccount, v => v.ComboBoxChargedAccount.SelectedItem).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.TargetAccounts, v => v.ComboBoxTargetAccount.Items).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.IsTransfer, v => v.ComboBoxTargetAccount.Visibility).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.TargetAccount, v => v.ComboBoxTargetAccount.SelectedItem).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedPayment.Amount, v => v.AmountTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.Category.Name, v => v.CategoryTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.Date, v => v.PaymentDatePicker.Date).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.Note, v => v.NoteTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.IsRecurring, v => v.RecurringSwitch.IsOn).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.RecurrenceList, v => v.RecurrenceComboBox.Items).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.Recurrence, v => v.RecurrenceComboBox.SelectedItem).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.IsEndless, v => v.EndlessCheckBox.IsChecked).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.EndDate, v => v.EndDatePicker.Date).DisposeWith(disposables);

                CancelImage.Events().Tapped.InvokeCommand(this, x => x.ViewModel.CancelCommand);
            });
        }

        private async void ToggleRecurringVisibility(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)DataContext;
            if (viewModel.SelectedPayment == null) return;
            if (viewModel.SelectedPayment.IsRecurring)
            {
                await RecurringStackPanel.Fade(1).StartAsync();
            } else
            {
                await RecurringStackPanel.Fade().StartAsync();
            }
        }

        private void SetVisibilityInitially(object sender, RoutedEventArgs e)
        {
            var viewModel = (ModifyPaymentViewModel)DataContext;

            if (viewModel?.SelectedPayment == null)
            {
                return;
            }

            if (!viewModel.SelectedPayment.IsRecurring)
            {
                ToggleRecurringVisibility(this, null);
            }
        }
    }
}
