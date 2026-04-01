using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Controllers;
using TestManagement.Api.Data;
using TestManagement.Api.Dtos;
using TestManagement.Api.Models;
using Xunit;

namespace TestManagement.Api.Tests.Controllers;

// This class contains unit tests for the TestCasesController
public class TestCasesControllerTests
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
    public async Task GetAll_ReturnsOk_WithAllTestCases()
    {   
        // arrange
        // use using statement to ensure the context is disposed after the test
        using var context = CreateContext();

        // add some test cases to the in-memory database
        context.TestCases.Add(new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test 1",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        });

        context.TestCases.Add(new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test 2",
            PickupCurrent = 150,
            FaultCurrent = 300,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        });

        await context.SaveChangesAsync();
        var controller = new TestCasesController(context);

        // act
        var result = await controller.GetAll();

        // assert
        // verify that the result is an OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // verify that the value of the OkObjectResult is an IEnumerable<TestCase>
        var testCases = Assert.IsAssignableFrom<IEnumerable<TestCase>>(okResult.Value);
        Assert.Equal(2, testCases.Count());
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenTestCaseExists()
    {
        // arrange
        using var context = CreateContext();

        var id = Guid.NewGuid();
        var testCase = new TestCase
        {
            Id = id,
            Name = "Existing Test 1",
            PickupCurrent = 100,
            FaultCurrent = 250,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };

        context.TestCases.Add(testCase);
        await context.SaveChangesAsync();

        var controller = new TestCasesController(context);

        // act
        var result = await controller.GetById(id);
        
        // assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var testCaseFound = Assert.IsType<TestCase>(okResult.Value);
        Assert.Equal(id, testCaseFound.Id);
    }
}
