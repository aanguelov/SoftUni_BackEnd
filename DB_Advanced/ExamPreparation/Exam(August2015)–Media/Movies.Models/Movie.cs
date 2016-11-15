namespace Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Movie
    {
        public Movie()
        {
            this.Users = new HashSet<User>();
            this.Ratings = new HashSet<Rating>();
        }

        public int Id { get; set; }

        [Required]
        public string Isbn { get; set; }

        [Required]
        [MinLength(2)]
        [MaxLength(100)]
        public string Title { get; set; }

        public AgeRestriction Restriction { get; set; }

        public virtual ICollection<User> Users { get; set; }

        public virtual ICollection<Rating> Ratings { get; set; }
    }
}
