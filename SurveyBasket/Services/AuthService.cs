
using Hangfire;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.WebUtilities;
using SurveyBasket.Abstractions;
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using SurveyBasket.Helpers;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        IJwtProvider jwtProvider, ILogger<AuthService> logger, 
        IEmailSender emailSender, IHttpContextAccessor httpContextAccessor) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly SignInManager<ApplicationUser> _signInManager = signInManager;
        private readonly IJwtProvider _jwtProvider = jwtProvider;
        private readonly ILogger<AuthService> _logger = logger;
        private readonly IEmailSender _emailSender = emailSender;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;


        private readonly int _refreshTokenExpiryDay = 14;
        public async Task< Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            
            
            
            var user = await _userManager.FindByEmailAsync(email);

            if (user is null)

                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);



            //var isValidPassword=await _userManager.CheckPasswordAsync(user, password);

            //if (!isValidPassword)
            //    return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);


            //PasswordSignInAsync is  also  check of the confirmation email 

            var result = await  _signInManager.PasswordSignInAsync(user, password, false, false);

            if (result.Succeeded)
            {

                // Generate Jwt Token:
                var (token, expiresIn) = _jwtProvider.GenerateToken(user);


                // Generate RefreshToken:

                var refreshToken = GenerateRefreshToken();
                var refreshTokenExpration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);

                user.RefreshTokens.Add(new RefreshToken
                {
                    Token = refreshToken,
                    ExpiresOn = refreshTokenExpration

                });
                await _userManager.UpdateAsync(user);

                var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token, expiresIn, refreshToken, refreshTokenExpration);

                return Result.Success<AuthResponse>(authResponse);
            }

            return Result.Failure<AuthResponse>(result.IsNotAllowed ? UserErrors.EmailNotConfirmed : UserErrors.InvalidCredentials);

   
        }


      
        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            // 1-  Validate (JWT Token) :
            var userId = _jwtProvider.ValidateToken(token);
            // this userId came with Request (JWT Token)
            // validate this  Token 
            if (userId is null)
           return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await   _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);



            // 2-  Validate (RefreshToken) :

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

            if (userRefreshToken is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidRefreshToken);

            // make the current Refresh Token InActive /RevokedOn:
            userRefreshToken.RevokedOn = DateTime.UtcNow;

            // Generate new (Jwt Token):
            var (newToken, expiresIn) = _jwtProvider.GenerateToken(user);


            // Generate RefreshToken:

            var newRefreshToken = GenerateRefreshToken();
            var refreshTokenExpration = DateTime.UtcNow.AddDays(_refreshTokenExpiryDay);

            user.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshToken,
                ExpiresOn = refreshTokenExpration

            });
            await _userManager.UpdateAsync(user);


            var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpration);
            return Result.Success(authResponse);




        }

        

        public async  Task<Result> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            // 1-  Validate (JWT Token) :
            var userId = _jwtProvider.ValidateToken(token);
            // this userId came with Request 
            // validate this  Token 

            if (userId is null)
                return Result.Failure(UserErrors.InvalidJwtToken);


            var user = await _userManager.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);



            // 2-  Validate (RefreshToken) :

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            // make the current Refresh Token InActive /RevokedOn:
            userRefreshToken.RevokedOn = DateTime.UtcNow;

          
            await _userManager.UpdateAsync(user);



            return Result.Success();

        }
        public async Task<Result> RegisterAsync(RegisterRequest request , CancellationToken cancellationToken = default)
        {

            var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email);


            if (emailIsExists)
        return Result.Failure(UserErrors.DuplicatedEmail);


            var user = request.Adapt<ApplicationUser>();


            var result =  await _userManager.CreateAsync(user, request.Password);

            if(result.Succeeded)
            {

                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

                _logger.LogInformation("Confirmation code : {code}", code );



                await SendConfirmationEmail(user, code);


                return Result.Success();
            }


            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));






        }

        public async Task<Result> ConfirmEmailAsync(ConfirmEmailRequest request)
        { 
            var user = await _userManager.FindByIdAsync(request.UserId);

            if (user is null)
         return Result.Failure(UserErrors.InvalidCode);


            if(user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);


            var code = request.Code;

            try
            {
                code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code))
;

            }
            catch (FormatException)
            {

                return Result.Failure(UserErrors.InvalidCode);

            }
            var result = await _userManager.ConfirmEmailAsync(user, code);




            if (result.Succeeded)
              return Result.Success();
            

            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));






        }

        public async Task<Result> ResendConfirmationEmailAsync (ResendConfirmationEmailRequest request)
        {


            var user = await _userManager.FindByEmailAsync(request.Email);

            if (user is null)
                return Result.Success();


            if (user.EmailConfirmed)
                return Result.Failure(UserErrors.DuplicatedConfirmation);




            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

            _logger.LogInformation("Confirmation code: {code}", code);





            await SendConfirmationEmail(user, code);

            return Result.Success();
        }


        private static string GenerateRefreshToken()
        {

            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }


        private async  Task SendConfirmationEmail(ApplicationUser user , string code )
        {
            var origin = _httpContextAccessor.HttpContext?.Request.Headers.Origin;

            //  Note->>>>   origin like : (https://localhost:7043) or (https://SurveyBasket.com)

            var emailBody = EmailBodyBuilder.GenerateEmailBody("EmailConfirmation",
                templateModel: new Dictionary<string, string>
                {
                        { "{{name}}" ,user.FirstName  },
                        {  "{{action_url}}" ,$"{origin}/auth/emailConfirmation?userId={user.Id}&code={code}"   }

                }

                );


            BackgroundJob.Enqueue(() => _emailSender.SendEmailAsync(user.Email!, "✅ Survey Basket: Email Confirmation", emailBody));

           await   Task.CompletedTask;
        }
    }
}
