using System.Reactive;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel
    {
        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        public SelectCategoryListViewModel(IScreen hostScreen,
                                           ICrudServicesAsync crudServicesAsync = null,
                                           IDialogService dialogService = null) : base(crudServicesAsync, dialogService)
        {
            HostScreen = hostScreen;
        }

        public override string UrlPathSegment => "SelectCategory";
        public override IScreen HostScreen { get; }

        /// <summary>
        ///     CategoryViewModel currently selected in the view.
        /// </summary>
        public CategoryViewModel SelectedCategory
        {
            get => selectedCategory;
            set => this.RaiseAndSetIfChanged(ref selectedCategory, value);
        }

        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override Task<Unit> ItemClick(CategoryViewModel category)
        {
            MessageBus.Current.SendMessage(new CategorySelectedMessage(this, category));
            HostScreen.Router.NavigateBack.Execute();
            return Task.FromResult(Unit.Default);
        }
    }
}