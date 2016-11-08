namespace Phonebook.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class UserMessage
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public virtual User Recipient { get; set; }

        [Required]
        public virtual User Sender { get; set; }
    }
}
