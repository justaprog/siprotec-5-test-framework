namespace TestManagement.Api.Dtos;

public class UpdateTestRunResultDto
{
    public bool ActualTrip { get; set; }
    public int ActualTripTimeMs { get; set; }
    public bool Passed { get; set; }
    public string ResultMessage { get; set; } = string.Empty;
}