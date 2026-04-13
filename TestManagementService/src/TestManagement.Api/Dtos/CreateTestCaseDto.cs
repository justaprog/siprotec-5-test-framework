namespace TestManagement.Api.Dtos;

/// <summary>
/// DTO for creating a new test case. This is used in the POST /api/testcases 
/// endpoint to receive the necessary data to create a new test case in the system.
/// </summary>
public class CreateTestCaseDto
{
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
}