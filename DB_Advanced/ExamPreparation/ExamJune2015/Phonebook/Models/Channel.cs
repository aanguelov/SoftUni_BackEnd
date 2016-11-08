namespace Phonebook.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Channel
    {
        public Channel()
        {
            this.Users = new HashSet<User>();
            this.Messages = new HashSet<ChannelMessage>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<ChannelMessage> Messages { get; set; }
    }
}
