using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Management.Core;
using MNX.MonitoringCenter.Management.DataAccess.Cfg;
using System.Reflection;

namespace MNX.MonitoringCenter.Management.DataAccess;

public class Context : DbContext
{
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Pool> Pools { get; set; }

    public DbSet<Algorithm> Algorithms { get; set; }

    public DbSet<Miner> Miners { get; set; }

    public DbSet<Preset> Presets { get; set; }

    public DbSet<Overclocking> Overclocking { get; set; }

    public Context(DbContextOptions<Context> option) : base(option)
    {
          //Database.EnsureDeleted();
          //Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        modelBuilder.Entity<Algorithm>().HasData(new Algorithm() { Name = "Algorithm" });
        modelBuilder.Entity<Miner>().HasData(new Miner() { Name = "Miner" });
    }
}