using BakeItCountApi.Dao.Swaps;
using BakeItCountApi.Dao.Schedules;

namespace BakeItCountApi.Cn.Swaps
{
    public class CnSwap
    {
        private readonly DaoSwap _swapDAO;
        private readonly DaoSchedule _scheduleDAO;

        public CnSwap(DaoSwap swapDAO, DaoSchedule scheduleDAO)
        {
            _swapDAO = swapDAO;
            _scheduleDAO = scheduleDAO;
        }

        public async Task<Swap> CreateSwapAsync(int sourceScheduleId, int targetScheduleId, int requestingUserId)
        {
            var source = await _scheduleDAO.GetByIdAsync(sourceScheduleId);
            var target = await _scheduleDAO.GetByIdAsync(targetScheduleId);

            if (source == null || target == null)
                throw new InvalidOperationException("Agendamento inválido.");

            if (source.Pair.UserId1 != requestingUserId && source.Pair.UserId2 != requestingUserId)
                throw new UnauthorizedAccessException("Somente usuários do par responsável podem solicitar um swap.");

            var swap = new Swap
            {
                SourceScheduleId = sourceScheduleId,
                TargetScheduleId = targetScheduleId,
                Status = SwapStatus.PENDING,
                RequestedAt = DateTime.UtcNow
            };

            return await _swapDAO.AddAsync(swap);
        }

        public async Task<Swap> RespondSwapAsync(int swapId, int respondingUserId, bool approve)
        {
            var swap = await _swapDAO.GetByIdAsync(swapId);
            if (swap == null) throw new InvalidOperationException("Swap não encontrado.");

            var target = swap.TargetSchedule;

            if (target.Pair.UserId1 != respondingUserId && target.Pair.UserId2 != respondingUserId)
                throw new UnauthorizedAccessException("Somente usuários da dupla alvo podem responder ao swap.");

            if (swap.Status != SwapStatus.PENDING)
                throw new InvalidOperationException("Este swap já foi respondido.");

            if (approve)
            {
                swap.Status = SwapStatus.APPROVED;

                var oldSourcePair = swap.SourceSchedule.PairId;
                swap.SourceSchedule.PairId = swap.TargetSchedule.PairId;
                swap.TargetSchedule.PairId = oldSourcePair;
            }
            else
            {
                swap.Status = SwapStatus.REJECTED;
            }

            await _swapDAO.UpdateAsync(swap);
            return swap;
        }

        public async Task<List<Swap>> GetAllAsync() => await _swapDAO.GetAllAsync();

        public async Task<Swap> GetByIdAsync(int swapId)
        {
            var swap = await _swapDAO.GetByIdAsync(swapId);
            if (swap == null) throw new InvalidOperationException("Swap não encontrado.");
            return swap;
        }
        public async Task<Swap> GetByScheduleIdAsync(int scheduleId)
        {
            var swap = await _swapDAO.GetByScheduleIdAsync(scheduleId);
            return swap;
        }

        public async Task DeleteAsync(int swapId)
        {
            var swap = await _swapDAO.GetByIdAsync(swapId);
            if (swap == null) throw new InvalidOperationException("Swap não encontrado.");
            await _swapDAO.DeleteAsync(swap);
        }
    }
}
