using BakeItCountApi.Cn.Users;
using BakeItCountApi.Data;
using Microsoft.EntityFrameworkCore;

namespace BakeItCountApi.Cn.Flavors
{
    public class DaoFlavor
    {
        private readonly Context _context;

        public DaoFlavor(Context context)
        {
            _context = context;
        }
        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Flavors.AnyAsync(f => f.FlavorId == id); // Para Flavor
        }

        public async Task<List<Flavor>> GetAllFlavorsAsync()
        {
            return await _context.Flavors.ToListAsync();
        }

        public async Task<Flavor?> GetMostPurchasedFlavorByUserAsync(int userId)
        {

            try
            {
                var flavorCounts = await _context.Purchases
                  .Where(p => p.Schedule.Pair.UserId1 == userId || p.Schedule.Pair.UserId2 == userId).ToListAsync();
                var favoriteFlavor = flavorCounts.SelectMany(p => new[] { p.Flavor1Id, p.Flavor2Id }).GroupBy(fId => fId).Select(g => new { FlavorId = g.Key, Count = g.Count() }).OrderByDescending(g => g.Count).FirstOrDefault();

                if (favoriteFlavor == null) return null;

                return await _context.Flavors
                    .FirstOrDefaultAsync(f => f.FlavorId == favoriteFlavor.FlavorId);
            }
            catch (Exception ex)
            {
                throw ex.InnerException;
            }
        }
    }
}
