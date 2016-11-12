namespace FootballBettingDatabase.Data
{
    using Models;
    using System.Data.Entity;

    public class FootballBettingContext : DbContext
    {
        public FootballBettingContext()
            : base("name=FootballBettingContext")
        {
        }

        public IDbSet<Bet> Bets { get; set; }

        public IDbSet<BetGame> BetGames { get; set; }

        public IDbSet<Colour> Colours { get; set; }

        public IDbSet<Competition> Competitions { get; set; }

        public IDbSet<Continent> Continents { get; set; }

        public IDbSet<Country> Countries { get; set; }

        public IDbSet<Game> Games { get; set; }

        public IDbSet<Player> Players { get; set; }

        public IDbSet<PlayerStatistic> PlayerStatistics { get; set; }

        public IDbSet<Position> Positions { get; set; }

        public IDbSet<Round> Rounds { get; set; }

        public IDbSet<Team> Teams { get; set; }

        public IDbSet<Town> Towns { get; set; }

        public IDbSet<User> Users { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PlayerStatistic>()
                .HasKey(ps => new
                {
                    ps.GameId,
                    ps.PlayerId
                });

            modelBuilder.Entity<BetGame>()
                .HasKey(bg => new
                {
                    bg.GameId,
                    bg.BetId
                });
                
            modelBuilder.Entity<BetGame>()
                .HasRequired(bg => bg.Bet)
                .WithMany(b => b.BetGames)
                .HasForeignKey(bg => bg.BetId);

            modelBuilder.Entity<BetGame>()
                .HasRequired(bg => bg.Game)
                .WithMany(g => g.BetGames)
                .HasForeignKey(bg => bg.GameId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasRequired(ps => ps.Game)
                .WithMany(g => g.PlayerStatistics)
                .HasForeignKey(ps => ps.GameId);

            modelBuilder.Entity<PlayerStatistic>()
                .HasRequired(ps => ps.Player)
                .WithMany(p => p.PlayerGames)
                .HasForeignKey(ps => ps.PlayerId);

            base.OnModelCreating(modelBuilder);
        }
    }
}