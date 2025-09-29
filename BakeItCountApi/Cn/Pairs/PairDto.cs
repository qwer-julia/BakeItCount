using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Cn.Users;

namespace BakeItCountApi.Cn.Pairs
{
    public class PairDto
    {
        public int PairId { get; set; }
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
        public virtual User User1 { get; set; }
        public virtual User User2 { get; set; }
        public int NextScheduleId { get; set; }
        public ICollection<Purchase> Purchases { get; set; } = new List<Purchase>();
        public ICollection<Schedule> Schedules { get; set; } = new List<Schedule>();

        public int PurchasesQuantity {  get; set; }


    }
}
