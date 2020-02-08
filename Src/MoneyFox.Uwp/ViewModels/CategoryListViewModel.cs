using AutoMapper;
using MediatR;
using MoneyFox.Application.Common.Interfaces;

namespace MoneyFox.Uwp.ViewModels
{
    public class CategoryListViewModel : AbstractCategoryListViewModel, ICategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        public CategoryListViewModel(IMediator mediator,
                                     IMapper mapper,
                                     IDialogService dialogService,
                                     INavigationService navigationService)
            : base(mediator, mapper, dialogService, navigationService)
        { }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void ItemClick(CategoryViewModel category)
        {
            EditCategoryCommand.Execute(category);
        }
    }
}
