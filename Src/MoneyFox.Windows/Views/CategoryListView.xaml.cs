using System.Reactive.Disposables;
using MoneyFox.ServiceLayer.ViewModels;
using ReactiveUI;

namespace MoneyFox.Windows.Views
{
    public class MyCategoryListView : ReactiveView<CategoryListViewModel> { }

    public sealed partial class CategoryListView
    {
        public CategoryListView() 
        {
            InitializeComponent();

            this.WhenActivated(disposable =>
            {
                CategoryListControl.ViewModel = ViewModel;

                this.OneWayBind(ViewModel, vm => vm.CreateNewCategoryCommand, v => v.AddCategoryButton.Command)
                    .DisposeWith(disposable);
                this.OneWayBind(ViewModel, vm => vm.Resources["CategoriesTitle"], v => v.CategoriesTitle.Text).DisposeWith(disposable);
                this.OneWayBind(ViewModel, vm => vm.Resources["AddCategoryLabel"], v => v.AddCategoryButton.Label)
                    .DisposeWith(disposable);
            });
        }
    }
}
