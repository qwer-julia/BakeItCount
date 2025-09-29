using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BakeItCountApi.Dao.Purchases
{
    public class DaoPurchase
    {
        private readonly Context _context;

        public DaoPurchase(Context context)
        {
            _context = context;
        }

        public async Task<Purchase> AddAsync(Purchase purchase)
        {
            _context.Purchases.Add(purchase);
            await _context.SaveChangesAsync();
            return purchase;
        }

        public async Task<Purchase> GetByIdAsync(int purchaseId)
        {
            return await _context.Purchases
                .Include(p => p.Schedule)
                .Include(p => p.Flavor1)
                .Include(p => p.Flavor2)
                .Include(p => p.Schedule.Pair)
                .Include(p => p.Schedule.Pair.User1)
                .Include(p => p.Schedule.Pair.User2)
                .FirstOrDefaultAsync(p => p.PurchaseId == purchaseId);
        }

        public async Task<int> GetPurchaseByScheduleId(int scheduleId)
        {
            int purchaseId = await _context.Purchases
                .Where(p => p.ScheduleId == scheduleId)
                .Select(p => p.PurchaseId)
                .FirstOrDefaultAsync();
            return purchaseId;
        }

        public async Task<List<Purchase>> GetPurchaseByUserIdAsync(int userId)
        {
            return await _context.Purchases
                .Include(p => p.Schedule)
                    .ThenInclude(s => s.Pair)
                        .ThenInclude(pair => pair.User1)
                .Include(p => p.Schedule)
                    .ThenInclude(s => s.Pair)
                        .ThenInclude(pair => pair.User2)
                .Include(p => p.Flavor1)
                .Include(p => p.Flavor2)
                .Where(p => p.Schedule.Pair.UserId1 == userId || p.Schedule.Pair.UserId2 == userId)
                .ToListAsync();
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _context.Purchases
                .Include(p => p.Schedule)
                .Include(p => p.Schedule.Pair)
                .Include(p => p.Schedule.Pair.User1)
                .Include(p => p.Schedule.Pair.User2)
                .Include(p => p.Flavor1)
                .Include(p => p.Flavor2)
                .ToListAsync();
        }

        public async Task UpdateAsync(Purchase purchase)
        {
            _context.Purchases.Update(purchase);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Purchase purchase)
        {
            _context.Purchases.Remove(purchase);
            await _context.SaveChangesAsync();
        }
    }
}
