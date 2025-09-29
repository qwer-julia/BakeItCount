using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;
using System;

namespace BakeItCountApi.Dao.Schedules
{
    public class DaoSchedule
    {
        private readonly Context _context;

        public DaoSchedule(Context context)
        {
            _context = context;
        }

        public async Task<List<Schedule>> GetAllAsync()
        {
            return await _context.Schedules
                .Include(s => s.Pair)
                .ToListAsync();
        }

        public async Task<Schedule> GetScheduleByFridayAsync(DateTime nextFriday)
        {
            var start = DateTime.SpecifyKind(nextFriday.Date, DateTimeKind.Utc);
            var end = start.AddDays(1);

            try
            {
                var nextSchedule = await _context.Schedules
                    .Include(s => s.Pair)
                        .ThenInclude(p => p.User1)
                    .Include(s => s.Pair)
                        .ThenInclude(p => p.User2)
                    .Where(s => s.WeekRef >= start && s.WeekRef < end)
                    .FirstOrDefaultAsync();

                return nextSchedule;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }

        }

        public async Task AddAsync(Schedule schedule)
        {
            _context.Schedules.Add(schedule);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteFutureSchedulesAsync()
        {
            var now = DateTime.UtcNow.Date;
            var futureSchedules = await _context.Schedules
                .Where(s => s.WeekRef >= now)
                .ToListAsync();

            _context.Schedules.RemoveRange(futureSchedules);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int scheduleId)
        {
            return await _context.Schedules.AnyAsync(s => s.ScheduleId == scheduleId);
        }

        public async Task<Schedule> GetByIdAsync(int scheduleId)
        {
            return await _context.Schedules.Include(s => s.Pair)
                .Include(s => s.Pair.User1)
                .Include(s => s.Pair.User2)
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);
        }

        public async Task<Schedule> ConfirmAsync(int scheduleId)
        {
            var schedule = await _context.Schedules
                .FirstOrDefaultAsync(s => s.ScheduleId == scheduleId);

            if (schedule == null)
                throw new InvalidOperationException($"Schedule {scheduleId} não encontrado.");

            schedule.Confirmed = true;
            _context.Schedules.Update(schedule);

            await _context.SaveChangesAsync();

            return schedule;
        }
    }
}
