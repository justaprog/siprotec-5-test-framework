using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Data;
using TestManagement.Api.Dtos;
using TestManagement.Api.Models;

namespace TestManagement.Api.Controllers;

[ApiController]
[Route("api/testruns")]
public class TestRunsController : ControllerBase
{
    // inject AppDbContext to access the database
    private readonly AppDbContext _context;

    public TestRunsController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all test runs with their associated test case summary
    /// </summary>
    /// <returns>
    /// returns a list of test runs with their associated test case summary 
    /// as HTTP 200 Ok response
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestRunResponseDto>>> GetAll()
    {
        var testRuns = await _context.TestRuns
            .Include(tr => tr.TestCase)
            .ToListAsync();

        var response = testRuns.Select(testRun => new TestRunResponseDto
            {
                Id = testRun.Id,
                TestCaseId = testRun.TestCaseId,
                Status = testRun.Status.ToString(),
                StartedAt = testRun.StartedAt,
                FinishedAt = testRun.FinishedAt,
                ActualTrip = testRun.ActualTrip,
                ActualTripTimeMs = testRun.ActualTripTimeMs,
                Passed = testRun.Passed,
                ResultMessage = testRun.ResultMessage,
                TestCase = testRun.TestCase == null ? null : new TestCaseSummaryDto
                {
                    Id = testRun.TestCase.Id,
                    Name = testRun.TestCase.Name,
                    DeviceFamily = testRun.TestCase.DeviceFamily,
                    ProtectionFunction = testRun.TestCase.ProtectionFunction
                }
            });

        return Ok(response);
    }

    /// <summary>
    /// Get a test run by id with its associated test case summary
    /// </summary>
    /// <param name="id"></param>
    /// <returns>
    /// returns the test run with the specified id and its associated test 
    /// case summary as HTTP 200 Ok response
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestRunResponseDto>> GetById(Guid id)
    {
        // fetch the test run with the specified id from the database
        // including the associated test case
        var testRun = await _context.TestRuns
            .Include(tr => tr.TestCase)
            .FirstOrDefaultAsync(tr => tr.Id == id);

        if (testRun == null)
        {
            return NotFound();
        }
        var response = new TestRunResponseDto
        {
            Id = testRun.Id,
            TestCaseId = testRun.TestCaseId,
            Status = testRun.Status.ToString(),
            StartedAt = testRun.StartedAt,
            FinishedAt = testRun.FinishedAt,
            ActualTrip = testRun.ActualTrip,
            ActualTripTimeMs = testRun.ActualTripTimeMs,
            Passed = testRun.Passed,
            ResultMessage = testRun.ResultMessage,
            TestCase = testRun.TestCase == null ? null : new TestCaseSummaryDto
            {
                Id = testRun.TestCase.Id,
                Name = testRun.TestCase.Name,
                DeviceFamily = testRun.TestCase.DeviceFamily,
                ProtectionFunction = testRun.TestCase.ProtectionFunction
            }
        };

        return Ok(response);
    }

    /// <summary>
    /// Create a new test run for a given test case and save it to the database
    /// </summary>
    /// <param name="dto"></param>
    /// <returns>
    /// returns the created test run as HTTP 201 Created response with a 
    /// Location header pointing to the new resource
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<TestRun>> Create(CreateTestRunDto dto)
    {
        // validate that the referenced test case exists in the database
        var testCaseExists = await _context.TestCases.AnyAsync(tc => tc.Id == dto.TestCaseId);

        if (!testCaseExists)
        {
            return BadRequest("Referenced TestCase does not exist.");
        }

        var testRun = new TestRun
        {
            Id = Guid.NewGuid(),
            TestCaseId = dto.TestCaseId,
            Status = TestRunStatus.Pending,
        };

        _context.TestRuns.Add(testRun);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = testRun.Id }, testRun);
    }

    /// <summary>
    /// Update the result of a test run and save the changes to the database
    /// </summary>
    /// <param name="id"></param>
    /// <param name="dto"></param>
    /// <returns>
    /// returns the updated test run as HTTP 200 Ok response
    /// </returns>
    [HttpPatch("{id:guid}/result")]
    public async Task<ActionResult<TestRun>> UpdateResult(Guid id, UpdateTestRunResultDto dto)
    {
        var testRun = await _context.TestRuns.FindAsync(id);

        if (testRun == null)
        {
            return NotFound();
        }

        testRun.ActualTrip = dto.ActualTrip;
        testRun.ActualTripTimeMs = dto.ActualTripTimeMs;
        testRun.Passed = dto.Passed;
        testRun.ResultMessage = dto.ResultMessage;
        testRun.Status = TestRunStatus.Completed;
        testRun.FinishedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return Ok(testRun);
    }
}