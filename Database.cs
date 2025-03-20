using InfraredConfigurator.Entities;
using Microsoft.EntityFrameworkCore;

namespace InfraredConfigurator;

public class DatabaseContext : DbContext
{
    public DatabaseContext(DbContextOptions<DatabaseContext> options)
        : base(options) { }

    public DbSet<Domain> Domains { get; set; } = null!;
    public DbSet<Server> Servers { get; set; } = null!;
    public DbSet<ProxyConfig> ProxyConfigs { get; set; } = null!;
}
