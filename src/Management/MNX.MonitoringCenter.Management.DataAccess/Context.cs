using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using System.Reflection;

namespace MNX.MonitoringCenter.Management.DataAccess;

/// <summary>
/// Контекст БД.
/// </summary>
public class Context : DbContext
{
    public DbSet<Core.Cryptocurrency> Cryptocurrencies { get; set; }

    public DbSet<Core.Wallet> Wallets { get; set; }

    public DbSet<Core.Pool> Pools { get; set; }

    public DbSet<Core.Algorithm> Algorithms { get; set; }

    public DbSet<Core.Miner> Miners { get; set; }

    public DbSet<Core.Preset> Presets { get; set; }

    public DbSet<Overclocking> Overclocking { get; set; }

    public Context(DbContextOptions<Context> option) : base(option) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Core.Algorithm>().HasData(new Core.Algorithm() { Name = "Algorithm" });
        modelBuilder.Entity<Core.Miner>().HasData(new Core.Miner() { Name = "Miner" });
    }
}