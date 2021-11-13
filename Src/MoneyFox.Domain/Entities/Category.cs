using JetBrains.Annotations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Domain.Entities
{
    public class Category
    {
        //used by EF Core
        [UsedImplicitly]
        private Category() { }

        public Category(string name, string note = "", bool requireNote = false)
        {
            CreationTime = DateTime.Now;
            UpdateData(name, note, requireNote);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; [UsedImplicitly] private set; }

        [Required] public string Name { get; private set; } = "";

        public string? Note { get; private set; }

        public bool RequireNote { get; private set; }

        public DateTime ModificationDate { get; private set; }

        public DateTime CreationTime { get; }

        public List<Payment> Payments { get; } = new List<Payment>();

        public void UpdateData(string name, string note = "", bool requireNote = false)
        {
            if(string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Note = note;
            RequireNote = requireNote;
            ModificationDate = DateTime.Now;
        }
    }
}