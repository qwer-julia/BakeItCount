using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BakeItCountApi.Cn.Schedules;

namespace BakeItCountApi.Cn.Pairs
{
    [Table("Pairs")]
    public class Pair
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PairId { get; set; }

        [ForeignKey(nameof(User1))]
        public int UserId1 { get; set; }

        [ForeignKey(nameof(User2))]
        public int UserId2 { get; set; }

        public int Order { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public virtual Users.User User1 { get; set; }
        public virtual Users.User User2 { get; set; }
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

    }
}
