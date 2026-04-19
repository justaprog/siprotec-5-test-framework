using TestManagement.Api.Models;
namespace TestManagement.Api.Dtos;

// request body example:
/*
  "name": "Overcurrent Phase A Basic",
  "description": "Trips when current exceeds threshold",
  "deviceFamily": "SIPROTEC_5",
  "protectionFunction": "Overcurrent",
  "simulation": {
    "faultType": "Overcurrent",
    "nominalCurrent": 100,
    "pickupCurrent": 300,
    "faultCurrent": 600,
    "faultStartMs": 100,
    "durationMs": 300,
    "samplingRateHz": 1000
  },
  "expectedOutcome": {
    "expectedTrip": true,
    "tripDelayMs": 50,
    "expectedTripMinMs": 145,
    "expectedTripMaxMs": 155
  }
}
*/

/// <summary>
/// DTO for creating a new test case. This is used in the POST /api/testcases 
/// endpoint to receive the necessary data to create a new test case in the system.
/// </summary>
public class CreateTestCaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DeviceFamily DeviceFamily { get; set; } = DeviceFamily.SIPROTEC_5;
    public ProtectionFunction ProtectionFunction { get; set; } = ProtectionFunction.Overcurrent;

    public CreateSimulationSettingsDto Simulation { get; set; } = new();
    public CreateExpectedOutcomeDto ExpectedOutcome { get; set; } = new();
}

public class CreateSimulationSettingsDto
{
    public FaultType FaultType { get; set; } = FaultType.Overcurrent;

    public double NominalCurrent { get; set; }
    public double PickupCurrent { get; set; }
    public double FaultCurrent { get; set; }

    public int FaultStartMs { get; set; }
    public int DurationMs { get; set; }
    public int SamplingRateHz { get; set; }
}

public class CreateExpectedOutcomeDto
{
    public bool ExpectedTrip { get; set; }

    public int? TripDelayMs { get; set; }
    public int? ExpectedTripMinMs { get; set; }
    public int? ExpectedTripMaxMs { get; set; }
}