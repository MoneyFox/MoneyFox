using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MyEditAccountView : ReactiveView<EditAccountViewModel> { }

    public sealed partial class EditAccountView
    {
        public EditAccountView() {
            InitializeComponent();

            this.WhenActivated(disposables => {
                ModifyAccountControl.ViewModel = ViewModel;

                this.OneWayBind(ViewModel, vm => vm.Title, v => v.TitlePage.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SaveCommand, v => v.DoneButton.Command)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CancelCommand, v => v.CancelButton.Command)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.DeleteCommand, v => v.DeleteButton.Command)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["DoneLabel"], v => v.DoneButton.Label)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["CancelLabel"], v => v.CancelButton.Label)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["DeleteLabel"], v => v.DeleteButton.Label)
                    .DisposeWith(disposables);
            });
        }
    }
}
