using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MyAddAccountView : ReactiveView<AddAccountViewModel> { }

    public sealed partial class AddAccountView : MyAddAccountView
    {
        public AddAccountView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                ModifyAccountControl.ViewModel = ViewModel;

                this.OneWayBind(ViewModel, vm => vm.Title, v => v.TitlePage.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SaveCommand, v => v.SaveButton.Command).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CancelCommand, v => v.CancelButton.Command)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["SaveLabel"], v => v.SaveButton.Label)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["CancelLabel"], v => v.CancelButton.Label)
                    .DisposeWith(disposables);
            });
        }
    }
}