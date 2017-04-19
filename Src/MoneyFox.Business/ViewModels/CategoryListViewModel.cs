using MoneyFox.DataAccess.Repositories;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryRepository">An instance of <see cref="IRepository{T}" />.</param>
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        public CategoryListViewModel(ICategoryRepository categoryRepository, IModifyDialogService modifyDialogService, IDialogService dialogService)
            : base(categoryRepository, modifyDialogService, dialogService)
        {
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
        
        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void ItemClick(CategoryViewModel category)
        {
            EditCategoryCommand.Execute(category);
        }
    }
}