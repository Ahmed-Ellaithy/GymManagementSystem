using GymManagement.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace GymManagementDAL.Data.DataSeed
{
	public static class IdentityDataSeeding
	{
        public static async Task SeedAsync(RoleManager<IdentityRole> roleManager
										,UserManager<ApplicationUser> userManager
										,ILogger logger,CancellationToken ct = default)
        {
            try
			{
				bool HasUsers = await userManager.Users.AnyAsync(ct);
				bool HasRoles = await roleManager.Roles.AnyAsync(ct);

				if (HasUsers && HasRoles) return ;
				if (!HasRoles)
				{
					var Roles = new List<IdentityRole>()
					{
						new IdentityRole(){Name = "SuperAdmin"},
						new IdentityRole(){Name = "Admin"}
					};

					foreach (var role in Roles)
					{
                        if (!await roleManager.RoleExistsAsync(role.Name) )
                        {
                            var roleResult = await roleManager.CreateAsync(role);
							if (!roleResult.Succeeded) 
							{
								logger.LogError($"Failed to add role {role.Name}");
							}

                        }
                    }
				}
				if (!HasUsers)
				{
					var MainAdmin = new ApplicationUser()
					{
						FirstName = "Hossam",
						LastName = "Mostafa",
						UserName = "HossamMostafa",
						Email = "Hossam@gmail.com",
						PhoneNumber = "01122655655"
					};
					await userManager.CreateAsync(MainAdmin, "P@ssw0rd");
					await userManager.AddToRoleAsync(MainAdmin, "SuperAdmin");

					var Admin = new ApplicationUser()
					{
						FirstName = "Ahmed",
						LastName = "Mohamed",
						UserName = "AhmedMohamed",
						Email = "Ahmed@gmail.com",
						PhoneNumber = "01222522622"
					};
					var createResult = await userManager.CreateAsync(Admin, "P@ssw0rd");
					await userManager.AddToRoleAsync(Admin, "Admin");
					logger.LogInformation("Identity Seeded Successfully");
				}
                 
			}
			catch (Exception ex)
			{
				logger.LogError(ex.Message);
				return;
            }
		}

	}
}
