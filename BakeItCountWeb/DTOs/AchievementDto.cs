using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.DTOs
{
    public class AchievementDto
    {
        public int AchievementId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Criteria { get; set; }
    }
}
