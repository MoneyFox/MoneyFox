using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core.Commands.Categories.CreateCategory;
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