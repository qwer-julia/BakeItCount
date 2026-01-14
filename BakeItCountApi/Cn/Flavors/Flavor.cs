using BakeItCountApi.Cn.Enum;
using BakeItCountApi.Cn.FlavorVotes;
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

        [NotMapped]
        public int Votes { get; set; }

        [Required]
        public FlavorCategory Category { get; set; }

        public ICollection<FlavorVote> FlavorVotes { get; set; }
    }
}
