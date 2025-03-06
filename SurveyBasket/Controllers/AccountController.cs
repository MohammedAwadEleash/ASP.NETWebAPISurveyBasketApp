using Microsoft.AspNetCore.RateLimiting;
using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Controllers
{
    [Route("account-info")]
    [ApiController]
    [Authorize]

    [EnableRateLimiting(RateLimiters.Concurrency)]

    public class AccountController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("")]
        public async Task<IActionResult>UserInfo()
        {

            var result = await  _userService.GetProfileAsync(User.GetUserId()!);



            return Ok(result.Value);
             

        }

        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileRequest request)
        {

            var result = await _userService.UpdateProfileAsync( User.GetUserId()!, request);



            return NoContent();
            


        }

        [HttpPut("change-password")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordRequest request)
        {

            var result = await _userService.ChangePasswordAsync(User.GetUserId()!, request);



            return result.IsSuccess ? NoContent() : result.ToProblem();




        }
    }
}
