namespace TestManagement.Api.Models;

public class TestCase
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string DeviceFamily { get; set; } = "SIPROTEC_5";
    // protection function being tested, eg: overcurrent, distance, Undervoltage, etc
    public string ProtectionFunction { get; set; } = "Overcurrent";
    public string FaultType { get; set; } = "Overcurrent";
    // threshold current where the protection should start reacting
    // current unit in A 
    public double PickupCurrent { get; set; }
    // Actual current injected during the simulated fault
    // could be higher than pickup current to ensure the protection reacts
    public double FaultCurrent { get; set; }
    // when the fault current starts being injected, in ms from the start of the test run
    // many test cases may have some time of normal current injection before 
    // the fault starts, to simulate real world conditions
    public int FaultStartMs { get; set; }
    // how long the relay is expected to wait before tripping after the fault current is injected
    // common in overcurrent protection wher the relay does not trip immediately,
    // but only after a configured delay.
    public int TripDelayMs { get; set; }
    // whether the test case is expected to result in a trip or not
    // e.g: current below pickup -> ExpectedTrip = False
    // current above pickup long enough -> ExpectedTrip = True
    public bool ExpectedTrip { get; set; }
    // allow null values for expected trip time range if not specified
    // Minimum acceptable trip time in milliseconds
    public int? ExpectedTripMinMs { get; set; }
    // Maximum acceptable trip time in milliseconds
    // the relay should trip between ExpectedTripMinMs and ExpectedTripMaxMs after the fault starts
    public int? ExpectedTripMaxMs { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // links to test runs
    public List<TestRun> TestRuns { get; set; } = new();
}