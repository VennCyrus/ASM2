using Microsoft.AspNetCore.Identity;
using FPTBOOK_STORE.Enums;
namespace FPTBOOK_STORE.Areas.Identity.Data
{
    public static class ContextSeed
    {
        public static async Task SeedRolesAsync(UserManager<FPTBOOKUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Roles
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Admin.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.StoreOwner.ToString()));
            await roleManager.CreateAsync(new IdentityRole(Enums.Roles.Customer.ToString()));
        }
        public static async Task SeedAdminAsync(UserManager<FPTBOOKUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            //Seed Default User
            var defaultUser = new FPTBOOKUser
            {
                UserName = "admin@gmail.com",
                Email = "admin@gmail.com",
                Name = "admin",
                Address = "Danang",
                PhoneNumber = "0935150481",
                //DOB = "",
                EmailConfirmed = true,
                PhoneNumberConfirmed = true
            };
            if (userManager.Users.All(u => u.Id != defaultUser.Id))
            {
                var user = await userManager.FindByEmailAsync(defaultUser.Email);
                if (user == null)
                {
                    await userManager.CreateAsync(defaultUser, "123Pa$$word.");
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Admin.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.StoreOwner.ToString());
                    await userManager.AddToRoleAsync(defaultUser, Enums.Roles.Customer.ToString());
                }

            }
        }
    }
}