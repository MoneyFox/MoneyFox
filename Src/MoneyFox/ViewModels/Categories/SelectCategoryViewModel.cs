namespace MoneyFox.ViewModels.Categories
{

    using AutoMapper;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.Common.Interfaces;
    using MediatR;

    public class SelectCategoryViewModel : CategoryListViewModel
    {
        public SelectCategoryViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(
            mediator: mediator,
            mapper: mapper,
            dialogService: dialogService) { }

        public RelayCommand<CategoryViewModel> SelectCategoryCommand
            => new RelayCommand<CategoryViewModel>(
                async c =>
                {
                    Messenger.Send(new CategorySelectedMessage(c.Id));
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                });
    }

}
