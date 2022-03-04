namespace MoneyFox.ViewModels.OverflowMenu
{
    using CommunityToolkit.Mvvm.ComponentModel;
    using CommunityToolkit.Mvvm.Input;
    using Core.Resources;
    using MoneyFox;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Xamarin.Forms;

    public class OverflowMenuViewModel : ObservableObject
    {
        public List<OverflowItem> OverflowEntries
            => new List<OverflowItem>
            {
                new OverflowItem
                {
                    Name = Strings.CategoriesTitle,
                    Type = OverflowMenuItemType.Categories
                },
                new OverflowItem
                {
                    Name = Strings.BackupTitle,
                    Type = OverflowMenuItemType.Backup
                },
                new OverflowItem
                {
                    Name = Strings.SettingsTitle,
                    Type = OverflowMenuItemType.Settings
                },
                new OverflowItem
                {
                    Name = Strings.AboutTitle,
                    Type = OverflowMenuItemType.About
                }
            };

        public AsyncRelayCommand<OverflowItem> GoToSelectedItemCommand
            => new AsyncRelayCommand<OverflowItem>(async s => await GoToSelectedItem(s));

        private static async Task GoToSelectedItem(OverflowItem item)
        {
            if(item.Type == OverflowMenuItemType.Categories)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.CategoryListRoute);
            }
            else if(item.Type == OverflowMenuItemType.Backup)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.BackupRoute);
            }
            else if(item.Type == OverflowMenuItemType.Settings)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.SettingsRoute);
            }
            else if(item.Type == OverflowMenuItemType.About)
            {
                await Shell.Current.GoToAsync(ViewModelLocator.AboutRoute);
            }
        }
    }

    public enum OverflowMenuItemType
    {
        Categories,
        Backup,
        Settings,
        About
    }

    public class OverflowItem
    {
        public string Name { get; set; } = "";
        public OverflowMenuItemType Type { get; set; }
    }
}