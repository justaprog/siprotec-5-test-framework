namespace TestManagement.Api.Dtos;
public class TestRunResponseDto
{
    public Guid Id { get; set; }
    public Guid TestCaseId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }
    public bool? ActualTrip { get; set; }
    public int? ActualTripTimeMs { get; set; }
    public bool? Passed { get; set; }
    public string? ResultMessage { get; set; }
    public TestCaseSummaryDto? TestCaseSummary { get; set; }
}