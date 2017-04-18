using MoneyFox.Foundation.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Background;
using System.Collections.ObjectModel;
using MoneyFox.Foundation.DataModels;
using MoneyFox.Foundation.Interfaces.Repositories;
using Windows.UI.Xaml;

namespace MoneyFox.Windows.Business
{
    class StartAssistant
    {
       private readonly ICategoryRepository categoryRepository;
        private readonly ISettingsManager settingsManager;

        public StartAssistant(ICategoryRepository categoryRepository, ISettingsManager settingsManager)
        {
            this.categoryRepository = categoryRepository;
            this.settingsManager = settingsManager;
        }

        public void AddCategory()
        {
            if (settingsManager.FirstUse)
            {
                CategoryViewModel Rent = new CategoryViewModel();
                CategoryViewModel Insurance = new CategoryViewModel();
                CategoryViewModel Groceries = new CategoryViewModel();

                Rent.Name = Foundation.Resources.Strings.Rent;
                Insurance.Name = Foundation.Resources.Strings.Insurance;
                Groceries.Name = Foundation.Resources.Strings.Grocery;

                categoryRepository.Save(Rent);
                categoryRepository.Save(Insurance);
                categoryRepository.Save(Groceries);

                settingsManager.FirstUse = false;
            }
            
        }
    }
 }
