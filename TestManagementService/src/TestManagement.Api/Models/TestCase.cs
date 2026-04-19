namespace TestManagement.Api.Models;

public enum DeviceFamily
{
    SIPROTEC_5
}

public enum ProtectionFunction
{
    Overcurrent,
    Undervoltage,
    Distance
}

public enum FaultType
{
    Overcurrent,
    ShortCircuit,
    GroundFault,
    Undervoltage
}

public class SimulationSettings
{
    // 
    public FaultType FaultType { get; set; } = FaultType.Overcurrent;
    // normal current injected before the fault starts
    public double NominalCurrent { get; set; }
    // threshold current where the protection should start reacting
    public double PickupCurrent { get; set; }
    // actual current injected during the simulated fault
    // could be higher than pickup current to ensure the protection reacts
    public double FaultCurrent { get; set; }
    // when the fault current starts being injected, in ms from the start of the test run
    public int FaultStartMs { get; set; }
    // The full simulation runs duration
    public int DurationMs { get; set; }
    // samples per second generated
    public int SamplingRateHz { get; set; }
}

public class ExpectedOutcome
{
    // whether the test case is expected to result in a trip or not
    public bool ExpectedTrip { get; set; }
    // how long the relay is expected to wait before tripping after the fault current is injected
    // common in overcurrent protection wher the relay does not trip immediately,
    // but only after a configured delay.
    public int? TripDelayMs { get; set; }
    // allow null values for expected trip time range if not specified
    // Minimum acceptable trip time in milliseconds
    public int? ExpectedTripMinMs { get; set; }
    // Maximum acceptable trip time in milliseconds
    // the relay should trip between ExpectedTripMinMs and ExpectedTripMaxMs after the fault starts
    public int? ExpectedTripMaxMs { get; set; }
}

public class TestCase
{
    public Guid Id { get; set; }

    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;

    public DeviceFamily DeviceFamily { get; set; } = DeviceFamily.SIPROTEC_5;
    public ProtectionFunction ProtectionFunction { get; set; } = ProtectionFunction.Overcurrent;

    public SimulationSettings Simulation { get; set; } = new();
    public ExpectedOutcome ExpectedOutcome { get; set; } = new();
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    // links to test runs
    public List<TestRun> TestRuns { get; set; } = new();
}