using G01;
using G01.Context;
using GymManagement.DAL.Models;
using GymManagementDAL.Data.DataSeed;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace GymManagementPL
{
    public static class ProgramExtensions
    {
        public static async Task MigrateAndSeedAsync(this WebApplication app)
        {
            //scope >unmanaged Resource
            using var scope = app.Services.CreateScope();
            //GymDbContext
            var dbContext = scope.ServiceProvider.GetRequiredService<GymDbContext>();

            //Logger
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var config = scope.ServiceProvider.GetRequiredService<IConfiguration>();

            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            // check if there any pending migration!
            var pending = await dbContext.Database.GetPendingMigrationsAsync();
            if (pending.Any())
            {
                logger.LogInformation("Applying {Count} pending migrations...", pending.Count());
                await dbContext.Database.MigrateAsync();
            }

            var seedPath = Path.Combine(app.Environment.ContentRootPath, "wwwroot", "Files");
            await GymDataSeeding.SeedAsync(dbContext, seedPath, logger);
            await IdentityDataSeeding.SeedAsync(roleManager, userManager, logger);
        }
    }
}
