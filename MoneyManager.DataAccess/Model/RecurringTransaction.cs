#region

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using MoneyManager.DataAccess.DataAccess;
using PropertyChanged;
using SQLite.Net.Attributes;

#endregion

namespace MoneyManager.DataAccess.Model {
	[ImplementPropertyChanged]
	[Table("RecurringTransactiont")]
	public class RecurringTransaction {
		private IEnumerable<Account> allAccounts {
			get { return ServiceLocator.Current.GetInstance<AccountDataAccess>().AllAccounts; }
		}

		private IEnumerable<Category> allCategories {
			get { return ServiceLocator.Current.GetInstance<CategoryDataAccess>().AllCategories; }
		}

		[PrimaryKey, AutoIncrement]
		public int Id { get; set; }

		public int ChargedAccountId { get; set; }
		public int TargetAccountId { get; set; }
		public DateTime StartDate { get; set; }
		public DateTime EndDate { get; set; }
		public bool IsEndless { get; set; }
		public double Amount { get; set; }
		public double AmountWithoutExchange { get; set; }
		public string Currency { get; set; }
		public int? CategoryId { get; set; }
		public int Type { get; set; }
		public int Recurrence { get; set; }
		public string Note { get; set; }

		[Ignore]
		public Account ChargedAccount {
			get { return allAccounts.FirstOrDefault(x => x.Id == ChargedAccountId); }
			set { ChargedAccountId = value.Id; }
		}

		[Ignore]
		public Account TargetAccount {
			get { return allAccounts.FirstOrDefault(x => x.Id == ChargedAccountId); }
			set { ChargedAccountId = value.Id; }
		}

		[Ignore]
		public Category Category {
			get { return allCategories.FirstOrDefault(x => x.Id == CategoryId); }
			set {
				CategoryId = value == null
					? (int?) null
					: value.Id;
			}
		}
	}
}