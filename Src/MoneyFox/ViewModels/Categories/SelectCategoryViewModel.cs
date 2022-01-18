using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core._Pending_.Common.Interfaces;
using MoneyFox.Core._Pending_.Common.Messages;
using Xamarin.Forms;

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
                await Application.Current.MainPage.Navigation.PopModalAsync();
            });
    }
}