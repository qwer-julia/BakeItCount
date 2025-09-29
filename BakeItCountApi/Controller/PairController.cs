using Microsoft.AspNetCore.Mvc;
using BakeItCountApi.Cn.Pairs;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PairController : ControllerBase
    {
        private readonly CnPair _pairCN;

        public PairController(CnPair pairCN)
        {
            _pairCN = pairCN;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePair([FromBody] PairRequest request)
        {
            try
            {
                var pair = await _pairCN.CreatePairAsync(request.UserId1, request.UserId2);
                return Ok(pair);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPut("update/{pairId}")]
        public async Task<IActionResult> UpdatePair(int pairId, [FromBody] PairRequest request)
        {
            try
            {
                var pair = await _pairCN.UpdatePairAsync(pairId, request.UserId1, request.UserId2);
                return Ok(pair);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpDelete("delete/{pairId}")]
        public async Task<IActionResult> DeletePair(int pairId)
        {
            try
            {
                await _pairCN.DeletePairAsync(pairId);
                return Ok(new { message = "Par excluído com sucesso." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("{pairId}")]
        public async Task<IActionResult> GetPair(int pairId)
        {
            try
            {
                var pair = await _pairCN.GetPairByIdAsync(pairId);
                return Ok(pair);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("getPairByUserId/{userId}")]
        public async Task<IActionResult> GetPairByUserId(int userId)
        {
            try
            {
                var pair = await _pairCN.GetPairByUserId(userId);
                return Ok(pair);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllPairs()
        {
            var pairs = await _pairCN.GetAllPairsAsync();
            return Ok(pairs);
        }

        [HttpGet("GetAllPairsWithPurchase")]
        public async Task<IActionResult> GetAllPairsWithPurchase()
        {
            var pairs = await _pairCN.GetAllPairsWithPurchaseAsync();
            return Ok(pairs);
        }
    }

    public class PairRequest
    {
        public int UserId1 { get; set; }
        public int UserId2 { get; set; }
    }
}
