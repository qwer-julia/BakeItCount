using BakeItCountApi.Cn.Schedules;
using BakeItCountApi.Cn.Flavors;

namespace BakeItCountApi.Cn.Purchases
{
    public class CnPurchase
    {
        private readonly DaoPurchase _purchaseDAO;
        private readonly CnSchedule _scheduleCN;
        private readonly CnFlavor _flavorCN;

        public CnPurchase(DaoPurchase purchaseDAO, CnSchedule scheduleCN, CnFlavor flavorCN)
        {
            _purchaseDAO = purchaseDAO;
            _scheduleCN = scheduleCN;
            _flavorCN = flavorCN;
        }

        public async Task<Purchase> CreatePurchaseAsync(int scheduleId, int flavor1Id, int flavor2Id, DateTime purchaseDate, string notes = "")
        {
            if (!await _scheduleCN.ExistsAsync(scheduleId))
                throw new InvalidOperationException("Agendamento não encontrado.");

            if (!await _flavorCN.ExistsAsync(flavor1Id))
                throw new InvalidOperationException("Sabor não encontrado.");

            if (!await _flavorCN.ExistsAsync(flavor2Id))
                throw new InvalidOperationException("Sabor não encontrado.");

            var purchase = new Purchase
            {
                ScheduleId = scheduleId,
                Flavor1Id = flavor1Id,
                Flavor2Id = flavor2Id,
                PurchaseDate = purchaseDate.ToUniversalTime(),
                Notes = notes
            };

            return await _purchaseDAO.AddAsync(purchase);
        }

        public async Task<Purchase> GetByIdAsync(int purchaseId)
        {
            return await _purchaseDAO.GetByIdAsync(purchaseId);
        }

        public async Task<List<Purchase>> GetAllAsync()
        {
            return await _purchaseDAO.GetAllAsync();
        }
        public async Task<int> GetPurchaseByScheduleId(int scheduleId)
        {
            return await _purchaseDAO.GetPurchaseByScheduleId(scheduleId);
        }

        public async Task<List<Purchase>> GetPurchaseByUserIdAsync(int userId)
        {
            return await _purchaseDAO.GetPurchaseByUserIdAsync(userId);
        }


    }
}
