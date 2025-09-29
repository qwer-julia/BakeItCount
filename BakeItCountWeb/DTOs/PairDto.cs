using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.DTOs
{
    public class PairDto
    {
        public int PairId { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public virtual UserDto User1 { get; set; }
        public virtual UserDto User2 { get; set; }
        public int NextScheduleId { get; set; }
        public int PurchasesQuantity { get; set; }

        public ICollection<PurchaseDto> Purchase { get; set; } = new List<PurchaseDto>();

        public ICollection<ScheduleDto> Schedules { get; set; } = new List<ScheduleDto>();
    }
}
