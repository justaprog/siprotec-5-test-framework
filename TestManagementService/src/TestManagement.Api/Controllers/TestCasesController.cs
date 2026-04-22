using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestManagement.Api.Data;
using TestManagement.Api.Dtos;
using TestManagement.Api.Models;

namespace TestManagement.Api.Controllers;

[ApiController]
[Route("api/testcases")]
public class TestCasesController : ControllerBase
{
    // inject AppDbContext to access the database
    private readonly AppDbContext _context;

    public TestCasesController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get all test cases
    /// </summary>
    /// <returns>
    /// returns a list of test cases in the database as HTTP 200 Ok response
    /// </returns>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<TestCaseResponseDto>>> GetAll()
    {
        
        // fetch all test cases from the database
        var testCases = await _context.TestCases.ToListAsync();

        // debugg
        var id = Guid.NewGuid();
        var testCase = new TestCase
        {
            Id = id,
            Name = "Sample Test Case to debug",
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
        _context.TestCases.Add(testCase);
        await _context.SaveChangesAsync();

        var testCaseDtos = testCases.Select(tc => new TestCaseResponseDto(tc));

        Console.WriteLine($"Fetched {testCaseDtos.Count()} test cases from the database.");
            
        return Ok(testCaseDtos);
    }

    /// <summary>
    /// Get a test case by id
    /// </summary>
    /// <param name="id">The id of the test case to retrieve</param>
    /// <returns>
    /// returns the test case with the specified id as HTTP 200 Ok response
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestCaseResponseDto>> GetById(Guid id)
    {
        var testCase = await _context.TestCases.FindAsync(id);

        if (testCase == null)
        {
            return NotFound();
        }

        return Ok(new TestCaseResponseDto(testCase));
    }

    /// <summary>
    /// Create a new test case and save it to the database
    /// </summary>
    /// <param name="dto">The test case data to create</param>
    /// <returns>
    /// returns the created test case as HTTP 201 Created response with a 
    /// Location header pointing to the new resource
    /// </returns>
    [HttpPost]
    public async Task<ActionResult<TestCaseResponseDto>> Create(CreateTestCaseDto dto)
    {
        CreateSimulationSettingsDto simulation = dto.Simulation;
        CreateExpectedOutcomeDto expectedOutcome = dto.ExpectedOutcome;
        
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required.");
        }

        if (simulation.PickupCurrent <= 0)
        {
            return BadRequest("PickupCurrent must be greater than 0.");
        }

        if (simulation.FaultCurrent <= simulation.PickupCurrent)
        {
            return BadRequest("FaultCurrent must be greater than PickupCurrent.");
        }

        if (expectedOutcome.ExpectedTrip && (expectedOutcome.ExpectedTripMinMs == null || expectedOutcome.ExpectedTripMaxMs == null))
        {
            return BadRequest("Expected trip time window is required when ExpectedTrip is true.");
        }

        var testCase = new TestCase
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            DeviceFamily = dto.DeviceFamily,
            ProtectionFunction = dto.ProtectionFunction,
            Simulation = new SimulationSettings
            {
                FaultType = simulation.FaultType,
                NominalCurrent = simulation.NominalCurrent,
                PickupCurrent = simulation.PickupCurrent,
                FaultCurrent = simulation.FaultCurrent,
                FaultStartMs = simulation.FaultStartMs,
                DurationMs = simulation.DurationMs,
                SamplingRateHz = simulation.SamplingRateHz
            },
            ExpectedOutcome = new ExpectedOutcome
            {
                ExpectedTrip = expectedOutcome.ExpectedTrip,
                TripDelayMs = expectedOutcome.TripDelayMs,
                ExpectedTripMinMs = expectedOutcome.ExpectedTripMinMs,
                ExpectedTripMaxMs = expectedOutcome.ExpectedTripMaxMs
            }
        };
        // add the new test case to the database and save changes
        _context.TestCases.Add(testCase);
        await _context.SaveChangesAsync();
        
        // return HTTP 201 Created response with the created test case and a Location header
        // the Location header points to the GetById action with the id of 
        // the newly created test case
        return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, new TestCaseResponseDto(testCase));
    }

    /// <summary>
    /// Delete a test case by id
    /// </summary>
    /// <param name="id">The id of the test case to delete</param>
    /// <returns></returns>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        var testCase = await _context.TestCases.FindAsync(id);

        if (testCase == null)
        {
            return NotFound();
        }

        _context.TestCases.Remove(testCase);
        await _context.SaveChangesAsync();
        // return HTTP 204 No Content response to indicate successful deletion
        return NoContent();
    }
}