namespace MoneyFox.ViewModels.Categories
{
    using Core._Pending_.Common.Interfaces;
    using Core.Commands.Categories.CreateCategory;
    using MediatR;
    using System.Threading.Tasks;

    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;

        public AddCategoryViewModel(
            IMediator mediator,
            IDialogService dialogService) : base(mediator, dialogService)
        {
            this.mediator = mediator;
        }

        protected override async Task SaveCategoryAsync()
            => await mediator.Send(
                new CreateCategoryCommand(
                    SelectedCategory.Name,
                    SelectedCategory.Note,
                    SelectedCategory.RequireNote));
    }
}