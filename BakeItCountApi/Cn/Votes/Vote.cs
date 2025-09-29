using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeItCountApi.Cn.Votes
{
    [Table("Votes")]
    public class Vote
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int VoteId { get; set; }

        [Required]
        public DateTime WeekRef { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Flavor))]
        public int FlavorId { get; set; }

        public virtual Users.User User { get; set; }
        public virtual Flavors.Flavor Flavor { get; set; }
    }
}
