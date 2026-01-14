using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.DTOs
{
    public class SwapRequest
    {
        public int SwapId { get; set; }
        public int SourceScheduleId { get; set; }
        public int TargetScheduleId { get; set; }
        public int RequestingUserId { get; set; }

        [Required]
        public SwapStatus Status { get; set; } = SwapStatus.PENDING;
        public virtual ScheduleDto TargetSchedule { get; set; }
    }

    public enum SwapStatus
    {
        PENDING,
        APPROVED,
        REJECTED
    }
}
