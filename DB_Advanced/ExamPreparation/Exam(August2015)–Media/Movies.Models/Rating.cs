namespace Movies.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Rating
    {
        public int Id { get; set; }

        [Required]
        [Range(0, 10)]
        public int Stars { get; set; }

        public virtual User User { get; set; }

        public virtual Movie Movie { get; set; }
    }
}
