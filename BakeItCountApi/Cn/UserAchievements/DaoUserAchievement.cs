using BakeItCountApi.Cn.Achievements;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BakeItCountApi.Cn.UserAchievements
{
    public class DaoUserAchievement
    {
        private readonly Context _context;

        public DaoUserAchievement(Context context)
        {
            _context = context;
        }
        public async Task<List<Achievement>> GetAchievementsByUserAsync(int userId)
        {
            return await _context.UserAchievements
                .Include(ua => ua.Achievement)
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.Achievement)
                .ToListAsync();
        }
    }
}
