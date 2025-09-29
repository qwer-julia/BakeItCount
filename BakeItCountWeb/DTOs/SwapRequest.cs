namespace BakeItCountWeb.DTOs
{
    public class SwapRequest
    {
        public int SwapId { get; set; }
        public int SourceScheduleId { get; set; }
        public int TargetScheduleId { get; set; }
        public int RequestingUserId { get; set; }

        public virtual ScheduleDto TargetSchedule { get; set; }
    }

}
