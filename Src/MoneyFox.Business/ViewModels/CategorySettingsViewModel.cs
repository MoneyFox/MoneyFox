namespace MoneyFox.Business.ViewModels
{
    public class CategorySettingsViewModel : BaseViewModel
    {
        public CategorySettingsViewModel(ICategoryListViewModel categoryListViewModel, ICategoryGroupListViewModel categoryGroupListViewModel)
        {
            CategoryListViewModel = categoryListViewModel;
            CategoryGroupListViewModel = categoryGroupListViewModel;
        }

        public ICategoryListViewModel CategoryListViewModel { get; set; }
        public ICategoryGroupListViewModel CategoryGroupListViewModel { get; set; }

        public override async void ViewAppearing()
        {
            await CategoryListViewModel.LoadCategoriesCommand.ExecuteAsync();
        }
    }
}
