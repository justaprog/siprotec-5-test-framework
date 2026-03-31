namespace TestManagement.Api.Dtos;
public class TestCaseSummaryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string DeviceFamily { get; set; } = string.Empty;
    public string ProtectionFunction { get; set; } = string.Empty;
}