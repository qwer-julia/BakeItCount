namespace BakeItCountWeb.DTOs
{
    public class FlavorVoteDto
    {
        public UserDto CurrentUser { get; set; }
        public List<FlavorDto> Flavors { get; set; }
    }

}
