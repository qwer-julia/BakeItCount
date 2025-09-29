using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BakeItCountApi.Dao.Pairs
{
    public class DaoPair
    {
        private readonly Context _context;

        public DaoPair(Context context)
        {
            _context = context;
        }

        public async Task<Pair> AddAsync(Pair pair)
        {
            _context.Pairs.Add(pair);
            await _context.SaveChangesAsync();
            return pair;
        }

        public async Task<Pair> GetExistingPairForUsers(int userId1, int userId2)
        {
            return await _context.Pairs
                .Include(p => p.User1)
                .Include(p => p.User2)
                .FirstOrDefaultAsync(p =>
                    p.UserId1 == userId1 || p.UserId2 == userId1 ||
                    p.UserId1 == userId2 || p.UserId2 == userId2
                );
        }

        public async Task<Pair> GetByIdAsync(int pairId)
        {
            return await _context.Pairs
                .Include(p => p.User1)
                .Include(p => p.User2)
                .FirstOrDefaultAsync(p => p.PairId == pairId);
        }

        public async Task<List<PairDto>> GetAllAsync()
        {
            var today = DateTime.UtcNow;

            return await _context.Pairs
                .Include(p => p.User1)
                .Include(p => p.User2)
                .OrderBy(p => p.Order)
                .Select(p => new PairDto
                {
                    PairId = p.PairId,
                    UserId1 = p.UserId1,
                    UserId2 = p.UserId2,
                    User1 = p.User1,
                    User2 = p.User2,
                    NextScheduleId = _context.Schedules
                        .Where(s => s.PairId == p.PairId && s.WeekRef >= today)
                        .OrderBy(s => s.WeekRef)
                        .Select(s => s.ScheduleId)
                        .FirstOrDefault()
                })
                .ToListAsync();
        }

        public async Task<List<PairDto>> GetAllPairsWithPurchaseAsync()
        {
            var pairsDto = await _context.Pairs
            .Select(p => new PairDto
            {
                PairId = p.PairId,
                UserId1 = p.UserId1,
                UserId2 = p.UserId2,
                User1 = p.User1,
                User2 = p.User2,
                Schedules = p.Schedules.Select(s => new Schedule
                {
                    ScheduleId = s.ScheduleId,
                    Purchases = s.Purchases.Select(pr => new Purchase
                    {
                        PurchaseId = pr.PurchaseId,
                        Flavor1 = pr.Flavor1,
                        Flavor2 = pr.Flavor2
                    }).ToList(),
                }).ToList(),
                PurchasesQuantity = p.Schedules.Sum(s => s.Purchases.Count())
            })
            .ToListAsync();           

            return pairsDto;
        }

        public async Task DeleteAsync(Pair pair)
        {
            _context.Pairs.Remove(pair);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Pair pair)
        {
            _context.Pairs.Update(pair);
            await _context.SaveChangesAsync();
        }

        public async Task<int> GetMaxOrderAsync()
        {
            if (!await _context.Pairs.AnyAsync())
                return 0;

            return await _context.Pairs.MaxAsync(p => p.Order);
        }
    }
}
