namespace BakeItCountWeb.DTOs
{
    public class ScheduleDto
    {
        public int ScheduleId { get; set; }
        public int PairId { get; set; }
        public DateTime WeekRef { get; set; } 
        public bool Confirmed { get; set; }

        public virtual PairDto Pair { get; set; }

        public virtual PurchaseDto Purchase {  get; set; }
    }
}
