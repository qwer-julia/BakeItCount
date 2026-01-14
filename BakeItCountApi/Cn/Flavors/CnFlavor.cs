
namespace BakeItCountApi.Cn.Flavors
{
    public class CnFlavor
    {
        private readonly DaoFlavor _flavorDAO;

        public CnFlavor(DaoFlavor flavorDAO)
        {
            _flavorDAO = flavorDAO;
        }

        public async Task<bool> ExistsAsync(int flavorId)
        {
            return await _flavorDAO.ExistsAsync(flavorId);
        }

        public async Task<List<Flavor>> GetAllFlavorsAsync()
        {
            return await _flavorDAO.GetAllFlavorsAsync();
        }
        public async Task<List<Flavor>> GetAllFlavorsWithVotesAsync()
        {
            return await _flavorDAO.GetAllFlavorsWithVotesAsync();
        }


        public async Task<Flavor> GetMostPurchasedFlavorByUserAsync(int userId)
        {
            return await _flavorDAO.GetMostPurchasedFlavorByUserAsync(userId);
        }
    }
}
