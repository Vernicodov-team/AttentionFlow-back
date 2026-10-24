using Application.DTO;
using Application.Service;

namespace API.Modules;

public static class UserSeeder
{
    public static async Task SeedUsersAsync(this WebApplication app)
    {
        UserSeedOptions? options = app.Configuration
            .GetSection(UserSeedOptions.SectionName)
            .Get<UserSeedOptions>();

        if (options is null || options.Users.Count == 0)
        {
            app.Logger.LogInformation("Seed users: section '{Section}' is empty, nothing to create",
                UserSeedOptions.SectionName);
            return;
        }

        using IServiceScope scope = app.Services.CreateScope();
        IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();

        foreach (UserSeedEntry entry in options.Users)
        {
            if (string.IsNullOrWhiteSpace(entry.Name) || string.IsNullOrWhiteSpace(entry.Password))
            {
                app.Logger.LogWarning("Seed users: entry skipped, name or password is empty");
                continue;
            }

            try
            {
                await userService.CreateUserAsync(new CreateUserDto
                {
                    Name = entry.Name,
                    Password = entry.Password,
                    Role = entry.Role
                });

                app.Logger.LogInformation("Seed users: user '{Name}' created", entry.Name);
            }
            catch (Exception ex) when (ex.Message == "UserAlreadyExists")
            {
                app.Logger.LogInformation("Seed users: user '{Name}' already exists, skipped", entry.Name);
            }
            catch (Exception ex)
            {
                app.Logger.LogError("Seed users: user '{Name}' was not created: {Reason}", entry.Name, ex.Message);
            }
        }
    }
}
