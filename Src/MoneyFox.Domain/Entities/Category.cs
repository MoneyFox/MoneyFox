using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoneyFox.Domain.Entities
{
    public class Category
    {
        //used by EF Core
        private Category() { }

        public Category(string name, string note = "", bool requireNote = false)
        {
            CreationTime = DateTime.Now;
            UpdateData(name, note, requireNote);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required] public string Name { get; private set; } = "";

        public string? Note { get;set; }

        public bool RequireNote { get; set; }

        public DateTime ModificationDate { get; private set; }

        public DateTime CreationTime { get; private set; }

        public List<Payment> Payments { get; private set; } = new List<Payment>();

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
