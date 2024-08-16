namespace Customer.API.Extensions;

public static class InitDataExtensions
{
    public static void SeedData(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        var services = scope.ServiceProvider;
        services.SeedRolesAndAdminAsync(new[] { "Admin", "User", "Manager" }, "admin@example.com", "Admin@123").GetAwaiter().GetResult();
    }

    private static async Task SeedRolesAndAdminAsync(this IServiceProvider serviceProvider, string[] roleNames, string adminEmail, string adminPassword)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<User>>();

        IdentityResult roleResult;

        foreach (var roleName in roleNames)
        {
            var roleExist = await roleManager.RoleExistsAsync(roleName);
            if (!roleExist)
            {
                roleResult = await roleManager.CreateAsync(new IdentityRole(roleName));
            }
        }

        var adminUser = new User
        {
            UserName = "admin",
            Email = adminEmail
        };

        var user = await userManager.FindByEmailAsync(adminUser.Email);

        if (user == null)
        {
            var createAdminUser = await userManager.CreateAsync(adminUser, adminPassword);
            if (createAdminUser.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
        }
    }
}
