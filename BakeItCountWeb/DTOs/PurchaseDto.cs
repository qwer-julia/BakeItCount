using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.DTOs
{
    public class PurchaseDto
    {
        public int PurchaseId { get; set; }

        public int ScheduleId { get; set; }

        public DateTime PurchaseDate { get; set; }

        public int Flavor1Id { get; set; }

        public virtual FlavorDto? Flavor1 { get; set; }

        public int Flavor2Id { get; set; }

        public virtual FlavorDto? Flavor2 { get; set; }

        public string Notes { get; set; }

        public virtual ScheduleDto Schedule { get; set; }
    }
}
