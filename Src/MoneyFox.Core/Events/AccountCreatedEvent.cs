using MoneyFox.SharedKernel;

namespace MoneyFox.Core.Events
{
    internal sealed class AccountCreatedEvent : BaseDomainEvent
    {
        public AccountCreatedEvent(int accountId)
        {
            AccountId = accountId;
        }

        public int AccountId { get; }
    }
}