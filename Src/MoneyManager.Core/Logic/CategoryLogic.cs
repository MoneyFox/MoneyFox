using Microsoft.Practices.ServiceLocation;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Core.Logic
{
    public class CategoryLogic
    {
        private static IDataAccess<Category> CategoryData => ServiceLocator.Current.GetInstance<IDataAccess<Category>>();

        public static async void DeleteCategory(Category category, bool skipConfirmation = false)
        {
            //TODO Refactor this
            if (skipConfirmation) //|| await Utilities.IsDeletionConfirmed())
            {
                CategoryData.Delete(category);
            }
        }
    }
}