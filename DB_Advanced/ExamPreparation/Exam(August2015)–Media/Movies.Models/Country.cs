namespace Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Country
    {
        public Country()
        {
            this.Users = new HashSet<User>();
        }

        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<User> Users { get; set; }
    }
}
