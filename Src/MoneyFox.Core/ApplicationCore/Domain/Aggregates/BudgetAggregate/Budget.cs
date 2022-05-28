namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate
{

    using System.Collections.Generic;
    using JetBrains.Annotations;
    using MoneyFox.Core.Common.Interfaces;

    public class Budget : EntityBase, IAggregateRoot
    {
        [UsedImplicitly]
        private Budget() { }

        private Budget(string name, double spendingLimit, List<int> includedCategories)
        {
            Name = name;
            SpendingLimit = spendingLimit;
            IncludedCategories = includedCategories;
        }

        public int Id
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public string Name
        {
            get;

            [UsedImplicitly]
            private set;
        } = string.Empty;

        public double SpendingLimit
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public List<int> IncludedCategories
        {
            get;

            [UsedImplicitly]
            private set;
        } = new List<int>();
    }

}
