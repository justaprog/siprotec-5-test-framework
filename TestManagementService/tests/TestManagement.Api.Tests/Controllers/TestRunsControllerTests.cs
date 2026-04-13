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

    [Fact]
    public async Task GetById_ReturnsOk_and_ReturnsNotFound()
    {
        // arrange
        using var context = CreateContext();

        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test Case for GetById",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };
        context.TestCases.Add(testCase);

        var testRun = new TestRun
        {
            Id = Guid.NewGuid(),
            TestCaseId = testCase.Id,
            Status = TestRunStatus.Pending,
        };
        context.TestRuns.Add(testRun);
        await context.SaveChangesAsync();

        var controller = new TestRunsController(context);

        // act
        var result = await controller.GetById(testRun.Id);

        // assert
        // return ok 
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var returnedTestRun = Assert.IsType<TestRunResponseDto>(okResult.Value);

        Assert.Equal(testRun.Id, returnedTestRun.Id);
        Assert.Equal(testRun.TestCaseId, returnedTestRun.TestCaseId);

        // return not found for non-existing id
        var notFoundResult = await controller.GetById(Guid.NewGuid());
        Assert.IsType<NotFoundResult>(notFoundResult.Result);
    }

    [Fact]
    public async Task Create_ReturnsCreatedAtAction()
    {
        // arrange
        using var context = CreateContext();

        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test Case for Create",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };
        context.TestCases.Add(testCase);
        await context.SaveChangesAsync();

        var controller = new TestRunsController(context);

        var createDto = new CreateTestRunDto
        {
            TestCaseId = testCase.Id
        };

        // act
        var result = await controller.Create(createDto);

        // assert
        var createdAtResult = Assert.IsType<CreatedAtActionResult>(result.Result);
        var createdTestRun = Assert.IsType<TestRunResponseDto>(createdAtResult.Value);

        Assert.Equal(createDto.TestCaseId, createdTestRun.TestCaseId);
        Assert.Equal(TestRunStatus.Pending.ToString(), createdTestRun.Status);
    }

    [Fact]
    public async Task Create_ReturnsBadRequest_ForNonExistingTestCase()
    {
        // arrange
        using var context = CreateContext();
        var controller = new TestRunsController(context);

        var createDto = new CreateTestRunDto
        {
            TestCaseId = Guid.NewGuid() // non-existing id
        };

        // act
        var result = await controller.Create(createDto);

        // assert
        Assert.IsType<BadRequestObjectResult>(result.Result);
    }

    [Fact]
    public async Task UpdateResult_ReturnNotFound()
    {
        // arrange
        using var context = CreateContext();
        var controller = new TestRunsController(context);

        var updateDto = new UpdateTestRunResultDto
        {
            ActualTrip = true,
            ActualTripTimeMs = 30,
            Passed = true,
            ResultMessage = "Test passed successfully"
        };

        // act
        var result = await controller.UpdateResult(Guid.NewGuid(), updateDto); // non-existing id

        // assert
        Assert.IsType<NotFoundResult>(result.Result);
    }

    [Fact]
    public async Task UpdateResult_ReturnsOk()
    {
        // arrange
        using var context = CreateContext();
        // create a test case
        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            Name = "Test Case for UpdateResult",
            PickupCurrent = 100,
            FaultCurrent = 200,
            FaultStartMs = 10,
            TripDelayMs = 20,
            ExpectedTrip = true,
            ExpectedTripMinMs = 25,
            ExpectedTripMaxMs = 35
        };
        // create a test run associated with the test case
        var testRun = new TestRun
        {
            Id = Guid.NewGuid(),
            TestCaseId = testCase.Id,
            Status = TestRunStatus.Pending,
        };
        // save the test case and test run to the in-memory database
        context.TestCases.Add(testCase);
        context.TestRuns.Add(testRun);
        await context.SaveChangesAsync();

        // create controller
        var controller = new TestRunsController(context);
        
        var updateDto = new UpdateTestRunResultDto
        {
            ActualTrip = true,
            ActualTripTimeMs = 30,
            Passed = true,
            ResultMessage = "Test passed successfully"
        };

        // act
        var result = await controller.UpdateResult(testRun.Id, updateDto);

        // assert
        var okResult = Assert.IsType<OkObjectResult>(result.Result);
        var updatedTestRun = Assert.IsType<TestRunResponseDto>(okResult.Value);

        Assert.Equal(testRun.Id, updatedTestRun.Id);
        Assert.Equal(updateDto.ActualTrip, updatedTestRun.ActualTrip);
        Assert.Equal(updateDto.ActualTripTimeMs, updatedTestRun.ActualTripTimeMs);
        Assert.Equal(updateDto.Passed, updatedTestRun.Passed);
        Assert.Equal(updateDto.ResultMessage, updatedTestRun.ResultMessage);
    }
}