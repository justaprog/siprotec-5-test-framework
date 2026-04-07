using TestManagement.Api.Models;

namespace TestManagement.Api.Dtos;

/// <summary>
/// DTO for returning test case details along with a summary of its 
/// associated test runs.
/// </summary>
public class TestCaseResponseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeviceFamily { get; set; } = "SIPROTEC_5";
    public string ProtectionFunction { get; set; } = string.Empty;
    public string FaultType { get; set; } = string.Empty;
    public double PickupCurrent { get; set; }
    public double FaultCurrent { get; set; }
    public int FaultStartMs { get; set; }
    public int TripDelayMs { get; set; }
    public bool ExpectedTrip { get; set; }
    public int? ExpectedTripMinMs { get; set; }
    public int? ExpectedTripMaxMs { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<TestRunSummaryDto> TestRuns { get; set; } = new();

    public TestCaseResponseDto(TestCase testCase)
    {
        Id = testCase.Id;
        Name = testCase.Name;
        Description = testCase.Description;
        DeviceFamily = testCase.DeviceFamily;
        ProtectionFunction = testCase.ProtectionFunction;
        FaultType = testCase.FaultType;
        PickupCurrent = testCase.PickupCurrent;
        FaultCurrent = testCase.FaultCurrent;
        FaultStartMs = testCase.FaultStartMs;
        TripDelayMs = testCase.TripDelayMs;
        ExpectedTrip = testCase.ExpectedTrip;
        ExpectedTripMinMs = testCase.ExpectedTripMinMs;
        ExpectedTripMaxMs = testCase.ExpectedTripMaxMs;
        CreatedAt = testCase.CreatedAt;
        TestRuns = testCase.TestRuns.Select(tr => new TestRunSummaryDto(tr)).ToList();
     }
}