using CommunityToolkit.Mvvm.Messaging.Messages;

namespace MoneyFox.Core._Pending_.Common.Messages
{
    public class CategorySelectedMessage : ValueChangedMessage<int>
    {
        public CategorySelectedMessage(int categoryId) : base(categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}