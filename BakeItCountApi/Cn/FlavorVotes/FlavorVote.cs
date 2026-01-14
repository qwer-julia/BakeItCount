using BakeItCountApi.Cn.Flavors;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BakeItCountApi.Cn.Users;

namespace BakeItCountApi.Cn.FlavorVotes
{
    [Table("FlavorVotes")]
    public class FlavorVote
    {
        [Key]
        public int FlavorVoteId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int FlavorId { get; set; }

        [Required]
        public DateTime VotedAt { get; set; } = DateTime.UtcNow;

        // Navegações
        public User User { get; set; }
        public Flavor Flavor { get; set; }
    }


}
