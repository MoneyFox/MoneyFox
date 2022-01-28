using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MoneyFox.Win.Services;
using NLog;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace MoneyFox.Win.ViewModels
{
    public class ShellViewModel : ObservableObject
    {
        private static readonly Logger Logger = LogManager.GetCurrentClassLogger();
        private readonly INavigationService navigationService;

        public ShellViewModel(INavigationService navigationService)
        {
            this.navigationService = navigationService;
            LoadMenuItems();
        }

        private ObservableCollection<MenuItem> menuItems;
        public ObservableCollection<MenuItem> MenuItems => menuItems;

        public void LoadMenuItems()
        {
            menuItems = new ObservableCollection<MenuItem>();
            //menuItems.Add(new MenuItem { Name = Strings.AccountsTitle, Icon = "\uE80F", ViewModelType = nameof(AccountListViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.StatisticsTitle, Icon = "\uE904", ViewModelType = nameof(StatisticSelectorViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.CategoriesTitle, Icon = "\uE8EC", ViewModelType = nameof(CategoryListViewModel) });
            //menuItems.Add(new MenuItem { Name = Strings.BackupTitle, Icon = "\uEA35", ViewModelType = nameof(BackupViewModel) });
        }

        public ICommand SelectedPageChangedCommand => new RelayCommand<MenuItem>((value) => NavigateToMenuItem(value));
        private void NavigateToMenuItem(MenuItem item)
        {
            //switch(item.ViewModelType)
            //{
            //    case nameof(AccountListViewModel):
            //        navigationService.NavigateTo<AccountListViewModel>();
            //        break;
            //    case nameof(StatisticSelectorViewModel):
            //        navigationService.NavigateTo<StatisticSelectorViewModel>();
            //        break;
            //    case nameof(CategoryListViewModel):
            //        navigationService.NavigateTo<CategoryListViewModel>();
            //        break;
            //    case nameof(BackupViewModel):
            //        navigationService.NavigateTo<BackupViewModel>();
            //        break;
            //}
        }

        public class MenuItem
        {
            public string Name { get; set; }
            public string Icon { get; set; }
            public string ViewModelType { get; set; }
        }
    }
}