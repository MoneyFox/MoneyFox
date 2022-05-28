namespace MoneyFox.ViewModels.Categories
{

    using AutoMapper;
    using CommunityToolkit.Mvvm.Input;
    using CommunityToolkit.Mvvm.Messaging;
    using Core._Pending_.Common.Messages;
    using Core.Common.Interfaces;
    using MediatR;
    using Xamarin.Forms;

    public class SelectCategoryViewModel : CategoryListViewModel
    {
        public SelectCategoryViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(
            mediator: mediator,
            mapper: mapper,
            dialogService: dialogService) { }

        public AsyncRelayCommand<CategoryViewModel> SelectCategoryCommand
            => new AsyncRelayCommand<CategoryViewModel>(
                async c =>
                {
                    var dataSet = new CategorySelectedDataSet(c.Id, c.Name);
                    Messenger.Send(new CategorySelectedMessage(dataSet));
                    await Application.Current.MainPage.Navigation.PopModalAsync();
                });
    }

}
