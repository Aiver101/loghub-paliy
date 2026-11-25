using LogHub;

namespace LogHubTests;

public class LogEntryTests
{
    [Fact]
    public void CanCreateLogEntry()
    {
        var entry = new LogEntry { Level = "INFO", Message = "test" };
        Assert.Equal("INFO", entry.Level);
        Assert.Equal("test", entry.Message);
    }
}