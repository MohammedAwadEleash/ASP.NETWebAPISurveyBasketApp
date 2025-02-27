using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services
{
    public class UserService(UserManager<ApplicationUser>userManager): IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;

        public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
           {

            // _userManager.Users is the same (_context.Users)
            var user = await _userManager.Users.Where(u => u.Id == userId)
                .ProjectToType<UserProfileResponse>().SingleAsync();


            return Result.Success(user);






        }


        public async Task<Result> UpdateProfileAsync(string userId , UpdateProfileRequest request)
        {

            //var user  = await _userManager.FindByIdAsync(userId);


            //user = request.Adapt(user);


            //await _userManager.UpdateAsync(user!);


            await _userManager.Users
                .Where(u => u.Id == userId)
                .ExecuteUpdateAsync(setters =>
                setters

                .SetProperty(u => u.FirstName, request.FirstName)
                .SetProperty(u => u.LastName, request.LastName)

                );


            return Result.Success();





        }


        public async Task<Result>ChangePasswordAsync(string userId, ChangePasswordRequest request)
        {
            var user = await _userManager.FindByIdAsync(userId);

        

           var result =  await _userManager.ChangePasswordAsync(user!, request.CurrentPassword, request.NewPassword);


            if (result.Succeeded)
                return Result.Success();



            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));


        }




    }
}
