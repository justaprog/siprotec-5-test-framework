using TestManagement.Api.Models;

namespace TestManagement.Api.Dtos;

public class TestRunSummaryDto
{
    public Guid Id { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool? Passed { get; set; }
    public DateTime? StartedAt { get; set; }
    public DateTime? FinishedAt { get; set; }

    public TestRunSummaryDto(TestRun testRun)
    {
        Id = testRun.Id;
        Status = testRun.Status.ToString();
        Passed = testRun.Passed;
        StartedAt = testRun.StartedAt;
        FinishedAt = testRun.FinishedAt;
    }
}