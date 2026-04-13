namespace TestManagement.Api.Dtos;

/// <summary>
/// DTO for creating a new test run. This is used in the POST /api/testruns 
/// endpoint to receive the necessary data to create a new test run in the system.
/// </summary>
public class CreateTestRunDto
{
    public Guid TestCaseId { get; set; }
}