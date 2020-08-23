namespace MoneyFox.Messages
{
    public class CategorySelectedMessage
    {
        public CategorySelectedMessage(int categoryId)
        {
            CategoryId = categoryId;
        }

        public int CategoryId { get; set; }
    }
}
