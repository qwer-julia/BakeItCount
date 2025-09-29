using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountApi.Cn.Swaps
{
    public enum SwapStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }

    [Table("Swaps")]
    public class Swap
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int SwapId { get; set; }

        [ForeignKey(nameof(SourceSchedule))]
        public int SourceScheduleId { get; set; }

        [ForeignKey(nameof(TargetSchedule))]
        public int TargetScheduleId { get; set; }

        [Required]
        public SwapStatus Status { get; set; } = SwapStatus.PENDING;

        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        public virtual Schedules.Schedule SourceSchedule { get; set; }
        public virtual Schedules.Schedule TargetSchedule { get; set; }
    }
}
