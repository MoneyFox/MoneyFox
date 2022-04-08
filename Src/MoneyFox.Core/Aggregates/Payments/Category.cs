namespace MoneyFox.Core.Aggregates.Payments
{

    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using Dawn;
    using JetBrains.Annotations;

    public class Category : EntityBase
    {
        [UsedImplicitly]
        private Category() { }

        public Category(string name, string note = "", bool requireNote = false)
        {
            UpdateData(name: name, note: note, requireNote: requireNote);
            CreationTime = DateTime.Now;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id
        {
            get;

            [UsedImplicitly]
            private set;
        }

        [Required]
        public string Name { get; private set; } = "";

        public string? Note { get; private set; }

        public bool RequireNote { get; private set; }

        [Obsolete("Will be removed")]
        public DateTime? ModificationDate { get; private set; }

        [Obsolete("Will be removed")]
        public DateTime CreationTime
        {
            get;

            [UsedImplicitly]
            private set;
        }

        public List<Payment> Payments
        {
            get;

            [UsedImplicitly]
            private set;
        } = new List<Payment>();

        public void UpdateData(string name, string note = "", bool requireNote = false)
        {
            Guard.Argument(value: name, name: nameof(name)).NotWhiteSpace();
            Name = name;
            Note = note;
            RequireNote = requireNote;
            ModificationDate = DateTime.Now;
        }
    }

}
