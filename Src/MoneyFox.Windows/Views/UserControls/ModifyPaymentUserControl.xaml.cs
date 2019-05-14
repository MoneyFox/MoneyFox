using System;
using System.Reactive;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;

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
                this.OneWayBind(ViewModel, vm => vm.ChargedAccounts, v => v.ComboBoxChargedAccount.ItemsSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SelectedPayment.ChargedAccount, v => v.ComboBoxChargedAccount.SelectedItem).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.TargetAccounts, v => v.ComboBoxTargetAccount.ItemsSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.IsTransfer, v => v.ComboBoxTargetAccount.Visibility).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SelectedPayment.TargetAccount, v => v.ComboBoxTargetAccount.SelectedItem).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.RecurrenceList, v => v.RecurrenceComboBox.ItemsSource).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.SelectedPayment.IsRecurring, v => v.RecurringStackPanel.Visibility).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.AccountHeader, v => v.ComboBoxTargetAccount.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["TargetAccountLabel"], v => v.ComboBoxTargetAccount.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AmountLabel"], v => v.AmountTextBox.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["CategoryLabel"], v => v.CategoryTextBox.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["DateLabel"], v => v.PaymentDatePicker.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["NoteLabel"], v => v.NoteTextBox.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["RecurringLabel"], v => v.RecurringSwitch.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["EndlessLabel"], v => v.EndlessCheckBox.Content).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["RecurrenceLabel"], v => v.RecurrenceComboBox.Header).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["EnddateLabel"], v => v.EndDatePicker.Header).DisposeWith(disposables);
                
                this.Bind(ViewModel, vm => vm.SelectedPayment.Amount, v => v.AmountTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.Category.Name, v => v.CategoryTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.Date, v => v.PaymentDatePicker.Date,
                          date => date, offset => offset.DateTime)
                    .DisposeWith(disposables);
                    
                this.Bind(ViewModel, vm => vm.SelectedPayment.Note, v => v.NoteTextBox.Text).DisposeWith(disposables);
                this.Bind(ViewModel, vm => vm.SelectedPayment.IsRecurring, v => v.RecurringSwitch.IsOn).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.Recurrence, v => v.RecurrenceComboBox.SelectedItem).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.IsEndless, v => v.EndlessCheckBox.IsChecked).DisposeWith(disposables);

                this.Bind(ViewModel, vm => vm.SelectedPayment.RecurringPayment.EndDate, v => v.EndDatePicker.Date,
                          date => date ?? DateTimeOffset.Now,
                          offset => offset.DateTime)
                    .DisposeWith(disposables);

                CategoryTextBox
                    .Events()
                    .GotFocus.Select(x => Unit.Default)
                    .InvokeCommand(this, x => x.ViewModel.GoToSelectCategoryDialogCommand);

                IconResetCategory
                    .Events()
                    .Tapped.Select(x => Unit.Default)
                    .InvokeCommand(this, x => x.ViewModel.ResetCategoryCommand);
            });
        }
    }
}
