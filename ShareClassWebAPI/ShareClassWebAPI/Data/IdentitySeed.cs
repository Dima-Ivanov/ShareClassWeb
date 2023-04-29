using Microsoft.AspNetCore.Identity;
using ShareClassWebAPI.Entities;

namespace ShareClassWebAPI.Data
{
    public static class IdentitySeed
    {
        public static async Task CreateUserRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            
            if (await roleManager.FindByNameAsync(Constants.adminRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Constants.adminRole));
            }
            if (await roleManager.FindByNameAsync(Constants.userRole) == null)
            {
                await roleManager.CreateAsync(new IdentityRole<int>(Constants.userRole));
            }
            
            if (await userManager.FindByNameAsync(Constants.adminRole) == null)
            {
                User admin = new() { Name = Constants.adminRole, UserName = Constants.adminRole, Login = Constants.adminRole };
                IdentityResult createAdminResult = await userManager.CreateAsync(admin, "qwerty54321");
                if (createAdminResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, Constants.adminRole);
                }
            }
            
            if (await userManager.FindByNameAsync(Constants.userRole) == null)
            {
                User testUser = new() { Name = Constants.userRole, UserName = Constants.userRole, Login = Constants.userRole };
                IdentityResult createUserResult = await userManager.CreateAsync(testUser, Constants.userRole);
                if (createUserResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(testUser, Constants.userRole);
                }
            }
        }
    }
}
