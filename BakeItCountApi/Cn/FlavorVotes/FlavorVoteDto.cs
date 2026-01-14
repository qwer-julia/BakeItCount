namespace BakeItCountApi.Cn.FlavorVotes
{
    public class FlavorVoteDto
    {
        public int UserId { get; set; }
        public List<int> Flavors { get; set; }
    }
}
