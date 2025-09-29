using BakeItCountApi.Cn.Achievements;

namespace BakeItCountApi.Cn.UserAchievements
{
    public class CnUserAchievement
    {
        private readonly DaoUserAchievement _daoUserAchievement;

        public CnUserAchievement(DaoUserAchievement daoUserAchievement)
        {
            _daoUserAchievement = daoUserAchievement;
        }

        public async Task<List<Achievement>> GetAchievementsByUserAsync(int userId)
        {
            return await _daoUserAchievement.GetAchievementsByUserAsync(userId);
        }
    }
}
