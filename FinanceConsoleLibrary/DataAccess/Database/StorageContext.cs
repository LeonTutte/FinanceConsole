using FinanceConsoleLibrary.Modules.Static;
using IniParser.Model;
using Microsoft.EntityFrameworkCore;

namespace FinanceConsoleLibrary.DataAccess.Database;

public class StorageContext : DbContext
{
    public StorageContext()
    {
        LogModule.WriteDebug("Initializing storageContext");
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: Add password to connection string
        LogModule.WriteDebug("Configuring storageContext");
        IniData settingsData = ConfigurationModule.GetConfiguration();
        LogModule.WriteDebug("Building connection string for storageContext");
        var connectionString = $"Data Source={settingsData["Database"]["Filename"]};";
        optionsBuilder.UseSqlite(connectionString);
        LogModule.WriteDebug("Finished configuring storageContext");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}