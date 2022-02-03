using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Aggregates.Payments;
using MoneyFox.Core.Commands.Categories.DeleteCategoryById;
using MoneyFox.Core.Commands.Categories.UpdateCategory;
using MoneyFox.Core.Queries.Categories.GetCategoryById;
using MoneyFox.Core.Resources;
using MoneyFox.Win.Services;
using NLog;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Win.ViewModels.Categories
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
            IDialogService dialogService,
            INavigationService navigationService,
            IMapper mapper) : base(mediator, navigationService, mapper, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncRelayCommand DeleteCommand => new AsyncRelayCommand(DeleteCategoryAsync);

        protected override async Task InitializeAsync()
        {
            Category category = await mediator.Send(new GetCategoryByIdQuery(CategoryId));
            SelectedCategory = mapper.Map<CategoryViewModel>(category);
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategoryAsync() =>
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));

        private async Task DeleteCategoryAsync()
        {
            if(SelectedCategory == null)
            {
                return;
            }

            if(await DialogService.ShowConfirmMessageAsync(
                   Strings.DeleteTitle,
                   Strings.DeleteCategoryConfirmationMessage))
            {
                await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
                logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);
                await CancelCommand.ExecuteAsync(null);
            }
        }
    }
}