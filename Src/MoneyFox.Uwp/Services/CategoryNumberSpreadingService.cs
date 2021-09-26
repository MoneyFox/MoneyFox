using MoneyFox.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace MoneyFox.Uwp.Services
{
    public static class CategoryNumberSpreadingService
    {
        private const string SettingsKey = "AppCategoryNumberSpreading";

        public static int CategoryNumberSpreading { get; set; } = 6;

        public static void Initialize() => CategoryNumberSpreading = LoadCategoryNumberSpreadingFromSettings();

        public static int LoadCategoryNumberSpreadingFromSettings()
        {
            string categoryNumberSettingValue = ApplicationData.Current.LocalSettings.Read<string>(SettingsKey);

            int categoryNumber = 0;

            if(!string.IsNullOrEmpty(categoryNumberSettingValue))
                categoryNumber = int.Parse(categoryNumberSettingValue);
            else
            {
                SaveCategoryNumberSpreadingInSettings(CategoryNumberSpreading);
                categoryNumber = CategoryNumberSpreading;
            }

            return categoryNumber;
        }

        public static void SaveCategoryNumberSpreadingInSettings(int categoryNumber) => ApplicationData.Current.LocalSettings.SaveString(SettingsKey, categoryNumber.ToString());
    }
}
