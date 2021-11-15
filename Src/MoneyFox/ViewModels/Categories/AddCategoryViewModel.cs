using MediatR;
using MoneyFox.Application.Categories.Command.CreateCategory;
using MoneyFox.Application.Common.Interfaces;
using System.Threading.Tasks;

namespace MoneyFox.ViewModels.Categories
{
    public class AddCategoryViewModel : ModifyCategoryViewModel
    {
        private readonly IMediator mediator;

        public AddCategoryViewModel(IMediator mediator,
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