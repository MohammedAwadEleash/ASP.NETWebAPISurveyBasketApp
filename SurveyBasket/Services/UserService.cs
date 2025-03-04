using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using SurveyBasket.Contracts.Users;

namespace SurveyBasket.Services
{
    public class UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext context, IRoleService roleService ): IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager = userManager;
        private readonly ApplicationDbContext _context = context;
        private readonly IRoleService _roleService = roleService;

        public async Task<IEnumerable<UserResponse>> GetAllAsync(CancellationToken cancellationToken) =>

            await (from u in _context.Users
                   join ur in _context.UserRoles
                   on u.Id equals ur.UserId
                   join r in _context.Roles
                  on ur.RoleId equals r.Id into roles
                   where !roles.Any(r => r.Name == DefaultRoles.Member)
                   select new
                   {
                       u.Id,
                       u.FirstName,
                       u.LastName,
                       u.Email,
                       u.IsDisabled,
                     Roles=  roles.Select(r => r.Name)
                       }

                   ).GroupBy(u => new { u.Id, u.FirstName, u.LastName, u.Email, u.IsDisabled })

            
            .Select(g => new UserResponse(

                           g.Key.Id,
                           g.Key.FirstName,
                           g.Key.LastName,
                           g.Key.Email,
                           g.Key.IsDisabled,
                            g.SelectMany(g=>g.Roles)

                                      ))
            
            .ToListAsync(cancellationToken);





        public async Task<Result<UserResponse>> GetAsync(string id)

        {

            var user =await  _userManager.FindByIdAsync(id);
            if (user is null)
                return Result.Failure<UserResponse>(UserErrors.UserNotFound);

            var userRoles = await _userManager.GetRolesAsync(user);

            var response = (user, userRoles).Adapt<UserResponse>();

            return Result.Success(response);
                    
                    }





        public async Task<Result<UserResponse>> AddAsync(CreateUserRequest request , CancellationToken  cancellationToken = default)
        {


            var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email, cancellationToken);
            if(emailIsExists)
                return Result.Failure<UserResponse>(UserErrors.DuplicatedEmail);


            //    var allowedRoles = _context.Roles.Select(r => r.Name);

            var allowedRoles = await _roleService.GetAllAsync(cancellationToken:cancellationToken);

            if(request.Roles.Except(allowedRoles.Select(r=>r.Name)).Any())
             return Result.Failure<UserResponse>(UserErrors.InvalidRoles);


            var user = request.Adapt<ApplicationUser>();


            var result = await _userManager.CreateAsync(user, request.Password);


            if(result.Succeeded)
            {
                await _userManager.AddToRolesAsync(user,request.Roles);



                var response = (user, request.Roles).Adapt<UserResponse>();


                return Result.Success(response);





            }



            var error = result.Errors.First();

            return Result.Failure<UserResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }



        public async Task<Result> UpdateAsync(string id ,UpdateUserRequest request, CancellationToken cancellationToken = default)
        {


            var user = await _userManager.FindByIdAsync(id);

            if(user is null)
                return Result.Failure(UserErrors.UserNotFound);


            var emailIsExists = await _userManager.Users.AnyAsync(u => u.Email == request.Email && u.Id !=id, cancellationToken);
            if (emailIsExists)
                return Result.Failure(UserErrors.DuplicatedEmail);


            //    var allowedRoles = _context.Roles.Select(r => r.Name);

            var allowedRoles = await _roleService.GetAllAsync(cancellationToken: cancellationToken);

            if (request.Roles.Except(allowedRoles.Select(r => r.Name)).Any())
                return Result.Failure(UserErrors.InvalidRoles);


            
             user = request.Adapt(user);


            var result = await _userManager.UpdateAsync(user);


            if (result.Succeeded)
            {

             

                await _context.UserRoles.Where(r=>r.UserId ==id)
                    .ExecuteDeleteAsync(cancellationToken);


                await _userManager.AddToRolesAsync(user, request.Roles);


                return Result.Success();





            }


    


            var error = result.Errors.First();

            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));
        }





        public async Task<Result> ToggleStatusAsync(string id)

        {


            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);


             user.IsDisabled = !user.IsDisabled;

            await _userManager.UpdateAsync(user);

            return Result.Success();

          
        }


        public async Task<Result> UnlockAsync(string id)

        {


            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
                return Result.Failure(UserErrors.UserNotFound);


            await _userManager.SetLockoutEndDateAsync(user, null);

            return Result.Success();


        }


        public async Task<Result<UserProfileResponse>> GetProfileAsync(string userId)
           {

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
