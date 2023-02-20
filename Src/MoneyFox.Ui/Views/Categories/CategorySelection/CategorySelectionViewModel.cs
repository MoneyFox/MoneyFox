namespace MoneyFox.Ui.Views.Categories.CategorySelection;

using AutoMapper;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Core.Common.Interfaces;
using MediatR;
using Messages;

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
                await Shell.Current.Navigation.PopModalAsync();
                Messenger.Send(new CategorySelectedMessage(dataSet));
            });
}
