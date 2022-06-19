namespace MoneyFox.Win.ViewModels;

using System.Collections.ObjectModel;
using System.Windows.Input;
using Accounts;
using Categories;
using CommunityToolkit.Mvvm.Input;
using Core.Resources;
using DataBackup;
using Microsoft.UI.Xaml.Controls;
using Services;
using Statistics;

internal sealed class ShellViewModel : BaseViewModel
{
    private readonly INavigationService navigationService;

    public ShellViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        LoadMenuItems();
    }

    public ObservableCollection<MenuItem> MenuItems { get; private set; }

    public ICommand SelectedPageChangedCommand => new RelayCommand<MenuItem>(value => NavigateToMenuItem(value));

    public void InitNavigationService(Frame frame)
    {
        navigationService.Initialize(frame);
    }

    private void LoadMenuItems()
    {
        MenuItems = new()
        {
            new() { Name = Strings.AccountsTitle, Icon = "\uE80F", ViewModelType = nameof(AccountListViewModel) },
            new() { Name = Strings.StatisticsTitle, Icon = "\uE904", ViewModelType = nameof(StatisticSelectorViewModel) },
            new() { Name = Strings.CategoriesTitle, Icon = "\uE8EC", ViewModelType = nameof(CategoryListViewModel) },
            new() { Name = Strings.BackupTitle, Icon = "\uEA35", ViewModelType = nameof(BackupViewModel) }
        };
    }

    private void NavigateToMenuItem(MenuItem item)
    {
        switch (item.ViewModelType)
        {
            case nameof(AccountListViewModel):
                navigationService.Navigate<AccountListViewModel>();

                break;
            case nameof(StatisticSelectorViewModel):
                navigationService.Navigate<StatisticSelectorViewModel>();

                break;
            case nameof(CategoryListViewModel):
                navigationService.Navigate<CategoryListViewModel>();

                break;
            case nameof(BackupViewModel):
                navigationService.Navigate<BackupViewModel>();

                break;
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ViewModelType { get; set; }
    }
}
