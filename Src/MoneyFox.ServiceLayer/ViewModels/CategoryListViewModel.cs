using System.Reactive;
using System.Reactive.Linq;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.ServiceLayer.Interfaces;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        public CategoryListViewModel(IScreen hostScreen,
                                     ICrudServicesAsync curdServicesAsync,
                                     IDialogService dialogService) : base(curdServicesAsync, dialogService)
        {
            HostScreen = hostScreen;
        }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task<Unit> ItemClick(CategoryViewModel category)
        {
            await EditCategoryCommand.Execute(category);
            return Unit.Default;
        }

        public override string UrlPathSegment => "CategoryList";
        public override IScreen HostScreen { get; }
    }
}