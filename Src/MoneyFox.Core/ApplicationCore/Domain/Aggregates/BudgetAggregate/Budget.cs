namespace MoneyFox.Core.ApplicationCore.Domain.Aggregates.BudgetAggregate
{

    using System.Collections.Generic;
    using Common.Interfaces;
    using JetBrains.Annotations;

    public class Budget : EntityBase, IAggregateRoot
    {
        [UsedImplicitly]
        private Budget() { }

        public Budget(string name, decimal spendingLimit, IReadOnlyList<int> includedCategories)
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

        public decimal SpendingLimit
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public IReadOnlyList<int> IncludedCategories
        {
            get;

            [UsedImplicitly]
            private set;
        } = new List<int>();
    }

}
