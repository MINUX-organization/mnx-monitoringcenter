using Microsoft.EntityFrameworkCore;

namespace MINUX.Backend.Unit.DataAccess;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> option) : base(option)
    {
        Database.EnsureCreated();
    }
}