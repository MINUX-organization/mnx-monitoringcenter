using Microsoft.EntityFrameworkCore;
using MNX.MonitoringCenter.Monitoring.Core;

namespace MNX.MonitoringCenter.Monitoring.DataAccess;

public class Context : DbContext
{
    public DbSet<Rig> Rigs { get; set; }

    public Context(DbContextOptions<Context> option) : base(option)
    {
        Database.EnsureCreated();
    }
}