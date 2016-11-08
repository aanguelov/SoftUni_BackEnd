namespace Phonebook.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Channels = new HashSet<Channel>();
            this.SentMessages = new HashSet<UserMessage>();
        }

        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string FullName { get; set; }

        public string Phone { get; set; }

        public virtual ICollection<Channel> Channels { get; set; }

        public virtual ICollection<UserMessage> SentMessages { get; set; }
    }
}
