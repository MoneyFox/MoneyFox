using PropertyChanged;

namespace MoneyFox.Foundation.Model
{
    [ImplementPropertyChanged]
    public class Category
    {
        public int Id { get; set; }

        public string Name { get; set; }
    }
}