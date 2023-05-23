using Microsoft.EntityFrameworkCore;

namespace FinanceConsoleLibrary.DataAccess.Database;

public class StorageContext : DbContext
{
    public StorageContext()
    {
        
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}