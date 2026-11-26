namespace LogHub.Core;

public class LogCollection : IEnumerable<LogEntry>
{
    private readonly List<LogEntry> _entries = new();

    public void Add(LogEntry entry) => _entries.Add(entry);

    public IEnumerator<LogEntry> GetEnumerator()
    {
        return new LogIterator(_entries);
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}