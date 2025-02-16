
using Microsoft.Extensions.Options;
using SurveyBasket.Authentication;

namespace SurveyBasket.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService,IOptions<JwtOptions> jwtOptions) : ControllerBase
    {
        private readonly IAuthService _authService = authService;
        private readonly JwtOptions _jwtOptions = jwtOptions.Value;

        [HttpPost("")]

        public async Task<IActionResult>LoginAsync([FromBody] LoginRequest request ,CancellationToken cancellationToken)
        {

            var authResult= await _authService.GetTokenAsync(request.Email, request.Password, cancellationToken);


            if (authResult is null)
                return BadRequest("Invalid email/password");

            return Ok (authResult);
        }
        [HttpPost("refresh")]

        public async Task<IActionResult> RefreshAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var authResult = await _authService.GetRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);


            if (authResult is null)
                return BadRequest("Invalid token");

            return Ok(authResult);
        }

        [HttpPost("revoke-refresh-token")]

        public async Task<IActionResult> RevokeRefreshTokenAsync([FromBody] RefreshTokenRequest request, CancellationToken cancellationToken)
        {

            var isRevoked = await _authService.RevokeRefreshTokenAsync(request.Token, request.RefreshToken, cancellationToken);

            if (!isRevoked)
                return BadRequest("Operation failed");
            
            return Ok(isRevoked);
        }
    }
}
