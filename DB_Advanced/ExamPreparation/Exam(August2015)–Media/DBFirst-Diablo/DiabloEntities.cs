namespace DBFirst_Diablo
{
    using System.Data.Entity;

    public partial class DiabloEntities : DbContext
    {
        public DiabloEntities()
            : base("name=DiabloEntities")
        {
        }

        public virtual DbSet<Character> Characters { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<GameType> GameTypes { get; set; }
        public virtual DbSet<Item> Items { get; set; }
        public virtual DbSet<ItemType> ItemTypes { get; set; }
        public virtual DbSet<Statistic> Statistics { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UsersGame> UsersGames { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Character>()
                .HasMany(e => e.UsersGames)
                .WithRequired(e => e.Character)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Game>()
                .HasMany(e => e.UsersGames)
                .WithRequired(e => e.Game)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameType>()
                .HasMany(e => e.Games)
                .WithRequired(e => e.GameType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<GameType>()
                .HasMany(e => e.Items)
                .WithMany(e => e.GameTypes)
                .Map(m => m.ToTable("GameTypeForbiddenItems").MapLeftKey("GameTypeId").MapRightKey("ItemId"));

            modelBuilder.Entity<Item>()
                .Property(e => e.Price)
                .HasPrecision(19, 4);

            modelBuilder.Entity<Item>()
                .HasMany(e => e.UsersGames)
                .WithMany(e => e.Items)
                .Map(m => m.ToTable("UserGameItems").MapLeftKey("ItemId").MapRightKey("UserGameId"));

            modelBuilder.Entity<ItemType>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.ItemType)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<Statistic>()
                .HasMany(e => e.GameTypes)
                .WithOptional(e => e.Statistic)
                .HasForeignKey(e => e.BonusStatsId);

            modelBuilder.Entity<Statistic>()
                .HasMany(e => e.Items)
                .WithRequired(e => e.Statistic)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .HasMany(e => e.UsersGames)
                .WithRequired(e => e.User)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<UsersGame>()
                .Property(e => e.Cash)
                .HasPrecision(19, 4);
        }
    }
}
