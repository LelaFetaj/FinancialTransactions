using FinancialTransactionsAPI.Models.Entities.Customers;
using FinancialTransactionsAPI.Models.Entities.Transactions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

public partial class StorageBroker : DbContext, IStorageBroker
{
    private readonly IConfiguration configuration;
    public StorageBroker(IConfiguration configuration)
    {
        this.configuration = configuration;
        this.Database.Migrate();
    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        SetTransactionProperties(modelBuilder);
    }
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        string connectionString = this.configuration.GetConnectionString(name: "DefaultConnection");
        optionsBuilder
            .UseSqlServer(connectionString)
            .ConfigureWarnings(warnings => warnings.Ignore(RelationalEventId.PendingModelChangesWarning));

    }

    private static void SetTransactionProperties(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>()
                .HasOne(x => x.Customer)
                .WithMany(x => x.Transactions)
                .HasForeignKey(x => x.CustomerId)
                .OnDelete(DeleteBehavior.NoAction);
    }

    public DbSet<Transaction> Transactions { get; set; }
    public DbSet<Customer> Customers { get; set; }
}