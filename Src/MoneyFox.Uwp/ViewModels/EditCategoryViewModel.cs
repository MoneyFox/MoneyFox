using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Ui.Shared.Commands;
using MoneyFox.Ui.Shared.Utilities;
using MoneyFox.Uwp.Services;
using NLog;

namespace MoneyFox.Uwp.ViewModels
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
                                     IDialogService dialogService,
                                     ISettingsFacade settingsFacade,
                                     IBackupService backupService,
                                     INavigationService navigationService,
                                     IMapper mapper)
            : base(mediator, navigationService, mapper, dialogService)
        {
            this.mediator = mediator;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteCategory);

        protected override async Task InitializeAsync()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(CategoryId)));
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));

            NavigationService.GoBackModal();
        }

        private async Task DeleteCategory()
        {
            if (await DialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                await mediator.Send(new DeleteCategoryByIdCommand(SelectedCategory.Id));
                logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);

                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                backupService.UploadBackupAsync().FireAndForgetSafeAsync();
                await CancelCommand.ExecuteAsync();
            }
        }
    }
}
