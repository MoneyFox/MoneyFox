namespace MoneyFox.ViewModels.Categories
{

    using System.Threading.Tasks;
    using Core.Commands.Categories.CreateCategory;
    using Core.Common.Interfaces;
    using MediatR;

    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;

        public AddCategoryViewModel(IMediator mediator, IDialogService dialogService) : base(mediator: mediator, dialogService: dialogService)
        {
            this.mediator = mediator;
        }

        protected override async Task SaveCategoryAsync()
        {
            await mediator.Send(new CreateCategoryCommand(name: SelectedCategory.Name, note: SelectedCategory.Note, requireNote: SelectedCategory.RequireNote));
        }
    }

}
