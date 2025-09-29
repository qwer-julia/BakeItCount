using BakeItCountApi.Cn.Pairs;
using BakeItCountApi.Cn.Users;
using Microsoft.AspNetCore.Mvc;

namespace BakeItCountApi.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly CnUser _userCN;

        public UserController(CnUser userCN)
        {
            _userCN = userCN;
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUser(int userId)
        {
            try
            {
                var user = await _userCN.GetUserById(userId);
                return Ok(user);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var userList = await _userCN.GetAllAsync();
                return Ok(userList);
            }
            catch (InvalidOperationException ex)
            {
                return NotFound(new { error = ex.Message });
            }
        }
    }
}
