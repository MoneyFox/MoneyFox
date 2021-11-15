using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Application.Common.Interfaces;
using MoneyFox.Application.Common.Messages;

namespace MoneyFox.ViewModels.Categories
{
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
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
            });
    }
}