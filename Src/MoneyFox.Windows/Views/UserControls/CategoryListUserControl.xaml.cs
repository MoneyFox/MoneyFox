using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
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

                this.OneWayBind(ViewModel, vm => vm.HasNoCategories, v => v.NoCategoriesTextBlock.Visibility)
                    .DisposeWith(disposables);

                CategoryList
                    .Events()
                    .ItemClick.Select(x => x.ClickedItem as CategoryViewModel)
                    .InvokeCommand(ViewModel.ItemClickCommand)
                    .DisposeWith(disposables);

                this.WhenAnyValue(x => x.SearchTextBox)
                    .Throttle(TimeSpan.FromMilliseconds(500), RxApp.TaskpoolScheduler)
                    .Select(searchTerm => searchTerm.Text.Trim())
                    .DistinctUntilChanged()
                    .Where(searchTerm => !string.IsNullOrWhiteSpace(searchTerm))
                    .ObserveOn(RxApp.MainThreadScheduler)
                    .InvokeCommand(ViewModel.SearchCommand);
            });
        }

        private void CategoryListRightTapped(object sender, RightTappedRoutedEventArgs e)
        {
            var senderElement = sender as FrameworkElement;
            var flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement) as MenuFlyout;

            flyoutBase?.ShowAt(senderElement, e.GetPosition(senderElement));
        }


        private void EditCategory(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement)sender;
            var category = element.DataContext as CategoryViewModel;
            if (category == null) {
                return;
            }

            ((AbstractCategoryListViewModel)DataContext).EditCategoryCommand.Execute(category);
        }

        private void DeleteCategory(object sender, RoutedEventArgs e) {
            var element = (FrameworkElement)sender;
            var category = element.DataContext as CategoryViewModel;
            if (category == null) {
                return;
            }

            ((AbstractCategoryListViewModel)DataContext).DeleteCategoryCommand.Execute(category);
        }
    }
}
