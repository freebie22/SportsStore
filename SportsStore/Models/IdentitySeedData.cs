using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace SportsStore.Models
{
    public static class IdentitySeedData
    {
        private static string adminUser = "Admin";
        private static string adminPassword = "Secret123$";

        public static async Task EnsurePopulatedIdentity(IApplicationBuilder app)
        {
            var context = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<AppIdentityDbContext>();
            if(context.Database.GetPendingMigrations().Any())
            {
                context.Database.Migrate();
            }
            var userManger = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

            IdentityUser user = await userManger.FindByIdAsync(adminUser);
            if(user == null)
            {
                user = new IdentityUser(adminUser);
                user.Email = "admin@example.com";
                user.PhoneNumber = "555-1234";
                await userManger.CreateAsync(user, adminPassword);
            }
        }

    }
}
