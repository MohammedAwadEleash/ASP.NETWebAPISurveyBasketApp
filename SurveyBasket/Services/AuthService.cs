
using Microsoft.AspNetCore.Identity;
using SurveyBasket.Authentication;
using System.Runtime.InteropServices;
using System.Security.Cryptography;

namespace SurveyBasket.Services
{
    public class AuthService(UserManager<ApplicationUser>userManger , IJwtProvider jwtProvider) : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManger = userManger;
        private readonly IJwtProvider _jwtProvider = jwtProvider;

        private readonly int _refreshTokenExpiryDay = 14;
        public async Task<AuthResponse?> GetTokenAsync(string email, string password, CancellationToken cancellationToken = default)
        {
            var user = await _userManger.FindByEmailAsync(email);

            if (user is null)
                return null;

            var isValidPassword=await _userManger.CheckPasswordAsync(user, password);

            if (!isValidPassword)
                return null;

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

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, token,expiresIn,refreshToken, refreshTokenExpration);
        }


      
        public async Task<AuthResponse?> GetRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            // 1-  Validate (JWT Token) :
            var userId = _jwtProvider.ValidateToken(token);
            // this userId came with Request 
            // validate this  Token 
            if (userId is null)
                return null;


            var user = await   _userManger.FindByIdAsync(userId);

            if (user is null)
                return null;



            // 2-  Validate (RefreshToken) :

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken&&r.IsActive);

            if (userRefreshToken is null)
                return null;

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

            return new AuthResponse(user.Id, user.Email, user.FirstName, user.LastName, newToken, expiresIn, newRefreshToken, refreshTokenExpration);



        }

        

        public async  Task<bool> RevokeRefreshTokenAsync(string token, string refreshToken, CancellationToken cancellationToken = default)
        {

            // 1-  Validate (JWT Token) :
            var userId = _jwtProvider.ValidateToken(token);
            // this userId came with Request 
            // validate this  Token 

            if (userId is null)
                return false;


            var user = await _userManger.FindByIdAsync(userId);

            if (user is null)
                return false;



            // 2-  Validate (RefreshToken) :

            var userRefreshToken = user.RefreshTokens.SingleOrDefault(r => r.Token == refreshToken && r.IsActive);

            if (userRefreshToken is null)
                return false;

            // make the current Refresh Token InActive /RevokedOn:
            userRefreshToken.RevokedOn = DateTime.UtcNow;

          
            await _userManger.UpdateAsync(user);

            return true;


        }
        private static string GenerateRefreshToken()
        {

            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

        }
    }
}
