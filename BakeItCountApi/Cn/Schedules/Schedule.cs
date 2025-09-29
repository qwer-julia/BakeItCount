using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BakeItCountApi.Cn.Purchases;

namespace BakeItCountApi.Cn.Schedules
{
    [Table("Schedules")]
    public class Schedule
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ScheduleId { get; set; }

        [ForeignKey(nameof(Pair))]
        public int PairId { get; set; }

        [Required]
        public DateTime WeekRef { get; set; }

        public bool Confirmed { get; set; } = false;

        public virtual Pairs.Pair Pair { get; set; }

        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
    }
}
