namespace BakeItCountApi.Cn.Achievements
{
    public class CnAchievement
    {
        private readonly DaoAchievement _daoAchievement;


        public CnAchievement(DaoAchievement daoAchievement)
        {
            _daoAchievement = daoAchievement;
        }

        public async Task<Achievement> GetAchievementAsync(int achievementId)
        {
            return await _daoAchievement.GetAchievementAsync(achievementId);
        }

        public async Task<List<Achievement>> GetAllAchievementsAsync()
        {
            return await _daoAchievement.GetAllAchievementsAsync();
        }

        public async Task<List<Achievement>> CheckPurchaseAchievementsAsync(int userId)
        {
            return await _daoAchievement.CheckPurchaseAchievementsAsync(userId);
        }

        public async Task<List<Achievement>> CheckSwapAchievementsAsync(int userId)
        {
            return await _daoAchievement.CheckSwapAchievementsAsync(userId);
        }
    }
}
