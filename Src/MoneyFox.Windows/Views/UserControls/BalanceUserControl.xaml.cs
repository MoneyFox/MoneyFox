using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyBalanceUserControl : ReactiveUserControl<BalanceViewModel>
    {
    }

    public sealed partial class BalanceUserControl
    {
        public BalanceUserControl()
        {
            InitializeComponent();
            ViewModel = Locator.Current.GetService<BalanceViewModel>();

            this.WhenActivated(async disposables =>
            {
                await ViewModel.UpdateBalance();

                this.OneWayBind(ViewModel, vm => vm.TotalBalance, v => v.TotalBalance.Text)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.EndOfMonthBalance, v => v.EndOfMonthBalance.Text)
                    .DisposeWith(disposables);
            });
        }
    }
}