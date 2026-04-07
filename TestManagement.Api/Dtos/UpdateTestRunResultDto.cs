namespace TestManagement.Api.Dtos;

/// <summary>
/// DTO for updating the result of a test run after it has been executed.
/// </summary>
public class UpdateTestRunResultDto
{
    public bool ActualTrip { get; set; }
    public int ActualTripTimeMs { get; set; }
    public bool Passed { get; set; }
    public string ResultMessage { get; set; } = string.Empty;
}