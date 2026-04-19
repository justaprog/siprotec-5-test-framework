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
        SimulationSettings simulation_1 = new SimulationSettings
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 100,
            PickupCurrent = 300,
            FaultCurrent = 600,
            FaultStartMs = 100,
            DurationMs = 300,
            SamplingRateHz = 1000
        };
        ExpectedOutcome expectedOutcome_1 = new ExpectedOutcome
        {
            ExpectedTrip = true,
            TripDelayMs = 50,
            ExpectedTripMinMs = 145,
            ExpectedTripMaxMs = 155
        };
        context.TestCases.Add(new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test 1",
            Simulation = simulation_1,
            ExpectedOutcome = expectedOutcome_1
        });

        SimulationSettings simulation_2 = new SimulationSettings
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 200,
            PickupCurrent = 500,
            FaultCurrent = 1000,
            FaultStartMs = 50,
            DurationMs = 200,
            SamplingRateHz = 2000
        };
        ExpectedOutcome expectedOutcome_2 = new ExpectedOutcome
        {
            ExpectedTrip = true,
            TripDelayMs = 30,
            ExpectedTripMinMs = 80,
            ExpectedTripMaxMs = 120
        };       

        context.TestCases.Add(new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test 2",
            Simulation = simulation_2,
            ExpectedOutcome = expectedOutcome_2
        });

        await context.SaveChangesAsync();
        var controller = new TestCasesController(context);

        // act
        var result = await controller.GetAll();

        // assert
        // verify that the result is an OkObjectResult
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        // verify that the value of the OkObjectResult is an IEnumerable<TestCaseResponseDto>
        var testCases = Assert.IsAssignableFrom<IEnumerable<TestCaseResponseDto>>(okResult.Value);
        Assert.Equal(2, testCases.Count());
    }

    [Fact]
    public async Task GetById_ReturnsOk_WhenTestCaseExists()
    {
        // arrange
        using var context = CreateContext();

        var id = Guid.NewGuid();
        SimulationSettings simulation = new SimulationSettings
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 100,
            PickupCurrent = 300,
            FaultCurrent = 600,
            FaultStartMs = 100,
            DurationMs = 300,
            SamplingRateHz = 1000
        };
        ExpectedOutcome expectedOutcome = new ExpectedOutcome
        {
            ExpectedTrip = true,
            TripDelayMs = 50,
            ExpectedTripMinMs = 145,
            ExpectedTripMaxMs = 155
        };
        var testCase = new TestCase
        {
            Id = id,
            Name = "Existing Test 1",
            Simulation = simulation,
            ExpectedOutcome = expectedOutcome
        };

        context.TestCases.Add(testCase);
        await context.SaveChangesAsync();

        var controller = new TestCasesController(context);

        // act
        var result = await controller.GetById(id);
        
        // assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var testCaseFound = Assert.IsType<TestCaseResponseDto>(okResult.Value);
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

        CreateSimulationSettingsDto simulation = new CreateSimulationSettingsDto
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 100,
            PickupCurrent = 300,
            FaultCurrent = 600,
            FaultStartMs = 100,
            DurationMs = 300,
            SamplingRateHz = 1000
        };
        CreateExpectedOutcomeDto expectedOutcome = new CreateExpectedOutcomeDto
        {
            ExpectedTrip = true,
            TripDelayMs = 50,
            ExpectedTripMinMs = 145,
            ExpectedTripMaxMs = 155
        };
        var dto = new CreateTestCaseDto
        {
            Name = "",
            Simulation = simulation,
            ExpectedOutcome = expectedOutcome
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

        CreateSimulationSettingsDto simulation = new CreateSimulationSettingsDto
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 100,
            PickupCurrent = 0, // invalid value
            FaultCurrent = 600,
            FaultStartMs = 100,
            DurationMs = 300,
            SamplingRateHz = 1000
        };
        CreateExpectedOutcomeDto expectedOutcome = new CreateExpectedOutcomeDto
        {
            ExpectedTrip = true,
            TripDelayMs = 50,
            ExpectedTripMinMs = 145,
            ExpectedTripMaxMs = 155
        };
        var dto = new CreateTestCaseDto
        {
            Name = "Test Case with Invalid PickupCurrent",
            Simulation = simulation,
            ExpectedOutcome = expectedOutcome
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

        CreateSimulationSettingsDto simulation = new CreateSimulationSettingsDto
        {
            FaultType = FaultType.Overcurrent,
            NominalCurrent = 100,
            PickupCurrent = 300,
            FaultCurrent = 200, // invalid value, should be greater than PickupCurrent
            FaultStartMs = 100,
            DurationMs = 300,
            SamplingRateHz = 1000
        };
        CreateExpectedOutcomeDto expectedOutcome = new CreateExpectedOutcomeDto
        {
            ExpectedTrip = true,
            TripDelayMs = 50,
            ExpectedTripMinMs = 145,
            ExpectedTripMaxMs = 155
        };

        var dto = new CreateTestCaseDto
        {
            Name = "Test Case with Invalid FaultCurrent",
            Simulation = simulation,
            ExpectedOutcome = expectedOutcome
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
            Simulation = new CreateSimulationSettingsDto
            {
                FaultType = FaultType.Overcurrent,
                NominalCurrent = 100,
                PickupCurrent = 300,
                FaultCurrent = 600,
                FaultStartMs = 100,
                DurationMs = 300,
                SamplingRateHz = 1000
            },
            ExpectedOutcome = new CreateExpectedOutcomeDto
            {
                ExpectedTrip = true,
                TripDelayMs = 50,
                ExpectedTripMinMs = null,
                ExpectedTripMaxMs = null
            }
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
            Simulation = new CreateSimulationSettingsDto
            {
                FaultType = FaultType.Overcurrent,
                NominalCurrent = 100,
                PickupCurrent = 300,
                FaultCurrent = 600,
                FaultStartMs = 100,
                DurationMs = 300,
                SamplingRateHz = 1000
            },
            ExpectedOutcome = new CreateExpectedOutcomeDto
            {
                ExpectedTrip = true,
                TripDelayMs = 50,
                ExpectedTripMinMs = 145,
                ExpectedTripMaxMs = 155
            }
        };

        // Act
        var result = await controller.Create(dto);

        // Assert
        var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTestCase = Assert.IsType<TestCaseResponseDto>(createdResult.Value);

        // verify that the action name in the CreatedAtActionResult is correct
        // Console.WriteLine($"Output of nameof(TestCasesController.GetById): {nameof(TestCasesController.GetById)}");
        Assert.Equal(nameof(TestCasesController.GetById), createdResult.ActionName);
        Assert.Equal("Valid Test", createdTestCase.Name);

        // verify that the test case was actually saved in the database
        var saved = await context.TestCases.FindAsync(createdTestCase.Id);
        Assert.NotNull(saved);
        Assert.Equal("Valid Test", saved!.Name);
    }

    [Fact]
    public async Task Delete_ReturnsNotFound_WhenTestCaseDoesNotExist()
    {
        // Arrange
        using var context = CreateContext();
        var controller = new TestCasesController(context);

        // Act
        var result = await controller.Delete(Guid.NewGuid());

        // Assert
        Assert.IsType<NotFoundResult>(result);
    }
    [Fact]
    public async Task Delete_ReturnsNoContent_WhenTestCaseIsDeleted()
    {
        // arrange
        using var context = CreateContext();

        var id = Guid.NewGuid();
        var testCase = new TestCase
        {
            Id = id,
            Name = "Test Case to Delete",
            Simulation = new SimulationSettings
            {
                FaultType = FaultType.Overcurrent,
                NominalCurrent = 100,
                PickupCurrent = 300,
                FaultCurrent = 600,
                FaultStartMs = 100,
                DurationMs = 300,
                SamplingRateHz = 1000
            },
            ExpectedOutcome = new ExpectedOutcome
            {
                ExpectedTrip = true,
                TripDelayMs = 50,
                ExpectedTripMinMs = 145,
                ExpectedTripMaxMs = 155
            }
        };
        // add testcase to database
        context.TestCases.Add(testCase);
        await context.SaveChangesAsync();

        // act
        var controller = new TestCasesController(context);
        var result = await controller.Delete(id);

        // assert
        // verify right response type
        Assert.IsType<NoContentResult>(result);
        // verify the test case was deleted from the database
        var deleted = await context.TestCases.FindAsync(id);
        Assert.Null(deleted);
        
    }
}
