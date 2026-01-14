using BakeItCountApi.Cn.Flavors;
using BakeItCountApi.Cn.FlavorVotes;
using BakeItCountApi.Cn.Schedules;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class FlavorController : ControllerBase
    {
        private readonly CnFlavor _cnFlavor;
        private readonly CnFlavorVote _cnFlavorVote;

        public FlavorController(CnFlavor cnFlavor, CnFlavorVote cnFlavorVote)
        {
            _cnFlavor = cnFlavor;
            _cnFlavorVote = cnFlavorVote;
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

        [HttpPost("vote")]
        public async Task<IActionResult> VoteForFavoriteFlavor([FromBody] FlavorVoteDto request)
        {
            bool saved = await _cnFlavorVote.VoteForFavoriteFlavorAsync(request.UserId, request.Flavors);
            return Ok(saved);
        }

        [HttpGet("allWithFlavors")]
        public async Task<IActionResult> GetAllVotes()
        {
            var flavorVotes = await _cnFlavor.GetAllFlavorsWithVotesAsync();
            return Ok(flavorVotes);
        }
    }
}
