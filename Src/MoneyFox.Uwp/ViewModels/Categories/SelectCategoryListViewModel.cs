using AutoMapper;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;
using MoneyFox.Uwp.Services;

#nullable enable
namespace MoneyFox.Uwp.ViewModels.Categories
{
    /// <inheritdoc cref="ISelectCategoryListViewModel"/>
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
    {
        private CategoryViewModel? selectedCategory;

        /// <summary>
        /// Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        public SelectCategoryListViewModel(IMediator mediator,
            IMapper mapper,
            IDialogService dialogService,
            NavigationService navigationService)
            : base(mediator, mapper, dialogService, navigationService)
        {
        }

        /// <summary>
        /// CategoryViewModel currently selected in the view.
        /// </summary>
        public CategoryViewModel? SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Post selected CategoryViewModel to message hub
        /// </summary>
        protected override void ItemClick(CategoryViewModel category) =>
            Messenger.Send(new CategorySelectedMessage(category.Id));
    }
}