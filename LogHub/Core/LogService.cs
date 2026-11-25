namespace LogHub.Core;

public class LogService
{
    private readonly ILogReader _reader;
    private readonly ILogParser _parser;
    private readonly ILogFilter _filter;
    private readonly ILogFormatter _formatter;
    private readonly IEnumerable<ILogWriter> _writers;

    public LogService(
        ILogReader reader,
        ILogParser parser,
        ILogFilter filter,
        ILogFormatter formatter,
        IEnumerable<ILogWriter> writers)
    {
        _reader = reader;
        _parser = parser;
        _filter = filter;
        _formatter = formatter;
        _writers = writers;
    }

    public void Process(string sourcePath)
    {
        var lines = _reader.ReadLines(sourcePath);
        
        foreach (var line in lines)
        {
            var entry = _parser.Parse(line);
            if (entry == null) continue; // Плохая строка

            if (!_filter.IsValid(entry)) continue; // Отфильтровано

            var formatted = _formatter.Format(entry);

            // Пишем во все переданные приёмники (OCP!)
            foreach (var writer in _writers)
            {
                writer.Write(formatted);
            }
        }
    }
}