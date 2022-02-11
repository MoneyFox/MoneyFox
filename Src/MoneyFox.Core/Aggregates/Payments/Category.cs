using Dawn;
using JetBrains.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Core.Aggregates.Payments
{
    public class Category : EntityBase
    {
        [UsedImplicitly]
        private Category() { }

        public Category(string name, string note = "", bool requireNote = false)
        {
            UpdateData(name, note, requireNote);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; [UsedImplicitly] private set; }

        [Required] public string Name { get; private set; } = "";

        public string? Note { get; private set; }

        public bool RequireNote { get; private set; }

        public List<Payment> Payments { get; [UsedImplicitly] private set; } = new List<Payment>();

        public void UpdateData(string name, string note = "", bool requireNote = false)
        {
            Guard.Argument(name, nameof(name)).NotWhiteSpace();

            Name = name;
            Note = note;
            RequireNote = requireNote;
        }
    }
}