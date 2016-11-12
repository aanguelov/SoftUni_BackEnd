namespace FootballBettingDatabase.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class User
    {
        public User()
        {
            this.Bets = new HashSet<Bet>();
        }

        public int Id { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        public string Email { get; set; }

        public string FullName { get; set; }

        public decimal Balance { get; set; }

        public virtual ICollection<Bet> Bets { get; set; }
    }
}