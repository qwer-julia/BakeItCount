using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountApi.Cn.Purchases
{
    [Table("Purchases")]
    public class Purchase
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PurchaseId { get; set; }

        [ForeignKey(nameof(Schedule))]
        public int ScheduleId { get; set; }

        [Required]
        public DateTime PurchaseDate { get; set; }

        [ForeignKey(nameof(Flavor1))]
        public int Flavor1Id { get; set; }

        public virtual Flavors.Flavor Flavor1 { get; set; }

        [ForeignKey(nameof(Flavor2))]
        public int Flavor2Id { get; set; }

        public virtual Flavors.Flavor Flavor2 { get; set; }

        public string Notes { get; set; }

        public virtual Schedules.Schedule Schedule { get; set; }

    }
}
