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

    public string DeviceFamily { get; set; } = string.Empty; // enum in model, string in DTO for better API usability
    public string ProtectionFunction { get; set; } = string.Empty; // enum in model, string in DTO for better API usability

    public SimulationSettingsResponseDto Simulation { get; set; } = new();
    public ExpectedOutcomeResponseDto ExpectedOutcome { get; set; } = new();

    public DateTime CreatedAt { get; set; }

    public List<TestRunSummaryDto> TestRuns { get; set; } = new();

    public TestCaseResponseDto(TestCase testCase)
    {
        Id = testCase.Id;
        Name = testCase.Name;
        Description = testCase.Description;
        DeviceFamily = testCase.DeviceFamily.ToString();
        ProtectionFunction = testCase.ProtectionFunction.ToString();

        Simulation = new SimulationSettingsResponseDto
        {
            FaultType = testCase.Simulation.FaultType.ToString(),
            NominalCurrent = testCase.Simulation.NominalCurrent,
            PickupCurrent = testCase.Simulation.PickupCurrent,
            FaultCurrent = testCase.Simulation.FaultCurrent,
            FaultStartMs = testCase.Simulation.FaultStartMs,
            DurationMs = testCase.Simulation.DurationMs,
            SamplingRateHz = testCase.Simulation.SamplingRateHz
        };

        ExpectedOutcome = new ExpectedOutcomeResponseDto
        {
            ExpectedTrip = testCase.ExpectedOutcome.ExpectedTrip,
            TripDelayMs = testCase.ExpectedOutcome.TripDelayMs,
            ExpectedTripMinMs = testCase.ExpectedOutcome.ExpectedTripMinMs,
            ExpectedTripMaxMs = testCase.ExpectedOutcome.ExpectedTripMaxMs
        };

        CreatedAt = testCase.CreatedAt;
        TestRuns = testCase.TestRuns.Select(tr => new TestRunSummaryDto(tr)).ToList();
    }
}

public class SimulationSettingsResponseDto
{
    public string FaultType { get; set; } = string.Empty;
    public double NominalCurrent { get; set; }
    public double PickupCurrent { get; set; }
    public double FaultCurrent { get; set; }
    public int FaultStartMs { get; set; }
    public int DurationMs { get; set; }
    public int SamplingRateHz { get; set; }
}

public class ExpectedOutcomeResponseDto
{
    public bool ExpectedTrip { get; set; }
    public int? TripDelayMs { get; set; }
    public int? ExpectedTripMinMs { get; set; }
    public int? ExpectedTripMaxMs { get; set; }
}