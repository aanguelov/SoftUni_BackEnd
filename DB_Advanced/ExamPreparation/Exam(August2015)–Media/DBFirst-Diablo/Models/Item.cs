namespace DBFirst_Diablo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Item
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Item()
        {
            GameTypes = new HashSet<GameType>();
            UsersGames = new HashSet<UsersGame>();
        }

        public int Id { get; set; }

        [StringLength(50)]
        public string Name { get; set; }

        public int ItemTypeId { get; set; }

        public int StatisticId { get; set; }

        [Column(TypeName = "money")]
        public decimal Price { get; set; }

        public int MinLevel { get; set; }

        public virtual ItemType ItemType { get; set; }

        public virtual Statistic Statistic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<GameType> GameTypes { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersGame> UsersGames { get; set; }
    }
}
