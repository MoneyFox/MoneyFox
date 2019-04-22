using System.Reactive.Disposables;
using Windows.UI.Xaml;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Splat;

namespace MoneyFox.Windows.Views
{
    public sealed partial class AddAccountView : IViewFor<AddAccountViewModel>
    {
        public static readonly DependencyProperty ViewModelProperty = DependencyProperty
            .Register(nameof(ViewModel), typeof(AddAccountViewModel), typeof(AddAccountView), null);

        public AddAccountView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm, v => v.ModifyAccountControl.ViewModel).DisposeWith(disposables);

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

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (AddAccountViewModel) value;
        }

        public AddAccountViewModel ViewModel {
            get => (AddAccountViewModel)GetValue(ViewModelProperty);
            set => SetValue(ViewModelProperty, value);
        }
    }
}