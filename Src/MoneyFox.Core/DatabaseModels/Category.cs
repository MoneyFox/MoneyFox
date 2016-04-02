using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using PropertyChanged;

namespace MoneyFox.Core.DatabaseModels
{
    [Table("Categories")]
    [ImplementPropertyChanged]
    public class Category
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public List<Payment> Payments { get; set; } 

        public override string ToString()
        {
            return $"Name: {Name}";
        }
    }
}