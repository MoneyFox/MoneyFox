#region

using Microsoft.Practices.ServiceLocation;
using MoneyManager.Business.Helper;
using MoneyManager.DataAccess.DataAccess;
using MoneyManager.DataAccess.Model;

#endregion

namespace MoneyManager.Business.Logic {
    public class CategoryLogic {
        private static CategoryDataAccess categoryData {
            get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>(); }
        }

        public static async void DeleteCategory(Category category, bool skipConfirmation = false) {
            if (skipConfirmation || await Utilities.IsDeletionConfirmed()) {
                categoryData.Delete(category);
            }
        }
    }
}