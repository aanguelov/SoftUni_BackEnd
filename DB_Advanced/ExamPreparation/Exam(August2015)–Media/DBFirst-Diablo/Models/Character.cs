namespace DBFirst_Diablo
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public partial class Character
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Character()
        {
            this.UsersGames = new HashSet<UsersGame>();
        }

        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public int? StatisticId { get; set; }

        public virtual Statistic Statistic { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<UsersGame> UsersGames { get; set; }
    }
}
