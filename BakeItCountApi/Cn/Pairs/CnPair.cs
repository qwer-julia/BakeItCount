using BakeItCountApi.Cn.Users;
using BakeItCountApi.Cn.Schedules;

namespace BakeItCountApi.Cn.Pairs
{
    public class CnPair
    {
        private readonly DaoPair _pairDAO;
        private readonly CnUser _userCN;
        private readonly CnSchedule _cnSchedule;

        public CnPair(DaoPair pairDAO, CnUser userCN, CnSchedule cnSchedule)
        {
            _pairDAO = pairDAO;
            _userCN = userCN;
            _cnSchedule = cnSchedule;
        }

        public async Task<Pair> CreatePairAsync(int userId1, int userId2)
        {
            if (userId1 == userId2)
                throw new InvalidOperationException("Não é possível criar um par com o mesmo usuário.");

            var existingPair = await _pairDAO.GetExistingPairForUsers(userId1, userId2);
            if (existingPair != null)
                throw new InvalidOperationException(
                    $"Um dos usuários já está cadastrado na dupla com '{existingPair.User1.Name}' e '{existingPair.User2.Name}'."
                );

            var lastOrder = await _pairDAO.GetMaxOrderAsync();
            var newOrder = lastOrder + 1;

            var pair = new Pair
            {
                UserId1 = userId1,
                UserId2 = userId2,
                Order = newOrder,
                CreatedAt = DateTime.UtcNow
            };

            var createdPair = await _pairDAO.AddAsync(pair);

            // 🔹 Regenera os schedules sempre que uma nova dupla é criada
            await _cnSchedule.RegenerateSchedulesAsync();

            return createdPair;
        }

        public async Task<Pair> UpdatePairAsync(int pairId, int newUserId1, int newUserId2)
        {
            var pair = await _pairDAO.GetByIdAsync(pairId);
            if (pair == null)
                throw new InvalidOperationException("Par não encontrado.");

            if (newUserId1 == newUserId2)
                throw new InvalidOperationException("Não é possível criar um par com o mesmo usuário.");

            if (!await _userCN.ExistsAsync(newUserId1) || !await _userCN.ExistsAsync(newUserId2))
                throw new InvalidOperationException("Um ou ambos os usuários não existem.");

            var existingPair = await _pairDAO.GetExistingPairForUsers(newUserId1, newUserId2);
            if (existingPair != null && existingPair.PairId != pairId)
                throw new InvalidOperationException(
                    $"Um dos usuários já está cadastrado na dupla com '{existingPair.User1.Name}' e '{existingPair.User2.Name}'."
                );

            pair.UserId1 = newUserId1;
            pair.UserId2 = newUserId2;
            pair.CreatedAt = DateTime.UtcNow;

            await _pairDAO.UpdateAsync(pair);
            return pair;
        }

        public async Task DeletePairAsync(int pairId)
        {
            var pair = await _pairDAO.GetByIdAsync(pairId);
            if (pair == null)
                throw new InvalidOperationException("Par não encontrado.");

            await _pairDAO.DeleteAsync(pair);
        }

        public async Task<List<PairDto>> GetAllPairsAsync()
        {
            return await _pairDAO.GetAllAsync();
        }
        public async Task<List<PairDto>> GetAllPairsWithPurchaseAsync()
        {
            return await _pairDAO.GetAllPairsWithPurchaseAsync();
        }

        public async Task<Pair> GetPairByIdAsync(int pairId)
        {
            var pair = await _pairDAO.GetByIdAsync(pairId);
            if (pair == null)
                throw new InvalidOperationException("Par não encontrado.");

            return pair;
        }


        public async Task<Pair> GetPairByUserId(int userId)
        {
            var pair = await _pairDAO.GetExistingPairForUsers(userId, 0);
            if (pair == null)
                throw new InvalidOperationException("Par não encontrado.");

            return pair;
        }
    }
}
