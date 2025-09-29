using Microsoft.AspNetCore.Mvc;
using BakeItCountApi.Cn.Purchases;
using BakeItCountApi.Cn.Schedules;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class PurchaseController : ControllerBase
    {
        private readonly CnPurchase _purchaseCN;

        public PurchaseController(CnPurchase purchaseCN)
        {
            _purchaseCN = purchaseCN;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreatePurchase([FromBody] PurchaseRequest request)
        {
            try
            {
                var purchase = await _purchaseCN.CreatePurchaseAsync(
                    request.ScheduleId,
                    request.Flavor1Id,
                    request.Flavor2Id,
                    request.PurchaseDate,
                    request.Notes
                );

                return Ok(purchase);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            var purchaseList = await _purchaseCN.GetAllAsync();
            return Ok(purchaseList);
        }

        [HttpGet("{purchaseId}")]
        public async Task<IActionResult> GetById(int purchaseId)
        {
            var purchase = await _purchaseCN.GetByIdAsync(purchaseId);
            return Ok(purchase);
        }

        [HttpGet("getPurchaseByScheduleId/{scheduleId}")]
        public async Task<IActionResult> GetPurchaseByScheduleId(int scheduleId)
        {
            return Ok(await _purchaseCN.GetPurchaseByScheduleId(scheduleId));
        }

        [HttpGet("getPurchaseByUserId/{userId}")]
        public async Task<IActionResult> GetPurchaseByUserId(int userId)
        {
            return Ok(await _purchaseCN.GetPurchaseByUserIdAsync(userId));
        }
    }

    public class PurchaseRequest
    {
        public int ScheduleId { get; set; }
        public int Flavor1Id { get; set; }
        public int Flavor2Id { get; set; }
        public DateTime PurchaseDate { get; set; }
        public string Notes { get; set; } = "";
    }
}
