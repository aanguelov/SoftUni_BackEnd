namespace ProductsShop.Models
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    public class User
    {
        private ICollection<User> userFriends;
        private ICollection<Product> boughtProducts;
        private ICollection<Product> sellerProducts;

        public User()
        {
            this.userFriends = new HashSet<User>();
            this.boughtProducts = new HashSet<Product>();
            this.sellerProducts = new HashSet<Product>();
        }

        [Key]
        public int Id { get; set; }

        public string FirstName { get; set; }

        [Required]
        [MinLength(3)]
        public string LastName { get; set; }

        public int? Age { get; set; }

        public virtual ICollection<User> UserFriends
        {
            get { return this.userFriends; }
            set { this.userFriends = value; }
        }

        public virtual ICollection<Product> BoughtProducts
        {
            get { return this.boughtProducts; }
            set { this.boughtProducts = value; }
        }

        public virtual ICollection<Product> SellerProducts
        {
            get { return this.sellerProducts; }
            set { this.sellerProducts = value; }
        }
    }
}
