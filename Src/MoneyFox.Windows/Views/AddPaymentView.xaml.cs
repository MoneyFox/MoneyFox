using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MyAddPaymentView : ReactiveView<AddPaymentViewModel>
    {
    }

    public sealed partial class AddPaymentView
    {
        public AddPaymentView()
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                this.Bind(ViewModel, vm => vm.SaveCommand, v => v.DoneButton.Command).DisposeWith(disposable);
                this.Bind(ViewModel, vm => vm.Resources["DoneLabel"], v => v.DoneButton.Label).DisposeWith(disposable);

                this.Bind(ViewModel, vm => vm.CancelCommand, v => v.CancelButton.Command).DisposeWith(disposable);
                this.Bind(ViewModel, vm => vm.Resources["CancelLabel"], v => v.CancelButton.Label).DisposeWith(disposable);
            });
        }
    }
}