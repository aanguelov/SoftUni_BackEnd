namespace Movies.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.FavouriteMovies = new HashSet<Movie>();
            this.RatedMovies = new HashSet<Rating>();
        }

        public int Id { get; set; }

        [Required]
        [MinLength(5)]
        public string Username { get; set; }

        public string Email { get; set; }

        public int? Age { get; set; }

        public Country Country { get; set; }

        public ICollection<Movie> FavouriteMovies { get; set; }

        public ICollection<Rating> RatedMovies { get; set; }
    }
}
