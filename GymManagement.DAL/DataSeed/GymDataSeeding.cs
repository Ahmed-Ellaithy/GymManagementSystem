using G01.Context;
using G01.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;

public static class GymDataSeeding
{
    public static async Task SeedAsync(GymDbContext dbContext, string seedFilesPath, ILogger logger)
    {
        try
        {
            if (!await dbContext.Plans.AnyAsync())
            {
                var plans = LoadDataFromJsonFile<Plan>(seedFilesPath, "plans.json");
                if (plans.Any())
                {
                    dbContext.Plans.AddRange(plans);
                    logger.LogInformation($"Plans Seeded With Count = {plans.Count}");
                }
            }

            if (dbContext.ChangeTracker.HasChanges())
                await dbContext.SaveChangesAsync();
            else
                logger.LogInformation("No new data to seed.");
        }       
        catch (Exception ex)
        {
            logger.LogError(ex, "Gym data seeding failed.");
            throw;
        }
    }

    private static List<T> LoadDataFromJsonFile<T>(string folderPath, string fileName)
    {

        var filePath = Path.Combine(folderPath, fileName);
        if (!File.Exists(filePath))
            throw new FileNotFoundException($"Seed data file not found: {filePath}");

        var data = File.ReadAllText(filePath);
        var options = new JsonSerializerOptions()
        {
            PropertyNameCaseInsensitive = true,
        };
        //options.Converters.Add(new JsonStringEnumConverter());

        return JsonSerializer.Deserialize<List<T>>(data, options) ?? [];
    }
}