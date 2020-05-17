using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Uwp.Services;
using NLog;
using System.Globalization;
using System.Threading.Tasks;

namespace MoneyFox.Uwp.ViewModels
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
                                     IDialogService dialogService,
                                     NavigationService navigationService,
                                     IMapper mapper) : base(mediator, navigationService, mapper, dialogService)
        {
            this.mediator = mediator;
            this.mapper = mapper;
        }

        /// <summary>
        /// Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteCategoryAsync);

        protected override async Task InitializeAsync()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(CategoryId)));
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }

        private async Task DeleteCategoryAsync()
        {
            if(await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
                logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);
                await CancelCommand.ExecuteAsync();
            }
        }
    }
}
