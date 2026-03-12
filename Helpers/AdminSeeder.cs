using Hei_Hei_Api.Data;
using Hei_Hei_Api.Enums;
using Hei_Hei_Api.Models;
using Hei_Hei_Api.Services.Infrastructure.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace Hei_Hei_Api.Helpers;

public static class AdminSeeder
{
    public static async Task SeedAsync(IServiceProvider serviceProvider)
    {
        using var scope = serviceProvider.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        bool adminExists = await context.Users.AnyAsync(u => u.Role == USER_ROLE.Admin);
        if (adminExists) return;

        var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
        var passwordService = scope.ServiceProvider.GetRequiredService<IPasswordService>();

        string email = configuration["Admin:Email"] ?? throw new InvalidOperationException("Admin email is not configured.");
        string userName = configuration["Admin:UserName"] ?? throw new InvalidOperationException("Admin username is not configured.");
        string fullName = configuration["Admin:FullName"] ?? throw new InvalidOperationException("Admin full name is not configured.");
        string phoneNumber = configuration["Admin:PhoneNumber"] ?? throw new InvalidOperationException("Admin phone number is not configured.");
        string homeAddress = configuration["Admin:HomeAddress"] ?? throw new InvalidOperationException("Admin home address is not configured.");

        string rawPassword = configuration["Admin:Password"] ?? throw new InvalidOperationException("Admin password is not configured.");
        string passwordHash = passwordService.HashPassword(rawPassword);

        var admin = new User
        {
            Email = email,
            UserName = userName,
            PasswordHash = passwordHash,
            FullName = fullName,
            PhoneNumber = phoneNumber,
            HomeAddress = homeAddress,
            Role = USER_ROLE.Admin,
            EmailConfirmed = true,
            CreatedAt = DateTime.UtcNow
        };

        context.Users.Add(admin);

        await context.SaveChangesAsync();
    }
}
