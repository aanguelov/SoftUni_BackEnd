namespace Phonebook
{
    using Migrations;
    using Models;
    using System.Data.Entity;

    public class PhonebookContext : DbContext
    {
        public PhonebookContext()
            : base("name=PhonebookContext")
        {
            Database.SetInitializer(new Configuration());
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Channel> Channels { get; set; }

        public IDbSet<ChannelMessage> ChannelMessages { get; set; }

        public IDbSet<UserMessage> UserMessages { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMessage>()
                .HasRequired(um => um.Sender)
                .WithMany(u => u.SentMessages)
                .WillCascadeOnDelete(false);

            base.OnModelCreating(modelBuilder);
        }
    }
}