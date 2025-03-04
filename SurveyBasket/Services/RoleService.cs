using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Query;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Org.BouncyCastle.Asn1.Pkcs;
using SurveyBasket.Contracts.Roles;
using System.Security.Claims;
using System.Threading;

namespace SurveyBasket.Services
{
    public class RoleService(RoleManager<ApplicationRole> roleManager, ApplicationDbContext context) : IRoleService
    {
        private readonly RoleManager<ApplicationRole> _roleManager = roleManager;
        private readonly ApplicationDbContext _context = context;

        public async Task<IEnumerable<RoleResponse>> GetAllAsync(bool includeDisabled=false, CancellationToken cancellationToken=default) =>
            await _roleManager.Roles.Where(r => !r.IsDefault && (!r.IsDeleted || includeDisabled )) 
            .ProjectToType<RoleResponse>().ToListAsync(cancellationToken);




        public async Task<Result<RoleDetailResponse>> GetAsync(string id)
        {

            var role = await _roleManager.FindByIdAsync(id);

            if (role is null)
                return Result.Failure<RoleDetailResponse>(RoleErrors.RoleNotFound);




            var permissions = await _roleManager.GetClaimsAsync(role);

            var response = new RoleDetailResponse(role.Id, role.Name!, role.IsDeleted, permissions.Select(r => r.Value));

            return Result.Success(response);




    }

        public async Task<Result<RoleDetailResponse>>AddAsync(RoleRequest request, CancellationToken cancellationToken=default)
        {

            var roleIsExists = await _roleManager.RoleExistsAsync(request.Name);

            if (roleIsExists)
                return Result.Failure<RoleDetailResponse>(RoleErrors.DuplicatedRole);

            var allowedPermissions = Permissions.GetAllPermissions();

            if(request.Permissions.Except(allowedPermissions).Any())
                return Result.Failure<RoleDetailResponse>(RoleErrors.InvalidPermissions);




            var role = new ApplicationRole
            {

                Name = request.Name,
                ConcurrencyStamp = Guid.NewGuid().ToString()
            };

            var result = await _roleManager.CreateAsync(role);

            if(result.Succeeded)
            {

                var permissions = request.Permissions.Select(p => new IdentityRoleClaim<string>
                {

                    ClaimType = Permissions.Type,
                    ClaimValue = p,
                    RoleId = role.Id

                });
                await _context.AddRangeAsync(permissions, cancellationToken);

                await _context.SaveChangesAsync(cancellationToken);

                var response = new RoleDetailResponse(role.Id, role.Name, role.IsDeleted,request.Permissions);

                return Result.Success(response);
            }

            var error = result.Errors.First();


            return Result.Failure<RoleDetailResponse>(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }


        public async Task<Result> UpdateAsync(string id ,RoleRequest request, CancellationToken cancellationToken = default)
        {


            var role =await  _roleManager.FindByIdAsync(id);

            if(role is null )
          return Result.Failure(RoleErrors.RoleNotFound);



            var roleIsExists = await _roleManager.Roles.AnyAsync(r => r.Name == request.Name && r.Id != id);

            if (roleIsExists)
                return Result.Failure(RoleErrors.DuplicatedRole);

            var allowedPermissions = Permissions.GetAllPermissions();

            if (request.Permissions.Except(allowedPermissions).Any())
                return Result.Failure(RoleErrors.InvalidPermissions);





            role.Name = request.Name;

            var result = await _roleManager.UpdateAsync(role);

            if (result.Succeeded)
            {


                var currentpermissions = await _context.RoleClaims
               .Where(rc => rc.RoleId == id && rc.ClaimType == Permissions.Type)
               .Select(r => r.ClaimValue).ToListAsync(cancellationToken);




                var newPermissions = request.Permissions.Except(currentpermissions)
                    .Select(p=> new IdentityRoleClaim<string>
                {

                    RoleId = role.Id,
                    ClaimType = Permissions.Type,
                    ClaimValue = p

                });


                var removedPermissions = currentpermissions.Except(request.Permissions);



                await _context.RoleClaims
                    .Where(rc => rc.RoleId == id && removedPermissions.Contains(rc.ClaimValue))
                    .ExecuteDeleteAsync();


              await   _context.RoleClaims.AddRangeAsync(newPermissions, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);


                return Result.Success();



            }

            var error = result.Errors.First();


            return Result.Failure(new Error(error.Code, error.Description, StatusCodes.Status400BadRequest));

        }


        public async Task<Result> ToggleStatusAsync(string id, CancellationToken cancellationToken = default)
        {

            var role  = await _roleManager.FindByIdAsync(id);

            if(role is null)
                return Result.Failure(RoleErrors.RoleNotFound);



            role.IsDeleted = !role.IsDeleted;

            await _roleManager.UpdateAsync(role);

            return Result.Success();


            //other way : 
            //await _context.SaveChangesAsync(cancellationToken);

            //return Result.Success();
        }
    }

}
