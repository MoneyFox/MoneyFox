using System;
using System.Threading.Tasks;
using AutoMapper;
using GalaSoft.MvvmLight.Views;
using GenericServices;
using MediatR;
using MoneyFox.Application.Categories.Queries.GetCategoryById;
using MoneyFox.Domain.Entities;
using MoneyFox.Presentation.Commands;
using MoneyFox.Presentation.Facades;
using MoneyFox.Presentation.Services;
using MoneyFox.Presentation.Utilities;

namespace MoneyFox.Presentation.ViewModels
{
    public interface IModifyCategoryViewModel : IBaseViewModel
    {
        /// <summary>
        ///     Saves changes to a CategoryViewModel
        /// </summary>
        AsyncCommand SaveCommand { get; }

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        AsyncCommand CancelCommand { get; }

        /// <summary>
        ///     Selected category.
        /// </summary>
        CategoryViewModel SelectedCategory { get; }
    }

    /// <summary>
    ///     View Model for creating and editing Categories without dialog
    /// </summary>
    public abstract class ModifyCategoryViewModel : BaseViewModel, IModifyCategoryViewModel
    {
        private readonly IMediator mediator;
        private readonly ISettingsFacade settingsFacade;
        private readonly IBackupService backupService;

        private CategoryViewModel selectedCategory;
        private string title;

        /// <summary>
        ///     Constructor
        /// </summary>
        protected ModifyCategoryViewModel(IMediator mediator,
                                          ISettingsFacade settingsFacade,
                                          IBackupService backupService,
                                          INavigationService navigationService)
        {
            this.settingsFacade = settingsFacade;
            this.backupService = backupService;
            this.mediator = mediator;

            NavigationService = navigationService;
        }

        protected abstract Task Initialize();

        protected abstract Task SaveCategory();

        protected INavigationService NavigationService { get; }

        public AsyncCommand InitializeCommand => new AsyncCommand(Initialize);

        public AsyncCommand SaveCommand => new AsyncCommand(SaveCategoryBase);

        /// <summary>
        ///     Cancel the current operation
        /// </summary>
        public AsyncCommand CancelCommand => new AsyncCommand(Cancel);

        /// <summary>
        ///     The currently selected CategoryViewModel
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set
            {
                selectedCategory = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the Title based on whether a CategoryViewModel is being created or edited
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                if (title == value) return;
                title = value;
                RaisePropertyChanged();
            }
        }

        public int CategoryId { get; set; }

        private async Task SaveCategoryBase()
        {
            await SaveCategory();

            settingsFacade.LastExecutionTimeStampSyncBackup = DateTime.Now;
            if (settingsFacade.IsBackupAutouploadEnabled) backupService.EnqueueBackupTask().FireAndForgetSafeAsync();
        }

        private async Task Cancel()
        {
            SelectedCategory = Mapper.Map<CategoryViewModel>(await mediator.Send(new GetCategoryByIdQuery {CategoryId = SelectedCategory.Id}));
            NavigationService.GoBack();
        }
    }
}