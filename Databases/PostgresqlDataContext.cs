using Microsoft.EntityFrameworkCore;

namespace EFMultipleMigration.Databases;

public class PostgresqlDataContext(IConfiguration configuration) : DataContext
{
    private readonly IConfiguration _configuration = configuration;

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseNpgsql(_configuration.GetConnectionString("Postgresql"));
    }
}
