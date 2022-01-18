using MoneyFox.SharedKernel;

namespace MoneyFox.Core.Events
{
    internal sealed class CategoryCreatedEvent : BaseDomainEvent
    {
        public CategoryCreatedEvent(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; }
    }
}