namespace MoneyFox.ViewModels.Categories
{

    using System.Threading.Tasks;
    using Core.ApplicationCore.UseCases.CategoryCreation;
    using Core.Common.Interfaces;
    using JetBrains.Annotations;
    using MediatR;

    [UsedImplicitly]
    internal sealed class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;

        public AddCategoryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator: mediator, dialogService: dialogService)
        {
            this.mediator = mediator;
        }

        protected override async Task SaveCategoryAsync()
        {
            var command = new CreateCategory.Command(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote);
            await mediator.Send(command);
        }
    }

}
