using BakeItCountApi.Cn.Flavors;

namespace BakeItCountApi.Cn.FlavorVotes
{
    public class CnFlavorVote
    {
        private readonly DaoFlavorVote _flavorVoteDAO;

        public CnFlavorVote(DaoFlavorVote flavorVoteDAO)
        {
            _flavorVoteDAO = flavorVoteDAO;
        }
        public async Task<bool> VoteForFavoriteFlavorAsync(int userId, List<int> favoriteFlavorsIds)
        {
            return await _flavorVoteDAO.VoteForFavoriteFlavorAsync(userId, favoriteFlavorsIds);
        }

    }
}
