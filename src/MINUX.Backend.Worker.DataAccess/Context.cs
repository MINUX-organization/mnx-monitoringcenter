using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Worker.Core;
using MINUX.Backend.Worker.DataAccess.Cfg;
using MINUX.Backend.Worker.DataAccess.Dto;

namespace MINUX.Backend.Worker.DataAccess;

public class Context : DbContext
{
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Pool> Pools { get; set; }

    public DbSet<Algorithm> Algorithms { get; set; }

    public DbSet<MinerDto> Miners { get; set; }

    public DbSet<MinerAlgorithm> MinerAlgorithms { get; set; }

    public DbSet<Preset> Presets { get; set; }

    public Context(DbContextOptions<Context> option) : base(option)
    {
        //Database.EnsureDeleted();
        Database.EnsureCreated();
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AlgorithmCfg());
        modelBuilder.ApplyConfiguration(new CryptocurrencyCfg());
        modelBuilder.ApplyConfiguration(new MinerDtoCfg());
        modelBuilder.ApplyConfiguration(new MinerAlgorithmCfg());
        modelBuilder.ApplyConfiguration(new WalletCfg());
        modelBuilder.ApplyConfiguration(new PoolCfg());

        modelBuilder.Entity<Algorithm>().HasData(new Algorithm() { Name = "Algorithm" });
        modelBuilder.Entity<MinerDto>().HasData(new MinerDto() { Name = "Miner" });
        modelBuilder.Entity<MinerAlgorithm>().HasData(new MinerAlgorithm() { MinerName = "Miner", AlgorithmName = "Algorithm" });
    }
}