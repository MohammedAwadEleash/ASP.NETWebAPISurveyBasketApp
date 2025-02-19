
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Abstractions;
using SurveyBasket.Authentication;
using SurveyBasket.Errors;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser>userManger , IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManger = userManger;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDay = 14;
        public async Task< Result<AuthResponse>> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManger.FindByEmailAsync(email);

            if (user is null)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

            var isValidPassword=await _userManger.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return Result.Failure<AuthResponse>(UserErrors.InvalidCredentials);

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
            await _userManger.UpdateAsync(user);

            var authResponse = new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token,expiresIn,refreshToken, refreshTokenExpration);

            return  Result.Success<AuthResponse>(authResponse);
        }


      
        public async Task<Result<AuthResponse>> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            // 1-  Validate (JWT Token) :
            var userId = _jwtProvider.ValidateToken(token);
            // this userId came with Request (JWT Token)
            // validate this  Token 
            if (userId is null)
           return Result.Failure<AuthResponse>(UserErrors.InvalidJwtToken);

            var user = await   _userManger.FindByIdAsync(userId);

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
            await _userManger.UpdateAsync(user);


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


            var user = await _userManger.FindByIdAsync(userId);

            if (user is null)
                return Result.Failure(UserErrors.InvalidJwtToken);



            // 2-  Validate (RefreshToken) :

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

            if (userRefreshToken is null)
                return Result.Failure(UserErrors.InvalidRefreshToken);

            // make the current Refresh Token InActive /RevokedOn:
            userRefreshToken.RevokedOn = DateTime.UtcNow;

          
            await _userManger.UpdateAsync(user);



            return Result.Success();

        }
        private static string GenerateRefreshToken()
        {

            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }
    }
}
