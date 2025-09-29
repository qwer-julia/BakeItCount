using BakeItCountApi.Cn.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeItCountApi.Cn.Flavors
{
    [Table("Flavors")]
    public class Flavor
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int FlavorId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        [Required]
        public FlavorCategory Category { get; set; }

        public int Votes { get; set; } = 0;

    }
}
