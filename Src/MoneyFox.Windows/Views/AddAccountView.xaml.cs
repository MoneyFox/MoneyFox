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
            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm, v => v.ModifyAccountUserControl.ViewModel)
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
