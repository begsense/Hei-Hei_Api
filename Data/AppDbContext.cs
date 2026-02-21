using Microsoft.EntityFrameworkCore;
using Hei_Hei_Api.Models;

namespace Hei_Hei_Api.Data;

public class AppDbContext : DbContext
{
    public DbSet<User> Users { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
}
