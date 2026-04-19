using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Models;

namespace TestManagement.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<TestCase> TestCases => Set<TestCase>();
    public DbSet<TestRun> TestRuns => Set<TestRun>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestCase>(entity =>
        {
            entity.HasMany(tc => tc.TestRuns) // each test case has many test runs
                .WithOne(tr => tr.TestCase) // each test run has one test case
                .HasForeignKey(tr => tr.TestCaseId) // foreign key in TestRun pointing to TestCase
                .OnDelete(DeleteBehavior.Cascade); // if a test case is deleted, its test runs will also be deleted

            entity.Property(tc => tc.DeviceFamily) // configure enum to be stored as string in the database
                .HasConversion<string>();

            entity.Property(tc => tc.ProtectionFunction) // configure enum to be stored as string in the database
                .HasConversion<string>();

            // configure owned types for SimulationSettings and ExpectedOutcome
            // this will create separate columns for the properties of these owned types in the TestCases table
            // e.g: Simulation_FaultType, Simulation_NominalCurrent, ExpectedOutcome_ExpectedTrip, etc.
            entity.OwnsOne(tc => tc.Simulation, simulation =>
            {
                simulation.Property(s => s.FaultType)
                    .HasConversion<string>();
            });

            entity.OwnsOne(tc => tc.ExpectedOutcome);
        });
    }
}