namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using MediatR;
using MoneyFox.Core.Common.Interfaces;
using MoneyFox.Core.Common.Messages;

internal sealed class CategorySelectionViewModel : CategoryListViewModel
{
    public CategorySelectionViewModel(IMediator mediator, IMapper mapper, IDialogService dialogService) : base(
        mediator: mediator,
        mapper: mapper,
        dialogService: dialogService) { }

    public AsyncRelayCommand<CategoryListItemViewModel> SelectCategoryCommand
        => new(
            async c =>
            {
                var dataSet = new CategorySelectedDataSet(categoryId: c.Id, name: c.Name);
                Messenger.Send(new CategorySelectedMessage(dataSet));
                await Shell.Current.Navigation.PopModalAsync();
            });
}
