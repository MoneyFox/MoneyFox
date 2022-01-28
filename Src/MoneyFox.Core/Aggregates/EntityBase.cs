using System;

namespace MoneyFox.Core.Aggregates
{
    public class EntityBase
    {
        public DateTime Created { get; set; }

        public DateTime? LastModified { get; set; }
    }
}
