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
    public async Task<ActionResult<IEnumerable<TestCase>>> GetAll()
    {
        
        // fetch all test cases from the database
        var testCases = await _context.TestCases.ToListAsync();
        return Ok(testCases);
    }

    /// <summary>
    /// Get a test case by id
    /// </summary>
    /// <param name="id">The id of the test case to retrieve</param>
    /// <returns>
    /// returns the test case with the specified id as HTTP 200 Ok response
    /// </returns>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<TestCase>> GetById(Guid id)
    {
        var testCase = await _context.TestCases.FindAsync(id);

        if (testCase == null)
        {
            return NotFound();
        }

        return Ok(testCase);
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
    public async Task<ActionResult<TestCase>> Create(CreateTestCaseDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Name))
        {
            return BadRequest("Name is required.");
        }

        if (dto.PickupCurrent <= 0)
        {
            return BadRequest("PickupCurrent must be greater than 0.");
        }

        if (dto.FaultCurrent <= dto.PickupCurrent)
        {
            return BadRequest("FaultCurrent must be greater than PickupCurrent.");
        }

        if (dto.ExpectedTrip && (dto.ExpectedTripMinMs == null || dto.ExpectedTripMaxMs == null))
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
            FaultType = dto.FaultType,
            PickupCurrent = dto.PickupCurrent,
            FaultCurrent = dto.FaultCurrent,
            FaultStartMs = dto.FaultStartMs,
            TripDelayMs = dto.TripDelayMs,
            ExpectedTrip = dto.ExpectedTrip,
            ExpectedTripMinMs = dto.ExpectedTripMinMs,
            ExpectedTripMaxMs = dto.ExpectedTripMaxMs
        };
        // add the new test case to the database and save changes
        _context.TestCases.Add(testCase);
        await _context.SaveChangesAsync();
        
        // return HTTP 201 Created response with the created test case and a Location header
        // the Location header points to the GetById action with the id of 
        // the newly created test case
        return CreatedAtAction(nameof(GetById), new { id = testCase.Id }, testCase);
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