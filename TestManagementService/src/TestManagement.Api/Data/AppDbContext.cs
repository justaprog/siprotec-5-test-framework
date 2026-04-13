// entity framework core db context 
// connect c# models to the database and tell ef core how to map the models to db tables
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Models;

namespace TestManagement.Api.Data;

public class AppDbContext : DbContext // main class ef core uses to interact with database
{   
    // this constructor receiveds db config and pass to the parent dbcontext
    // options e.g: which database provider, connection string, postgreSQL, logging settings,... 
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }
    // represents the tables in the database, allows us to query and save entities
    public DbSet<TestCase> TestCases => Set<TestCase>();
    public DbSet<TestRun> TestRuns => Set<TestRun>();

    // config how models map to db
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestCase>()
            .HasMany(tc => tc.TestRuns) // one test case can have many test runs
            .WithOne(tr => tr.TestCase) // each test run is associated with one test case
            .HasForeignKey(tr => tr.TestCaseId) // foreign key in TestRun pointing to TestCase
            .OnDelete(DeleteBehavior.Cascade); // if a test case is deleted, its related test runs will also be deleted
    }
}