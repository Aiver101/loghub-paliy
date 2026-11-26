using System.Collections;

namespace LogHub.Core;

// Наш кастомный итератор
public class LogIterator : IEnumerator<LogEntry>
{
    private readonly IReadOnlyList<LogEntry> _entries;
    private int _position = -1;

    public LogIterator(IReadOnlyList<LogEntry> entries)
    {
        _entries = entries;
    }

    public LogEntry Current => _entries[_position];

    object IEnumerator.Current => Current;

    public bool MoveNext()
    {
        _position++;
        return _position < _entries.Count;
    }

    public void Reset()
    {
        _position = -1;
    }

    public void Dispose() { }
}