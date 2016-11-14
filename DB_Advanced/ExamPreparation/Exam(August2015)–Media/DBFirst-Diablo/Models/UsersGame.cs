namespace DBFirst_Diablo
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations.Schema;

    public partial class UsersGame
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public UsersGame()
        {
            Items = new HashSet<Item>();
        }

        public int Id { get; set; }

        public int GameId { get; set; }

        public int UserId { get; set; }

        public int CharacterId { get; set; }

        public int Level { get; set; }

        public DateTime JoinedOn { get; set; }

        [Column(TypeName = "money")]
        public decimal Cash { get; set; }

        public virtual Character Character { get; set; }

        public virtual Game Game { get; set; }

        public virtual User User { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<Item> Items { get; set; }
    }
}
