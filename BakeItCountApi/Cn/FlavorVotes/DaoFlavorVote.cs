using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.FlavorVotes;
using BakeItCountApi.Data;

namespace BakeItCountApi.Cn.FlavorVotes
{
    public class DaoFlavorVote
    {
        private readonly Context _context;

        public DaoFlavorVote(Context context)
        {
            _context = context;
        }

        public async Task<bool> VoteForFavoriteFlavorAsync(int userId, List<int> flavors)
        {
            if (flavors == null || flavors.Count == 0 || flavors.Count > 2)
                throw new ArgumentException("O usuário deve escolher no máximo 2 sabores.");

            var existingVotes = _context.FlavorVotes.Where(fv => fv.UserId == userId);
            _context.FlavorVotes.RemoveRange(existingVotes);

            foreach (var flavorId in flavors)
            {
                _context.FlavorVotes.Add(new FlavorVote
                {
                    UserId = userId,
                    FlavorId = flavorId,
                    VotedAt = DateTime.UtcNow
                });
            }

            return await _context.SaveChangesAsync() > 0;
        }
    }
}
