namespace FootballBettingDatabase.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Colour
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
