using System.Collections.ObjectModel;
using System.Linq;
using MoneyFox.Foundation.Groups;
using MvvmCross.Commands;
using MvvmCross.Navigation;

namespace MoneyFox.Business.ViewModels
{
    public interface ICategoryGroupListViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Navigate to the modify group view.
        /// </summary>
        MvxAsyncCommand CreateNewGroupCommand { get; }

        /// <summary>
        ///     Collection with categories alphanumeric grouped by
        /// </summary>
        ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryGroupList { get; set; }

        bool IsGroupListEmpty { get; }
    }

    /// <inheritdoc cref="ICategoryGroupListViewModel"/> /> 
    public class CategoryGroupListViewModel : BaseViewModel, ICategoryGroupListViewModel
    {
        private readonly IMvxNavigationService navigationService;

        private ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> groupList;

        public CategoryGroupListViewModel(IMvxNavigationService navigationService)
        {
            this.navigationService = navigationService;
        }

        public MvxAsyncCommand CreateNewGroupCommand => new MvxAsyncCommand(async () => await navigationService.Navigate<ModifyCategoryGroupViewModel>());

        /// <inheritdoc />
        public ObservableCollection<AlphaGroupListGroup<CategoryViewModel>> CategoryGroupList
        {
            get => groupList;
            set
            {
                if (groupList == value) return;
                groupList = value;
                RaisePropertyChanged();
                RaisePropertyChanged(nameof(IsGroupListEmpty));
            }
        }

        /// <inheritdoc />
        public bool IsGroupListEmpty => !CategoryGroupList.Any();
    }
}
