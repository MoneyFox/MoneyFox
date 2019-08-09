using System;
using System.Globalization;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Application.Resources;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
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
            : base(mediator, settingsFacade, backupService, navigationService)
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
            SelectedCategory = mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery {CategoryId = CategoryId}));
            Title = string.Format(CultureInfo.InvariantCulture, Strings.EditCategoryTitle, SelectedCategory.Name);
        }

        protected override async Task SaveCategory()
        {
            // TODO: Reimplement
            //await crudServices.UpdateAndSaveAsync(SelectedCategory);
            //if (!crudServices.IsValid)
            //{
            //    await dialogService.ShowMessage(Strings.GeneralErrorTitle, crudServices.GetAllErrors());
            //}

            await CancelCommand.ExecuteAsync();
        }

        private async Task DeleteCategory()
        {
            if (await dialogService.ShowConfirmMessage(Strings.DeleteTitle, Strings.DeleteCategoryConfirmationMessage))
            {
                // TODO: Reimplement
                //await crudServices.DeleteAndSaveAsync<Category>(SelectedCategory.Id);

                logManager.Info("Category with Id {id} deleted.", SelectedCategory.Id);

                settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
                backupService.EnqueueBackupTask().FireAndForgetSafeAsync();
                await CancelCommand.ExecuteAsync();
            }
        }
    }
}