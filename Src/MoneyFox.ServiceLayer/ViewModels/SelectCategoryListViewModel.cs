using System.Reactive;
using System.Threading.Tasks;
using GenericServices;
using MoneyFox.ServiceLayer.Interfaces;
using MoneyFox.ServiceLayer.Messages;
using MvvmCross.Plugin.Messenger;
using ReactiveUI;

namespace MoneyFox.ServiceLayer.ViewModels
{
    /// <summary>
    ///     Represents the SelectCategoryListView
    /// </summary>
    public interface ISelectCategoryListViewModel
    {
        CategoryViewModel SelectedCategory { get; }
    }

    /// <inheritdoc cref="ISelectCategoryListViewModel"/>
    public class SelectCategoryListViewModel : AbstractCategoryListViewModel, ISelectCategoryListViewModel
    {
        private readonly IMvxMessenger messenger;
        private CategoryViewModel selectedCategory;

        /// <summary>
        ///     Creates an CategoryListViewModel for the usage of providing a CategoryViewModel selection.
        /// </summary>
        public SelectCategoryListViewModel(IScreen hostScreen,
            ICrudServicesAsync crudServicesAsync,
            IDialogService dialogService,
            IMvxMessenger messenger) : base(crudServicesAsync, dialogService)
        {
            this.messenger = messenger;

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
        protected override async Task<Unit> ItemClick(CategoryViewModel category)
        {
            messenger.Publish(new CategorySelectedMessage(this, category));
            HostScreen.Router.NavigateBack.Execute();
            return Unit.Default;
        }
    }
}