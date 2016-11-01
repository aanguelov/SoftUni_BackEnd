namespace ProductsShop.Data
{
    using Migrations;
    using Models;
    using System.Data.Entity;

    public class ProductsShopContext : DbContext
    {
        public ProductsShopContext()
            : base("name=ProductsShopContext")
        {
            Database.SetInitializer(new MigrateDatabaseToLatestVersion<ProductsShopContext, Configuration>());
        }

        public IDbSet<User> Users { get; set; }

        public IDbSet<Product> Products { get; set; }

        public IDbSet<Category> Categories { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasMany(u => u.UserFriends)
                .WithMany()
                .Map(m =>
                {
                    m.MapLeftKey("UserId");
                    m.MapRightKey("FriendId");
                    m.ToTable("UserFriends");
                });

            // This sets OnCascadeDelete to false only for the 'Product' entity.
            modelBuilder.Entity<Product>()
                .HasRequired(p => p.Seller)
                .WithMany(u => u.ProductsForSale)
                .HasForeignKey(p => p.SellerId)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<User>()
                .Property(u => u.Age)
                .IsOptional();

            modelBuilder.Entity<Product>()
                .Property(p => p.BuyerId)
                .IsOptional();

            // This will remove the convention globally
            //modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            base.OnModelCreating(modelBuilder);
        }
    }
}