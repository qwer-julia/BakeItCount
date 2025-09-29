using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlavorController : ControllerBase
    {
        private readonly CnFlavor _cnFlavor;

        public FlavorController(CnFlavor cnFlavor)
        {
            _cnFlavor = cnFlavor;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllFlavors()
        {
            var flavors = await _cnFlavor.GetAllFlavorsAsync();
            return Ok(flavors);
        }

        [HttpGet("favoriteFlavor/{userId}")]
        public async Task<IActionResult> GetMostPurchasedFlavorByUser(int userId)
        {
            var flavor = await _cnFlavor.GetMostPurchasedFlavorByUserAsync(userId);
            if (flavor == null)
            {
                flavor = new Flavor { Name = "Nenhum sabor foi comprado ainda :(" };
            }
            return Ok(flavor);
        }
    }
}
