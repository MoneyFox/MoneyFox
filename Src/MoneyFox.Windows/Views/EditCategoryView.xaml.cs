using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using MoneyFox.Presentation.Views;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;
using Xamarin.Forms.Platform.UWP;

namespace MoneyFox.Windows.Views
{
    public class MyEditCategoryView : ReactiveView<EditCategoryViewModel> { }

    public sealed partial class EditCategoryView
    {
        public EditCategoryView() {
            this.InitializeComponent();

            this.WhenActivated(disposables => {
                this.OneWayBind(ViewModel, vm => vm.Title, v => v.PageTitle.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.SaveCommand, v => v.DoneButton.Command).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.CancelCommand, v => v.CancelButton.Command).DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["DoneLabel"], v => v.DoneButton.Label).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["CancelLabel"], v => v.CancelButton.Label).DisposeWith(disposables);

                this.Events().SizeChanged.Do((args) => {
                    ContentGrid.Children.Clear();
                    ContentGrid.Children.Add(new AddCategoryPage { BindingContext = ViewModel }.CreateFrameworkElement());
                });
            });
        }
    }
}
