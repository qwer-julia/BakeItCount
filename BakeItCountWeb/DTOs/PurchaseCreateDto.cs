namespace BakeItCountWeb.DTOs
{
    public class PurchaseCreateDto
    {
        public int ScheduleId { get; set; }
        public DateTime PurchaseDate { get; set; }
        public int Flavor1Id { get; set; }
        public int Flavor2Id { get; set; }
        public string? Notes { get; set; }
    }

}
