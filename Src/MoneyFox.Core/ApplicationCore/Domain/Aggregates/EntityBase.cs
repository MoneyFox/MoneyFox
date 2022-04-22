namespace MoneyFox.Core.Aggregates
{

    using System;

    public class EntityBase
    {
        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }

}
