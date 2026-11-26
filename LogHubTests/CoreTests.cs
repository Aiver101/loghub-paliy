using LogHub;
using LogHub.Core;

namespace LogHubTests;

public class CoreTests
{
    // 1. Тест парсера: корректная строка
    [Fact]
    public void Parser_ValidLine_ReturnsEntry()
    {
        var parser = new PlainTextLogParser();
        var result = parser.Parse("[2026-11-25 10:00:00] INFO Привет");
        
        Assert.NotNull(result);
        Assert.Equal("[2026-11-25", result.Date);
        Assert.Equal("10:00:00]", result.Level);
        Assert.Equal("INFO Привет", result.Message);
    }

    // 2. Тест парсера: пустая строка или null
    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Parser_EmptyOrNull_ReturnsNull(string? line)
    {
        var parser = new PlainTextLogParser();
        var result = parser.Parse(line!);
        Assert.Null(result);
    }

    // 3. Тест парсера: строка без нужного количества частей
    [Fact]
    public void Parser_IncompleteLine_ReturnsNull()
    {
        var parser = new PlainTextLogParser();
        var result = parser.Parse("ТолькоОдноСлово");
        Assert.Null(result);
    }

    // 4. Тест фильтра: отбрасывает DEBUG
    [Fact]
    public void Filter_DebugLevel_ReturnsFalse()
    {
        var filter = new DebugLogFilter();
        var entry = new LogEntry { Level = "DEBUG" };
        Assert.False(filter.IsValid(entry));
    }

    // 5. Тест фильтра: пропускает INFO
    [Fact]
    public void Filter_InfoLevel_ReturnsTrue()
    {
        var filter = new DebugLogFilter();
        var entry = new LogEntry { Level = "INFO" };
        Assert.True(filter.IsValid(entry));
    }

    // 6. Тест форматтера
    [Fact]
    public void Formatter_ValidEntry_ReturnsFormattedString()
    {
        var formatter = new PlainTextLogFormatter();
        var entry = new LogEntry 
        { 
            Date = "[2026-11-25", 
            Level = "10:00:00]", 
            Message = "INFO Привет" 
        };
        
        var result = formatter.Format(entry);
        
        Assert.Equal("[2026-11-25 | 10:00:00] | INFO Привет", result);
    }

    // 7. Тест маршрутизации (Лог-сервис с фейковым приёмником)
    [Fact]
    public void LogService_ProcessesAndWrites_ToInMemoryWriter()
    {
        var reader = new FakeReader(new[] { "[2026-11-25 10:00:00] INFO Тест" });
        var parser = new PlainTextLogParser();
        var filter = new DebugLogFilter();
        var formatter = new PlainTextLogFormatter();
        var writer = new FakeWriter();

        var service = new LogService(reader, parser, filter, formatter, new[] { writer });
        service.Process("dummy");

        Assert.Single(writer.WrittenLines);
        Assert.Contains("INFO", writer.WrittenLines[0]);
    }
    
    // 8. Тест Синглтона (ЛР5)
    [Fact]
    public void Singleton_ReturnsSameInstance()
    {
        var instance1 = LogHubSettings.Instance;
        var instance2 = LogHubSettings.Instance;
        
        Assert.Same(instance1, instance2);
    }

    // 9. Тест Синглтона (ЛР5)
    [Fact]
    public void Singleton_SettingsCanBeChanged()
    {
        LogHubSettings.Instance.SourcePath = "test.txt";
        Assert.Equal("test.txt", LogHubSettings.Instance.SourcePath);
    }
    
    // 10. Тест JSON-парсера: валидная строка (ЛР6)
    [Fact]
    public void JsonParser_ValidLine_ReturnsEntry()
    {
        var parser = new JsonLogParser();
        var result = parser.Parse("{\"date\":\"2026-11-25\",\"level\":\"INFO\",\"message\":\"Тест\"}");
        
        Assert.NotNull(result);
        Assert.Equal("2026-11-25", result.Date);
        Assert.Equal("INFO", result.Level);
        Assert.Equal("Тест", result.Message);
    }

    // 11. Тест JSON-парсера: плохая строка (ЛР6)
    [Fact]
    public void JsonParser_InvalidLine_ReturnsNull()
    {
        var parser = new JsonLogParser();
        var result = parser.Parse("это не json");
        Assert.Null(result);
    }

    // 12. Тест Итератора: обход коллекции (ЛР6)
    [Fact]
    public void Iterator_IteratesThroughCollection()
    {
        var collection = new LogCollection();
        collection.Add(new LogEntry { Message = "1" });
        collection.Add(new LogEntry { Message = "2" });

        var count = 0;
        foreach (var entry in collection)
        {
            count++;
        }
        Assert.Equal(2, count);
    }
}

// Заглушки для теста маршрутизации, чтобы не зависеть от файлов (DIP)
public class FakeReader : ILogReader
{
    private readonly string[] _lines;
    public FakeReader(string[] lines) => _lines = lines;
    public IEnumerable<string> ReadLines(string sourcePath) => _lines;
}

public class FakeWriter : ILogWriter
{
    public List<string> WrittenLines { get; } = new();
    public void Write(string formattedLine) => WrittenLines.Add(formattedLine);
}