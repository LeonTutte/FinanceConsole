using FinanceConsoleLibrary.DataAccess.Database.Models;
using FinanceConsoleLibrary.Modules.Static;
using Microsoft.EntityFrameworkCore;

namespace FinanceConsoleLibrary.DataAccess.Database;

public class StorageContext : DbContext
{
#pragma warning disable CS8618
    public StorageContext()
#pragma warning restore CS8618
    {
        LogModule.WriteDebug("Initializing storageContext");
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Account> Accounts { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // TODO: Add password to connection string
        LogModule.WriteDebug("Configuring storageContext");
        var settingsData = ConfigurationModule.GetConfiguration();
        LogModule.WriteDebug("Building connection string for storageContext");
        var connectionString = $"Data Source={settingsData["Database"]["Filename"]};";
        optionsBuilder.UseSqlite(connectionString);
        LogModule.WriteDebug("Finished configuring storageContext");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // User
        modelBuilder.Entity<User>()
            .HasKey(nameof(User.Id));
        modelBuilder.Entity<User>()
            .Property(nameof(User.Label))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(256);
        modelBuilder.Entity<User>()
            .Property(nameof(User.LoginName))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(32);
        modelBuilder.Entity<User>()
            .Property(nameof(User.PasswordHash))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(1024);
        modelBuilder.Entity<User>()
            .HasMany<Account>(x => x.Accounts)
            .WithOne(x => x.User)
            .HasForeignKey(x => x.UserId);

        // Account
        modelBuilder.Entity<Account>()
            .HasKey(nameof(Account.Id));
        modelBuilder.Entity<Account>()
            .Property(nameof(Account.Label))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(128);
        modelBuilder.Entity<Account>()
            .Property(nameof(Account.Iban))
            .IsUnicode(false)
            .HasMaxLength(128)
            .IsRequired(false);
        modelBuilder.Entity<Account>()
            .HasMany<Transaction>(x => x.Transactions)
            .WithOne(x => x.BaseAccount)
            .HasForeignKey(x => x.BaseAccountId);
        modelBuilder.Entity<Account>()
            .HasOne<User>(x => x.User)
            .WithMany(x => x.Accounts)
            .HasForeignKey(x => x.UserId);

        // Transaction
        modelBuilder.Entity<Transaction>()
            .HasKey(nameof(Transaction.Id));
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.AmountInCent))
            .IsRequired();
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.Target))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(128);
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.TargetIban))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(128);
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.Description))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(1024);
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.Reference))
            .IsRequired()
            .IsUnicode(false)
            .HasMaxLength(1024);
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.BookingDate))
            .IsRequired();
        modelBuilder.Entity<Transaction>()
            .Property(nameof(Transaction.ValueDate))
            .IsRequired();
        modelBuilder.Entity<Transaction>()
            .HasOne<Account>(x => x.BaseAccount)
            .WithMany(x => x.Transactions)
            .HasForeignKey(x => x.BaseAccountId);
    }
}