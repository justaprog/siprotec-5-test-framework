using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Controllers;
using TestManagement.Api.Data;
using TestManagement.Api.Dtos;
using TestManagement.Api.Models;
using Xunit;

namespace TestManagement.Api.Tests.Controllers;

// this class contains unit tests for the TestRunsController
public class TestRunsControllerTests
{
    // helper method to create a new AppDbContext with an in-memory database for testing
    private AppDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            // use a unique in-memory database for each test to ensure isolation
            .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
            .Options;

        return new AppDbContext(options);
    }

    [Fact]
    public async Task GetAll_ReturnsOk_WithAllTestRuns()
    {
        // arrange
        // use using statement to ensure the context is disposed after the test
        using var context = CreateContext();

        // create a test case to associate with the test run
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test Case for GetAll",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };
        context.TestCases.Add(testCase);
        
        // create few test runs
        var testRun1 = new TestRun
        {
            Id = Guid.NewGuid(),
            TestCaseId = testCase.Id,
            Status = TestRunStatus.Pending,
        };
        var testRun2 = new TestRun
        {
            Id = Guid.NewGuid(),
            TestCaseId = testCase.Id,
            Status = TestRunStatus.Pending,
        };
        // add test runs to the context
        context.TestRuns.AddRange(testRun1, testRun2);
        // save changes to the in-memory database
        await context.SaveChangesAsync();

        // create the controller instance with the test context
        var controller = new TestRunsController(context);

        // act
        var result = await controller.GetAll();

        // assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var testRuns = Assert.IsAssignableFrom<IEnumerable<TestRunResponseDto>>(okResult.Value);

        Assert.Equal(2, testRuns.Count());
        Assert.Contains(testRuns, tr => tr.Id == testRun1.Id);
        Assert.Contains(testRuns, tr => tr.Id == testRun2.Id);
    }
}