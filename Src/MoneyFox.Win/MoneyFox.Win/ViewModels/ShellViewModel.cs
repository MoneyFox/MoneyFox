namespace MoneyFox.Win.ViewModels;

using Accounts;
using Categories;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Core.Resources;
using DataBackup;
using NLog;
using Services;
using Statistics;
using System.Collections.ObjectModel;
using System.Windows.Input;

public class ShellViewModel : ObservableObject
{
    private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
    private readonly INavigationService navigationService;

    public ShellViewModel(INavigationService navigationService)
    {
        this.navigationService = navigationService;
        LoadMenuItems();
    }

    public ObservableCollection<MenuItem> MenuItems { get; private set; }

    public void LoadMenuItems()
    {
        MenuItems = new ObservableCollection<MenuItem>();
        MenuItems.Add(new MenuItem
        {
            Name = Strings.AccountsTitle, Icon = "\uE80F", ViewModelType = nameof(AccountListViewModel)
        });
        MenuItems.Add(new MenuItem
        {
            Name = Strings.StatisticsTitle, Icon = "\uE904", ViewModelType = nameof(StatisticSelectorViewModel)
        });
        MenuItems.Add(new MenuItem
        {
            Name = Strings.CategoriesTitle, Icon = "\uE8EC", ViewModelType = nameof(CategoryListViewModel)
        });
        MenuItems.Add(new MenuItem
        {
            Name = Strings.BackupTitle, Icon = "\uEA35", ViewModelType = nameof(BackupViewModel)
        });
    }

    public ICommand SelectedPageChangedCommand => new RelayCommand<MenuItem>(value => NavigateToMenuItem(value));

    private void NavigateToMenuItem(MenuItem item)
    {
        switch(item.ViewModelType)
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