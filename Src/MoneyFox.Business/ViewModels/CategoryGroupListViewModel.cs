using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public interface ICategoryGroupListViewModel
    {
        /// <summary>
        ///     Navigate to the modify group view.
        /// </summary>
        MvxAsyncCommand CreateNewGroupCommand { get; }
    }

    public class CategoryGroupListViewModel : ICategoryGroupListViewModel
    {
        private readonly IMvxNavigationService navigationService;

        public CategoryGroupListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public MvxAsyncCommand CreateNewGroupCommand => new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyCategoryGroupViewModel>());
    }
}
