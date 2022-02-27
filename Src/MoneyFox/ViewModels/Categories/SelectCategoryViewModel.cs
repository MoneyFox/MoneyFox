namespace MoneyFox.ViewModels.Categories
{
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Interfaces;
    using Core._Pending_.Common.Messages;
    using global::AutoMapper;
    using MediatR;
    using Xamarin.Forms;

    public class SelectCategoryViewModel : CategoryListViewModel
    {
        public SelectCategoryViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService)
            : base(mediator, mapper, dialogService)
        {
        }

        public RelayCommand<CategoryViewModel> SelectCategoryCommand => new RelayCommand<CategoryViewModel>(
            async c =>
            {
                Messenger.Send(new CategorySelectedMessage(c.Id));
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
    }
}