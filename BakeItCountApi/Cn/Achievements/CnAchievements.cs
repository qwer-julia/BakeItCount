using BakeItCountApi.Cn.Achievements;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Cn.Swaps;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BakeItCountApi.Cn.Achievements
{
    public class CnAchievement
    {
        private readonly Context _context;

        public CnAchievement(Context context)
        {
            _context = context;
        }

        public async Task<List<Achievement>> CheckAndUnlockAchievementsAsync(int userId)
        {
            var purchases = await _context.Purchases
        .Include(p => p.Flavor1)
        .Include(p => p.Flavor2)
        .Include(p => p.Schedule)
            .ThenInclude(s => s.Pair)
        .Where(p => p.Schedule.Pair.UserId1 == userId || p.Schedule.Pair.UserId2 == userId)
        .ToListAsync();

            var swaps = await _context.Swaps
                .Include(s => s.SourceSchedule).ThenInclude(sc => sc.Pair)
                .Include(s => s.TargetSchedule).ThenInclude(tc => tc.Pair)
                .Where(s =>
                    s.SourceSchedule.Pair.UserId1 == userId || s.SourceSchedule.Pair.UserId2 == userId ||
                    s.TargetSchedule.Pair.UserId1 == userId || s.TargetSchedule.Pair.UserId2 == userId
                )
                .ToListAsync();

            var unlocked = await _context.UserAchievements
                .Where(ua => ua.UserId == userId)
                .Select(ua => ua.AchievementId)
                .ToListAsync();

            var achievementsToUnlock = new List<int>();

            // 🔹 Monta lista de todos os sabores consumidos
            var allPurchasedFlavors = purchases
                .SelectMany(p => new[] { p.Flavor1, p.Flavor2 })
                .Where(f => f != null) // segurança
                .Distinct()
                .ToList();

            // 🔹 Conquista 1 – Primeiro Delícia
            if (purchases.Count >= 1 && !unlocked.Contains(1))
                achievementsToUnlock.Add(1);

            // 🔹 Conquista 2 – 3 chocolates
            if (allPurchasedFlavors.Count(f => f.Name.Contains("chocolate", StringComparison.OrdinalIgnoreCase)) >= 3
                && !unlocked.Contains(2))
                achievementsToUnlock.Add(2);

            // 🔹 Conquista 3 – 5 premium
            if (allPurchasedFlavors.Count(f => f.Category == Enum.FlavorCategory.Caro) >= 5 && !unlocked.Contains(3))
                achievementsToUnlock.Add(3);

            // 🔹 Conquista 4 – 5 pedidos confirmados
            if (purchases.Count(p => p.Schedule.Confirmed) >= 5 && !unlocked.Contains(4))
                achievementsToUnlock.Add(4);

            // 🔹 Conquista 5 – 1 de cada sabor
            var allFlavors = await _context.Flavors.Select(f => f.FlavorId).ToListAsync();
            if (allFlavors.All(fid => allPurchasedFlavors.Any(f => f.FlavorId == fid)) && !unlocked.Contains(5))
                achievementsToUnlock.Add(5);

            // 🔹 Conquista 6 – 7 dias diferentes
            if (purchases.Select(p => p.PurchaseDate.Date).Distinct().Count() >= 7 && !unlocked.Contains(6))
                achievementsToUnlock.Add(6);

            // 🔹 Conquista 7 – 50 bolos
            if (purchases.Count >= 50 && !unlocked.Contains(7))
                achievementsToUnlock.Add(7);

            // 🔹 Conquista 8 – Nenhuma troca em 10 pedidos
            if (purchases.Count >= 10 && swaps.Count == 0 && !unlocked.Contains(8))
                achievementsToUnlock.Add(8);

            // 🔹 Conquista 11 – Caloteiro
            if (purchases.Any(p => p.Schedule.Confirmed == false) && !unlocked.Contains(11))
                achievementsToUnlock.Add(11);

            // 🔹 Conquista 12 – Troca-troca (5+ swaps)
            if (swaps.Count >= 5 && !unlocked.Contains(12))
                achievementsToUnlock.Add(12);

            // 🔹 Conquista 13 – Trem Bão (mineiros)
            var saboresMineiros = new[]
            {
        "fubá com goiabada",
        "queijadinha",
        "fubá com doce de leite",
        "fubá cremoso"
    };

            if (allPurchasedFlavors.Count(f => saboresMineiros.Contains(f.Name.ToLower())) >= 5 && !unlocked.Contains(13))
                achievementsToUnlock.Add(13);

            // 🔹 Conquista 14 – Luiz Felipe
            if (allPurchasedFlavors.Any(f => f.Name.Equals("Luiz Felipe", StringComparison.OrdinalIgnoreCase)) && !unlocked.Contains(14))
                achievementsToUnlock.Add(14);

            // 🔹 Conquista 15 – Mestre Confeiteiro
            if (allFlavors.All(fid => allPurchasedFlavors.Any(f => f.FlavorId == fid)) && !unlocked.Contains(15))
                achievementsToUnlock.Add(15);

            // 🔹 Conquista 16 – Cobertura Extra
            if (purchases.Any(p => !string.IsNullOrEmpty(p.Notes) && p.Notes.Contains("cobertura", StringComparison.OrdinalIgnoreCase))
                && !unlocked.Contains(16))
                achievementsToUnlock.Add(16);

            // 🔹 Conquista 17 – Saudável & Doce
            if (allPurchasedFlavors.Any(f => f.Category == Enum.FlavorCategory.Diet) && !unlocked.Contains(17))
                achievementsToUnlock.Add(17);

            // 🔹 Conquista 19 – Seguidor de Multidões
            var mostVotedFlavor = await _context.Flavors.OrderByDescending(f => f.Votes).FirstOrDefaultAsync();
            if (mostVotedFlavor != null && allPurchasedFlavors.Any(f => f.FlavorId == mostVotedFlavor.FlavorId) && !unlocked.Contains(19))
                achievementsToUnlock.Add(19);

            // 🔹 Persistência
            var newAchievements = new List<Achievement>();
            foreach (var id in achievementsToUnlock)
            {
                _context.UserAchievements.Add(new UserAchievement
                {
                    UserId = userId,
                    AchievementId = id,
                    EarnedAt = DateTime.UtcNow
                });

                var achievement = await _context.Achievements.FindAsync(id);
                if (achievement != null)
                    newAchievements.Add(achievement);
            }

            if (newAchievements.Any())
                await _context.SaveChangesAsync();

            return newAchievements;
        }
    }
}
