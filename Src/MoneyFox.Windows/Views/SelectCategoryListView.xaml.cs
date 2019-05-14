using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.System;
using Windows.UI.Xaml;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MySelectCategoryListView: ReactiveView<SelectCategoryListViewModel> { }

    public sealed partial class SelectCategoryListView
    {
        public SelectCategoryListView()
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                CategoryListControl.ViewModel = ViewModel;

                this.OneWayBind(ViewModel, vm => vm.Resources["SelectCategoryTitle"], v => v.TitlePage.Text).DisposeWith(disposables);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddLabel"], v => v.AddCategoryButton.Label).DisposeWith(disposables); 

                this.Events()
                    .KeyDown
                    .Where(e => e.Key == VirtualKey.Enter)
                    .Do(x => ViewModel.ItemClickCommand.Execute(ViewModel.SelectedCategory));
            });
        }
    }
}
