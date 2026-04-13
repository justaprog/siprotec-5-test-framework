using TestManagement.Api.Models;

namespace TestManagement.Api.Dtos;
public class TestCaseSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DeviceFamily { get; set; } = string.Empty;
    public string ProtectionFunction { get; set; } = string.Empty;

    /// <summary>
    /// constructor to create TestCaseSummaryDto from TestCase model
    /// </summary>
    /// <param name="testCase">The TestCase model to initialize the summary with</param>
    /// <returns>
    /// returns a TestCaseSummaryDto instance with properties mapped from the provided TestCase model
    /// </returns>
    public TestCaseSummaryDto(TestCase testCase)
    {
        Id = testCase.Id;
        Name = testCase.Name;
        DeviceFamily = testCase.DeviceFamily;
        ProtectionFunction = testCase.ProtectionFunction;
     }
}