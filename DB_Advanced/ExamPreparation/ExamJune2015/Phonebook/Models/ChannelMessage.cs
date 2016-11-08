namespace Phonebook.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ChannelMessage
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }

        public virtual User User { get; set; }

        public virtual Channel Channel { get; set; }
    }
}
