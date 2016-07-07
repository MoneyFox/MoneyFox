using MoneyFox.Shared.Interfaces;
using MoneyFox.Shared.Model;
using MoneyFox.Shared.Repositories;
using MvvmCross.Core.ViewModels;
using PropertyChanged;

namespace MoneyFox.Shared.ViewModels
{
    [ImplementPropertyChanged]
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IUnitOfWork" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(IUnitOfWork unitOfWork, IDialogService dialogService)
            : base(unitOfWork, dialogService)
        {
        }
    }
}