#region

using System;
using MoneyManager.Foundation.OperationContracts;
using Xamarin;

#endregion

namespace MoneyManager.Foundation {
	public abstract class AbstractDataAccess<T> : IDataAccess<T> {
		public void Save(T itemToSave) {
			try {
				SaveToDb(itemToSave);
			}
			catch (Exception ex) {
				Insights.Report(ex);
			}
		}

		public void Delete(T itemToDelete) {
			try {
				DeleteFromDatabase(itemToDelete);
			}
			catch (Exception ex) {
				Insights.Report(ex);
			}
		}

		public void LoadList() {
			try {
				GetListFromDb();
			}
			catch (Exception ex) {
				Insights.Report(ex);
			}
		}

		public void Update(T itemToUpdate) {
			try {
				UpdateItem(itemToUpdate);
			}
			catch (Exception ex) {
				Insights.Report(ex);
			}
		}

		protected abstract void SaveToDb(T itemToAdd);
		protected abstract void DeleteFromDatabase(T itemToDelete);
		protected abstract void GetListFromDb();
		protected abstract void UpdateItem(T itemToUpdate);
	}
}