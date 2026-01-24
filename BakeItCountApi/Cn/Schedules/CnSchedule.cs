using BakeItCountApi.Cn.Pairs;

namespace BakeItCountApi.Cn.Schedules
{
    public class CnSchedule
    {
        private readonly DaoSchedule _scheduleDAO;
        private readonly DaoPair _pairDAO;

        public CnSchedule(DaoSchedule scheduleDAO, DaoPair pairDAO)
        {
            _scheduleDAO = scheduleDAO;
            _pairDAO = pairDAO;
        }

        public async Task<Schedule> GetNextSchedule()
        {
            var today = DateTime.Now;
            var rangeEnd = today.AddMonths(1);

            var fridays = GetFridays(today, rangeEnd);

            var nextFriday = fridays.FirstOrDefault(d => d >= today.Date);

            if (nextFriday == default)
                return null;

            var nextSchedule = await _scheduleDAO.GetScheduleByFridayAsync(nextFriday);
            return nextSchedule;
        }

        public async Task<Schedule> GetScheduleById(int scheduleId)
        {
            var schedule = await _scheduleDAO.GetByIdAsync(scheduleId);
            return schedule;
        }

        public async Task<Schedule> ConfirmAsync(int scheduleId)
        {
            var schedule = await _scheduleDAO.ConfirmAsync(scheduleId);
            return schedule;
        }

        public async Task RegenerateSchedulesAsync()
        {
            await _scheduleDAO.DeleteFutureSchedulesAsync();

            var pairs = await _pairDAO.GetAllAsync();
            if (pairs == null || pairs.Count == 0)
                return;

            var start = DateTime.UtcNow.Date;
            var end = start.AddMonths(3);

            var fridays = GetFridays(start, end);

            int index = 0;
            foreach (var friday in fridays)
            {
                var pair = pairs[index % pairs.Count];

                var schedule = new Schedule
                {
                    PairId = pair.PairId,
                    WeekRef = friday, // já UTC
                    Confirmed = false
                };

                await _scheduleDAO.AddAsync(schedule);
                index++;
            }
        }

        private List<DateTime> GetFridays(DateTime start, DateTime end)
        {
            var fridays = new List<DateTime>();
            var current = start;

            while (current.DayOfWeek != DayOfWeek.Friday)
                current = current.AddDays(1);

            while (current <= end)
            {
                fridays.Add(current.Date);
                current = current.AddDays(7);
            }

            return fridays;
        }
        public async Task<List<Schedule>> GetAllSchedulesAsync()
        {
            return await _scheduleDAO.GetAllAsync();
        }

        public async Task<bool> ExistsAsync(int scheduleId)
        {
            return await _scheduleDAO.ExistsAsync(scheduleId);
        }
    }
}
