namespace MoneyFox.Business.ViewModels
{
    public interface ICategorySettingsViewModel : IBaseViewModel
    {
        ICategoryListViewModel CategoryListViewModel { get;  }
        ICategoryGroupListViewModel CategoryGroupListViewModel { get;  }
    }

    public class CategorySettingsViewModel : BaseViewModel, ICategorySettingsViewModel
    {
        public CategorySettingsViewModel(ICategoryListViewModel categoryListViewModel, ICategoryGroupListViewModel categoryGroupListViewModel)
        {
            CategoryListViewModel = categoryListViewModel;
            CategoryGroupListViewModel = categoryGroupListViewModel;
        }

        public ICategoryListViewModel CategoryListViewModel { get;  }
        public ICategoryGroupListViewModel CategoryGroupListViewModel { get;  }

        public override async void ViewAppearing()
        {
            await CategoryListViewModel.LoadCategoriesCommand.ExecuteAsync();
        }
    }
}
