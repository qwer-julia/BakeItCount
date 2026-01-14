using BakeItCountApi.Cn.Swaps;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BakeItCountApi.Cn.Swaps
{
    public class DaoSwap
    {
        private readonly Context _context;

        public DaoSwap(Context context)
        {
            _context = context;
        }

        public async Task<Swap> AddAsync(Swap swap)
        {
            _context.Swaps.Add(swap);
            await _context.SaveChangesAsync();
            return swap;
        }

        public async Task<Swap?> GetByIdAsync(int swapId)
        {
            return await _context.Swaps
                .Include(s => s.SourceSchedule).ThenInclude(sc => sc.Pair).ThenInclude(p => p.User1)
                .Include(s => s.SourceSchedule).ThenInclude(sc => sc.Pair).ThenInclude(p => p.User2)
                .Include(s => s.TargetSchedule).ThenInclude(tc => tc.Pair).ThenInclude(p => p.User1)
                .Include(s => s.TargetSchedule).ThenInclude(tc => tc.Pair).ThenInclude(p => p.User2)
                .FirstOrDefaultAsync(s => s.SwapId == swapId);
        }
        public async Task<Swap?> GetByScheduleIdAsync(int scheduleId)
        {
            return await _context.Swaps
                .Include(s => s.SourceSchedule).ThenInclude(sc => sc.Pair).ThenInclude(p => p.User1)
                .Include(s => s.SourceSchedule).ThenInclude(sc => sc.Pair).ThenInclude(p => p.User2)
                .Include(s => s.TargetSchedule).ThenInclude(tc => tc.Pair).ThenInclude(p => p.User1)
                .Include(s => s.TargetSchedule).ThenInclude(tc => tc.Pair).ThenInclude(p => p.User2)
                .Where(s => s.SourceScheduleId == scheduleId)
                .OrderByDescending(s => s.SwapId)
                .FirstOrDefaultAsync();
        }

        public async Task<List<Swap>> GetAllAsync()
        {
            return await _context.Swaps
                .Include(s => s.SourceSchedule).Include(s => s.TargetSchedule)
                .ToListAsync();
        }

        public async Task UpdateAsync(Swap swap)
        {
            _context.Swaps.Update(swap);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Swap swap)
        {
            _context.Swaps.Remove(swap);
            await _context.SaveChangesAsync();
        }
    }
}
