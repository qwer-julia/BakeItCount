using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BakeItCountWeb.DTOs
{
    public class FlavorDto
    {
        public int FlavorId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public FlavorCategory Category { get; set; }

        public int Votes { get; set; } = 0;
    }
}
