using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml.Controls;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views.UserControls
{
    public class MyCategoryListUserControl : ReactiveUserControl<CategoryListViewModel> {}

    public sealed partial class CategoryListUserControl
    {
        public CategoryListUserControl() 
        {
            InitializeComponent();

            this.WhenActivated(disposables =>
            {
                this.OneWayBind(ViewModel, vm => vm.CategoryList, v => v.CategoryCollectionViewSource.Source)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.Resources["NoCategoriesMessage"], v => v.NoCategoriesTextBlock.Text)
                    .DisposeWith(disposables);

                this.OneWayBind(ViewModel, vm => vm.IsCategoriesEmpty, v => v.NoCategoriesTextBlock.Visibility)
                    .DisposeWith(disposables);

                CategoryList
                    .Events()
                    .ItemClick.Select(x => x.ClickedItem as CategoryViewModel)
                    .InvokeCommand(ViewModel.ItemClickCommand)
                    .DisposeWith(disposables);
            });
        }
    }
}
