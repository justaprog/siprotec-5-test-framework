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

    [Fact]
    public async Task GetById_ReturnsNotFound_WhenTestCaseDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        // Act
        var result = await controller.GetById(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenNameIsMissing()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        var dto = new CreateTestCaseDto
        {
            Name = "",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Name is required.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenPickupCurrentIsInvalid()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        var dto = new CreateTestCaseDto
        {
            Name = "Test Case with Invalid PickupCurrent",
            PickupCurrent = 0,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("PickupCurrent must be greater than 0.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenFaultCurrentNotGreaterThanPickupCurrent()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        var dto = new CreateTestCaseDto
        {
            Name = "Test Case with Invalid FaultCurrent",
            PickupCurrent = 100,
            FaultCurrent = 50,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };

        // Act
        var result = await controller.Create(dto);

        // Assert 
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("FaultCurrent must be greater than PickupCurrent.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_WhenExpectedTripTimeWindowMissing()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        var dto = new CreateTestCaseDto
        {
            Name = "Test Case with Missing Expected Trip Time Window",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = null,
            ExpectedTripMaxMs = null
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var badRequestResult = Assert.IsType<BadRequestObjectResult>(result.Result);
        Assert.Equal("Expected trip time window is required when ExpectedTrip is true.", badRequestResult.Value);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAndSavesTestCase_WhenInputIsValid()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        var dto = new CreateTestCaseDto
        {
            Name = "Valid Test",
            Description = "Test description",
            DeviceFamily = "SIPROTEC_5",
            ProtectionFunction = "Overcurrent",
            FaultType = "PhaseA",
            PickupCurrent = 100,
            FaultCurrent = 300,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTestCase = Assert.IsType<TestCase>(createdResult.Value);

        // verify that the action name in the CreatedAtActionResult is correct
        // Console.WriteLine($"Output of nameof(TestCasesController.GetById): {nameof(TestCasesController.GetById)}");
        Assert.Equal(nameof(TestCasesController.GetById), createdResult.ActionName);
        Assert.Equal("Valid Test", createdTestCase.Name);

        // verify that the test case was actually saved in the database
        var saved = await context.TestCases.FindAsync(createdTestCase.Id);
        Assert.NotNull(saved);
        Assert.Equal("Valid Test", saved!.Name);
    }
}
