using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AddAccountView : IViewFor<AddAccountViewModel>
    {
        public AddAccountView() {
            this.InitializeComponent();

            ViewModel = Locator.Current.GetService<AddAccountViewModel>();
            ModifyAccountControl.ViewModel = ViewModel;
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.TitlePage.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SaveCommand, v => v.SaveButton.Command).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CancelCommand, v => v.CancelButton.Command).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["SaveLabel"], v => v.SaveButton.Label)
                    .DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["CancelLabel"], v => v.CancelButton.Label)
                    .DisposeWith(disposables);
            });
        }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AddAccountViewModel)value;
        }

        public AddAccountViewModel ViewModel { get; set; }
    }
}
