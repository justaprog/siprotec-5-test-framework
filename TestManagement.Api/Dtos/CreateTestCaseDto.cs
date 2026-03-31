namespace TestManagement.Api.Dtos;

public class CreateTestCaseDto
{
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeviceFamily { get; set; } = "SIPROTEC_5";
    public string ProtectionFunction { get; set; } = "Overcurrent";
    public string FaultType { get; set; } = "Overcurrent";
    public double PickupCurrent { get; set; }
    public double FaultCurrent { get; set; }
    public int FaultStartMs { get; set; }
    public int TripDelayMs { get; set; }
    public bool ExpectedTrip { get; set; }
    public int? ExpectedTripMinMs { get; set; }
    public int? ExpectedTripMaxMs { get; set; }
}