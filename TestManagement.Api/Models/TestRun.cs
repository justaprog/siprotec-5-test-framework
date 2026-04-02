namespace TestManagement.Api.Models;
// represents a single execution of a test case, with actual results and status
// run on a device or in this case: simulator
public enum TestRunStatus
{
    // created but not started yet, waiting in the queue
    Pending,
    // currently being executed
    Running,
    // finished successfully with results matching expectations
    Completed,
    // execution failed 
    Failed,
    // execution was cancelled before completion
    Cancelled,
    // no result received within expected time frame
    TimedOut
}

/// <summary>
/// TestRun model represents a single execution of a test case, with actual results and status
/// </summary>
public class TestRun
{
    public Guid Id { get; set; }
    // foreign key to TestCase
    public Guid TestCaseId { get; set; }
    // e.g: Pending, Running, Completed, Failed, etc. could be replaced with an enum
    public TestRunStatus Status { get; set; } = TestRunStatus.Pending;
    // when the test run started
    public DateTime? StartedAt { get; set; }
    // when the test result is received and written 
    public DateTime? FinishedAt { get; set; }
    // actual observed result from the test run, to be compared with expected results in TestCase
    public bool? ActualTrip { get; set; }
    // actual trip time observed in ms from the start of the fault, if the relay tripped
    // to be compared with ExpectedTripMinMs and ExpectedTripMaxMs in TestCase
    public int? ActualTripTimeMs { get; set; }
    // trip and trip time results combined into a single pass/fail status
    public bool? Passed { get; set; }
    // explaination or details about the test run result
    public string? ResultMessage { get; set; }
    // navigation property to access related TestCase details when needed without an extra query
    // e.g: testRun.TestCase.Name 
    public TestCase? TestCase { get; set; }
}