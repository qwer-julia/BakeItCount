using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BakeItCountApi.Cn.Achievements
{
    [Table("UserAchievements")]
    public class UserAchievement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserAchievementId { get; set; }

        [ForeignKey(nameof(User))]
        public int UserId { get; set; }

        [ForeignKey(nameof(Achievement))]
        public int AchievementId { get; set; }

        public DateTime EarnedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        public virtual Users.User User { get; set; }
        public virtual Achievement Achievement { get; set; }
    }
}
