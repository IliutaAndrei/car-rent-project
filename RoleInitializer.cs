using Microsoft.AspNetCore.Identity;

public static class RoleInitializer
{
    public static async Task SeedRolesAsync(IServiceProvider serviceProvider)
    {
        using (var scope = serviceProvider.CreateScope())
        {
            var services = scope.ServiceProvider;
            var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = services.GetRequiredService<UserManager<IdentityUser>>();
            var logger = services.GetRequiredService<ILogger<Program>>();

            // 1. Creare rol Admin
            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                var result = await roleManager.CreateAsync(new IdentityRole("Admin"));
                if (!result.Succeeded)
                {
                    logger.LogError("Eroare la crearea rolului Admin: {Errors}", string.Join(", ", result.Errors));
                    return;
                }
            }

            // 2. Creare user admin
            string adminEmail = "admin@example.com";
            string adminPassword = "Admin123!";

            var adminUser = await userManager.FindByEmailAsync(adminEmail);

            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createUser = await userManager.CreateAsync(adminUser, adminPassword);

                if (!createUser.Succeeded)
                {
                    logger.LogError("Eroare la crearea userului admin: {Errors}", string.Join(", ", createUser.Errors));
                    return;
                }
            }

            // 3. Adauga userul la rol
            if (!await userManager.IsInRoleAsync(adminUser, "Admin"))
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }

            logger.LogInformation("Admin user and role seeded successfully.");
        }
    }
}
