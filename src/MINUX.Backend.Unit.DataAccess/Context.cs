using Microsoft.EntityFrameworkCore;
using MINUX.Backend.Unit.Core;
using MINUX.Backend.Unit.DataAccess.Cfg;
using MINUX.Backend.Unit.DataAccess.Dto;

namespace MINUX.Backend.Unit.DataAccess;

public class Context : DbContext
{
    public DbSet<Cryptocurrency> Cryptocurrencies { get; set; }

    public DbSet<Wallet> Wallets { get; set; }

    public DbSet<Pool> Pools { get; set; }

    public DbSet<Algorithm> Algorithms { get; set; }

    public DbSet<MinerDto> Miners { get; set; }

    public DbSet<MinerAlgorithm> MinerAlgorithms { get; set; }

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

        modelBuilder.Entity<Algorithm>().HasData(new Algorithm() { Name = "Algorithm" });
        modelBuilder.Entity<MinerDto>().HasData(new MinerDto() { Name = "Miner" });
        modelBuilder.Entity<MinerAlgorithm>().HasData(new MinerAlgorithm() { MinerName = "Miner", AlgorithmName = "Algorithm1" });
    }
}