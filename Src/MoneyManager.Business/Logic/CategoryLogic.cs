using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.Foundation.Model;
using MoneyManager.Foundation.OperationContracts;

namespace MoneyManager.Business.Logic
{
    public class CategoryLogic
    {
        private static IDataAccess<Category> CategoryData => ServiceLocator.Current.GetInstance<IDataAccess<Category>>();

        public static async void DeleteCategory(Category category, bool skipConfirmation = false)
        {
            if (skipConfirmation || await Utilities.IsDeletionConfirmed())
            {
                CategoryData.Delete(category);
            }
        }
    }
}