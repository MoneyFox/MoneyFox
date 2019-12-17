using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Categories.Command.DeleteCategoryById;
using MoneyFox.Application.Categories.Command.UpdateCategory;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Categories.Queries.GetIfCategoryWithNameExists;
using MoneyFox.Application.Common.CloudBackup;
using MoneyFox.Application.Common.Facades;
using MoneyFox.Application.Resources;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Utilities;
using NLog;
using IDialogService = MoneyFox.Presentation.Interfaces.IDialogService;

namespace MoneyFox.Presentation.ViewModels
{
    public class EditCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly Logger logManager = LogManager.GetCurrentClassLogger();

        private readonly IMediator mediator;
        private readonly IDialogService dialogService;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;
        private readonly IMapper mapper;

        public EditCategoryViewModel(IMediator mediator,
                                     IDialogService dialogService,
                                     ISettingsFacade settingsFacade,
                                     IBackupService backupService,
                                     INavigationService navigationService,
                                     IMapper mapper)
            : base(mediator, settingsFacade, backupService, navigationService, mapper)
        {
            this.mediator = mediator;
            this.dialogService = dialogService;
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.mapper = mapper;
        }

        /// <summary>
        ///     Delete the selected CategoryViewModel from the database
        /// </summary>
        public AsyncCommand DeleteCommand => new AsyncCommand(DeleteCategory);

        protected override async Task Initialize()
        {
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery(CategoryId)));
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategory()
        {

            if (string.IsNullOrEmpty(SelectedCategory.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (await mediator.Send(new GetIfCategoryWithNameExistsQuery { CategoryName = SelectedCategory.Name }))
            {
                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateCategoryMessage);
                return;
            }

            await mediator.Send(new UpdateCategoryCommand(mapper.Map<Category>(SelectedCategory)));
        }

        private async Task DeleteCategory()
        {
            if (await dialogService.ShowConfirmMessageAsync(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
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
